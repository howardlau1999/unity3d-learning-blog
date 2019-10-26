using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    public delegate void EscapeEvent(GameObject patrol);

    public static event EscapeEvent OnGoalLost;

    public delegate void FollowEvent(GameObject patrol);

    public static event FollowEvent OnFollowing;

    public delegate void GameOverEvent();

    public static event GameOverEvent GameOver;

    public delegate void WinEvent();

    public static event WinEvent Win;


    public void PlayerEscape(GameObject patrol)
    {
        OnGoalLost?.Invoke(patrol);
    }


    public void FollowPlayer(GameObject patrol)
    {
        OnFollowing?.Invoke(patrol);
    }


    public void OnPlayerCaught()
    {
        GameOver?.Invoke();
    }


    public void TimeUp()
    {
        Win?.Invoke();
    }
}