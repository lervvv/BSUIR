package com.example.flashcards.data

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.Query
import kotlinx.coroutines.flow.Flow

@Dao
interface CachedWordDao {
    @Insert
    suspend fun insert(cachedWord: CachedWord)

    @Insert
    suspend fun insertAll(words: List<CachedWord>)

    @Query("SELECT * FROM cached_words ORDER BY RANDOM() LIMIT 1")
    suspend fun getRandom(): CachedWord?

    @Query("SELECT COUNT(*) FROM cached_words")
    suspend fun getCount(): Int

    @Query("DELETE FROM cached_words WHERE timestamp < :cutoff")
    suspend fun deleteOld(cutoff: Long)

    // Для получения последнего (если нужно)
    @Query("SELECT * FROM cached_words ORDER BY timestamp DESC LIMIT 1")
    fun getLatest(): Flow<CachedWord?>

    @Query("DELETE FROM cached_words WHERE id = :id")
    suspend fun deleteById(id: Long)
}