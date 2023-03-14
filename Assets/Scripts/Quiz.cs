using UnityEngine;
using TMPro;
using System;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    TextMeshProUGUI answer;
    [SerializeField] GameObject[] answerButtons;

    void Start()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < 4; i++) {
            answer.text = (string)answerButtons.GetValue(i);
        }
    }
}
