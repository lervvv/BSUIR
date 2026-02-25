package com.example.flashcards.ui.screens

import android.app.Activity
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
    val dark by settingsVm.dark.collectAsStateWithLifecycle()
    val lang by settingsVm.lang.collectAsStateWithLifecycle()

    val context = LocalContext.current
    val activity = context as? Activity

    Scaffold(
        topBar = { TopAppBar(title = { Text(stringResource(R.string.screen_profile)) }) }
    ) { inner ->
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

            Card(Modifier.fillMaxWidth()) {
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(16.dp),
                    horizontalArrangement = Arrangement.SpaceBetween
                ) {
                    Text(stringResource(R.string.theme))
                    Switch(
                        checked = dark,
                        onCheckedChange = { settingsVm.setDark(it) }
                    )
                }
            }

            Card(Modifier.fillMaxWidth()) {
                Column(
                    modifier = Modifier.padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text(stringResource(R.string.language))

                    Row(horizontalArrangement = Arrangement.spacedBy(10.dp)) {
                        FilterChip(
                            selected = lang == "en",
                            onClick = {
                                settingsVm.setLang("en")
                                activity?.recreate()
                            },
                            label = { Text("English") }
                        )
                        FilterChip(
                            selected = lang == "ru",
                            onClick = {
                                settingsVm.setLang("ru")
                                activity?.recreate()
                            },
                            label = { Text("Русский") }
                        )
                    }
                }
            }
        }
    }
}
