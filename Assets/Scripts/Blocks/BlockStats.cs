using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStats
{
    public ushort Id;
    public string Name;
    public bool IsSolid;
    public bool IsTransparent;
    public ushort Hp;
    public ushort SolidityLevel;
    public BlockTypes BlockType;
    public ushort TextureId;

    public BlockStats(ushort id, string name, bool isSolid, bool isTransparent, ushort hp, ushort solidityLevel, BlockTypes blockType, ushort textureId)
    {
        Id = id;
        Name = name;
        IsSolid = isSolid;
        IsTransparent = isTransparent;
        Hp = hp;
        SolidityLevel = solidityLevel;
        BlockType = blockType;
        TextureId = textureId;
    }
}
