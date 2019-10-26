using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            print("Player saw!");
            this.gameObject.transform.parent.GetComponent<PatrolData>().isPlayerInRange = true;
            this.gameObject.transform.parent.GetComponent<PatrolData>().player = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        
        if (collider.gameObject.tag == "Player")
        {
            print("Player lost!");
            PatrolData data = gameObject.transform.parent.GetComponent<PatrolData>();
            data.isPlayerInRange = false;
            data.isFollowing = false;
            data.isLost = true;
            data.player = null;
        }
    }
}