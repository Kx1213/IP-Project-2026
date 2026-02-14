using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class QuizController : MonoBehaviour
{
    [Header("UI")]
    public GameObject startScreen;
    public GameObject quizPanel;
    public Button startButton;

    public TMP_Text questionText;
    public TMP_Text feedbackText;

    public Button[] answerButtons;

    [Header("Settings")]
    public float autoNextDelay = 2f;
    public int questionsPerQuiz = 5;
    public int pointsPerCorrect = 100;

    [Header("Doors")]
    public DoorLockController[] doors;

    [Header("Instruction UI")]
    public InstructionUI instructionUI; // Optional script

    private FirebaseAuth auth;
    private DatabaseReference dbRef;

    private List<Question> allQuestions = new List<Question>();
    private List<Question> selectedQuestions = new List<Question>();

    private int currentQuestionIndex = 0;
    private int score = 0;
    private int earnedPoints = 0;

    private bool quizEnded = false;
    private bool answered = false;

    // =========================
    // 1️⃣ START
    // =========================
    async void Start()
    {
        startScreen.SetActive(true);
        quizPanel.SetActive(false);
        startButton.interactable = false;

        LockAllDoors();

        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            dbRef = FirebaseDatabase.DefaultInstance.RootReference;

            LoadQuestionsFromRealtimeDB();
        }
        else
        {
            Debug.LogError("Firebase not ready: " + dependencyStatus);
        }
    }

    // =========================
    // 2️⃣ LOAD QUESTIONS
    // =========================
    void LoadQuestionsFromRealtimeDB()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("questions")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach (var child in snapshot.Children)
                    {
                        string question = child.Child("question").Value?.ToString();
                        int correctIndex = int.Parse(child.Child("correctIndex").Value.ToString());

                        List<string> answers = new List<string>();
                        foreach (var ans in child.Child("answers").Children)
                        {
                            answers.Add(ans.Value.ToString());
                        }

                        if (!string.IsNullOrEmpty(question) && answers.Count == 4)
                        {
                            allQuestions.Add(new Question(question, answers, correctIndex));
                        }
                    }

                    if (allQuestions.Count >= 5)
                    {
                        startButton.interactable = true;
                    }
                }
            });
    }

    // =========================
    // 3️⃣ START QUIZ
    // =========================
    public void StartQuiz()
    {
        if (allQuestions.Count < questionsPerQuiz)
            return;

        startScreen.SetActive(false);
        quizPanel.SetActive(true);

        score = 0;
        earnedPoints = 0;
        currentQuestionIndex = 0;
        quizEnded = false;

        selectedQuestions.Clear();
        List<Question> temp = new List<Question>(allQuestions);

        for (int i = 0; i < questionsPerQuiz; i++)
        {
            int randomIndex = Random.Range(0, temp.Count);
            selectedQuestions.Add(temp[randomIndex]);
            temp.RemoveAt(randomIndex);
        }

        ShowQuestion();
    }

    // =========================
    // 4️⃣ SHOW QUESTION
    // =========================
    void ShowQuestion()
    {
        if (currentQuestionIndex >= selectedQuestions.Count)
        {
            FinishQuiz();
            return;
        }

        answered = false;
        feedbackText.text = "";

        Question q = selectedQuestions[currentQuestionIndex];
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].interactable = true;

            int index = i;
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => SelectAnswer(index));
        }
    }

    // =========================
    // 5️⃣ SELECT ANSWER
    // =========================
    public void SelectAnswer(int chosen)
    {
        if (answered) return;

        answered = true;

        Question q = selectedQuestions[currentQuestionIndex];

        if (chosen == q.correctIndex)
        {
            score++;
            earnedPoints += pointsPerCorrect;
            feedbackText.text = "Correct!";
        }
        else
        {
            feedbackText.text = "Wrong!";
        }

        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }

        Invoke(nameof(NextQuestion), autoNextDelay);
    }

    // =========================
    // 6️⃣ NEXT QUESTION
    // =========================
    void NextQuestion()
    {
        currentQuestionIndex++;
        ShowQuestion();
    }

    // =========================
    // 7️⃣ FINISH QUIZ
    // =========================
    void FinishQuiz()
    {
        quizEnded = true;
    
        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(false);
        }

        // Display final result in questionText instead
        questionText.text =
            $"QUIZ COMPLETED!\n\n" +
            $"Score: {score}/{questionsPerQuiz}\n" +
            $"Points Earned: {earnedPoints}";

        feedbackText.text = "";

        SavePointsToFirebase();
        UnlockAllDoors();

        if (instructionUI != null)
            instructionUI.OnQuizCompleted();

        Invoke(nameof(HidePanel), 5f);
    }


    void HidePanel()
    {
        quizPanel.SetActive(false);
    }

    // =========================
    // 8️⃣ SAVE POINTS
    // =========================
    void SavePointsToFirebase()
    {
        if (auth.CurrentUser == null)
            return;

        string uid = auth.CurrentUser.UserId;

        DatabaseReference pointsRef = FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(uid)
            .Child("points");

        pointsRef.RunTransaction(mutableData =>
        {
            int currentPoints = 0;

            if (mutableData.Value != null)
                currentPoints = int.Parse(mutableData.Value.ToString());

            mutableData.Value = currentPoints + earnedPoints;
            return TransactionResult.Success(mutableData);
        });
    }

    // =========================
    // DOOR CONTROL
    // =========================
    void LockAllDoors()
    {
        foreach (var door in doors)
        {
            door.LockDoor();
        }
    }

    void UnlockAllDoors()
    {
        foreach (var door in doors)
        {
            door.UnlockDoor();
        }
    }
}

// =========================
// QUESTION CLASS
// =========================
[System.Serializable]
public class Question
{
    public string question;
    public List<string> answers;
    public int correctIndex;

    public Question(string q, List<string> a, int c)
    {
        question = q;
        answers = a;
        correctIndex = c;
    }
}
