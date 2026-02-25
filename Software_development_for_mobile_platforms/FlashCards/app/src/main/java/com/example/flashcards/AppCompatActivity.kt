package com.example.flashcards

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.activity.compose.setContent
import androidx.core.splashscreen.SplashScreen.Companion.installSplashScreen
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import androidx.lifecycle.viewmodel.compose.viewModel
import com.example.flashcards.data.SettingsRepository
import com.example.flashcards.ui.theme.FlashCardsTheme
import com.example.flashcards.ui.viewmodel.SettingsViewModel
import com.example.flashcards.ui.viewmodel.SettingsViewModelFactory

// Точка входа в приложение
class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        installSplashScreen()
        super.onCreate(savedInstanceState)

        val app = application as FlashCardsApplication
        val dao = app.db.wordDao()

        setContent {
            val settingsVm: SettingsViewModel = viewModel(
                factory = SettingsViewModelFactory(SettingsRepository(this))
            )
            val dark = settingsVm.dark.collectAsStateWithLifecycle().value

            FlashCardsTheme(darkTheme = dark) {
                FlashCardsApp(wordDao = dao, settingsVm = settingsVm)
            }
        }
    }
}
