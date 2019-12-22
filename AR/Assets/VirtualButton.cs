using UnityEngine;
using Vuforia;
public class VirtualButton :  MonoBehaviour
{
    public GameObject vb, solid, dragon;
    public Animator ani;
    VirtualButtonBehaviour behaviour;
    public Material pressed, normal;
    void Awake()
    {
        behaviour = vb.GetComponent<VirtualButtonBehaviour>();
        Debug.Log(behaviour);
        if (behaviour)
        {
            behaviour.RegisterOnButtonReleased(OnButtonReleased);
            behaviour.RegisterOnButtonPressed(OnButtonPressed);
        }
        ani = dragon.GetComponent<Animator>();
    }

    void Destroy()
    {
        behaviour = vb.GetComponent<VirtualButtonBehaviour>();
        if (behaviour)
        {
            behaviour.UnregisterOnButtonReleased(OnButtonReleased);
            behaviour.UnregisterOnButtonPressed(OnButtonPressed);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);
        solid.GetComponent<MeshRenderer>().sharedMaterial = pressed;
        ani.SetTrigger("Take Off");
    }
    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);
        solid.GetComponent<MeshRenderer>().sharedMaterial = normal;
        ani.SetTrigger("Land");
    }
}
