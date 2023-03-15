using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    [SerializeField] QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    bool hasAnsweredEarly;

    [Header("Buttons Colours")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;

    readonly static List<QuestionSO> readOnlyQuestions = new List<QuestionSO>();

    public bool isComplete;

    bool setupComplete = false;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count - 1;
        progressBar.value = 0;
    }

    void Update()
    {
        if (!setupComplete)
        {
            hasAnsweredEarly = true;
            GetNextQuestion();
            timer.loadNextQuestion = true;

            setupComplete = true;
        }
        else
        {
            if (timer.loadNextQuestion)
            {
                hasAnsweredEarly = false;
                GetNextQuestion();
                timer.loadNextQuestion = false;
            }
            else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
            {
                DisplayAnswer(-1);
                SetButtonState(false);
            }
        }
    }

    public void OnAnswerSelected(int index)
    {
        DisplayAnswer(index);
        SetButtonState(false);
        hasAnsweredEarly = true;
        timer.CancelTimer();
        scoreText.text = $"Score: {scoreKeeper.CalculateCurrentScore()}%";

        if (progressBar.value == progressBar.maxValue)
        {
            isComplete = true;
        }
    }

    void DisplayAnswer(int index)
    {
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "You are one step closer to becoming a true BatemaDevelopment historian!";
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "You aren't a true BatemaDevelopment historian!" +
                "\n" +
                $"Answer: {currentQuestion.GetAnswer(currentQuestion.GetCorrectAnswerIndex())}";
        }

        answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>().sprite = correctAnswerSprite;
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }
}
