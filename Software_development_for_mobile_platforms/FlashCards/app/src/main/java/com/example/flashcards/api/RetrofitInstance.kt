package com.example.flashcards.api

import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object RetrofitInstance {
    private const val WORD_BASE_URL = "https://random-word-api.herokuapp.com/"
    private const val TRANSLATION_BASE_URL = "https://api.mymemory.translated.net/"

    val wordApi: WordApiService by lazy {
        Retrofit.Builder()
            .baseUrl(WORD_BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(WordApiService::class.java)
    }

    val translationApi: TranslationApiService by lazy {
        Retrofit.Builder()
            .baseUrl(TRANSLATION_BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(TranslationApiService::class.java)
    }
}