using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizUIController : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] answers; // MUST be size 4
        public int correctIndex; // 0-3
    }

    [Header("Intro")]
    public string introLine = "Class Begins";
    public float buttonAppearDelay = 1.5f;

    [Header("UI")]
    public TMP_Text mainText;
    public TMP_Text feedbackText;
    public Button startButton;

    [Header("Multiple Choice Buttons (4)")]
    public Button[] answerButtons = new Button[4];

    [Header("Behaviour")]
    public bool autoNext = true;
    public float autoNextDelay = 1.0f;

    Question[] questions;
    int current = 0;
    int score = 0;
    bool answered = false;

    void Start()
    {
        BuildQuestions();
        ShowIntro();
        HideAnswers();
    }

    void BuildQuestions()
    {
        questions = new Question[]
        {
            new Question{ question="What is 2 + 2 ?", answers=new[]{"3","4","5","6"}, correctIndex=1 },
            new Question{ question="What is 5 - 3 ?", answers=new[]{"1","2","3","4"}, correctIndex=1 },
            new Question{ question="What is 3 × 3 ?", answers=new[]{"6","8","9","12"}, correctIndex=2 },
            new Question{ question="What is 10 ÷ 2 ?", answers=new[]{"2","4","5","6"}, correctIndex=2 },
            new Question{ question="What is 7 + 1 ?", answers=new[]{"6","7","8","9"}, correctIndex=2 },
        };
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

    public void StartQuiz()
    {
        startButton.gameObject.SetActive(false);
        score = 0;
        current = 0;
        ShowQuestion();
    }

    void ShowQuestion()
    {
        CancelInvoke();
        answered = false;

        if (current >= questions.Length)
        {
            FinishQuiz();
            return;
        }

        var q = questions[current];
        mainText.text = q.question;
        if (feedbackText) feedbackText.text = "";

        for (int i = 0; i < 4; i++)
        {
            int captured = i;

            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].interactable = true;

            // Set label text
            var label = answerButtons[i].GetComponentInChildren<TMP_Text>();
            char letter = (char)('A' + i);
            label.text = $"{letter}. {q.answers[i]}";

            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => SelectAnswer(captured));
        }
    }

    void SelectAnswer(int chosen)
    {
        if (answered) return;
        answered = true;

        var q = questions[current];
        bool correct = chosen == q.correctIndex;
        if (correct) score++;

        if (feedbackText)
            feedbackText.text = correct ? "Correct" : $"Wrong\nCorrect: {q.answers[q.correctIndex]}";

        for (int i = 0; i < 4; i++)
            answerButtons[i].interactable = false;

        if (autoNext)
            Invoke(nameof(NextQuestion), autoNextDelay);
    }

    public void NextQuestion()
    {
        current++;
        ShowQuestion();
    }

    void FinishQuiz()
    {
        HideAnswers();
        mainText.text = "Class Ended";
        if (feedbackText) feedbackText.text = $"Score: {score} / {questions.Length}";
    }

    void HideAnswers()
    {
        for (int i = 0; i < 4; i++)
            if (answerButtons[i] != null)
                answerButtons[i].gameObject.SetActive(false);
    }
}
