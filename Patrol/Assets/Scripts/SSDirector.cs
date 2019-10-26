using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : Singleton<SSDirector>
{
    public ISceneController currentSceneController { get; set; }
    public bool running { get; set; }
}
