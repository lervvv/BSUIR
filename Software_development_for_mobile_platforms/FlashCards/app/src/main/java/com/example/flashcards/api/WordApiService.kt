package com.example.flashcards.api

import retrofit2.http.GET

interface WordApiService {
    @GET("word?number=1")
    suspend fun getRandomWord(): List<String>
}