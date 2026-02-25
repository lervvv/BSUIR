package com.example.flashcards.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.flashcards.data.SettingsRepository
import kotlinx.coroutines.flow.SharingStarted
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.stateIn
import kotlinx.coroutines.launch

class SettingsViewModel(private val repo: SettingsRepository) : ViewModel() {
    val dark: StateFlow<Boolean> =
        repo.darkThemeFlow.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), false)

    val lang: StateFlow<String> =
        repo.langFlow.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), "en")

    fun setDark(value: Boolean) = viewModelScope.launch { repo.setDarkTheme(value) }
    fun setLang(value: String) = viewModelScope.launch { repo.setLang(value) }
}

class SettingsViewModelFactory(private val repo: SettingsRepository) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        return SettingsViewModel(repo) as T
    }
}
