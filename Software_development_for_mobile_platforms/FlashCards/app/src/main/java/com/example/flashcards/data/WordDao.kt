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

    @Query("SELECT * FROM words WHERE front LIKE :query OR back LIKE :query ORDER BY id DESC")
    fun searchWords(query: String): Flow<List<Word>>

    @Query("SELECT * FROM words ORDER BY createdAt DESC")
    fun sortByDate(): Flow<List<Word>>

    @Query("SELECT * FROM words ORDER BY front ASC")
    fun sortByAlphabet(): Flow<List<Word>>

    @Query("SELECT * FROM words ORDER BY knownCount ASC")
    fun sortByProgress(): Flow<List<Word>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun upsert(word: Word)

    // Фильтр по категории
    @Query("SELECT * FROM words WHERE category = :category ORDER BY id DESC")
    fun filterByCategory(category: String): Flow<List<Word>>

    // Получить все уникальные категории
    @Query("SELECT DISTINCT category FROM words")
    suspend fun getAllCategories(): List<String>

    @Query("SELECT DISTINCT category FROM words")
    fun getCategoriesFlow(): Flow<List<String>>

    @Delete
    suspend fun delete(word: Word)
}