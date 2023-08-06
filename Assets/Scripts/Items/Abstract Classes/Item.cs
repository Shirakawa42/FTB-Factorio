using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ushort Id { get; }
    public string Name { get; }
    public string Description { get; }
    public int MaxStack { get; }
    public int CurrentStack { get; set; }
    public Sprite Sprite { get; }
    
    public virtual void LeftClick(Animation animation)
    {

    }
    public virtual void RightClick(Animation animation)
    {

    }

    public Item(ushort id, string name, string description, int maxStack, int currentStack, Sprite sprite)
    {
        Id = id;
        Name = name;
        Description = description;
        MaxStack = maxStack;
        CurrentStack = currentStack;
        Sprite = sprite;
    }

    public virtual Item Clone()
    {
        return new Item(Id, Name, Description, MaxStack, CurrentStack, Sprite);
    }
}
