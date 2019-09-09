---
layout: post
title:  "离散仿真引擎基础"
---

## 基本概念

### GameObjects

在 Unity 中，`GameObject` 是一个个游戏对象，就像在 Java 等语言中 `new` 出来的对象一样。`GameObject` 是 Unity 中的基础的对象，构成了人物、道具、场景等等。是动态的资源，在运行时可能会动态创建很多 `GameObject` 或者改变不同 `GameObject` 的属性。

### Assets

在 Unity 中，`Assets` 是资源，例如游戏中可能用到的图片、文本、音频、3D 模型等素材。是静态的资源，一般在开发的时候就导入好。

`Assets` 可以用来创建 `GameObject`，或者被 `GameObject` 用来改变自己的外观行为等。

## 游戏项目资源、对象组织结构

![project]({{ site.url }}{{ site.baseurl }}/assets/images/project.png)

这是从官方 Assets Store 下载回来的一个 [FPS 项目](https://assetstore.unity.com/packages/templates/tutorials/149310)，中间是其中一个场景的对象组织结构，右边是项目资源的组织结构。

可以看到对象的组织结构一般是按照聚合的方式，也就是关系比较紧密，由多个部分构成整体的对象组织成父子关系，实现了组合模式。

而资源文件一般按照资源文件的类型来组织，例如图片、声音资源分别放在单独的文件夹里，而声音里又会再区分 BGM、SFX 等。

## `MonoBehaviour` 事件触发

在 Unity 中，程序员不需要手动管理游戏对象，而是让所有的游戏对象都继承 `MonoBehaviour`，并实现 `OnXXX` 等方法来接收游戏引擎触发的事件，从而达到实现游戏逻辑的目的。在 Unity 中，一个对象的执行周期可以参考官方文档：[https://docs.unity3d.com/Manual/ExecutionOrder.html](https://docs.unity3d.com/Manual/ExecutionOrder.html)

当然这个是相当完整的执行过程，一般我们编程的时候只关注其中几个比较关键的方法：

| 事件名称 | 执行条件或时机 |
| --- | --- |
| Awake | 当一个脚本实例被载入时被调用。或者脚本构造时调用 |
| Start | 第一次进入游戏循环时调用 |
| FixUpdate | 每个游戏循环，由物理引擎调用 |
| Update | 所有 Start 调用完后，被游戏循环调用 |
| LastUpdate | 所有 Update 调用完后，被游戏循环调用 |
| OnDisable | 当对象变为不可用或非激活状态时调用 |
| OnEnable | 当对象变为可用或激活状态时调用 |
| OnGUI | 游戏循环在渲染过程中，场景渲染之后调用 |

下面是一个验证代码，验证这些事件触发的时机，为了避免输出过多，一些会被频繁调用的函数只输出一次：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBehaviour : MonoBehaviour
{
    private bool hasBeenUpdated = false;
    private bool hasBeenFixedUpdated = false;
    private bool hasBeenLateUpdated = false;
    private bool hasBeenCalledOnGUI = false;
    void Awake()
    {
        Debug.Log("Awake");
    }
    void Start()
    {
        Debug.Log("Start");
    }

    void Update()
    {
        if (!hasBeenUpdated)
        {
            Debug.Log("Update");
            hasBeenUpdated = true;
        }
    }

    void FixedUpdate()
    {
        if (!hasBeenFixedUpdated)
        {
            Debug.Log("Fixed Update");
            hasBeenFixedUpdated = true;
        }
    }

    void LateUpdate()
    {
        if (!hasBeenLateUpdated)
        {
            Debug.Log("Late Update");
            hasBeenLateUpdated = true;
        }
    }

    void OnDisable()
    {
        Debug.Log("On Disable");
    }

    void OnEnable()
    {
        Debug.Log("On Enable");
    }

    void OnGUI()
    {
        if (!hasBeenCalledOnGUI)
        {
            Debug.Log("On GUI");
            hasBeenCalledOnGUI = true;
        }
    }
}
```

在场景中新建一个空对象，然后将脚本添加到对象的组件中，按播放键执行代码，在 Console 中查看输出：

![console]({{ site.url }}{{ site.baseurl }}/assets/images/console.png)

可以看到和表格的行为一样。OnDisable 是在点了停止按钮之后输出的。

## 了解 Unity 中基本类型

### GameObject

[https://docs.unity3d.com/ScriptReference/GameObject.html](https://docs.unity3d.com/ScriptReference/GameObject.html)

Unity 场景中所有实体的基类。

### Transform

[https://docs.unity3d.com/ScriptReference/Transform.html](https://docs.unity3d.com/ScriptReference/Transform.html)

一个对象的位置、旋转、缩放信息。场景里的每个对象都有一个 Transform 属性，用来存储和操作对象的位置、旋转、缩放。每个 Transform 都可以有一个父 Transform，这样就可以对对象继承地应用位置、旋转和缩放。在继承面板看到的就是继承关系。

### Component

[https://docs.unity3d.com/ScriptReference/Component.html](https://docs.unity3d.com/ScriptReference/Component.html)

所有附加到 GameObject 上的对象的基类。在代码中从不直接创建 Component，而是将写好的脚本附加到 GameObject 上。

用 UML 图表示三者的关系如下:

![uml]({{ site.url }}{{ site.baseurl }}/assets/images/uml.png)

下面来看看可视化编辑界面和 Unity API 的对应关系：

![table]({{ site.url }}{{ site.baseurl }}/assets/images/table.png)

关注右边的 Inspector 界面，从上到下分别是：

- table 对象本身
  - Name: 对象的名字
  - Tag: 对象的标签 
  - Layer: 对象所处层级
- Transform
  - Position: 位置坐标
  - Rotation: 旋转角度，单位是度
  - Scale: 缩放
- Mesh Filter
  - Mesh: 用来渲染的网格
- Mesh Renderer
  - 3D 模型的渲染器
- Box Collider
  - 用来碰撞检测，可以编辑碰撞检测器的形状
- Material
  - 渲染 3D 模型的材质
  
## 对象的基本操作

### 查找对象

#### `GameObject.Find(string name)`

查找对象有几种方法，第一种是使用 `GameObject.Find(string name)` 方法在继承层级中查找：

```csharp
public class InitBehaviour : MonoBehaviour
{
    private GameObject chair3;

    void Start()
    {
        chair3 = GameObject.Find("table/chair3");
    }

    void Update()
    {
        chair3.GetComponent<Transform>().Rotate(new Vector3(.1f, 0, 0));
    }
}
```

![findobject]({{ site.url }}{{ site.baseurl }}/assets/images/findobject.png)

可以看到启动后 `chair3` 旋转了起来。

#### `GameObject.FindGameObjectsWithTag(string tag)`

第二种是使用对象的标签来查找，可以先在 Inspector 窗口里给对象打 tag，然后用这个方法把带有指定 tag 的对象全部查找出来：

```csharp
public class InitBehaviour : MonoBehaviour
{
    private GameObject[] chairs;

    void Start()
    {
        chairs = GameObject.FindGameObjectsWithTag("Chair");
    }

    void Update()
    {
        foreach (GameObject chair in chairs)
        {
            chair.GetComponent<Transform>().Rotate(new Vector3(.1f, 0, 0));
        }
    }
}
```

![findobjectbytag]({{ site.url }}{{ site.baseurl }}/assets/images/findobjectbytag.png)

可以看到椅子全部旋转了起来。

### 添加子对象

添加子对象其实是把一个对象的父亲设置为自己，至于这个子对象怎么来的可以有很多方法，这里演示一下使用 `GameObject.CreatePrimitive()` 方法创建一个子对象然后添加到 table 中：

```csharp
public class InitBehaviour : MonoBehaviour
{
    void Start()
    {
        GameObject chair5 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        chair5.name = "chair5";
        chair5.transform.position = new Vector3(0f, 2f, 0f);
        chair5.transform.localScale = new Vector3(1f, .1f, 1f);
        chair5.transform.parent = this.transform;
    }
}
```

![child]({{ site.url }}{{ site.baseurl }}/assets/images/child.png)

可以看到多了一个椅子。

### 遍历对象树

```csharp
public class InitBehaviour : MonoBehaviour
{
    void TraverseTree(Transform root)
    {
        Queue<Transform> queue = new Queue<Transform>();
        HashSet<Transform> visited = new HashSet<Transform>();
        queue.Enqueue(root);
        while (queue.Count != 0)
        {
            Transform cur = queue.Dequeue();
            visited.Add(cur);
            foreach (Transform child in cur)
            {
                if (!visited.Contains(child))
                    queue.Enqueue(child);
                Debug.Log(child.gameObject.name);
            }
        }
    }

    void Start()
    {
        TraverseTree(transform.root);
        Debug.Log("Start");
    }
}
```

![traverse]({{ site.url }}{{ site.baseurl }}/assets/images/traverse.png)

### 清除所有子对象

```csharp
public class InitBehaviour : MonoBehaviour
{
    void TraverseTree(Transform root)
    {
        Queue<Transform> queue = new Queue<Transform>();
        HashSet<Transform> visited = new HashSet<Transform>();
        queue.Enqueue(root);
        while (queue.Count != 0)
        {
            Transform cur = queue.Dequeue();
            visited.Add(cur);
            foreach (Transform child in cur)
            {
                if (!visited.Contains(child))
                    queue.Enqueue(child);
                Debug.Log(child.gameObject.name);
                Destroy(child.gameObject);
            }
        }
    }

    void Start()
    {
        TraverseTree(transform.root);
        Debug.Log("Start");
    }
}
```

![destroy]({{ site.url }}{{ site.baseurl }}/assets/images/destroy.png)

## 资源预设 (Prefabs) 与对象克隆 (clone)

### 资源预设的好处

预设可以看成是游戏对象的模板，包含了完整的组件和属性，可以实例化成游戏对象，使用预设可以很方便的统一修改所有游戏对象的属性，避免了重复劳动，节约游戏资源。

### 预设与对象克隆的关系

预设和实例化的对象有关联，预设改变了，对象也会跟着变。而克隆出来的对象之间是独立的，和被克隆的对象之间没有关系。

### 实践用 Prefab 实例化一个桌子

```csharp
public class InitBehaviour : MonoBehaviour
{
    public GameObject tablePrefab;

    void Start()
    {
        GameObject anotherTable = GameObject.Instantiate(tablePrefab);
        anotherTable.transform.position = new Vector3(0f, 1f, 0f);
        anotherTable.transform.parent = this.transform;
    }
}
```

![prefab]({{ site.url }}{{ site.baseurl }}/assets/images/prefab.png)

可以看到运行之后多了一个 table。
