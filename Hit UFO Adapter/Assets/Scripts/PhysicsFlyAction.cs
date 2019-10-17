using UnityEngine;

public class PhysicsFlyAction : SSAction
{
    public Vector3 direction;
    public float speed;
    private Rigidbody rigidbody;
    private Camera cam;
    
    public static PhysicsFlyAction GetSSAction(Vector3 direction, float speed)
    {
        PhysicsFlyAction action = ScriptableObject.CreateInstance<PhysicsFlyAction>();
        action.direction = direction;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        Vector3 viewPosition = cam.WorldToViewportPoint(gameObject.transform.position);
        
        if (!this.gameObject.activeSelf)
        {
            rigidbody.velocity = Vector3.zero;
            this.destroy = true;
        }

        if (viewPosition.x > 1.1f || viewPosition.y > 1.1f || viewPosition.x < -0.1f || viewPosition.y < -0.1f)
        {
            this.destroy = true;
            rigidbody.velocity = Vector3.zero;
            if (this.gameObject.activeSelf)
                this.callback.SSActionEvent(this);
        }
    }

    public override void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.velocity = direction * speed;
        cam = Camera.main;
    }
}