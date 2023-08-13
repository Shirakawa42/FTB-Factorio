using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePrimaryBlocks : PrimaryBlocks
{
    public Sprite GroundSprite { get; }
    public float SpriteScale { get; }
    public Vector2 SpriteOffset { get; }
    public bool SpriteUnderPlayer { get; }

    public SpritePrimaryBlocks(ushort id, string name, string description, ushort maxStack, ushort currentStack, Sprite sprite, ushort dropId,
            ushort textureId, BlockTypes blockType, bool isSolid, bool isTransparent, short hp, short hpMax, ushort solidityLevel, Sprite groundSprite, float spriteScale,
            Vector2 spriteOffset, bool spriteUnderPlayer, ushort lightSourcePower = 0)
            : base(id, name, description, maxStack, currentStack, sprite, dropId, textureId, blockType, isSolid, isTransparent, hp, hpMax, solidityLevel, lightSourcePower)
    {
        GroundSprite = groundSprite;
        SpriteScale = spriteScale;
        SpriteOffset = spriteOffset;
        SpriteUnderPlayer = spriteUnderPlayer;
    }

    public override Item Clone()
    {
        return new SpritePrimaryBlocks(Id, Name, Description, MaxStack, CurrentStack, Sprite, DropId, TextureId, BlockType, IsSolid, IsTransparent, Hp, HpMax, SolidityLevel, GroundSprite, SpriteScale, SpriteOffset, SpriteUnderPlayer, LightSourcePower);
    }

}
