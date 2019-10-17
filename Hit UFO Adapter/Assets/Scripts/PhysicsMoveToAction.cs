using UnityEngine;

public class PhysicsMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;


    public static PhysicsMoveToAction GetSSAction(Vector3 target, float speed)
    {
        PhysicsMoveToAction action = ScriptableObject.CreateInstance<PhysicsMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start()
    {
        
    }
}