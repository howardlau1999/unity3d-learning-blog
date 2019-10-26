using System.Collections.Generic;
using UnityEngine;

public class PatrolFactory : Singleton<PatrolFactory>
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Purple
    }

    private readonly Stack<GameObject> cache;


    protected PatrolFactory()
    {
        cache = new Stack<GameObject>();
        
    }

    public GameObject GetPatrol()
    {
        if (cache.Count > 0)
            return cache.Pop();
        
        GameObject patrol = Instantiate(Resources.Load<GameObject>("Prefabs/Patrol"));
        patrol.AddComponent<PatrolData>();
        return patrol;
    }

    public void Recycle(GameObject patrol)
    {
        
    }
}