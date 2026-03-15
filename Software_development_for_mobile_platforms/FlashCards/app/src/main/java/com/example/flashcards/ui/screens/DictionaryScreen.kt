package com.example.flashcards.ui.screens

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.example.flashcards.R
import com.example.flashcards.data.Word
import com.example.flashcards.ui.viewmodel.DictionaryViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun DictionaryScreen(vm: DictionaryViewModel, contentPadding: PaddingValues) {
    // Получаем список всех слов из ViewModel (наблюдаемый StateFlow)
    val words by vm.words.collectAsStateWithLifecycle()

    // Состояние диалога: null = режим добавления, Word = режим редактирования
    var dialogMode by remember { mutableStateOf<Word?>(null) }
    // Флаг видимости диалога
    var showDialog by remember { mutableStateOf(false) }

    Scaffold(
        topBar = { TopAppBar(title = { Text(stringResource(R.string.screen_dictionary)) }) },
        floatingActionButton = {
            // Кнопка добавления нового слова, учитывает нижний отступ от навигации
            FloatingActionButton(
                modifier = Modifier.padding(bottom = contentPadding.calculateBottomPadding()),
                onClick = { dialogMode = null; showDialog = true }
            ) {
                Icon(Icons.Default.Add, contentDescription = null)
            }
        }
    ) { inner ->
        // Рассчитываем отступы с учётом верхней панели и внешнего контента (нижняя навигация)
        val pad = PaddingValues(
            start = 16.dp, end = 16.dp,
            top = inner.calculateTopPadding(),
            bottom = contentPadding.calculateBottomPadding() + 16.dp
        )

        if (words.isEmpty()) {
            // Если словарь пуст — показываем информационное сообщение по центру
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(pad),
                contentAlignment = Alignment.Center
            ) {
                Text(stringResource(R.string.no_words_yet))
            }
        } else {
            // Иначе выводим список слов в LazyColumn для эффективной прокрутки
            LazyColumn(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(pad),
                verticalArrangement = Arrangement.spacedBy(10.dp)
            ) {
                // Ключом выступает id, чтобы правильно обновлять элементы при изменениях
                items(words, key = { it.id }) { w ->
                    Card(
                        modifier = Modifier
                            .fillMaxWidth()
                            // Клик по карточке открывает диалог редактирования
                            .clickable { dialogMode = w; showDialog = true }
                    ) {
                        Row(
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(16.dp),
                            horizontalArrangement = Arrangement.SpaceBetween,
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            // Левая часть: слово, перевод и статус
                            Column(Modifier.weight(1f)) {
                                Text(w.front, style = MaterialTheme.typography.titleMedium)
                                Text(w.back, style = MaterialTheme.typography.bodyMedium)
                                Text(
                                    text = if (w.learned)
                                        stringResource(R.string.learned)       // "Выучено"
                                    else
                                        stringResource(R.string.progress_fmt, w.knownCount), // прогресс X/3
                                    style = MaterialTheme.typography.labelSmall
                                )
                            }
                            // Правая часть: иконка удаления (не перекрывает клик по карточке)
                            IconButton(onClick = { vm.delete(w) }) {
                                Icon(Icons.Default.Delete, contentDescription = null)
                            }
                        }
                    }
                }
            }
        }
    }

    // Отображаем диалог добавления/редактирования, если флаг установлен
    if (showDialog) {
        AddEditWordDialog(
            existing = dialogMode,          // null для добавления, Word для редактирования
            onDismiss = { showDialog = false },
            onSave = { front, back ->
                val existing = dialogMode
                if (existing == null) {
                    vm.add(front, back)     // добавляем новое слово
                } else {
                    vm.update(existing, front, back) // обновляем существующее
                }
                showDialog = false
            }
        )
    }
}

/**
 * Диалог для создания или редактирования слова.
 * @param existing если null — режим добавления, иначе редактирование с предзаполненными полями
 */
@Composable
private fun AddEditWordDialog(
    existing: Word?,
    onDismiss: () -> Unit,
    onSave: (String, String) -> Unit
) {
    // Состояния полей ввода (инициализируются значениями existing, если есть)
    var front by remember { mutableStateOf(existing?.front ?: "") }
    var back by remember { mutableStateOf(existing?.back ?: "") }

    AlertDialog(
        onDismissRequest = onDismiss,
        title = {
            Text(
                if (existing == null) stringResource(R.string.add_new_word)
                else stringResource(R.string.edit_word)
            )
        },
        text = {
            Column(verticalArrangement = Arrangement.spacedBy(10.dp)) {
                OutlinedTextField(
                    value = front,
                    onValueChange = { front = it },
                    label = { Text(stringResource(R.string.original_word)) },
                    singleLine = true
                )
                OutlinedTextField(
                    value = back,
                    onValueChange = { back = it },
                    label = { Text(stringResource(R.string.translated_word)) },
                    singleLine = true
                )
            }
        },
        confirmButton = {
            TextButton(
                // Кнопка "Сохранить" активна только если оба поля непустые
                enabled = front.isNotBlank() && back.isNotBlank(),
                onClick = { onSave(front, back) }
            ) { Text(stringResource(R.string.save)) }
        },
        dismissButton = {
            TextButton(onClick = onDismiss) { Text(stringResource(R.string.cancel)) }
        }
    )
}