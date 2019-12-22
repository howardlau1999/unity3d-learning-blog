## Vuforia AR Project

演示视频地址：[https://www.bilibili.com/video/av80246744/](https://www.bilibili.com/video/av80246744/)

由于 Vuforia 版本已经更新多次，原有教程已经不适用了，下面是 VirtualButton 的代码：

```csharp
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

```