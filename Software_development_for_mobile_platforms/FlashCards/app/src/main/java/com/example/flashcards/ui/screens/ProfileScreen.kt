package com.example.flashcards.ui.screens

import android.app.Activity
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.example.flashcards.R
import com.example.flashcards.ui.viewmodel.SettingsViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ProfileScreen(
    settingsVm: SettingsViewModel,
    learnedCount: Int,
    contentPadding: PaddingValues
) {
    // Получаем текущие значения настроек (тема, язык) из ViewModel
    val dark by settingsVm.dark.collectAsStateWithLifecycle()
    val lang by settingsVm.lang.collectAsStateWithLifecycle()

    // Получаем контекст и пытаемся привести его к Activity для возможности перезапуска
    val context = LocalContext.current
    val activity = context as? Activity

    Scaffold(
        topBar = { TopAppBar(title = { Text(stringResource(R.string.screen_profile)) }) }
    ) { inner ->
        // Рассчитываем отступы с учётом верхней панели и внешнего контента (нижняя навигация)
        val pad = PaddingValues(
            start = 16.dp,
            end = 16.dp,
            top = inner.calculateTopPadding(),
            bottom = contentPadding.calculateBottomPadding() + 16.dp
        )

        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(pad),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            // Карточка со статистикой: количество выученных слов
            Card(Modifier.fillMaxWidth()) {
                Column(
                    modifier = Modifier.padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(6.dp)
                ) {
                    Text(
                        text = stringResource(R.string.words_learned),
                        style = MaterialTheme.typography.titleMedium
                    )
                    Text(
                        text = learnedCount.toString(),
                        style = MaterialTheme.typography.displayMedium
                    )
                }
            }

            // Карточка с переключателем тёмной темы
            Card(Modifier.fillMaxWidth()) {
                Column(
                    modifier = Modifier.padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text(stringResource(R.string.theme))
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.spacedBy(10.dp)
                    ) {
                        FilterChip(
                            selected = !dark,
                            onClick = { settingsVm.setDark(false) },
                            label = { Text(stringResource(R.string.light_theme)) },
                            border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline)
                        )
                        FilterChip(
                            selected = dark,
                            onClick = { settingsVm.setDark(true) },
                            label = { Text(stringResource(R.string.dark_theme)) },
                            border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline)
                        )
                    }
                }
            }

            // Карточка выбора языка интерфейса
            Card(Modifier.fillMaxWidth()) {
                Column(
                    modifier = Modifier.padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text(stringResource(R.string.language))

                    Row(modifier = Modifier.fillMaxWidth(),
                    horizontalArrangement = Arrangement.spacedBy(10.dp)
                    ) {
                        FilterChip(
                            selected = lang == "en",
                            onClick = {
                                settingsVm.setLang("en")
                                activity?.recreate()
                            },
                            label = { Text("English") },
                            border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline)
                        )
                        FilterChip(
                            selected = lang == "ru",
                            onClick = {
                                settingsVm.setLang("ru")
                                activity?.recreate()
                            },
                            label = { Text("Русский") },
                            border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline)
                        )
                    }
                }
            }
        }
    }
}