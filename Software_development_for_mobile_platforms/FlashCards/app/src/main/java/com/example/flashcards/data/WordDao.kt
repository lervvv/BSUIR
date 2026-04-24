package com.example.flashcards.data

import androidx.room.*
import kotlinx.coroutines.flow.Flow

@Dao
interface WordDao {
    @Query("SELECT * FROM words WHERE userId = :userId ORDER BY id DESC")
    fun allWords(userId: String): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE learned = 0 AND userId = :userId ORDER BY id DESC")
    fun notLearnedWords(userId: String): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE learned = 1 AND userId = :userId ORDER BY id DESC")
    fun learnedWords(userId: String): Flow<List<Word>>

    @Query("SELECT COUNT(*) FROM words WHERE learned = 1 AND userId = :userId")
    fun learnedCount(userId: String): Flow<Int>

    @Query("SELECT * FROM words WHERE (front LIKE :query OR back LIKE :query) AND userId = :userId ORDER BY id DESC")
    fun searchWords(userId: String, query: String): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE userId = :userId ORDER BY createdAt DESC")
    fun sortByDate(userId: String): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE userId = :userId ORDER BY front ASC")
    fun sortByAlphabet(userId: String): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE userId = :userId ORDER BY knownCount ASC")
    fun sortByProgress(userId: String): Flow<List<Word>>

    @Query("SELECT * FROM words WHERE category = :category AND userId = :userId ORDER BY id DESC")
    fun filterByCategory(userId: String, category: String): Flow<List<Word>>

    @Query("SELECT DISTINCT category FROM words WHERE userId = :userId")
    suspend fun getAllCategories(userId: String): List<String>

    @Query("SELECT DISTINCT category FROM words WHERE userId = :userId")
    fun getCategoriesFlow(userId: String): Flow<List<String>>

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun upsert(word: Word)

    @Delete
    suspend fun delete(word: Word)
}