using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Controller : MonoBehaviour, IController, ISSActionCallback
{
    public int nRound = 10;
    public int nTrial = 10;
    public int nUFO = 10;
    public int currentTrial = 0;
    public int currentRound = 0;
    public int score = 0;
    public bool isPhysics = false;
    
    private Ruler ruler;
    private List<GameObject> UFOs;

    public GameObject mainCamera;
    public IActionManager actionManager;

    private void Awake()
    {
        Director.Instance.currentController = this;
        actionManager = GetComponent<ActionManagerAdapter>();
        UFOs = new List<GameObject>();
    }

    private void Start()
    {
        NextRound();
    }

    private void Update()
    {
        Camera ca;
        if (mainCamera != null) ca = mainCamera.GetComponent<Camera>();
        else ca = Camera.main;

        if (Input.GetButtonDown("Fire1"))
        {
            // create ray, origin is camera, and direction to mousepoint
            Ray ray = ca.ScreenPointToRay(Input.mousePosition);

            // Return the ray's hit
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject UFO = UFOs.Find(o => o == (isPhysics
                                                    ? hit.transform.gameObject
                                                    : hit.transform.parent.gameObject));
                if (UFO != null)
                {
                    score += UFO.GetComponent<UFOBehaviour>().score;
                    UFOs.Remove(UFO);
                    UFOFactory.Instance.Recycle(UFO);
                    if (UFOs.Count >= nUFO) return;
                    GameObject newUFO = ruler.generateUFO();
                    if (newUFO == null) return;
                    UFOs.Add(newUFO);
                }
            }
        }
    }

    public void SSActionEvent(SSAction action)
    {
        GameObject UFO = action.gameObject;
        UFOs.Remove(UFO);
        UFOFactory.Instance.Recycle(UFO);
        if (UFOs.Count >= nUFO) return;
        GameObject newUFO = ruler.generateUFO();
        if (newUFO == null) return;
        UFOs.Add(newUFO);
    }
    
    public void Restart()
    {
        currentRound = 0;
        currentTrial = 0;
        score = 0;
        NextRound();
    }

    public void LoadResources()
    {
        throw new NotImplementedException();
    }

    public void NextTrial()
    {
        ++currentTrial;
        if (currentTrial > nTrial)
        {
            NextRound();
            return;
        }

        foreach (GameObject UFO in UFOs)
        {
            UFOFactory.Instance.Recycle(UFO);
        }

        UFOs.Clear();

        nUFO = ruler.getUFOCount();
        for (int i = 0; i < nUFO; ++i)
        {
            UFOs.Add(ruler.generateUFO());
        }
    }

    public void NextRound()
    {
        if (currentRound == nRound)
            currentRound = 0;
        ruler = new Ruler(++currentRound, this, actionManager);
        currentTrial = 0;
        NextTrial();
    }
    
    
}