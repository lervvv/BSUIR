package com.example.flashcards

import androidx.appcompat.app.AppCompatDelegate
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Book
import androidx.compose.material.icons.filled.Person
import androidx.compose.material.icons.filled.Style
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.res.stringResource
import androidx.core.os.LocaleListCompat
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.NavGraph.Companion.findStartDestination
import androidx.navigation.compose.*
import com.example.flashcards.data.WordDao
import com.example.flashcards.ui.screens.CardsScreen
import com.example.flashcards.ui.screens.DictionaryScreen
import com.example.flashcards.ui.screens.ProfileScreen
import com.example.flashcards.ui.viewmodel.*

sealed class Screen(val route: String, val titleRes: Int, val icon: ImageVector) {
    object Dictionary : Screen("dictionary", R.string.screen_dictionary, Icons.Default.Book)
    object Cards : Screen("cards", R.string.screen_cards, Icons.Default.Style)
    object Profile : Screen("profile", R.string.screen_profile, Icons.Default.Person)
}

// Корневой компонент навигации и основная структура UI
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun FlashCardsApp(
    wordDao: WordDao,
    settingsVm: SettingsViewModel
) {
    val nav = rememberNavController()

    // Применяем язык ко всему приложению (перезапустит Activity автоматически)
    val lang = settingsVm.lang.collectAsStateWithLifecycle().value
    LaunchedEffect(lang) {
        AppCompatDelegate.setApplicationLocales(LocaleListCompat.forLanguageTags(lang))
    }

    val dictVm: DictionaryViewModel = viewModel(factory = DictionaryViewModelFactory(wordDao))
    val cardsVm: CardsViewModel = viewModel(factory = CardsViewModelFactory(wordDao))
    val statsFlow = remember { wordDao.learnedCount() }
    val learnedCount by statsFlow.collectAsStateWithLifecycle(initialValue = 0)

    val backStack by nav.currentBackStackEntryAsState()
    val current = backStack?.destination?.route

    Scaffold(
        bottomBar = {
            NavigationBar {
                listOf(Screen.Dictionary, Screen.Cards, Screen.Profile).forEach { s ->
                    NavigationBarItem(
                        selected = current == s.route,
                        onClick = {
                            nav.navigate(s.route) {
                                popUpTo(nav.graph.findStartDestination().id) { saveState = true }
                                launchSingleTop = true
                                restoreState = true
                            }
                        },
                        icon = { Icon(s.icon, contentDescription = null) },
                        label = { Text(stringResource(s.titleRes)) }
                    )
                }
            }
        }
    ) { padding ->
        NavHost(
            navController = nav,
            startDestination = Screen.Dictionary.route
        ) {
            composable(Screen.Dictionary.route) {
                DictionaryScreen(vm = dictVm, contentPadding = padding)
            }
            composable(Screen.Cards.route) {
                CardsScreen(vm = cardsVm, contentPadding = padding)
            }
            composable(Screen.Profile.route) {
                ProfileScreen(
                    settingsVm = settingsVm,
                    learnedCount = learnedCount,
                    contentPadding = padding
                )
            }
        }
    }
}
