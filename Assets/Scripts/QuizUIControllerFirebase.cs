using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
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

    [Header("Intro")]
    public string introLine = "Class Begins";
    public float buttonAppearDelay = 1.5f;

    [Header("UI")]
    public GameObject panel;
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

    DatabaseReference db;
    FirebaseAuth auth;

    List<Question> allQuestions = new List<Question>();
    List<Question> quizQuestions = new List<Question>();

    int current = 0;
    int score = 0;
    int earnedPoints = 0;
    bool answered = false;
    bool quizEnded = false;

    void Start()
    {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        HideAnswers();
        ShowIntro();
        LoadQuestionsFromRealtimeDB();
    }

    void ShowIntro()
    {
        CancelInvoke();

        mainText.text = introLine;
        if (feedbackText) feedbackText.text = "";

        startButton.gameObject.SetActive(false);
        Invoke(nameof(ShowStartButton), buttonAppearDelay);
    }

    void ShowStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    void LoadQuestionsFromRealtimeDB()
    {
        db.Child("questions").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || !task.IsCompleted)
            {
                Debug.LogError("Failed to load questions from Realtime DB");
                return;
            }

            DataSnapshot snapshot = task.Result;
            allQuestions.Clear();

            foreach (var child in snapshot.Children)
            {
                Question q = new Question
                {
                    question = child.Child("question").Value.ToString(),
                    answers = child.Child("answers")
                        .Children
                        .Select(a => a.Value.ToString())
                        .ToArray(),
                    correctIndex = int.Parse(child.Child("correctIndex").Value.ToString())
                };

                allQuestions.Add(q);
            }
        });
    }

    public void StartQuiz()
    {
        if (allQuestions.Count < 5)
        {
            Debug.LogError("Not enough questions in database");
            return;
        }

        CancelInvoke();

        startButton.gameObject.SetActive(false);
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

    void ShowQuestion()
    {
        CancelInvoke();
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
            feedbackText.text = correct
                ? "+100 Points!\nCorrect"
                : $"Wrong\nCorrect: {q.answers[q.correctIndex]}";

        foreach (var btn in answerButtons)
            btn.interactable = false;

        Invoke(nameof(NextQuestion), autoNextDelay);
    }

    void NextQuestion()
    {
        current++;
        ShowQuestion();
    }

    void FinishQuiz()
    {
        quizEnded = true;
        HideAnswers();

        mainText.text = "Class Ended";
        if (feedbackText)
            feedbackText.text = $"Score: {score} / 5\nPoints Earned: {earnedPoints}";

        SavePointsToFirebase();
        Invoke(nameof(HidePanel), panelHideDelay);
    }

    void SavePointsToFirebase()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("User not logged in");
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

    public void OnPanelClicked()
    {
        if (!quizEnded) return;
        CancelInvoke();
        HidePanel();
    }
}
