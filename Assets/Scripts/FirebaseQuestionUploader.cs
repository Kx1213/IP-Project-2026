using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;

public class RealtimeQuestionUploader : MonoBehaviour
{
    DatabaseReference db;

    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        UploadQuestions();
    }

    void UploadQuestions()
    {
        Dictionary<string, object> questions = new Dictionary<string, object>();

        questions["q1"] = new Dictionary<string, object>
        {
            { "question", "What is the main function of carbohydrates?" },
            { "answers", new string[] { "Build muscles", "Main source of energy", "Protect organs", "Heal wounds" } },
            { "correctIndex", 1 }
        };

        questions["q2"] = new Dictionary<string, object>
        {
            { "question", "Which of the following is a simple carbohydrate?" },
            { "answers", new string[] { "Rice", "Potatoes", "Honey", "Bread" } },
            { "correctIndex", 2 }
        };

        questions["q3"] = new Dictionary<string, object>
        {
            { "question", "Which food is a complex carbohydrate?" },
            { "answers", new string[] { "Sugar", "Maple syrup", "Rice", "Honey" } },
            { "correctIndex", 2 }
        };

        questions["q4"] = new Dictionary<string, object>
        {
            { "question", "What is one function of proteins?" },
            { "answers", new string[] { "Maintain eyesight", "Growth and repair of body cells", "Prevent constipation", "Regulate temperature" } },
            { "correctIndex", 1 }
        };

        questions["q5"] = new Dictionary<string, object>
        {
            { "question", "Which is a plant source of protein?" },
            { "answers", new string[] { "Fish", "Milk", "Beans", "Egg" } },
            { "correctIndex", 2 }
        };

        questions["q6"] = new Dictionary<string, object>
        {
            { "question", "When does the body use protein as a source of energy?" },
            { "answers", new string[] { "When vitamins are lacking", "When carbohydrates and fats are lacking", "After exercise", "During sleep" } },
            { "correctIndex", 1 }
        };

        questions["q7"] = new Dictionary<string, object>
        {
            { "question", "Which is a function of fats?" },
            { "answers", new string[] { "Heal wounds", "Help absorb iron", "Keep the body warm", "Release energy from food" } },
            { "correctIndex", 2 }
        };

        questions["q8"] = new Dictionary<string, object>
        {
            { "question", "Which is an example of animal fat?" },
            { "answers", new string[] { "Olive oil", "Sunflower oil", "Butter", "Canola oil" } },
            { "correctIndex", 2 }
        };

        questions["q9"] = new Dictionary<string, object>
        {
            { "question", "Why should fat be eaten in moderation?" },
            { "answers", new string[] { "It has no nutrients", "Too much fat is bad for health", "It causes dehydration", "It prevents digestion" } },
            { "correctIndex", 1 }
        };

        questions["q10"] = new Dictionary<string, object>
        {
            { "question", "Which vitamin helps maintain good eyesight?" },
            { "answers", new string[] { "Vitamin B", "Vitamin C", "Vitamin D", "Vitamin A" } },
            { "correctIndex", 3 }
        };

        questions["q11"] = new Dictionary<string, object>
        {
            { "question", "Which food is rich in Vitamin A?" },
            { "answers", new string[] { "Oranges", "Carrots", "Wholegrains", "Fish" } },
            { "correctIndex", 1 }
        };

        questions["q12"] = new Dictionary<string, object>
        {
            { "question", "What is the main function of Vitamin B group?" },
            { "answers", new string[] { "Heal wounds", "Release energy from food", "Build bones", "Maintain fluid balance" } },
            { "correctIndex", 1 }
        };

        questions["q13"] = new Dictionary<string, object>
        {
            { "question", "Which vitamin helps the body absorb calcium?" },
            { "answers", new string[] { "Vitamin A", "Vitamin B", "Vitamin C", "Vitamin D" } },
            { "correctIndex", 3 }
        };

        questions["q14"] = new Dictionary<string, object>
        {
            { "question", "Which mineral helps build strong bones and teeth?" },
            { "answers", new string[] { "Iron", "Sodium", "Calcium", "Vitamin D" } },
            { "correctIndex", 2 }
        };

        questions["q15"] = new Dictionary<string, object>
        {
            { "question", "Which mineral keeps red blood cells healthy?" },
            { "answers", new string[] { "Iron", "Calcium", "Sodium", "Potassium" } },
            { "correctIndex", 0 }
        };

        questions["q16"] = new Dictionary<string, object>
        {
            { "question", "What is one function of water in the body?" },
            { "answers", new string[] { "Build muscles", "Regulate body temperature", "Provide vitamins", "Strengthen bones" } },
            { "correctIndex", 1 }
        };

        questions["q17"] = new Dictionary<string, object>
        {
            { "question", "Which is a source of water?" },
            { "answers", new string[] { "Bread", "Cheese", "Watermelon", "Butter" } },
            { "correctIndex", 2 }
        };

        questions["q18"] = new Dictionary<string, object>
        {
            { "question", "What is the main function of dietary fibre?" },
            { "answers", new string[] { "Provide energy", "Aids digestion", "Maintain eyesight", "Build muscles" } },
            { "correctIndex", 1 }
        };

        questions["q19"] = new Dictionary<string, object>
        {
            { "question", "Which food is high in dietary fibre?" },
            { "answers", new string[] { "Wholegrains", "Butter", "Milk", "Eggs" } },
            { "correctIndex", 0 }
        };

        questions["q20"] = new Dictionary<string, object>
        {
            { "question", "What causes coronary heart disease?" },
            { "answers", new string[] { "Low sugar intake", "Blocked blood vessels with fatty deposits", "Lack of water", "Vitamin deficiency" } },
            { "correctIndex", 1 }
        };

        questions["q21"] = new Dictionary<string, object>
        {
            { "question", "Which habit increases the risk of high blood pressure?" },
            { "answers", new string[] { "Eating fruits", "Exercising regularly", "Eating too much salty food", "Drinking water" } },
            { "correctIndex", 2 }
        };

        questions["q22"] = new Dictionary<string, object>
        {
            { "question", "What is obesity?" },
            { "answers", new string[] { "Low body weight", "Excessive accumulation of body fat", "Lack of vitamins", "Low blood sugar" } },
            { "correctIndex", 1 }
        };

        questions["q23"] = new Dictionary<string, object>
        {
            { "question", "Which action helps reduce the risk of obesity?" },
            { "answers", new string[] { "Eating more sugary food", "Skipping meals", "Exercising regularly", "Avoiding fruits" } },
            { "correctIndex", 2 }
        };

        questions["q24"] = new Dictionary<string, object>
        {
            { "question", "What is a cause of type 2 diabetes?" },
            { "answers", new string[] { "Excess intake of sugary food", "Low fat intake", "Drinking water", "Eating vegetables" } },
            { "correctIndex", 0 }
        };

        questions["q25"] = new Dictionary<string, object>
        {
            { "question", "Which practice helps prevent food contamination?" },
            { "answers", new string[] { "Leaving food uncovered", "Mixing raw and cooked food", "Cooking food properly", "Storing food in warm places" } },
            { "correctIndex", 2 }
        };

        db.Child("questions").SetValueAsync(questions);
        Debug.Log("25 nutrition questions uploaded to Realtime Database");
    }
}
