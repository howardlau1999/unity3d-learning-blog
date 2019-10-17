using UnityEngine;

public class ActionManagerAdapter : MonoBehaviour, IActionManager
{
    public void FlyAway(GameObject gameObject, Vector3 direction, float speed, ISSActionCallback callback)
    {
        Controller controller = Director.Instance.currentController as Controller;
        SSAction flyAction;
        if (controller.isPhysics)
        {
            if (gameObject.GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>().useGravity = false;
            }
            flyAction = PhysicsFlyAction.GetSSAction(direction, speed);
        }
        else
        {
            flyAction = CCFlyAction.GetSSAction(direction, speed);
        }
        
        GetComponent<SSActionManager>().RunAction(gameObject, flyAction, callback);
    }
}