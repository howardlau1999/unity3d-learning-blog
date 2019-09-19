using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void Restart();
    void MoveBoat();
    void PriestToBoat();
    void DevilToBoat();
    void PriestToBank();
    void DevilToBank();
    FirstController.GameStatus GetGameStatus();
}
