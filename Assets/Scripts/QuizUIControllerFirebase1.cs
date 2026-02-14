using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;

public class QuizUIController : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] answers;
        public int correctIndex;
    }

    [Header("Start Screen")]
    public GameObject startScreen;

    [Header("Quiz Panel")]
    public GameObject panel;

    [Header("UI")]
    public TMP_Text mainText;
    public TMP_Text feedbackText;
    public Button startButton;
    public Button[] answerButtons;

    [Header("Behaviour")]
    public float autoNextDelay = 1.0f;

    [Header("Outro")]
    public float panelHideDelay = 2.0f;

    [Header("Points")]
    public int pointsPerCorrect = 100;

    [Header("Doors unlocked after quiz")]
    public DoorLockController[] doorsToUnlock;

    [Header("Instruction UI")]
    public InstructionUI instructionUI;

    DatabaseReference db;
    FirebaseAuth auth;

    List<Question> allQuestions = new List<Question>();
    List<Question> quizQuestions = new List<Question>();

    int current = 0;
    int score = 0;
    int earnedPoints = 0;
    bool answered = false;
    bool quizEnded = false;
    bool questionsLoaded = false;

    // =========================
    // SAFE FIREBASE INIT (FIX)
    // =========================
    async void Start()
    {
        // UI setup first
        startScreen.SetActive(true);
        panel.SetActive(false);
        HideAnswers();
        startButton.interactable = false;

        // Lock doors at start
        if (doorsToUnlock != null)
        {
            foreach (var door in doorsToUnlock)
            {
                if (door)
                    door.LockDoor();
            }
        }

        Debug.Log("Checking Firebase dependencies...");

        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == DependencyStatus.Available)
        {
            Debug.Log("Firebase initialized successfully");

            auth = FirebaseAuth.DefaultInstance;
            db = FirebaseDatabase.DefaultInstance.RootReference;

            LoadQuestionsFromRealtimeDB();
        }
        else
        {
            Debug.LogError("Firebase NOT ready: " + dependencyStatus);
        }
    }

    // =======================
    // LOAD QUESTIONS
    // =======================

    void LoadQuestionsFromRealtimeDB()
    {
        Debug.Log("Loading questions from Firebase...");

        db.Child("questions").GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("DB Load Faulted: " + task.Exception);
                return;
            }

            if (task.IsCanceled)
            {
                Debug.LogError("DB Load Canceled");
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
            {
                Debug.LogError("No questions node found in DB!");
                return;
            }

            allQuestions.Clear();

            foreach (var child in snapshot.Children)
            {
                if (child.Child("question").Value == null)
                    continue;

                Question q = new Question
                {
                    question = child.Child("question").Value.ToString(),
                    answers = child.Child("answers")
                        .Children
                        .Select(a => a.Value.ToString())
                        .ToArray(),
                    correctIndex = int.Parse(child.Child("correctIndex").Value.ToString())
                };

                if (q.answers.Length >= 4)
                    allQuestions.Add(q);
            }

            Debug.Log("Questions loaded: " + allQuestions.Count);

            if (allQuestions.Count >= 5)
            {
                questionsLoaded = true;
                startButton.interactable = true;
            }
            else
            {
                Debug.LogError("Database has less than 5 valid questions.");
            }
        });
    }

    // =======================
    // START QUIZ
    // =======================

    public void StartQuiz()
    {
        if (!questionsLoaded)
        {
            Debug.Log("Questions still loading...");
            return;
        }

        startScreen.SetActive(false);
        panel.SetActive(true);
        startButton.interactable = false;

        score = 0;
        earnedPoints = 0;
        current = 0;
        quizEnded = false;

        quizQuestions = allQuestions
            .OrderBy(x => Random.value)
            .Take(5)
            .ToList();

        ShowQuestion();
    }

    // =======================
    // QUESTION DISPLAY
    // =======================

    void ShowQuestion()
    {
        answered = false;

        if (current >= quizQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        var q = quizQuestions[current];
        mainText.text = q.question;

        if (feedbackText) feedbackText.text = "";

        for (int i = 0; i < 4; i++)
        {
            int index = i;

            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].interactable = true;

            answerButtons[i]
                .GetComponentInChildren<TMP_Text>()
                .text = $"{(char)('A' + i)}. {q.answers[i]}";

            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => SelectAnswer(index));
        }
    }

    void SelectAnswer(int chosen)
    {
        if (answered) return;
        answered = true;

        var q = quizQuestions[current];
        bool correct = chosen == q.correctIndex;

        if (correct)
        {
            score++;
            earnedPoints += pointsPerCorrect;
        }

        if (feedbackText)
        {
            feedbackText.text = correct
                ? $"+{pointsPerCorrect} Points!\nCorrect"
                : $"Wrong\nCorrect: {q.answers[q.correctIndex]}";
        }

        foreach (var btn in answerButtons)
            btn.interactable = false;

        Invoke(nameof(NextQuestion), autoNextDelay);
    }

    void NextQuestion()
    {
        current++;
        ShowQuestion();
    }

    // =======================
    // FINISH
    // =======================

    void FinishQuiz()
    {
        quizEnded = true;
        HideAnswers();

        mainText.text = "Class Ended";

        if (feedbackText)
            feedbackText.text = $"Score: {score} / 5\nPoints Earned: {earnedPoints}";

        SavePointsToFirebase();

        if (doorsToUnlock != null)
        {
            foreach (var door in doorsToUnlock)
            {
                if (door)
                    door.UnlockDoor();
            }
        }

        Invoke(nameof(HidePanel), panelHideDelay);

        if (instructionUI)
            instructionUI.OnQuizCompleted();
    }

    void SavePointsToFirebase()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("User not logged in, cannot save points");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        DatabaseReference pointsRef = db.Child("users").Child(uid).Child("points");

        pointsRef.RunTransaction(mutableData =>
        {
            int currentPoints = 0;

            if (mutableData.Value != null)
                int.TryParse(mutableData.Value.ToString(), out currentPoints);

            mutableData.Value = currentPoints + earnedPoints;
            return TransactionResult.Success(mutableData);
        });
    }

    void HidePanel()
    {
        panel.SetActive(false);
    }

    void HideAnswers()
    {
        foreach (var btn in answerButtons)
            btn.gameObject.SetActive(false);
    }
}
