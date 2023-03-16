using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace BDQuizzer
{
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

        bool isComplete = false;

        void Start()
        {
            timer = FindObjectOfType<Timer>();
            scoreKeeper = FindObjectOfType<ScoreKeeper>();
            progressBar.maxValue = questions.Count;
            progressBar.value = 0;

            GetNextQuestion();
        }

        void Update()
        {
            timer.UpdateTimer();

            if (timer.loadNextQuestion)
            {
                hasAnsweredEarly = false;
                GetNextQuestion();
                timer.loadNextQuestion = false;
            }
            else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
            {
                int i = 0;
                while (i != currentQuestion.GetCorrectAnswerIndex())
                {
                    i++;
                }
                DisplayAnswer(i);
                SetButtonState(false);
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
            if (!isComplete)
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
        }

        void GetNextQuestion()
        {
            if (questions.Count > 0 && questions.Count < 5)
            {
                SetButtonState(true);
                SetDefaultButtonSprites();
                GetRandomQuestion();
                DisplayQuestion();
                progressBar.value++;
                scoreKeeper.IncrementQuestionsSeen();
            }
            else if (questions.Count == 0)
            {
                if (scoreKeeper.CalculateCurrentScore() == 100)
                {
                    questionText.text = $"You have completed the quiz! Your score was {scoreKeeper.CalculateCurrentScore()}%. You passed, but how do you know this much? Did you stalk me? As a precaution, this will count as a faliure for the reason of stalking me >:)";
                }
                else
                {
                    questionText.text = $"You have completed the quiz! Your score was {scoreKeeper.CalculateCurrentScore()}%. You failed, as you don't know much about me >:)";
                }
            }
            else if (questions.Count == 5)
            {
                timer.isAnsweringQuestion = false;

                timer.CancelTimer();
                timer.CancelTimer();

                SetDefaultButtonSprites();
                GetRandomQuestion();
                DisplayQuestion();
            }
        }

        void GetRandomQuestion()
        {
            if (questions.Contains(currentQuestion) && questions.Count < 5)
            {
                int index = Random.Range(0, questions.Count);
                currentQuestion = questions[index];

                questions.Remove(currentQuestion);
            }
            else if (questions.Count == 5)
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
}
