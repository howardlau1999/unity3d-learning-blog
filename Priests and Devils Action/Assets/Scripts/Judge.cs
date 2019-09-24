using UnityEngine;

public class Judge
{
    public enum GameResult
    {
        Win,
        Lose
    }

    public enum Action
    {
        MoveBoat,
        ToBank
    }

    public static void JudgeGame(Action action, int priestsOnLeft, int devilsOnLeft, int priestsOnRight,
        int devilsOnRight)
    {
        FirstController controller = SSDirector.getInstance().currentSceneController as FirstController;
        switch (action)
        {
            case Action.ToBank:
                if (priestsOnLeft == 3 && devilsOnLeft == 3)
                    controller.GameOver(GameResult.Win);
                break;
            case Action.MoveBoat:
                if (priestsOnLeft != 0 && priestsOnLeft < devilsOnLeft ||
                    priestsOnRight != 0 && priestsOnRight < devilsOnRight)
                    controller.GameOver(GameResult.Lose);
                break;
        }
    }
}