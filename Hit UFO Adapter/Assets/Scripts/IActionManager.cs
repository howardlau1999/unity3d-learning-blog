using UnityEngine;

public interface IActionManager
{
    void FlyAway(GameObject gameObject, Vector3 direction, float speed, ISSActionCallback callback);
}