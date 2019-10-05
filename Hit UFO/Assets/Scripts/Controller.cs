using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Controller : MonoBehaviour, IController
{
    public int nRound = 10;
    public int nTrial = 10;
    public int currentTrial = 0;
    public int currentRound = 0;
    public int score = 0;
    
    private Ruler ruler;
    private List<GameObject> UFOs;

    public GameObject mainCamera;

    private void Awake()
    {
        Director.Instance.currentController = this;
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

        List<GameObject> invisible = UFOs.FindAll(o =>
        {
            Vector3 viewPosition = ca.WorldToViewportPoint(o.transform.position);
            return viewPosition.x > 1.1f || viewPosition.y > 1.1f || viewPosition.x < -0.1f || viewPosition.y < -0.1f;
        });

        foreach (GameObject UFO in invisible)
        {
            UFOs.Remove(UFO);
            UFOFactory.Instance.Recycle(UFO);
            GameObject newUFO = ruler.generateUFO();
            if (newUFO == null) return;
            UFOs.Add(newUFO);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            // create ray, origin is camera, and direction to mousepoint


            Ray ray = ca.ScreenPointToRay(Input.mousePosition);

            // Return the ray's hit
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject UFO = UFOs.Find(o => o == hit.transform.parent.gameObject);
                if (UFO != null)
                {
                    score += UFO.GetComponent<UFOBehaviour>().score;
                    UFOs.Remove(UFO);
                    UFOFactory.Instance.Recycle(UFO);
                    GameObject newUFO = ruler.generateUFO();
                    if (newUFO == null) return;
                    UFOs.Add(newUFO);
                }
            }
        }
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

        int nUFO = ruler.getUFOCount();
        for (int i = 0; i < nUFO; ++i)
        {
            UFOs.Add(ruler.generateUFO());
        }
    }

    public void NextRound()
    {
        if (currentRound == nRound)
            currentRound = 0;
        ruler = new Ruler(++currentRound, this);
        currentTrial = 0;
        NextTrial();
    }
}