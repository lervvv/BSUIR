package com.example.flashcards.data

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "cached_words")
data class CachedWord(
    @PrimaryKey(autoGenerate = true) val id: Long = 0,
    val word: String,
    val translation: String,
    val timestamp: Long = System.currentTimeMillis()
)