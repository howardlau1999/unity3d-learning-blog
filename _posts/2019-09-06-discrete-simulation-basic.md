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

> Unity 场景中所有实体的基类。

### Transform

[https://docs.unity3d.com/ScriptReference/Transform.html](https://docs.unity3d.com/ScriptReference/Transform.html)

> 一个对象的位置、旋转、缩放信息。场景里的每个对象都有一个 Transform 属性，用来存储和操作对象的位置、旋转、缩放。每个 Transform 都可以有一个父 Transform，这样就可以对对象继承地应用位置、旋转和缩放。在继承面板看到的就是继承关系。

### Component

[https://docs.unity3d.com/ScriptReference/Component.html](https://docs.unity3d.com/ScriptReference/Component.html)

> 所有附加到 GameObject 上的对象的基类。在代码中从不直接创建 Component，而是将写好的脚本附加到 GameObject 上。
