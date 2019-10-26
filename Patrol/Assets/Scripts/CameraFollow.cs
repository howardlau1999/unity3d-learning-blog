using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public GameObject player; 
    public float smothing = 5f;  
    Vector3 offset;

    void Start() {
        offset = new Vector3(0, 3, -5);
    }

    void FixedUpdate() {
        Vector3 target = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, target, smothing * Time.deltaTime);
    }
}