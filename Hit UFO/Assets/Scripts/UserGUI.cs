using System;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private Controller controller;

    private void Start()
    {
        controller = Director.Instance.currentController as Controller;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Next Round"))
        {
            controller.NextRound();
        }

        if (GUI.Button(new Rect(100, 0, 100, 100), "Next Trial"))
        {
            controller.NextTrial();
        }
        
        GUI.Label(new Rect(0, 100, 200, 100),
            "Round: " + controller.currentRound + ", Trial: " + controller.currentTrial);
        
        GUI.Label(new Rect(0, 130, 200, 100),
            "Score: " + controller.score);
    }
}