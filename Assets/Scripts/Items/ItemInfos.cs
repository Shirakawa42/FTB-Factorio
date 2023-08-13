using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemInfos
{
    public static Dictionary<ushort, Item> Items;

    public static void InitItems()
    {
        Items = new Dictionary<ushort, Item>
        {
            #region PrimaryTools
            {
                ItemIds.Pickaxe_Wood,
                new PrimaryTool(
                    id: ItemIds.Pickaxe_Wood,
                    name: "Wooden Pickaxe",
                    description: "A Wooden Pickaxe",
                    maxStack: 1,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_594"),
                    miningPower: 2,
                    blockType: BlockTypes.Hard,
                    miningLevel: 2,
                    maxDurability: 30
                )
            },
            {
                ItemIds.Pickaxe_Stone,
                new PrimaryTool(
                    id: ItemIds.Pickaxe_Stone,
                    name: "Stone Pickaxe",
                    description: "A Stone Pickaxe",
                    maxStack: 1,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_586"),
                    miningPower: 3,
                    blockType: BlockTypes.Hard,
                    miningLevel: 3,
                    maxDurability: 50
                )
            },
            {
                ItemIds.Pickaxe_Iron,
                new PrimaryTool(
                    id: ItemIds.Pickaxe_Iron,
                    name: "Iron Pickaxe",
                    description: "An Iron Pickaxe",
                    maxStack: 1,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_351"),
                    miningPower: 4,
                    blockType: BlockTypes.Hard,
                    miningLevel: 4,
                    maxDurability: 100
                )
            },
            {
                ItemIds.Pickaxe_Gold,
                new PrimaryTool(
                    id: ItemIds.Pickaxe_Gold,
                    name: "Gold Pickaxe",
                    description: "A Gold Pickaxe",
                    maxStack: 1,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_445"),
                    miningPower: 5,
                    blockType: BlockTypes.Hard,
                    miningLevel: 5,
                    maxDurability: 30
                )
            },
            {
                ItemIds.Pickaxe_Diamond,
                new PrimaryTool(
                    id: ItemIds.Pickaxe_Diamond,
                    name: "Diamond Pickaxe",
                    description: "A Diamond Pickaxe",
                    maxStack: 1,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_504"),
                    miningPower: 6,
                    blockType: BlockTypes.Hard,
                    miningLevel: 6,
                    maxDurability: 200
                )
            },
            #endregion
            #region PrimaryBlocks
            {
                ItemIds.Stone,
                new PrimaryBlocks(
                    id: ItemIds.Stone,
                    name: "Stone",
                    description: "A Stone",
                    maxStack: 64,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_372"),
                    dropId: ItemIds.Stone,
                    textureId: TextureIds.Stone,
                    blockType: BlockTypes.Hard,
                    isSolid: true,
                    isTransparent: false,
                    hp: 32,
                    hpMax: 32,
                    solidityLevel: 2
                )
            },
            {
                ItemIds.Dirt,
                new PrimaryBlocks(
                    id: ItemIds.Dirt,
                    name: "Dirt",
                    description: "A Dirt",
                    maxStack: 64,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_162"),
                    dropId: ItemIds.Dirt,
                    textureId: TextureIds.Dirt,
                    blockType: BlockTypes.Soft,
                    isSolid: true,
                    isTransparent: false,
                    hp: 16,
                    hpMax: 16,
                    solidityLevel: 1
                )
            },
            {
                ItemIds.Grass,
                new PrimaryBlocks(
                    id: ItemIds.Grass,
                    name: "Grass",
                    description: "A Grass",
                    maxStack: 64,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_686"),
                    dropId: ItemIds.Grass,
                    textureId: TextureIds.GrassTop,
                    blockType: BlockTypes.Soft,
                    isSolid: true,
                    isTransparent: false,
                    hp: 16,
                    hpMax: 16,
                    solidityLevel: 1
                )
            },
            {
                ItemIds.Air,
                new PrimaryBlocks(
                    id: ItemIds.Air,
                    name: "Air",
                    description: "Air",
                    maxStack: 64,
                    currentStack: 1,
                    sprite: null,
                    dropId: ItemIds.Air,
                    textureId: 0,
                    blockType: BlockTypes.Gas,
                    isSolid: false,
                    isTransparent: true,
                    hp: 0,
                    hpMax: 0,
                    solidityLevel: 0
                )
            },
            #endregion
            #region SpritePrimaryBlocks
            {
                ItemIds.Wood,
                new SpritePrimaryBlocks(
                    id: ItemIds.Wood,
                    name: "Wood",
                    description: "A Wood",
                    maxStack: 64,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_429"),
                    dropId: ItemIds.Wood,
                    textureId: TextureIds.None,
                    blockType: BlockTypes.Wood,
                    isSolid: true,
                    isTransparent: false,
                    hp: 12,
                    hpMax: 12,
                    solidityLevel: 1,
                    groundSprite: Globals.Sprites.GetSprite("Trees_1"),
                    spriteScale: 2f,
                    spriteOffset: new Vector2(0, 0),
                    spriteUnderPlayer: false
                )
            },
            {
                ItemIds.Torch,
                new SpritePrimaryBlocks(
                    id: ItemIds.Torch,
                    name: "Torch",
                    description: "A Torch",
                    maxStack: 64,
                    currentStack: 1,
                    sprite: Globals.Sprites.GetSprite("3208_341"),
                    dropId: ItemIds.Torch,
                    textureId: TextureIds.None,
                    blockType: BlockTypes.Wood,
                    isSolid: false,
                    isTransparent: true,
                    hp: 4,
                    hpMax: 4,
                    solidityLevel: 1,
                    groundSprite: Globals.Sprites.GetSprite("3208_341"),
                    spriteScale: 0.75f,
                    spriteOffset: new Vector2(0, 0.25f),
                    spriteUnderPlayer: true,
                    lightSourcePower: 32
                )
            }
            #endregion
        };
    }

    public static Item GetItemFromId(ushort id)
    {
        if (Items.ContainsKey(id))
            return Items[id];
        return null;
    }

    public static Item GenerateItemFromId(ushort id)
    {
        if (Items.ContainsKey(id))
            return Items[id].Clone();
        return null;
    }

}