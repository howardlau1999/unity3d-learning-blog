using System.Collections.Generic;
using UnityEngine;

public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int index = 0;

    public static CCSequenceAction GetSSAciton(int repeat, int index, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.index = index;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (index < sequence.Count)
        {
            sequence[index].Update();
        }
    }

    public void SSActionEvent(SSAction action)
    {
        action.destroy = true;
        ++this.index;
        if (this.index >= sequence.Count)
        {
            this.index = 0;
            if (repeat > 0)
            {
                --repeat;
            }
            else if (repeat == 0)
            {
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }
}