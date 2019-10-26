using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEventType : int
{
    Started,
    Completed
}

public enum GameState
{
    Running,
    Pause,
    Start,
    Lose,
    Win
}

public class Controller : MonoBehaviour, ISceneController, IUserAction
{
    public GameObject player;
    public int playerRegion;
    private GameState gameState;
    public PatrolActionManager patrolActionManager;
    public ScoreRecorder scoreRecorder;
    public PatrolFactory patrolFactory;
    private List<GameObject> patrols;
    private void Start()
    {
        SSDirector.Instance.currentSceneController = this;
        patrolFactory = PatrolFactory.Instance;
        scoreRecorder = ScoreRecorder.Instance;
        patrolActionManager =  gameObject.AddComponent<PatrolActionManager>();
        patrols = new List<GameObject>();
        LoadResources();
        
        for (int i = 0; i < patrols.Count; i++) {
            patrolActionManager.Patrol(patrols[i]);
        }
    }

    public void LoadResources()
    {
        float[] pos_x = {5.5f, -4.3f, -14f};
        float[] pos_z = {3.3f, -6.6f, -16.5f};
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject patrol = patrolFactory.GetPatrol();
                patrol.transform.position = new Vector3(pos_x[j], 0, pos_z[i]);
                patrol.GetComponent<PatrolData>().patrolRegion = i * 3 + j;
                patrol.GetComponent<PatrolData>().playerRegion = 4;
                patrol.GetComponent<PatrolData>().isPlayerInRange = false;
                patrol.GetComponent<PatrolData>().isFollowing = false;
                patrol.GetComponent<PatrolData>().isCollided = false;
                patrol.GetComponent<PatrolData>().isLost = false;
                patrols.Add(patrol);
            }
        }
    }

    public void MovePlayer(float translationX, float translationZ)
    {
        if (Math.Abs(translationX) > 0 || Math.Abs(translationZ) > 0)
        {
            player.GetComponent<Animator>().SetBool("Running", true);
        }
        else
        {
            player.GetComponent<Animator>().SetBool("Running", false);
        }

        translationX *= Time.deltaTime;
        translationZ *= Time.deltaTime;

        player.transform.LookAt(new Vector3(player.transform.position.x + translationX, player.transform.position.y,
            player.transform.position.z + translationZ));
        if (translationX == 0)
            player.transform.Translate(0, 0, Mathf.Abs(translationZ) * 2);
        else if (translationZ == 0)
            player.transform.Translate(0, 0, Mathf.Abs(translationX) * 2);
        else
            player.transform.Translate(0, 0, Mathf.Abs(translationZ) + Mathf.Abs(translationX));
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public int GetScore()
    {
        return scoreRecorder.score;
    }
    
    void OnEnable() {
        GameEventManager.OnGoalLost += OnGoalLost;
        GameEventManager.OnFollowing += OnFollowing;
        GameEventManager.GameOver += GameOver;
        GameEventManager.Win += Win;
    }

    void OnDisable() {
        GameEventManager.OnGoalLost -= OnGoalLost;
        GameEventManager.OnFollowing -= OnFollowing;
        GameEventManager.GameOver -= GameOver;
        GameEventManager.Win -= Win;
    }
    
    public void OnGoalLost(GameObject patrol) {
        print("Goal Lost!");
        patrolActionManager.Patrol(patrol);
        scoreRecorder.Score();
    }

    public void OnFollowing(GameObject patrol) {
        patrolActionManager.Follow(player, patrol);
    }
    
    public void GameOver() {
        gameState = GameState.Lose;
        print(gameState);
        StopAllCoroutines();
        // patrolFactory.PausePatrol()
        player.GetComponent<Animator>().SetTrigger("Dead");
        patrolActionManager.DestroyAll();
    }

    public void Win() {
        gameState = GameState.Win;
        StopAllCoroutines();
        // patrolFactory.PausePatrol();
        player.GetComponent<Animator>().SetBool("pause", true);
    }
}