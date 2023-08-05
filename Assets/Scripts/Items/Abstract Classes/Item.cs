using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public abstract ushort Id { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract int MaxStack { get; }
    public abstract int CurrentStack { get; set; }
    public abstract Texture2D Texture2D { get; }
    
    public abstract void LeftClick();
    public abstract void RightClick();
}
