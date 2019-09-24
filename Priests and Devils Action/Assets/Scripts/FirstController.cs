using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FirstController : MonoBehaviour, ISSActionCallback, ISceneController, IUserAction
{
    private List<GameObject>[] priests, devils, moving;
    private GameObject water, boat, groundLeft, groundRight;
    private Vector3 boatRightPosition = new Vector3(1.5f, .1f, 0f), boatLeftPosition = new Vector3(-1.5f, .1f, 0f);
    private Vector3 groundRightPosition = new Vector3(9f, 0f, 0f), groundLeftPosition = new Vector3(-8f, 0f, 0f);
    private const float boatMoveSpeed = 5f;
    private const float characterMoveSpeed = 10f;
    private const float characterSpacing = 0.5f;
    private Vector3[,] characterPositions;
    private Vector3[] positionOnBoat = {new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f)};
    private GameObject[] passengers;
    private GameStatus status;
    private SSAction boatToLeft, boatToRight, toBoat, toBank;
    private SSActionManager actionManager { get; set; }
    private int seated, boatPos;

    public enum GameStatus
    {
        Win,
        Lose,
        Playing,
        Animating
    }

    private enum BoatPosition
    {
        Left = 0,
        Right = 1,
        Left2Right,
        Right2Left
    }

    private enum Character
    {
        Devil = 0,
        Priest = 1
    }

    BoatPosition boatPosistionState = BoatPosition.Right;

    private void Awake()
    {
        SSDirector.getInstance().currentSceneController = this;
        actionManager = GetComponent<SSActionManager>();
        LoadResources();
    }

    private void Update()
    {
    }


    public void LoadResources()
    {
        priests = new[] {new List<GameObject>(), new List<GameObject>()};
        devils = new[] {new List<GameObject>(), new List<GameObject>()};
        passengers = new GameObject[2];
        boat = Instantiate(Resources.Load<GameObject>("Prefabs/Boat"), boatRightPosition,
            Quaternion.Euler(-90f, -90f, 0f));
        water = Instantiate(Resources.Load<GameObject>("Prefabs/Water"));
        groundRight = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundRightPosition,
            Quaternion.identity);
        groundLeft = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundLeftPosition, Quaternion.identity);
        characterPositions = new Vector3[2, 6];
        
        seated = 0;
        boatPosistionState = BoatPosition.Right;
        status = GameStatus.Playing;

        for (var i = 0; i < 3; ++i)
        {
            GameObject devil = Instantiate(Resources.Load<GameObject>("Prefabs/Devil"),
                characterPositions[(int) BoatPosition.Right, i] =
                    new Vector3(4.75f, .5f, -1f) + new Vector3(0, 0, characterSpacing) * i,
                Quaternion.Euler(0f, -180f, 0f));
            devil.name = "Devil";
            devils[(int) BoatPosition.Right].Add(devil);
        }

        for (var i = 3; i < 6; ++i)
        {
            GameObject priest = Instantiate(Resources.Load<GameObject>("Prefabs/Priest"),
                characterPositions[(int) BoatPosition.Right, i] =
                    new Vector3(4.75f, .5f, -1f) + new Vector3(0, 0, characterSpacing) * i,
                Quaternion.Euler(0f, -180f, 0f));
            priest.name = "Priest";
            priests[(int) BoatPosition.Right].Add(priest);
            characterPositions[(int) BoatPosition.Left, i] =
                Vector3.Scale(characterPositions[(int) BoatPosition.Left, i], new Vector3(-1f, 0f, 0f));
        }

        for (var i = 0; i < 6; ++i)
        {
            characterPositions[(int) BoatPosition.Left, i] =
                Vector3.Scale(characterPositions[(int) BoatPosition.Right, i], new Vector3(-1f, 1f, 1f));
        }
    }

    public void MoveBoat()
    {
        if (seated == 0) return;
        switch (boatPosistionState)
        {
            case BoatPosition.Left:
                boatToRight = CCMoveToAction.GetSSAction(boatRightPosition, boatMoveSpeed);
                status = GameStatus.Animating;
                boatPosistionState = BoatPosition.Left2Right;
                actionManager.RunAction(boat, boatToRight, this);
                break;
            case BoatPosition.Right:
                boatToLeft = CCMoveToAction.GetSSAction(boatLeftPosition, boatMoveSpeed);
                status = GameStatus.Animating;
                boatPosistionState = BoatPosition.Right2Left;
                actionManager.RunAction(boat, boatToLeft, this);
                break;
        }

    }

    private void ToBoat(List<GameObject>[] characters)
    {
        if (seated == 2 || characters[(int) boatPosistionState].Count == 0) return;
        if (boatPosistionState != BoatPosition.Left && boatPosistionState != BoatPosition.Right) return;
        GameObject character = characters[(int) boatPosistionState].Last();
        if (!character) return;
        boatPos = -1;
        for (int i = 0; i < passengers.Length; ++i)
        {
            if (passengers[i] == null)
            {
                boatPos = i;
                break;
            }
        }

        toBoat = CCMoveToAction.GetSSAction(boat.transform.position + positionOnBoat[boatPos],
            characterMoveSpeed);
        status = GameStatus.Animating;
        moving = characters;
        actionManager.RunAction(character, toBoat, this);
    }

    private void ToBank(List<GameObject>[] characters, string characterName)
    {
        if (seated == 0) return;
        if (boatPosistionState != BoatPosition.Left && boatPosistionState != BoatPosition.Right) return;
        GameObject character = null;
        for (int i = 0; i < passengers.Length; ++i)
        {
            if (passengers[i] != null && passengers[i].name == characterName)
            {
                boatPos = i;
                character = passengers[i];
                break;
            }
        }

        if (!character) return;
        Vector3 boatPosition = boat.transform.position;

        Vector3 target = characterPositions[(int) boatPosistionState,
            characters[(int) boatPosistionState].Count + 3 * (characterName == "Priest" ? 1 : 0)];

        toBank = CCMoveToAction.GetSSAction(target, characterMoveSpeed);
        status = GameStatus.Animating;
        moving = characters;
        actionManager.RunAction(character, toBank, this);
    }

    public void Restart()
    {
        Destroy(boat);
        Destroy(water);
        Destroy(groundLeft);
        Destroy(groundRight);

        for (int i = 0; i < passengers.Length; i++)
        {
            passengers[i] = null;
        }

        foreach (List<GameObject> devilsList in devils)
        {
            foreach (GameObject devil in devilsList)
            {
                Object.Destroy(devil);
            }
        }

        foreach (List<GameObject> priestsList in priests)
        {
            foreach (GameObject priest in priestsList)
            {
                Object.Destroy(priest);
            }
        }

        LoadResources();
    }

    public void PriestToBoat()
    {
        ToBoat(priests);
    }

    public void DevilToBoat()
    {
        ToBoat(devils);
    }

    public void PriestToBank()
    {
        ToBank(priests, "Priest");
    }

    public void DevilToBank()
    {
        ToBank(devils, "Devil");
    }

    public void Resume()
    {
        throw new System.NotImplementedException();
    }

    public void SSActionEvent(SSAction action)
    {
        status = GameStatus.Playing;
        if (action == boatToLeft)
        {
            boatPosistionState = BoatPosition.Left;
            CallJudge(Judge.Action.MoveBoat);
        }
        else if (action == boatToRight)
        {
            boatPosistionState = BoatPosition.Right;
            CallJudge(Judge.Action.MoveBoat);
        }
        else if (action == toBoat)
        {
            GameObject character = action.gameObject;
            character.transform.parent = boat.transform;
            passengers[boatPos] = character;
            moving[(int) boatPosistionState].Remove(character);
            moving = null;
            ++seated;
        }
        else if (action == toBank)
        {
            GameObject character = action.gameObject;
            character.transform.parent = null;
            passengers[boatPos] = null;
            moving[(int) boatPosistionState].Add(character);
            moving = null;
            --seated;
            CallJudge(Judge.Action.ToBank);
        }
    }

    public void GameOver(Judge.GameResult result)
    {
        switch (result)
        {
            case Judge.GameResult.Lose:
                status = GameStatus.Lose;
                break;
            case Judge.GameResult.Win:
                status = GameStatus.Win;
                break;
        }
    }

    private void CallJudge(Judge.Action action)
    {
        if (status != GameStatus.Playing) return;
        Judge.JudgeGame(action, priests[(int) BoatPosition.Left].Count, devils[(int) BoatPosition.Left].Count,
            priests[(int) BoatPosition.Right].Count, devils[(int) BoatPosition.Right].Count);
    }


    public GameStatus GetGameStatus()
    {
        return status;
    }

    public void Pause()
    {
        throw new System.NotImplementedException();
    }
}