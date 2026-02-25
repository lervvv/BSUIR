package com.example.flashcards.data

import androidx.room.Entity
import androidx.room.PrimaryKey

// Класс для Room (компонент для Базы Данных)
@Entity(tableName = "words")
data class Word(
    @PrimaryKey(autoGenerate = true) val id: Long = 0,
    val front: String,           // слово
    val back: String,            // перевод
    val createdAt: Long = System.currentTimeMillis(),
    val knownCount: Int = 0,     // сколько раз свайпнули вправо
    val learned: Boolean = false // knownCount >= 3
)
