using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseTest : MonoBehaviour
{
    void Start()
    {
        // --- Debug info for troubleshooting native library issues ---
        Debug.Log("Unity Platform: " + Application.platform);
        Debug.Log("Processor Type: " + SystemInfo.processorType);
        Debug.Log("Device Model: " + SystemInfo.deviceModel);
        Debug.Log("Operating System: " + SystemInfo.operatingSystem);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready!");

                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

                // send test message to check if it's linked properly
                reference.Child("testMessage").SetValueAsync("Firebase connected!")
                    .ContinueWithOnMainThread(writeTask =>
                    {
                        if (writeTask.IsCompleted)
                        {
                            Debug.Log("Successfully wrote test message to database!");
                        }
                        else
                        {
                            Debug.LogError("Failed to write message: " + writeTask.Exception);
                        }
                    });
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + dependencyStatus);
            }
        });
    }
}
