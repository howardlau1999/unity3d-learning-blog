using System;
using UnityEngine;

public class UFOBehaviour : MonoBehaviour
{
    public UFOFactory.Color color;
    public Vector3 direction;
    public float speed;
    public int score;
    private void Update()
    {
        this.transform.position += Time.deltaTime * speed * direction;
    }
}