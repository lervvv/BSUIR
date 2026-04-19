package com.example.flashcards.data

import android.util.Log
import com.google.firebase.firestore.FirebaseFirestore
import com.google.firebase.firestore.SetOptions
import kotlinx.coroutines.tasks.await

class FirestoreRepository {
    private val db = FirebaseFirestore.getInstance()
    private val wordsCollection = db.collection("words")

    suspend fun saveWord(word: Word): String? {
        return try {
            val docRef = if (word.firestoreId.isNullOrEmpty()) {
                wordsCollection.document()
            } else {
                wordsCollection.document(word.firestoreId!!)
            }
            val data = mapOf(
                "front" to word.front,
                "back" to word.back,
                "category" to word.category,
                "createdAt" to word.createdAt,
                "lastUpdated" to System.currentTimeMillis(),
                "knownCount" to word.knownCount,
                "learned" to word.learned
            )
            docRef.set(data, SetOptions.merge()).await()
            docRef.id
        } catch (e: Exception) {
            Log.e("Firestore", "Error saving word", e)
            null
        }
    }

    suspend fun deleteWord(firestoreId: String) {
        try {
            wordsCollection.document(firestoreId).delete().await()
        } catch (e: Exception) {
            Log.e("Firestore", "Error deleting word", e)
        }
    }

    suspend fun loadAllWords(): List<Word> {
        return try {
            val snapshot = wordsCollection.get().await()
            snapshot.documents.mapNotNull { doc ->
                val data = doc.data ?: return@mapNotNull null
                Word(
                    front = data["front"] as? String ?: return@mapNotNull null,
                    back = data["back"] as? String ?: return@mapNotNull null,
                    category = data["category"] as? String ?: "Без категории",
                    createdAt = data["createdAt"] as? Long ?: System.currentTimeMillis(),
                    lastUpdated = data["lastUpdated"] as? Long ?: System.currentTimeMillis(),
                    knownCount = (data["knownCount"] as? Long)?.toInt() ?: 0,
                    learned = data["learned"] as? Boolean ?: false,
                    firestoreId = doc.id
                )
            }
        } catch (e: Exception) {
            Log.e("Firestore", "Error loading words", e)
            emptyList()
        }
    }
}