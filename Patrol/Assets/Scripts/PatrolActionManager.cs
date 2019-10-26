using UnityEngine;

public class PatrolActionManager : SSActionManager, ISSActionCallback
{
    public void Patrol(GameObject patrol)
    {
        PatrolAction patrolAction = PatrolAction.GetAction(patrol.transform.position);
        this.RunAction(patrol, patrolAction, this);
    }


    public void Follow(GameObject player, GameObject patrol)
    {
        PatrolFollowAction followAction = PatrolFollowAction.GetAction(player);
        this.RunAction(patrol, followAction, this);
    }

    public void SSActionEvent(SSAction source)
    {
    }
}