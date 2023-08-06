using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryTool : Item
{
    public float MiningPower { get; set; }
    public BlockTypes BlockType { get; }
    public int MiningLevel { get; }
    public int CurrentDurability { get; set; }
    public int MaxDurability { get; set; }

    public override void RightClick(Animation animation)
    {

    }

    public override void LeftClick(Animation animation)
    {
        animation.Play("ItemBasic");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Vector2.Distance(mousePosition, Globals.Player.transform.position) > 1.5f)
            return;
        Globals.BlockBreaking.GetComponent<BlockBreaking>().AttackBlock(mousePosition, MiningPower, BlockType, MiningLevel);
    }

    public PrimaryTool(ushort id, string name, string description, int maxStack, int currentStack, Sprite sprite, float miningPower, BlockTypes blockType, int miningLevel, int maxDurability) : base(id, name, description, maxStack, currentStack, sprite)
    {
        MiningPower = miningPower;
        BlockType = blockType;
        MiningLevel = miningLevel;
        CurrentDurability = maxDurability;
        MaxDurability = maxDurability;
    }

    public override Item Clone()
    {
        return new PrimaryTool(Id, Name, Description, MaxStack, CurrentStack, Sprite, MiningPower, BlockType, MiningLevel, MaxDurability);
    }
}
