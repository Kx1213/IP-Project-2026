using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;

public class RealtimeExtraQuestionUploader : MonoBehaviour
{
    DatabaseReference db;

    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        UploadExtraQuestions();
    }

    void UploadExtraQuestions()
    {
        List<Dictionary<string, object>> extraQuestions = new List<Dictionary<string, object>>();

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which carbohydrate releases energy more slowly?" },
            { "answers", new string[] { "Honey", "Sugar", "Rice", "Maple syrup" } },
            { "correctIndex", 2 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which of the following is NOT a plant source of protein?" },
            { "answers", new string[] { "Beans", "Lentils", "Egg", "Nuts" } },
            { "correctIndex", 2 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which fat helps protect internal organs?" },
            { "answers", new string[] { "Butter", "Body fat", "Sugar", "Salt" } },
            { "correctIndex", 1 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which vitamin helps heal wounds and absorb iron?" },
            { "answers", new string[] { "Vitamin A", "Vitamin B", "Vitamin C", "Vitamin D" } },
            { "correctIndex", 2 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which mineral helps control muscle contractions?" },
            { "answers", new string[] { "Iron", "Calcium", "Sodium", "Vitamin D" } },
            { "correctIndex", 1 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which food is high in sodium chloride?" },
            { "answers", new string[] { "Fresh fruit", "Milk", "Salted nuts", "Rice" } },
            { "correctIndex", 2 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which is NOT a function of water?" },
            { "answers", new string[] { "Regulate body temperature", "Remove waste", "Build muscles", "Produce sweat" } },
            { "correctIndex", 2 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which food helps prevent constipation?" },
            { "answers", new string[] { "Butter", "Dietary fibre", "Sugar", "Salt" } },
            { "correctIndex", 1 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which habit helps reduce the risk of coronary heart disease?" },
            { "answers", new string[] { "Eating more fatty food", "Using olive oil", "Skipping exercise", "Eating salty snacks" } },
            { "correctIndex", 1 }
        });

        extraQuestions.Add(new Dictionary<string, object>
        {
            { "question", "Which practice helps prevent food contamination?" },
            { "answers", new string[] { "Keeping food uncovered", "Mixing raw and cooked food", "Cleaning equipment thoroughly", "Storing food in warm places" } },
            { "correctIndex", 2 }
        });

        // Push questions so existing ones will not be overwritten
        foreach (var q in extraQuestions)
        {
            db.Child("questions").Push().SetValueAsync(q);
        }

        Debug.Log("10 extra nutrition questions pushed to Firebase");
    }
}
