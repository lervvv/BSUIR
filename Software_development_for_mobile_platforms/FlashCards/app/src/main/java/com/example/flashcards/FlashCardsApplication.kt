package com.example.flashcards

import android.app.Application
import com.example.flashcards.data.AppDatabase

// Для доступа к БД из любой точки приложения
class FlashCardsApplication : Application() {
    val db: AppDatabase by lazy { AppDatabase.get(this) }
}
