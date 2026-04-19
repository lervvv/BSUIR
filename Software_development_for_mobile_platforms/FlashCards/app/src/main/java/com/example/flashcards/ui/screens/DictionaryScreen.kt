package com.example.flashcards.ui.screens

import android.widget.Toast
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Delete
import androidx.compose.material.icons.filled.DateRange
import androidx.compose.material.icons.filled.Search
import androidx.compose.material.icons.filled.SortByAlpha
import androidx.compose.material.icons.filled.TrendingUp
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.example.flashcards.R
import com.example.flashcards.data.Word
import com.example.flashcards.ui.viewmodel.DictionaryViewModel
import com.example.flashcards.ui.viewmodel.SortType

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun DictionaryScreen(vm: DictionaryViewModel, contentPadding: PaddingValues) {
    val words by vm.words.collectAsStateWithLifecycle()
    val searchQuery by vm.searchQuery.collectAsStateWithLifecycle()
    val selectedCategory by vm.selectedCategory.collectAsStateWithLifecycle()
    val categories by vm.categories.collectAsStateWithLifecycle()
    val sortType by vm.sortType.collectAsStateWithLifecycle()
    val context = LocalContext.current

    // Всплывающие уведомления
    LaunchedEffect(searchQuery) {
        if (searchQuery.isNotBlank()) {
            Toast.makeText(context, context.getString(R.string.toast_search, searchQuery), Toast.LENGTH_SHORT).show()
        }
    }
    LaunchedEffect(selectedCategory) {
        val msg = if (selectedCategory == null) context.getString(R.string.toast_all_categories)
        else context.getString(R.string.toast_category, selectedCategory)
        Toast.makeText(context, msg, Toast.LENGTH_SHORT).show()
    }
    LaunchedEffect(sortType) {
        val msg = when (sortType) {
            SortType.DATE_DESC -> context.getString(R.string.toast_sort_date)
            SortType.ALPHABET -> context.getString(R.string.toast_sort_alphabet)
            SortType.PROGRESS -> context.getString(R.string.toast_sort_progress)
        }
        Toast.makeText(context, msg, Toast.LENGTH_SHORT).show()
    }

    var dialogMode by remember { mutableStateOf<Word?>(null) }
    var showDialog by remember { mutableStateOf(false) }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text(stringResource(R.string.screen_dictionary)) },
                actions = {
                    IconButton(onClick = { vm.setSortType(SortType.DATE_DESC) }) {
                        Icon(Icons.Default.DateRange, contentDescription = "Date")
                    }
                    IconButton(onClick = { vm.setSortType(SortType.ALPHABET) }) {
                        Icon(Icons.Default.SortByAlpha, contentDescription = "Alphabet")
                    }
                    IconButton(onClick = { vm.setSortType(SortType.PROGRESS) }) {
                        Icon(Icons.Default.TrendingUp, contentDescription = "Progress")
                    }
                }
            )
        },
        floatingActionButton = {
            FloatingActionButton(
                modifier = Modifier.padding(bottom = contentPadding.calculateBottomPadding()),
                onClick = { dialogMode = null; showDialog = true }
            ) {
                Icon(Icons.Default.Add, contentDescription = null)
            }
        }
    ) { inner ->
        val pad = PaddingValues(
            start = 16.dp, end = 16.dp,
            top = inner.calculateTopPadding(),
            bottom = contentPadding.calculateBottomPadding() + 16.dp
        )

        Column(modifier = Modifier.fillMaxSize().padding(pad)) {
            // Поле поиска
            OutlinedTextField(
                value = searchQuery,
                onValueChange = { vm.setSearchQuery(it) },
                modifier = Modifier.fillMaxWidth(),
                placeholder = { Text(stringResource(R.string.search_hint)) },
                leadingIcon = { Icon(Icons.Default.Search, null) },
                singleLine = true
            )
            Spacer(modifier = Modifier.height(8.dp))

            // Фильтр по категориям
            Row(
                horizontalArrangement = Arrangement.spacedBy(8.dp),
                modifier = Modifier.fillMaxWidth()
            ) {
                FilterChip(
                    selected = selectedCategory == null,
                    onClick = { vm.setCategory(null) },
                    label = { Text(stringResource(R.string.all_categories)) }
                )
                categories.forEach { cat ->
                    FilterChip(
                        selected = selectedCategory == cat,
                        onClick = { vm.setCategory(cat) },
                        label = { Text(cat) }
                    )
                }
            }
            Spacer(modifier = Modifier.height(16.dp))

            // Список слов
            if (words.isEmpty()) {
                Box(modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center) {
                    Text(stringResource(R.string.no_words_yet))
                }
            } else {
                LazyColumn(verticalArrangement = Arrangement.spacedBy(10.dp)) {
                    items(words, key = { it.id }) { w ->
                        Card(
                            modifier = Modifier.fillMaxWidth().clickable { dialogMode = w; showDialog = true }
                        ) {
                            Row(
                                modifier = Modifier.fillMaxWidth().padding(16.dp),
                                horizontalArrangement = Arrangement.SpaceBetween,
                                verticalAlignment = Alignment.CenterVertically
                            ) {
                                Column(Modifier.weight(1f)) {
                                    Text(w.front, style = MaterialTheme.typography.titleMedium)
                                    Text(w.back, style = MaterialTheme.typography.bodyMedium)
                                    Text(
                                        text = if (w.learned) stringResource(R.string.learned)
                                        else stringResource(R.string.progress_fmt, w.knownCount),
                                        style = MaterialTheme.typography.labelSmall
                                    )
                                    // Отображение категории
                                    Text(
                                        text = w.category,
                                        style = MaterialTheme.typography.labelSmall,
                                        color = MaterialTheme.colorScheme.primary
                                    )
                                }
                                IconButton(onClick = { vm.deleteWord(w) }) {
                                    Icon(Icons.Default.Delete, contentDescription = null)
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    if (showDialog) {
        AddEditWordDialog(
            existing = dialogMode,
            onDismiss = { showDialog = false },
            onSave = { front, back, category ->
                val existing = dialogMode
                if (existing == null) vm.addWord(front, back, category)
                else vm.updateWord(existing, front, back, category)
                showDialog = false
            }
        )
    }
}

@Composable
private fun AddEditWordDialog(
    existing: Word?,
    onDismiss: () -> Unit,
    onSave: (String, String, String) -> Unit
) {
    var front by remember { mutableStateOf(existing?.front ?: "") }
    var back by remember { mutableStateOf(existing?.back ?: "") }
    var category by remember { mutableStateOf(existing?.category ?: "Без категории") }

    AlertDialog(
        onDismissRequest = onDismiss,
        title = { Text(if (existing == null) stringResource(R.string.add_new_word) else stringResource(R.string.edit_word)) },
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
                OutlinedTextField(
                    value = category,
                    onValueChange = { category = it },
                    label = { Text("Category") },
                    singleLine = true
                )
            }
        },
        confirmButton = {
            TextButton(
                enabled = front.isNotBlank() && back.isNotBlank(),
                onClick = { onSave(front, back, category) }
            ) { Text(stringResource(R.string.save)) }
        },
        dismissButton = {
            TextButton(onClick = onDismiss) { Text(stringResource(R.string.cancel)) }
        }
    )
}