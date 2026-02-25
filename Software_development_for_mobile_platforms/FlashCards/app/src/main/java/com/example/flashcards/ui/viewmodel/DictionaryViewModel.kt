package com.example.flashcards.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.flashcards.data.Word
import com.example.flashcards.data.WordDao
import kotlinx.coroutines.flow.SharingStarted
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.stateIn
import kotlinx.coroutines.launch

class DictionaryViewModel(private val dao: WordDao) : ViewModel() {

    val words: StateFlow<List<Word>> =
        dao.allWords().stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), emptyList())

    fun add(front: String, back: String) = viewModelScope.launch {
        dao.upsert(Word(front = front.trim(), back = back.trim()))
    }

    fun update(word: Word, newFront: String, newBack: String) = viewModelScope.launch {
        dao.upsert(word.copy(front = newFront.trim(), back = newBack.trim()))
    }

    fun delete(word: Word) = viewModelScope.launch { dao.delete(word) }
}

class DictionaryViewModelFactory(private val dao: WordDao) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        return DictionaryViewModel(dao) as T
    }
}
