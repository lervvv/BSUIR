package com.example.flashcards.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.flashcards.data.Word
import com.example.flashcards.data.WordDao
import com.example.flashcards.data.FirestoreRepository
import kotlinx.coroutines.flow.*
import kotlinx.coroutines.launch
import androidx.lifecycle.ViewModelProvider

class DictionaryViewModel(
    private val dao: WordDao,
    private val firestoreRepo: FirestoreRepository,
    private val userId: String
) : ViewModel() {

    private val _searchQuery = MutableStateFlow("")
    val searchQuery: StateFlow<String> = _searchQuery.asStateFlow()

    private val _selectedCategory = MutableStateFlow<String?>(null)
    val selectedCategory: StateFlow<String?> = _selectedCategory.asStateFlow()

    private val _sortType = MutableStateFlow(SortType.DATE_DESC)
    val sortType: StateFlow<SortType> = _sortType.asStateFlow()

    private val _categories = MutableStateFlow<List<String>>(emptyList())
    val categories: StateFlow<List<String>> = _categories.asStateFlow()

    // Основной поток слов: получаем все слова из Room (они уже синхронизированы с Firestore)
    private val allWordsFromRoom: Flow<List<Word>> = dao.allWords(userId)

    // Применяем поиск, фильтр и сортировку к потоку из Room
    val words: StateFlow<List<Word>> = combine(
        allWordsFromRoom,
        _searchQuery,
        _selectedCategory,
        _sortType
    ) { words, query, category, sort ->
        var filtered = words
        // Фильтр по поиску (нечёткий)
        if (query.isNotBlank()) {
            val lowerQuery = query.lowercase()
            filtered = filtered.filter { word ->
                word.front.lowercase().contains(lowerQuery) ||
                        word.back.lowercase().contains(lowerQuery) ||
                        levenshteinDistance(word.front.lowercase(), lowerQuery) <= 2
            }
        }
        // Фильтр по категории
        if (category != null) {
            filtered = filtered.filter { it.category == category }
        }
        // Сортировка
        when (sort) {
            SortType.DATE_DESC -> filtered.sortedByDescending { it.createdAt }
            SortType.ALPHABET -> filtered.sortedBy { it.front.lowercase() }
            SortType.PROGRESS -> filtered.sortedBy { it.knownCount }
        }
    }.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    private fun levenshteinDistance(s1: String, s2: String): Int {
        val dp = Array(s1.length + 1) { IntArray(s2.length + 1) }
        for (i in 0..s1.length) dp[i][0] = i
        for (j in 0..s2.length) dp[0][j] = j
        for (i in 1..s1.length) {
            for (j in 1..s2.length) {
                val cost = if (s1[i-1] == s2[j-1]) 0 else 1
                dp[i][j] = minOf(dp[i-1][j] + 1, dp[i][j-1] + 1, dp[i-1][j-1] + cost)
            }
        }
        return dp[s1.length][s2.length]
    }

    init {
        // Загружаем начальные категории
        loadCategories()
        // Подписываемся на изменения из Firestore и обновляем Room
        viewModelScope.launch {
            firestoreRepo.listenForWords(userId).collect { remoteWords ->
                remoteWords.forEach { remoteWord ->
                    val existing = dao.allWords(userId).firstOrNull()?.find { it.firestoreId == remoteWord.firestoreId }
                    if (existing == null) {
                        dao.upsert(remoteWord)
                    } else if (remoteWord.lastUpdated > existing.lastUpdated) {
                        dao.upsert(remoteWord)
                    }
                }
                loadCategories()
            }
        }
    }

    fun addWord(front: String, back: String, category: String) {
        viewModelScope.launch {
            val word = Word(
                front = front.trim(),
                back = back.trim(),
                category = category.ifBlank { "Без категории" },
                userId = userId
            )
            dao.upsert(word)
            val firestoreId = firestoreRepo.saveWord(word)
            if (firestoreId != null) {
                dao.upsert(word.copy(firestoreId = firestoreId))
            }
            loadCategories()
        }
    }

    fun updateWord(word: Word, newFront: String, newBack: String, newCategory: String) {
        viewModelScope.launch {
            val updatedWord = word.copy(
                front = newFront.trim(),
                back = newBack.trim(),
                category = newCategory.ifBlank { "Без категории" },
                lastUpdated = System.currentTimeMillis()
            )
            dao.upsert(updatedWord)
            firestoreRepo.saveWord(updatedWord)
            loadCategories()
        }
    }

    fun deleteWord(word: Word) {
        viewModelScope.launch {
            dao.delete(word)
            word.firestoreId?.let { firestoreRepo.deleteWord(it) }
            loadCategories()
        }
    }

    private fun loadCategories() {
        viewModelScope.launch {
            val cats = dao.getAllCategories(userId)
            _categories.value = cats
        }
    }

    fun setSearchQuery(query: String) { _searchQuery.value = query }
    fun setCategory(category: String?) { _selectedCategory.value = category }
    fun setSortType(sortType: SortType) { _sortType.value = sortType }
}

enum class SortType { DATE_DESC, ALPHABET, PROGRESS }

class DictionaryViewModelFactory(
    private val dao: WordDao,
    private val firestoreRepo: FirestoreRepository,
    private val userId: String
) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        return DictionaryViewModel(dao, firestoreRepo, userId) as T
    }
}