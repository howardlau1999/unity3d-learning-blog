using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
    void LoadResources();
    GameState GetGameState();
    int GetScore();
}
