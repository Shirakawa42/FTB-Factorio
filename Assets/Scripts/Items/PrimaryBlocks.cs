using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryBlocks : Item
{
    public ushort DropId { get; }
    public ushort TextureId { get; }
    public BlockTypes BlockType { get; }
    public bool IsSolid { get; }
    public bool IsTransparent { get; }
    public short Hp { get; set; }
    public short HpMax { get; }
    public ushort SolidityLevel { get; }

    public override void RightClick(Animation animation)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousePosition, Globals.Player.transform.position);
        if (distance > 4f || distance < 1f || WorldsHelper.GetBlockStats(mousePosition, Globals.CurrentWorldId, ChunkTypes.Solid).Id != ItemIds.Air)
            return;
        WorldsHelper.SetBlock(mousePosition, Id, Globals.CurrentWorldId, ChunkTypes.Solid);
    }

    public PrimaryBlocks(ushort id, string name, string description, ushort maxStack, ushort currentStack, Sprite sprite, ushort dropId, ushort textureId, BlockTypes blockType, bool isSolid, bool isTransparent, short hp, short hpMax, ushort solidityLevel) : base(id, name, description, maxStack, currentStack, sprite)
    {
        DropId = dropId;
        TextureId = textureId;
        BlockType = blockType;
        IsSolid = isSolid;
        IsTransparent = isTransparent;
        Hp = hp;
        HpMax = hpMax;
        SolidityLevel = solidityLevel;
    }

    public override Item Clone()
    {
        return new PrimaryBlocks(Id, Name, Description, MaxStack, CurrentStack, Sprite, DropId, TextureId, BlockType, IsSolid, IsTransparent, Hp, HpMax, SolidityLevel);
    }

    public float GetHpPercentage()
    {
        return (float)Hp / HpMax;
    }
}
