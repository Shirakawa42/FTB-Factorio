using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PrimaryTool : Item
{
    public abstract float MiningPower { get; set; }
    public abstract BlockTypes BlockTypes { get; }
    public abstract int MiningLevel { get; }

    public override void RightClick()
    {

    }

    public override void LeftClick()
    {
        
    }
}
