package com.example.flashcards.ui.viewmodel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.google.firebase.auth.FirebaseAuth
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.tasks.await

class AuthViewModel : ViewModel() {

    private val auth = FirebaseAuth.getInstance()

    private val _isAuthenticated = MutableStateFlow(auth.currentUser != null)
    val isAuthenticated: StateFlow<Boolean> = _isAuthenticated

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error

    private suspend fun handleAuthCall(block: suspend () -> Unit) {
        try {
            block()
            _isAuthenticated.value = true
            _error.value = null
        } catch (e: Exception) {
            _error.value = e.localizedMessage ?: "Authentication failed"
        }
    }

    fun signIn(email: String, password: String) {
        viewModelScope.launch {
            handleAuthCall {
                auth.signInWithEmailAndPassword(email, password).await()
            }
        }
    }

    fun signUp(email: String, password: String) {
        viewModelScope.launch {
            handleAuthCall {
                auth.createUserWithEmailAndPassword(email, password).await()
            }
        }
    }

    fun signOut() {
        auth.signOut()
        _isAuthenticated.value = false
        _error.value = null
    }

    fun clearError() {
        _error.value = null
    }
}