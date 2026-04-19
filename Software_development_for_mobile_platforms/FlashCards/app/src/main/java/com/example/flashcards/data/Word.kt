package com.example.flashcards.data

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "words")
data class Word(
    @PrimaryKey(autoGenerate = true) val id: Long = 0,
    val front: String,
    val back: String,
    val category: String = "Без категории",   // новое поле
    val createdAt: Long = System.currentTimeMillis(),
    val lastUpdated: Long = System.currentTimeMillis(),
    val knownCount: Int = 0,
    val learned: Boolean = false,
    val firestoreId: String? = null
)