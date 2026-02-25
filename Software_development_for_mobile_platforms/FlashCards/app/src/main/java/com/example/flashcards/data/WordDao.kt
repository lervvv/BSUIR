package com.example.flashcards.data

import androidx.room.*
import kotlinx.coroutines.flow.Flow

@Dao
interface WordDao {

    @Query("SELECT * FROM words ORDER BY id DESC")
    fun allWords(): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE learned = 0 ORDER BY id DESC")
    fun notLearnedWords(): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE learned = 1 ORDER BY id DESC")
    fun learnedWords(): Flow<List<Word>>

    @Query("SELECT COUNT(*) FROM words WHERE learned = 1")
    fun learnedCount(): Flow<Int>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun upsert(word: Word)

    @Delete
    suspend fun delete(word: Word)
}
