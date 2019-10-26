using UnityEngine;

public class PatrolFollowAction : SSAction
{
    private float speed = 1f;
    private GameObject player;
    private PatrolData data;

    public static PatrolFollowAction GetAction(GameObject player)
    {
        PatrolFollowAction action = CreateInstance<PatrolFollowAction>();
        action.player = player;
        return action;
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        data = this.gameObject.GetComponent<PatrolData>();
        if (SSDirector.Instance.currentSceneController.GetGameState().Equals(GameState.Running))
        {
            transform.position =
                Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            this.transform.LookAt(player.transform.position);

            if (data.isLost)
            {
                data.isLost = false;
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
                GameEventManager.Instance.PlayerEscape(this.gameObject);
            }
        }
    }
}