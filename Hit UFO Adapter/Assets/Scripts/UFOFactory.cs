using System.Collections.Generic;
using UnityEngine;

public class UFOFactory : Singleton<UFOFactory>
{
    public enum Color
    {
        Red,
        Green,
        Blue,
        Purple
    }

    private readonly Dictionary<Color, Stack<GameObject>> cache;


    protected UFOFactory()
    {
        cache = new Dictionary<Color, Stack<GameObject>>();
        
    }

    public GameObject getUFO(Color color)
    {
        if (!cache.ContainsKey(color))
        {
            cache[color] = new Stack<GameObject>();
        }

        // We no UFO of that color
        if (cache[color].Count == 0)
        {
            // We don't have enough UFOs, instantiate one and hide it
            GameObject UFO = Instantiate(Resources.Load<GameObject>("Prefabs/UFO"), new Vector3(-100, -100, -100),
                Quaternion.identity);
            UFOBehaviour behaviour = UFO.AddComponent<UFOBehaviour>();
            behaviour.color = color;
            // Color it
            Material material = Instantiate(Resources.Load<Material>("Materials/" + color.ToString("G")));
            foreach (Transform transform in UFO.transform)
            {
                MeshRenderer renderer = transform.gameObject.GetComponent<MeshRenderer>();
                renderer.material = material;
            }

            return UFO;
        }

        GameObject cached = cache[color].Pop();
        cached.SetActive(true);

        return cached;
    }

    public void Recycle(GameObject UFO)
    {
        // Hide it
        UFO.SetActive(false);
        UFO.transform.position = new Vector3(-100, -100, -100);

        // Store it in cache
        cache[UFO.GetComponent<UFOBehaviour>().color].Push(UFO);
    }
}