using UnityEngine;

public class RegionDetector : MonoBehaviour
{
    public int region; 
    private Controller sceneController; 

    void OnTriggerEnter(Collider collider)
    {
        sceneController = SSDirector.Instance.currentSceneController as Controller;
        if (collider.gameObject.tag == "Player")
        {
            sceneController.playerRegion = region;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Patrol")
        {
            print(collider.gameObject.name);
            collider.gameObject.GetComponent<PatrolData>().isCollided = true;
        }
    }
}