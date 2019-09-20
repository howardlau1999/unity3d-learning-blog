---
layout: post
title:  "空间与运动"
---

## 游戏对象运动的本质

游戏对象的本质，就是游戏对象的空间属性随着时间的变化而变化，例如 `Position` 属性和 `Rotation` 属性。

## 实现物体的抛物线运动

抛物线运动的方程式如下：

$$
x = vt \\
y=\frac{1}{2}gt^2 \\
g=9.8\ \text{m/s}^2
$$

可以看出水平方向上物体做匀速运动，垂直方向上物体做匀加速运动，根据物理原理，物体的速度等于各速度矢量的和，因此我们可以分别处理横轴和竖轴上的运动，再将其合成，就得到了物体的抛物线运动。

### 直接分别计算 X 轴和 Y 轴的运动

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move1 : MonoBehaviour {
    public float a_y = 9.8;
    public float v_y = 1;
    public float v_x = 1;
    void Update () {
        this.transform.position += Vector3.down * Time.deltaTime * v_y;
        this.transform.position += Vector3.right * Time.deltaTime * v_x;
        v_y += a_y;
    }
}
```

### 先求合速度再根据合速度计算下一时刻坐标

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2 : MonoBehaviour {
    public float a_y = 9.8;
    public float v_y = 1;
    public float v_x = 1;
    void Update () {
        Vector3 v = new Vector3(v_x, -v_y, 0);
        this.transform.position += Time.deltaTime * v;
        v_y += a_y;
    }
}
```

### 使用 `translate` 方法计算位移

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move3 : MonoBehaviour {
    public float a_y = 9.8;
    public float v_y = 1;
    public float v_x = 1;
    void Update () {
        Vector3 v = new Vector3(v_x, -v_y, 0);
        this.transform.translate(Time.deltaTime * v);
        v_y += a_y;
    }
}
```