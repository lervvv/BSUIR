package com.example.flashcards.ui.screens

import androidx.compose.foundation.BorderStroke
import androidx.compose.foundation.gestures.detectDragGestures
import androidx.compose.foundation.gestures.detectTapGestures
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.input.pointer.pointerInput
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.IntOffset
import androidx.compose.ui.unit.dp
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import com.example.flashcards.R
import com.example.flashcards.data.Word
import com.example.flashcards.ui.viewmodel.CardsMode
import com.example.flashcards.ui.viewmodel.CardsViewModel
import com.example.flashcards.ui.viewmodel.RandomWordState
import kotlin.math.roundToInt

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun CardsScreen(vm: CardsViewModel, contentPadding: PaddingValues) {
    val mode by vm.mode.collectAsStateWithLifecycle()
    val deck by vm.deck.collectAsStateWithLifecycle()
    val randomState by vm.randomWordState.collectAsStateWithLifecycle()
    var index by remember { mutableIntStateOf(0) }

    LaunchedEffect(deck.size) {
        index = if (deck.isNotEmpty()) index % deck.size else 0
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text(stringResource(R.string.screen_cards)) },
                actions = {
                    Row(Modifier.padding(end = 8.dp)) {
                        FilterChip(
                            selected = mode == CardsMode.DICTIONARY,
                            onClick = { vm.setMode(CardsMode.DICTIONARY) },
                            label = { Text(stringResource(R.string.my_words)) },
                            border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline)
                        )
                        Spacer(Modifier.width(16.dp))
                        FilterChip(
                            selected = mode == CardsMode.RANDOM,
                            onClick = { vm.setMode(CardsMode.RANDOM) },
                            label = { Text(stringResource(R.string.random)) },
                            border = BorderStroke(1.dp, MaterialTheme.colorScheme.outline)
                        )
                    }
                }
            )
        }
    ) { inner ->
        val pad = PaddingValues(
            start = 16.dp,
            end = 16.dp,
            top = inner.calculateTopPadding(),
            bottom = contentPadding.calculateBottomPadding() + 16.dp
        )

        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(pad),
            contentAlignment = Alignment.Center
        ) {
            when (mode) {
                CardsMode.DICTIONARY -> {
                    if (deck.isEmpty()) {
                        Text(stringResource(R.string.no_words_to_learn))
                    } else {
                        val word = deck[index % deck.size]
                        SwipeCard(
                            word = word,
                            onLeft = { vm.swipeUnknown(word); index++ },
                            onRight = { vm.swipeKnown(word); index++ }
                        )
                    }
                }
                CardsMode.RANDOM -> {
                    when (val state = randomState) {
                        is RandomWordState.Idle -> {
                            Button(onClick = { vm.loadRandomWord() }) {
                                Text(stringResource(R.string.load_random_word))
                            }
                        }
                        is RandomWordState.Loading -> {
                            CircularProgressIndicator()
                        }
                        is RandomWordState.Success -> {
                            RandomWordCard(
                                word = state.word,
                                translation = state.translation,
                                isOffline = state.isOffline,
                                onSwipeLeft = { vm.processRandomSwipe(false) },
                                onSwipeRight = { vm.processRandomSwipe(true) },
                                onNext = { vm.loadRandomWord() }
                            )
                        }
                        is RandomWordState.Error -> {
                            val errorText = when (state.message) {
                                "NO_INTERNET" -> stringResource(R.string.error_no_internet)
                                "UNKNOWN" -> stringResource(R.string.error_unknown)
                                else -> state.message
                            }
                            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                                Text(text = errorText)
                                Spacer(Modifier.height(8.dp))
                                Button(onClick = { vm.loadRandomWord() }) {
                                    Text(stringResource(R.string.retry))
                                }
                            }
                        }
                        is RandomWordState.NoCache -> {
                            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                                Text(
                                    text = stringResource(R.string.check_internet_connection),
                                    textAlign = TextAlign.Center
                                )
                                Spacer(Modifier.height(8.dp))
                                Button(onClick = { vm.loadRandomWord() }) {
                                    Text(stringResource(R.string.retry))
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

@Composable
private fun SwipeCard(
    word: Word,
    onLeft: () -> Unit,
    onRight: () -> Unit
) {
    var dx by remember { mutableFloatStateOf(0f) }
    var dy by remember { mutableFloatStateOf(0f) }
    var showBack by remember { mutableStateOf(false) }
    val threshold = 140f

    Card(
        modifier = Modifier
            .size(width = 320.dp, height = 420.dp)
            .offset { IntOffset(dx.roundToInt(), dy.roundToInt()) }
            .graphicsLayer { rotationZ = (dx / 18f).coerceIn(-15f, 15f) }
            .pointerInput(word.id) {
                detectDragGestures(
                    onDrag = { change, drag ->
                        change.consume()
                        dx += drag.x
                        dy += drag.y
                    },
                    onDragEnd = {
                        when {
                            dx > threshold -> {
                                dx = 0f; dy = 0f; showBack = false
                                onRight()
                            }
                            dx < -threshold -> {
                                dx = 0f; dy = 0f; showBack = false
                                onLeft()
                            }
                            else -> {
                                dx = 0f; dy = 0f
                            }
                        }
                    }
                )
            }
            .pointerInput(word.id) {
                detectTapGestures(
                    onTap = { showBack = !showBack }
                )
            },
        colors = CardDefaults.cardColors(
            containerColor = when {
                dx > 80 -> MaterialTheme.colorScheme.primaryContainer
                dx < -80 -> MaterialTheme.colorScheme.errorContainer
                else -> MaterialTheme.colorScheme.surfaceVariant
            }
        )
    ) {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(22.dp),
            contentAlignment = Alignment.Center
        ) {
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                Text(
                    text = word.front,
                    style = MaterialTheme.typography.displaySmall,
                    textAlign = TextAlign.Center
                )
                Spacer(Modifier.height(18.dp))
                if (showBack) {
                    Divider(Modifier.width(120.dp))
                    Spacer(Modifier.height(12.dp))
                    Text(
                        text = word.back,
                        style = MaterialTheme.typography.headlineMedium,
                        textAlign = TextAlign.Center
                    )
                } else {
                    Text(
                        text = stringResource(R.string.tap_to_translate),
                        style = MaterialTheme.typography.labelMedium,
                        textAlign = TextAlign.Center
                    )
                }
                Spacer(Modifier.height(18.dp))
                Text(
                    text = stringResource(R.string.progress_fmt, word.knownCount),
                    style = MaterialTheme.typography.labelSmall
                )
            }
        }
    }
}

@Composable
private fun RandomWordCard(
    word: String,
    translation: String,
    isOffline: Boolean,
    onSwipeLeft: () -> Unit,
    onSwipeRight: () -> Unit,
    onNext: () -> Unit
) {
    var dx by remember { mutableFloatStateOf(0f) }
    var dy by remember { mutableFloatStateOf(0f) }
    var showBack by remember { mutableStateOf(false) }
    val threshold = 140f

    Card(
        modifier = Modifier
            .size(width = 320.dp, height = 420.dp)
            .offset { IntOffset(dx.roundToInt(), dy.roundToInt()) }
            .graphicsLayer { rotationZ = (dx / 18f).coerceIn(-15f, 15f) }
            .pointerInput(word) {
                detectDragGestures(
                    onDrag = { change, drag ->
                        change.consume()
                        dx += drag.x
                        dy += drag.y
                    },
                    onDragEnd = {
                        when {
                            dx > threshold -> {
                                dx = 0f; dy = 0f; showBack = false
                                onSwipeRight()
                            }
                            dx < -threshold -> {
                                dx = 0f; dy = 0f; showBack = false
                                onSwipeLeft()
                            }
                            else -> {
                                dx = 0f; dy = 0f
                            }
                        }
                    }
                )
            }
            .pointerInput(word) {
                detectTapGestures(
                    onTap = { showBack = !showBack }
                )
            },
        colors = CardDefaults.cardColors(
            containerColor = when {
                dx > 80 -> MaterialTheme.colorScheme.primaryContainer
                dx < -80 -> MaterialTheme.colorScheme.errorContainer
                else -> MaterialTheme.colorScheme.surfaceVariant
            }
        )
    ) {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(22.dp),
            contentAlignment = Alignment.Center
        ) {
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                if (isOffline) {
                    Text(
                        text = stringResource(R.string.offline_mode),
                        style = MaterialTheme.typography.labelSmall,
                        color = MaterialTheme.colorScheme.error
                    )
                    Spacer(Modifier.height(8.dp))
                }
                Text(
                    text = word,
                    style = MaterialTheme.typography.displaySmall,
                    textAlign = TextAlign.Center
                )
                Spacer(Modifier.height(18.dp))
                if (showBack) {
                    Divider(Modifier.width(120.dp))
                    Spacer(Modifier.height(12.dp))
                    Text(
                        text = translation,
                        style = MaterialTheme.typography.headlineMedium,
                        textAlign = TextAlign.Center
                    )
                } else {
                    Text(
                        text = stringResource(R.string.tap_to_translate),
                        style = MaterialTheme.typography.labelMedium,
                        textAlign = TextAlign.Center
                    )
                }
                Spacer(Modifier.height(18.dp))
                Text(
                    text = stringResource(R.string.swipe_right_add),
                    style = MaterialTheme.typography.labelSmall
                )
                Spacer(Modifier.height(12.dp))
                Button(onClick = onNext) {
                    Text(stringResource(R.string.next))
                }
            }
        }
    }
}