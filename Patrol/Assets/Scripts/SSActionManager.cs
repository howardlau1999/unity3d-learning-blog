using System;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitForAdding = new List<SSAction>();
    private List<int> waitForDeletion = new List<int>();

    protected void Update()
    {
        foreach (SSAction action in waitForAdding)
        {
            actions[action.GetInstanceID()] = action;
        }
        
        waitForAdding.Clear();

        foreach (KeyValuePair<int,SSAction> pair in actions)
        {
            SSAction action = pair.Value;
            if (action.destroy)
            {
                waitForDeletion.Add(action.GetInstanceID());
            } else if (action.enable)
            {
                action.Update();
            }
        }

        foreach (int key in waitForDeletion)
        {
            SSAction action = actions[key];
            actions.Remove(key);
            Destroy(action);
        }
        
        waitForDeletion.Clear();
    }

    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback callback)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = callback;
        
        waitForAdding.Add(action);
        action.Start();
    }
    
    public void DestroyAll() {
        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction action = kv.Value;
            action.destroy = true;
        }
    }
    
    protected void Start()
    {
        
    }
}