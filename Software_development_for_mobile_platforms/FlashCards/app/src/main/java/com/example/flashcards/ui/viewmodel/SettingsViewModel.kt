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

    val notificationsEnabled: StateFlow<Boolean> =
        repo.notificationsEnabledFlow.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), false)

    val notificationHour: StateFlow<Int> =
        repo.notificationHourFlow.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), 9)

    val notificationMinute: StateFlow<Int> =
        repo.notificationMinuteFlow.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), 0)

    val avatarUri: StateFlow<String?> =
        repo.avatarUriFlow.stateIn(viewModelScope, SharingStarted.WhileSubscribed(5000), null)

    fun setDark(value: Boolean) = viewModelScope.launch { repo.setDarkTheme(value) }
    fun setLang(value: String) = viewModelScope.launch { repo.setLang(value) }

    fun setNotificationsEnabled(value: Boolean) = viewModelScope.launch { repo.setNotificationsEnabled(value) }
    fun setNotificationHour(value: Int) = viewModelScope.launch { repo.setNotificationHour(value) }
    fun setNotificationMinute(value: Int) = viewModelScope.launch { repo.setNotificationMinute(value) }

    fun setAvatarUri(uri: String?) = viewModelScope.launch { repo.setAvatarUri(uri) }
}

class SettingsViewModelFactory(private val repo: SettingsRepository) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        return SettingsViewModel(repo) as T
    }
}