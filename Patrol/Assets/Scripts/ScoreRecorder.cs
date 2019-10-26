using UnityEngine;

public class ScoreRecorder : Singleton<ScoreRecorder>
{
    public int score;

    void Start()
    {
        score = 0;
    }

    public void Score()
    {
        score++;
    }

    public void Reset()
    {
        score = 0;
    }
}