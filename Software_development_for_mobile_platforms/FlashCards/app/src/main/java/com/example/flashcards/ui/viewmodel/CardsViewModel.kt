package com.example.flashcards.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.flashcards.data.Word
import com.example.flashcards.data.WordDao
import com.example.flashcards.data.WordApiRepository
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.launch

enum class CardsMode {
    DICTIONARY,
    RANDOM
}

sealed class RandomWordState {
    object Idle : RandomWordState()
    object Loading : RandomWordState()
    data class Success(val word: String, val translation: String, val isOffline: Boolean = false) : RandomWordState()
    data class Error(val message: String) : RandomWordState()
    object NoCache : RandomWordState()
}

class CardsViewModel(
    private val dao: WordDao,
    private val apiRepository: WordApiRepository,
    private val userId: String
) : ViewModel() {

    private val _mode = MutableStateFlow(CardsMode.DICTIONARY)
    val mode: StateFlow<CardsMode> = _mode.asStateFlow()

    private val _selectedCategoryForCards = MutableStateFlow<String?>(null)
    val selectedCategoryForCards: StateFlow<String?> = _selectedCategoryForCards.asStateFlow()

    val availableCategories: StateFlow<List<String>> = dao.getCategoriesFlow(userId)
        .stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    private val notLearned: StateFlow<List<Word>> =
        dao.notLearnedWords(userId).stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    private val learned: StateFlow<List<Word>> =
        dao.learnedWords(userId).stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    val deck: StateFlow<List<Word>> = combine(
        _mode,
        _selectedCategoryForCards
    ) { mode, category ->
        if (mode == CardsMode.DICTIONARY) category else null
    }.flatMapLatest { category ->
        if (category == null || category.isBlank()) {
            combine(notLearned, learned) { a, b -> if (a.isNotEmpty()) a else b }
        } else {
            dao.filterByCategory(userId, category).map { filtered ->
                val notLearnedCat = filtered.filter { !it.learned }
                val learnedCat = filtered.filter { it.learned }
                if (notLearnedCat.isNotEmpty()) notLearnedCat else learnedCat
            }
        }
    }.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    private val _randomWordState = MutableStateFlow<RandomWordState>(RandomWordState.Idle)
    val randomWordState: StateFlow<RandomWordState> = _randomWordState.asStateFlow()

    init {
        apiRepository.startCacheMaintenance(viewModelScope)
    }

    fun setMode(newMode: CardsMode) {
        _mode.value = newMode
        if (newMode == CardsMode.RANDOM) {
            loadRandomWord()
        }
    }

    fun setCategoryForCards(category: String?) {
        _selectedCategoryForCards.value = category?.takeIf { it.isNotBlank() }
    }

    fun loadRandomWord() {
        viewModelScope.launch {
            _randomWordState.value = RandomWordState.Loading
            try {
                val result = apiRepository.fetchRandomWordWithTranslation()
                result.onSuccess { (word, translation) ->
                    _randomWordState.value = RandomWordState.Success(word, translation, isOffline = false)
                }.onFailure { error ->
                    when (error.message) {
                        "NO_INTERNET_AND_NO_CACHE" -> _randomWordState.value = RandomWordState.NoCache
                        else -> {
                            val cached = apiRepository.getCachedWordWithTranslation()
                            if (cached != null) {
                                val (word, translation) = cached
                                _randomWordState.value = RandomWordState.Success(word, translation, isOffline = true)
                            } else {
                                val errorMessage = when (error.message) {
                                    "NO_INTERNET" -> "NO_INTERNET"
                                    else -> error.message ?: "UNKNOWN"
                                }
                                _randomWordState.value = RandomWordState.Error(errorMessage)
                            }
                        }
                    }
                }
            } catch (e: Exception) {
                val cached = apiRepository.getCachedWordWithTranslation()
                if (cached != null) {
                    val (word, translation) = cached
                    _randomWordState.value = RandomWordState.Success(word, translation, isOffline = true)
                } else {
                    _randomWordState.value = RandomWordState.Error("UNKNOWN")
                }
            }
        }
    }

    fun processRandomSwipe(isKnown: Boolean) {
        val currentState = _randomWordState.value
        if (currentState is RandomWordState.Success) {
            val word = currentState.word
            val translation = currentState.translation
            if (isKnown) {
                viewModelScope.launch {
                    dao.upsert(Word(front = word, back = translation, userId = userId))
                }
            }
            loadRandomWord()
        }
    }

    fun swipeKnown(word: Word) = viewModelScope.launch {
        val newCount = word.knownCount + 1
        dao.upsert(word.copy(knownCount = newCount, learned = newCount >= 3))
    }

    fun swipeUnknown(word: Word) = viewModelScope.launch {
        dao.upsert(word.copy(knownCount = 0, learned = false))
    }
}

class CardsViewModelFactory(
    private val dao: WordDao,
    private val apiRepository: WordApiRepository,
    private val userId: String
) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        return CardsViewModel(dao, apiRepository, userId) as T
    }
}