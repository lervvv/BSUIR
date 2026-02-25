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
        val LANG = stringPreferencesKey("lang") // "en" / "ru"
    }

    val darkThemeFlow: Flow<Boolean> = context.dataStore.data.map { it[Keys.DARK] ?: false }
    val langFlow: Flow<String> = context.dataStore.data.map { it[Keys.LANG] ?: "en" }

    suspend fun setDarkTheme(value: Boolean) {
        context.dataStore.edit { it[Keys.DARK] = value }
    }

    suspend fun setLang(value: String) {
        context.dataStore.edit { it[Keys.LANG] = value }
    }
}
