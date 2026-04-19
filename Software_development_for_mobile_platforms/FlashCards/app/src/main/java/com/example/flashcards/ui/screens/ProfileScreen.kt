package com.example.flashcards.ui.screens

import android.Manifest
import android.app.Activity
import android.app.TimePickerDialog
import android.net.Uri
import android.widget.Toast
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import coil.compose.AsyncImage
import com.example.flashcards.R
import com.example.flashcards.ui.viewmodel.SettingsViewModel
import com.example.flashcards.utils.NotificationHelper
import com.example.flashcards.utils.NotificationScheduler
import com.google.accompanist.permissions.isGranted
import com.google.accompanist.permissions.rememberPermissionState
import java.io.File
import java.util.Calendar
import android.os.Build
import com.google.accompanist.permissions.ExperimentalPermissionsApi

@OptIn(ExperimentalMaterial3Api::class, ExperimentalPermissionsApi::class)
@Composable
fun ProfileScreen(
    settingsVm: SettingsViewModel,
    learnedCount: Int,
    contentPadding: PaddingValues
) {
    val dark by settingsVm.dark.collectAsStateWithLifecycle()
    val lang by settingsVm.lang.collectAsStateWithLifecycle()
    val notificationsEnabled by settingsVm.notificationsEnabled.collectAsStateWithLifecycle()
    val notificationHour by settingsVm.notificationHour.collectAsStateWithLifecycle()
    val notificationMinute by settingsVm.notificationMinute.collectAsStateWithLifecycle()
    val avatarUri by settingsVm.avatarUri.collectAsStateWithLifecycle()
    var showDeleteAvatarDialog by remember { mutableStateOf(false) }
    val context = LocalContext.current
    val activity = context as? Activity

    // Запрос разрешения на уведомления (Android 13+)
    if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
        val notificationPermissionState = rememberPermissionState(Manifest.permission.POST_NOTIFICATIONS)
        LaunchedEffect(Unit) {
            if (!notificationPermissionState.status.isGranted) {
                notificationPermissionState.launchPermissionRequest()
            }
        }
    }

    // Функция сохранения изображения во внутреннее хранилище
    fun saveImageToInternalStorage(uri: Uri): String? {
        val contentResolver = context.contentResolver
        val fileName = "avatar_${System.currentTimeMillis()}.jpg"
        val file = File(context.filesDir, fileName)
        return try {
            contentResolver.openInputStream(uri)?.use { input ->
                file.outputStream().use { output ->
                    input.copyTo(output)
                }
            }
            file.absolutePath
        } catch (e: Exception) {
            e.printStackTrace()
            null
        }
    }

    val avatarPickerLauncher = rememberLauncherForActivityResult(ActivityResultContracts.GetContent()) { uri ->
        uri?.let {
            val path = saveImageToInternalStorage(it)
            if (path != null) {
                settingsVm.setAvatarUri(path)
            } else {
                Toast.makeText(context, "Failed to save image", Toast.LENGTH_SHORT).show()
            }
        }
    }

    fun deleteAvatar() {
        val oldPath = avatarUri
        if (oldPath != null) {
            try {
                File(oldPath).delete()
            } catch (e: Exception) { }
        }
        settingsVm.setAvatarUri(null)
        Toast.makeText(context, context.getString(R.string.avatar_deleted), Toast.LENGTH_SHORT).show()
    }

    // Создаём канал уведомлений
    LaunchedEffect(Unit) {
        NotificationHelper.createNotificationChannel(context)
    }

    val scheduler = remember { NotificationScheduler(context) }

    fun showTimePicker() {
        val calendar = Calendar.getInstance()
        val currentHour = calendar.get(Calendar.HOUR_OF_DAY)
        val currentMinute = calendar.get(Calendar.MINUTE)
        TimePickerDialog(
            context,
            { _, hourOfDay, minute ->
                settingsVm.setNotificationHour(hourOfDay)
                settingsVm.setNotificationMinute(minute)
                if (notificationsEnabled) {
                    scheduler.scheduleDailyNotification(hourOfDay, minute)
                    Toast.makeText(context, context.getString(R.string.reminder_set, hourOfDay, minute), Toast.LENGTH_SHORT).show()
                }
            },
            currentHour,
            currentMinute,
            true
        ).show()
    }

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
            // Карточка аватара
            Card(Modifier.fillMaxWidth()) {
                Column(
                    modifier = Modifier.padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text("Profile picture")
                    Row(
                        verticalAlignment = Alignment.CenterVertically,
                        horizontalArrangement = Arrangement.spacedBy(12.dp) // расстояние между элементами
                    ) {
                        if (avatarUri != null) {
                            val avatarFile = File(avatarUri)
                            if (avatarFile.exists()) {
                                AsyncImage(
                                    model = avatarFile,
                                    contentDescription = "Avatar",
                                    modifier = Modifier.size(64.dp).clip(CircleShape),
                                    contentScale = ContentScale.Crop
                                )
                                Button(onClick = { showDeleteAvatarDialog = true }) {
                                    Text("Delete")
                                }
                            }
                        }
                        Button(onClick = { avatarPickerLauncher.launch("image/*") }) {
                            Text(if (avatarUri == null) "Choose image" else "Change image")
                        }
                    }
                }
            }
            // Диалог подтверждения удаления аватара
            if (showDeleteAvatarDialog) {
                AlertDialog(
                    onDismissRequest = { showDeleteAvatarDialog = false },
                    title = { Text(stringResource(R.string.delete_avatar_title)) },
                    text = { Text(stringResource(R.string.delete_avatar_message)) },
                    confirmButton = {
                        TextButton(
                            onClick = {
                                deleteAvatar()
                                showDeleteAvatarDialog = false
                            }
                        ) {
                            Text(stringResource(R.string.delete))
                        }
                    },
                    dismissButton = {
                        TextButton(onClick = { showDeleteAvatarDialog = false }) {
                            Text(stringResource(R.string.cancel))
                        }
                    }
                )
            }

            // Карточка статистики
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

            // Карточка темы
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

            // Карточка языка
            Card(Modifier.fillMaxWidth()) {
                Column(
                    modifier = Modifier.padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text(stringResource(R.string.language))
                    Row(
                        modifier = Modifier.fillMaxWidth(),
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

            // Карточка уведомлений
            Card(Modifier.fillMaxWidth()) {
                Column(
                    modifier = Modifier.padding(16.dp),
                    verticalArrangement = Arrangement.spacedBy(10.dp)
                ) {
                    Text(stringResource(R.string.daily_reminders))
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.SpaceBetween,
                        verticalAlignment = Alignment.CenterVertically
                    ) {
                        Text(stringResource(R.string.select_time))
                        Button(
                            onClick = { showTimePicker() },
                            enabled = notificationsEnabled
                        ) {
                            Text("$notificationHour:$notificationMinute")
                        }
                    }
                    Switch(
                        checked = notificationsEnabled,
                        onCheckedChange = { enabled ->
                            settingsVm.setNotificationsEnabled(enabled)
                            if (enabled) {
                                scheduler.scheduleDailyNotification(notificationHour, notificationMinute)
                            } else {
                                scheduler.cancelNotification()
                            }
                        }
                    )
                }
            }
        }
    }
}