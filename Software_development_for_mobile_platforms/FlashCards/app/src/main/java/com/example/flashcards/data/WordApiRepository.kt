package com.example.flashcards.data

import android.content.Context
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import com.example.flashcards.api.RetrofitInstance
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock
import kotlinx.coroutines.flow.firstOrNull

class WordApiRepository(
    private val context: Context,
    private val cachedWordDao: CachedWordDao
) {
    private val wordApi = RetrofitInstance.wordApi
    private val translationApi = RetrofitInstance.translationApi

    // Внутренний метод для получения одного слова с переводом (без сохранения в кэш)
    private suspend fun fetchSingleWordWithTranslation(): Result<Pair<String, String>> {
        return try {
            val wordResponse = wordApi.getRandomWord()
            val word = wordResponse.firstOrNull() ?: return Result.failure(Exception("Empty word response"))
            val translationResponse = translationApi.getTranslation(word)
            val translation = translationResponse.responseData.translatedText
            if (translation.isBlank()) {
                return Result.failure(Exception("Empty translation"))
            }
            Result.success(word to translation)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    // Предзагрузка указанного количества слов (только если есть интернет)
    suspend fun prefetchWords(count: Int) {
        if (!isNetworkAvailable()) return
        val words = mutableListOf<CachedWord>()
        repeat(count) {
            try {
                val result = fetchSingleWordWithTranslation()
                if (result.isSuccess) {
                    val (word, translation) = result.getOrNull()!!
                    words.add(CachedWord(word = word, translation = translation))
                }
            } catch (e: Exception) {
                // игнорируем ошибки при предзагрузке
            }
        }
        if (words.isNotEmpty()) {
            cachedWordDao.insertAll(words)
        }
    }

    suspend fun fetchRandomWordWithTranslation(): Result<Pair<String, String>> {
        return if (isNetworkAvailable()) {
            // Интернет есть — берём новое слово напрямую из API, не трогаем кэш
            fetchSingleWordWithTranslation()
        } else {
            // Интернета нет — берём случайное слово из кэша и удаляем его
            val cached = cachedWordDao.getRandom()
            if (cached != null) {
                cachedWordDao.deleteById(cached.id)
                Result.success(cached.word to cached.translation)
            } else {
                Result.failure(Exception("NO_INTERNET_AND_NO_CACHE"))
            }
        }
    }

    suspend fun getCachedCount(): Int = cachedWordDao.getCount()

    fun isNetworkAvailable(): Boolean {
        val connectivityManager = context.getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager
        val activeNetwork = connectivityManager.activeNetwork ?: return false
        val capabilities = connectivityManager.getNetworkCapabilities(activeNetwork) ?: return false
        return when {
            capabilities.hasTransport(NetworkCapabilities.TRANSPORT_WIFI) -> true
            capabilities.hasTransport(NetworkCapabilities.TRANSPORT_CELLULAR) -> true
            capabilities.hasTransport(NetworkCapabilities.TRANSPORT_ETHERNET) -> true
            else -> false
        }
    }

    suspend fun getCachedWordWithTranslation(): Pair<String, String>? {
        return cachedWordDao.getRandom()?.let { it.word to it.translation }
    }

    private val cacheMutex = Mutex()
    private var isMaintaining = false

    fun startCacheMaintenance(scope: CoroutineScope) {
        scope.launch {
            cacheMutex.withLock {
                if (isMaintaining) return@launch
                isMaintaining = true
            }
            while (true) {
                try {
                    if (isNetworkAvailable()) {
                        val currentCount = cachedWordDao.getCount()
                        if (currentCount < 50) {
                            val needed = 50 - currentCount
                            // Загружаем пачками по 10, чтобы не перегружать сеть
                            val words = fetchWordsBatch(needed.coerceAtMost(10))
                            if (words.isNotEmpty()) {
                                cachedWordDao.insertAll(words)
                            }
                        }
                    }
                    delay(5000) // проверяем каждые 5 секунд
                } catch (e: Exception) {
                    // логируем ошибку и продолжаем
                    delay(10000)
                }
            }
        }
    }

    // Вспомогательная функция для загрузки пачки слов
    private suspend fun fetchWordsBatch(count: Int): List<CachedWord> {
        val result = mutableListOf<CachedWord>()
        repeat(count) {
            try {
                val resultPair = fetchSingleWordWithTranslation().getOrNull()
                if (resultPair != null) {
                    val (word, translation) = resultPair
                    result.add(CachedWord(word = word, translation = translation))
                }
            } catch (e: Exception) {
                // игнорируем одиночные ошибки
            }
        }
        return result
    }
}