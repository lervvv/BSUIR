package com.example.flashcards.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.flashcards.data.Word
import com.example.flashcards.data.WordDao
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.launch

class CardsViewModel(private val dao: WordDao) : ViewModel() {

    private val notLearned: StateFlow<List<Word>> =
        dao.notLearnedWords().stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    private val learned: StateFlow<List<Word>> =
        dao.learnedWords().stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    // Если невыученные кончились — показываем выученные
    val deck: StateFlow<List<Word>> =
        combine(notLearned, learned) { a, b -> if (a.isNotEmpty()) a else b }
            .stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    // Увеличение knownCount на 1, если свайп вправо, если knownCount = 3 -> выучено
    fun swipeKnown(word: Word) = viewModelScope.launch {
        val newCount = word.knownCount + 1
        dao.upsert(word.copy(knownCount = newCount, learned = newCount >= 3))
    }

    // Свайп влево — сбрасываем прогресс
    fun swipeUnknown(word: Word) = viewModelScope.launch {
        dao.upsert(word.copy(knownCount = 0, learned = false))
    }
}

class CardsViewModelFactory(private val dao: WordDao) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        return CardsViewModel(dao) as T
    }
}
