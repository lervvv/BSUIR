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
import com.example.flashcards.data.FirestoreRepository
import com.example.flashcards.data.WordDao
import com.example.flashcards.data.WordApiRepository
import com.example.flashcards.ui.screens.*
import com.example.flashcards.ui.viewmodel.*
import com.google.firebase.auth.FirebaseAuth

sealed class Screen(val route: String, val titleRes: Int, val icon: ImageVector) {
    object Dictionary : Screen("dictionary", R.string.screen_dictionary, Icons.Default.Book)
    object Cards : Screen("cards", R.string.screen_cards, Icons.Default.Style)
    object Profile : Screen("profile", R.string.screen_profile, Icons.Default.Person)
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun FlashCardsApp(
    wordDao: WordDao,
    settingsVm: SettingsViewModel,
    apiRepository: WordApiRepository
) {
    val auth = FirebaseAuth.getInstance()
    var userId by remember { mutableStateOf(auth.currentUser?.uid) }

    // Слушаем изменения состояния входа
    DisposableEffect(Unit) {
        val listener = FirebaseAuth.AuthStateListener { firebaseAuth ->
            userId = firebaseAuth.currentUser?.uid
        }
        auth.addAuthStateListener(listener)
        onDispose { auth.removeAuthStateListener(listener) }
    }

    if (userId == null) {
        AuthScreen(onAuthSuccess = {})
        return
    }

    val nav = rememberNavController()
    val lang = settingsVm.lang.collectAsStateWithLifecycle().value
    LaunchedEffect(lang) {
        AppCompatDelegate.setApplicationLocales(LocaleListCompat.forLanguageTags(lang))
    }

    val firestoreRepo = FirestoreRepository()
    val dictVm: DictionaryViewModel = viewModel(
        factory = DictionaryViewModelFactory(wordDao, firestoreRepo, userId!!)
    )
    val cardsVm: CardsViewModel = viewModel(
        factory = CardsViewModelFactory(wordDao, apiRepository, userId!!)
    )
    val statsFlow = remember { wordDao.learnedCount(userId!!) }
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