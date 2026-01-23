using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;

public class FirebaseDemoUploader : MonoBehaviour
{
    DatabaseReference dbRef;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                UploadDemoData();
            }
            else
            {
                Debug.LogError("Firebase dependency error");
            }
        });
    }

    void UploadDemoData()
    {
        // ---------- USER DATA ----------
        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "username", "playerOne" },
            { "email", "player1@email.com" },
            { "password", "password_here" },
            { "currentScene", 2 }
        };

        // ---------- PROGRESS DATA ----------
        Dictionary<string, object> progressData = new Dictionary<string, object>
        {
            { "lastScene", 3 },
            { "lastSavedAt", System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") }
        };

        // ---------- QUIZ DATA ----------
        Dictionary<string, object> quizData = new Dictionary<string, object>
        {
            {
                "question", "What are the nutritions that cannot be absorb by the body?"
            },
            {
                "options", new Dictionary<string, string>
                {
                    { "A", "Carbohydrate" },
                    { "B", "Protein" },
                    { "C", "Dietary Fibre" },
                    { "D", "Fats" }
                }
            },
            {
                "correctAnswer", "B"
            }
        };

        // ---------- UPLOAD TO FIREBASE ----------
        dbRef.Child("users").Child("user_001").SetValueAsync(userData);
        dbRef.Child("userProgress").Child("user_001").SetValueAsync(progressData);
        dbRef.Child("quizzes").Child("scene_1").Child("q1").SetValueAsync(quizData);

        Debug.Log("Demo data uploaded to Firebase");
    }
}
