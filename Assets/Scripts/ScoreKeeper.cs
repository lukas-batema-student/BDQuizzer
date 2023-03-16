using static System.Math;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    bool answeredQuestion = false;
    int correctAnswers = 0;
    int questionsSeen = 1;

    public int GetCorrectAnswers()
    {
        return correctAnswers;
    }

    public int GetQuestionsSeen()
    {
        return questionsSeen;
    }

    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }

    public void IncrementQuestionsSeen()
    {
        if (questionsSeen > 0 && answeredQuestion)
        {
            questionsSeen++;
        }
        else if (questionsSeen == 1 && !answeredQuestion)
        {
            answeredQuestion = true;
        }
    }

    public float CalculateCurrentScore()
    {
        return (float) Round((float) (correctAnswers / questionsSeen) * 100, 2);
    }
}
