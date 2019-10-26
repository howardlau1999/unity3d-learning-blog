using UnityEngine;

public class PatrolCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.GetComponent<Animator>().SetTrigger("Shoot");
            GameEventManager.Instance.OnPlayerCaught();
        }
        else
        {
            this.GetComponent<PatrolData>().isCollided = true;
        }
    }
}