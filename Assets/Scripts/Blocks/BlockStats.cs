using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStats
{
    public ushort Id;
    public string Name;
    public bool IsSolid;
    public bool IsTransparent;
    public short Hp;
    public short HpMax;
    public ushort SolidityLevel;
    public BlockTypes BlockType;
    public ushort TextureId;

    public BlockStats(ushort id, string name, bool isSolid, bool isTransparent, short hpMax, ushort solidityLevel, BlockTypes blockType, ushort textureId)
    {
        Id = id;
        Name = name;
        IsSolid = isSolid;
        IsTransparent = isTransparent;
        Hp = hpMax;
        HpMax = hpMax;
        SolidityLevel = solidityLevel;
        BlockType = blockType;
        TextureId = textureId;
    }

    public float GetHpPercentage()
    {
        return (float)Hp / HpMax;
    }
}
