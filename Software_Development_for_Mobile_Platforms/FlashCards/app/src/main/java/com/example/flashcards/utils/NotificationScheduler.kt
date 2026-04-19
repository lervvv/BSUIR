package com.example.flashcards.utils

import android.content.Context
import androidx.work.*
import java.util.Calendar
import java.util.concurrent.TimeUnit

class NotificationScheduler(private val context: Context) {

    fun scheduleDailyNotification(hour: Int, minute: Int) {
        val workManager = WorkManager.getInstance(context)
        val workRequest = PeriodicWorkRequestBuilder<NotificationWorker>(
            repeatInterval = 1, TimeUnit.DAYS
        ).setInitialDelay(calculateInitialDelay(hour, minute), TimeUnit.MILLISECONDS)
            .build()
        workManager.enqueueUniquePeriodicWork(
            "daily_notification",
            ExistingPeriodicWorkPolicy.REPLACE,
            workRequest
        )
    }

    fun cancelNotification() {
        WorkManager.getInstance(context).cancelUniqueWork("daily_notification")
    }

    private fun calculateInitialDelay(hour: Int, minute: Int): Long {
        val now = Calendar.getInstance()
        val alarmTime = Calendar.getInstance().apply {
            set(Calendar.HOUR_OF_DAY, hour)
            set(Calendar.MINUTE, minute)
            set(Calendar.SECOND, 0)
        }
        if (alarmTime.before(now)) {
            alarmTime.add(Calendar.DAY_OF_YEAR, 1)
        }
        return alarmTime.timeInMillis - now.timeInMillis
    }
}

class NotificationWorker(context: Context, params: WorkerParameters) : CoroutineWorker(context, params) {
    override suspend fun doWork(): Result {
        // Показать уведомление
        NotificationHelper.showNotification(
            applicationContext,
            "Time to practice!",
            "Open FlashCards and review your words."
        )
        return Result.success()
    }
}