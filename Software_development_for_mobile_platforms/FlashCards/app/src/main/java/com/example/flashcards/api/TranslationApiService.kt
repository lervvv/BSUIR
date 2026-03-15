package com.example.flashcards.api

import retrofit2.http.GET
import retrofit2.http.Query

data class TranslationResponse(
    val responseData: ResponseData
)

data class ResponseData(
    val translatedText: String
)

interface TranslationApiService {
    @GET("get")
    suspend fun getTranslation(
        @Query("q") text: String,
        @Query("langpair") langPair: String = "en|ru"
    ): TranslationResponse
}