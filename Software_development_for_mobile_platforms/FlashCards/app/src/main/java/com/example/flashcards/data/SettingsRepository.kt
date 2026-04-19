package com.example.flashcards.data

import android.content.Context
import androidx.datastore.core.DataStore
import androidx.datastore.preferences.core.*
import androidx.datastore.preferences.preferencesDataStore
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.map

private val Context.dataStore: DataStore<Preferences> by preferencesDataStore("settings")

class SettingsRepository(private val context: Context) {

    private object Keys {
        val DARK = booleanPreferencesKey("dark")
        val LANG = stringPreferencesKey("lang")
        val NOTIFICATIONS_ENABLED = booleanPreferencesKey("notifications_enabled")
        val NOTIFICATION_HOUR = intPreferencesKey("notification_hour")
        val NOTIFICATION_MINUTE = intPreferencesKey("notification_minute")
        val AVATAR_URI = stringPreferencesKey("avatar_uri")
    }

    // Тема
    val darkThemeFlow: Flow<Boolean> = context.dataStore.data.map { it[Keys.DARK] ?: false }
    suspend fun setDarkTheme(value: Boolean) {
        context.dataStore.edit { it[Keys.DARK] = value }
    }

    // Язык
    val langFlow: Flow<String> = context.dataStore.data.map { it[Keys.LANG] ?: "en" }
    suspend fun setLang(value: String) {
        context.dataStore.edit { it[Keys.LANG] = value }
    }

    // Уведомления: включены
    val notificationsEnabledFlow: Flow<Boolean> = context.dataStore.data.map { it[Keys.NOTIFICATIONS_ENABLED] ?: false }
    suspend fun setNotificationsEnabled(value: Boolean) {
        context.dataStore.edit { it[Keys.NOTIFICATIONS_ENABLED] = value }
    }

    // Уведомления: час
    val notificationHourFlow: Flow<Int> = context.dataStore.data.map { it[Keys.NOTIFICATION_HOUR] ?: 9 }
    suspend fun setNotificationHour(value: Int) {
        context.dataStore.edit { it[Keys.NOTIFICATION_HOUR] = value }
    }

    // Уведомления: минута
    val notificationMinuteFlow: Flow<Int> = context.dataStore.data.map { it[Keys.NOTIFICATION_MINUTE] ?: 0 }
    suspend fun setNotificationMinute(value: Int) {
        context.dataStore.edit { it[Keys.NOTIFICATION_MINUTE] = value }
    }

    // Аватар
    val avatarUriFlow: Flow<String?> = context.dataStore.data.map { it[Keys.AVATAR_URI] }
    suspend fun setAvatarUri(uri: String?) {
        context.dataStore.edit {
            if (uri == null) it.remove(Keys.AVATAR_URI)
            else it[Keys.AVATAR_URI] = uri
        }
    }
}