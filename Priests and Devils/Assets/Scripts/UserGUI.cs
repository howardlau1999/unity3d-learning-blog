using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction userAction;
    void Start()
    {
        userAction = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 100, 100, 100), "Restart"))
        {
            userAction.Restart();
        }
        FirstController.GameStatus status = userAction.GetGameStatus();
        if (status != FirstController.GameStatus.Playing)
        {
            if (status == FirstController.GameStatus.Lose)
            {
                GUI.Label(new Rect(0, 0, 100, 100), "You LOSE!");
            }
            else
            {
                GUI.Label(new Rect(0, 0, 100, 100), "You WIN!");
            }

            return;
        }
        if (GUI.Button(new Rect(0, 0, 100, 100), "Move Boat"))
        {
            userAction.MoveBoat();
        }
        
        if (GUI.Button(new Rect(100, 0, 100, 100), "Priest to Boat"))
        {
            userAction.PriestToBoat();
        }
        
        if (GUI.Button(new Rect(200, 0, 100, 100), "Devil to Boat"))
        {
            userAction.DevilToBoat();
        }
        
        if (GUI.Button(new Rect(100, 100, 100, 100), "Priest to Bank"))
        {
            userAction.PriestToBank();
        }
        
        if (GUI.Button(new Rect(200, 100, 100, 100), "Devil to Bank"))
        {
            userAction.DevilToBank();
        }
    }
}
