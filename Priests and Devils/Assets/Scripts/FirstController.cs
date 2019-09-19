using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    private List<GameObject>[] priests, devils;
    private GameObject water, boat, groundLeft, groundRight;
    private Vector3 boatRightPosition = new Vector3(1.5f, .1f, 0f), boatLeftPosition = new Vector3(-1.5f, .1f, 0f);
    private Vector3 groundRightPosition = new Vector3(9f, 0f, 0f), groundLeftPosition = new Vector3(-8f, 0f, 0f);
    private const float boatMoveSpeed = 2f;
    private const float characterSpacing = 0.5f;
    private Vector3[,] characterPositions;
    private Vector3[] positionOnBoat = {new Vector3(0f, -2f, 0f), new Vector3(0f, 5f, 0f)};
    private List<GameObject> passengers;
    private GameStatus status;
    public enum GameStatus
    {
        Win, Lose, Playing
    }
    private enum BoatPosition
    {
        Left = 0, Right = 1, Left2Right, Right2Left
    }

    private enum Character
    {
        Devil = 0, Priest = 1
    }

    BoatPosition boatPosistionState = BoatPosition.Right;

    private void Awake()
    {
        SSDirector.getInstance().currentSceneController = this;
        LoadResources();
    }
    private void Update()
    {
        switch (boatPosistionState)
        {
            case BoatPosition.Left2Right:
                boat.transform.position = Vector3.MoveTowards(boat.transform.position, boatRightPosition, boatMoveSpeed * Time.deltaTime);
                break;
            case BoatPosition.Right2Left:
                boat.transform.position = Vector3.MoveTowards(boat.transform.position, boatLeftPosition, boatMoveSpeed * Time.deltaTime);
                break;
        }

        if (boat.transform.position == boatRightPosition)
        {
            this.boatPosistionState = BoatPosition.Right;
        } else if (boat.transform.position == boatLeftPosition)
        {
            this.boatPosistionState = BoatPosition.Left;
        }
    }

    public void LoadResources()
    {
        priests = new List<GameObject>[2] {new List<GameObject>(), new List<GameObject>()};
        devils = new List<GameObject>[2] {new List<GameObject>(), new List<GameObject>()};
        passengers = new List<GameObject>();
        boat = Instantiate(Resources.Load<GameObject>("Prefabs/Boat"), boatRightPosition, Quaternion.Euler(-90f, -90f, 0f));
        water = Instantiate(Resources.Load<GameObject>("Prefabs/Water"));
        groundRight = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundRightPosition, Quaternion.identity);
        groundLeft = Instantiate(Resources.Load<GameObject>("Prefabs/Ground"), groundLeftPosition, Quaternion.identity);
        characterPositions = new Vector3[2, 6];

        status = GameStatus.Playing;
        
        for (var i = 0; i < 3; ++i)
        {
            GameObject devil = Instantiate(Resources.Load<GameObject>("Prefabs/Devil"),
                characterPositions[(int) BoatPosition.Right, i] = new Vector3(4.75f, .5f, -1f) + new Vector3(0, 0, characterSpacing) * i,
                Quaternion.Euler(0f, -180f, 0f));
            devil.name = "Devil";
            devils[(int) BoatPosition.Right].Add(devil);
        }
        
        for (var i = 3; i < 6; ++i)
        {
            GameObject priest = Instantiate(Resources.Load<GameObject>("Prefabs/Priest"),
                characterPositions[(int) BoatPosition.Right, i] = new Vector3(4.75f, .5f, -1f) + new Vector3(0, 0, characterSpacing) * i,
                Quaternion.Euler(0f, -180f, 0f));
            priest.name = "Priest";
            priests[(int) BoatPosition.Right].Add(priest);
            characterPositions[(int) BoatPosition.Left, i] = Vector3.Scale(characterPositions[(int) BoatPosition.Left, i], new Vector3(-1f, 0f, 0f));
        }

        for (var i = 0; i < 6; ++i)
        {
            characterPositions[(int) BoatPosition.Left, i] =
                Vector3.Scale(characterPositions[(int) BoatPosition.Right, i], new Vector3(-1f, 1f, 1f));
        }
    }
    
    public void MoveBoat()
    {
        if (passengers.Count == 0) return;
        switch (boatPosistionState)
        {
            case BoatPosition.Left:
                boatPosistionState = BoatPosition.Left2Right;
                break;
            case BoatPosition.Right:
                boatPosistionState = BoatPosition.Right2Left;
                break;
        }
        JudgeLose();
    }

    private void ToBoat(List<GameObject>[] characters)
    {
        if (passengers.Count == 2 || characters[(int) boatPosistionState].Count == 0) return;
        if (boatPosistionState != BoatPosition.Left && boatPosistionState != BoatPosition.Right) return;
        GameObject character = characters[(int) boatPosistionState].Last();
        if (!character) return;
        character.transform.parent = boat.transform;
        character.transform.localPosition = positionOnBoat[passengers.Count];
        passengers.Add(character);
        characters[(int) boatPosistionState].Remove(character);
    }
    
    private void ToBank(List<GameObject>[] characters, string characterName)
    {
        if (passengers.Count == 0) return;
        if (boatPosistionState != BoatPosition.Left && boatPosistionState != BoatPosition.Right) return;
        GameObject character = passengers.FindLast(passenger => passenger.name == characterName);
        if (!character) return;
        character.transform.parent = null;
        character.transform.localPosition = characterPositions[(int) boatPosistionState,
            characters[(int) boatPosistionState].Count + 3 * (characterName == "Priest" ? 1 : 0)];
        passengers.Remove(character);
        characters[(int) boatPosistionState].Add(character);
        
        JudgeWin();
    }

    public void Restart()
    {
        Destroy(boat);
        Destroy(water);
        Destroy(groundLeft);
        Destroy(groundRight);
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

    
    
    private void JudgeLose()
    {
        if (status != GameStatus.Playing) return;
        for (int side = 0; side <= 1; ++side)
        {
            int nPriest = priests[side].Count;
            int nDevil = devils[side].Count;
            if (nPriest > 0 && nDevil > nPriest)
            {
                Debug.Log("Devils out number priests on side " + side);
                status = GameStatus.Lose;
                return;
            }
        }
    }
    
    private void JudgeWin()
    {
        if (status != GameStatus.Playing) return;
        if (passengers.Count == 0 && priests[(int) BoatPosition.Left].Count == 3 &&
            devils[(int) BoatPosition.Left].Count == 3)
        {
            Debug.Log("You win");
            status = GameStatus.Win;
        }
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
