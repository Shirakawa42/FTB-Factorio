using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ushort Id { get; }
    public string Name { get; }
    public string Description { get; }
    public ushort MaxStack { get; }
    public ushort CurrentStack { get; set; }
    public Sprite Sprite { get; }
    
    public virtual void LeftClick(Animation animation)
    {
        animation.Play("ItemBasic");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(mousePosition, Globals.Player.transform.position) > 1.5f)
            return;
        Globals.BlockBreaking.GetComponent<BlockBreaking>().AttackBlock(mousePosition, 1, BlockTypes.Soft, 1, Globals.CurrentWorldId);
    }
    public virtual void RightClick(Animation animation)
    {

    }

    public Item(ushort id, string name, string description, ushort maxStack, ushort currentStack, Sprite sprite)
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
