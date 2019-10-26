using UnityEngine;
public class PatrolAction : SSAction
{
    private float x, z;
    private bool turn = true;                  
    private PatrolData data;                    
    public static PatrolAction GetAction(Vector3 location) {
        PatrolAction action = CreateInstance<PatrolAction>();
        action.x = location.x;
        action.z = location.z;
        return action;
    }

    public override void Start() {
        data = this.gameObject.GetComponent<PatrolData>();
    }

    public override void Update() {
        if (SSDirector.Instance.currentSceneController.GetGameState().Equals(GameState.Running)) {
            Patrol();

            if (!data.isFollowing && data.isPlayerInRange && !data.isCollided) {
                this.destroy = true;
                this.enable = false;
                this.gameObject.GetComponent<PatrolData>().isFollowing = true;
                GameEventManager.Instance.FollowPlayer(this.gameObject);
            }
        }
    }

    void Patrol() {
        if (turn) {
            x = this.transform.position.x + Random.Range(-5f, 5f);
            z = this.transform.position.z + Random.Range(-5f, 5f);
            this.transform.LookAt(new Vector3(x, 0, z));
            this.gameObject.GetComponent<PatrolData>().isCollided = false;
            turn = false;
        }
        float distance = Vector3.Distance(transform.position, new Vector3(x, 0, z));

        if (this.gameObject.GetComponent<PatrolData>().isCollided) {
            this.transform.Rotate(Vector3.up, 180);
            GameObject temp = new GameObject();
            temp.transform.position = this.transform.position;
            temp.transform.rotation = this.transform.rotation;
            temp.transform.Translate(0, 0, Random.Range(0.5f, 3f));
            x = temp.transform.position.x;
            z = temp.transform.position.z;
            this.transform.LookAt(new Vector3(x, 0, z));
            this.gameObject.GetComponent<PatrolData>().isCollided = false;
            Destroy(temp);
        } else if (distance <= 0.1) {
            turn = true;
        } else {
            this.transform.Translate(0, 0, Time.deltaTime);
        }
    }
}