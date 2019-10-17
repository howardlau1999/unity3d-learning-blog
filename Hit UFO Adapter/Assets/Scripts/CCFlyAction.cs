using UnityEngine;

public class CCFlyAction : SSAction
{
    public Vector3 direction;
    public float speed;
    private Camera cam;
    
    public static CCFlyAction GetSSAction(Vector3 direction, float speed)
    {
        CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
        action.direction = direction;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        gameObject.transform.position += direction * speed * Time.deltaTime;
        Vector3 viewPosition = cam.WorldToViewportPoint(gameObject.transform.position);
        if (!this.gameObject.activeSelf)
            this.destroy = true;
        
        if (viewPosition.x > 1.1f || viewPosition.y > 1.1f || viewPosition.x < -0.1f || viewPosition.y < -0.1f)
        {
            this.destroy = true;
            if (this.gameObject.activeSelf)
                this.callback.SSActionEvent(this);
        }
    }

    public override void Start()
    {
        cam = Camera.main;
    }
}