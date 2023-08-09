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
            { ItemIds.Pickaxe_Wood, new PrimaryTool(ItemIds.Pickaxe_Wood, "Wooden Pickaxe", "A Wooden Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_594"), 2, BlockTypes.Hard, 2, 30) },
            { ItemIds.Pickaxe_Stone, new PrimaryTool(ItemIds.Pickaxe_Stone, "Stone Pickaxe", "A Stone Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_586"), 3, BlockTypes.Hard, 3, 50) },
            { ItemIds.Pickaxe_Iron, new PrimaryTool(ItemIds.Pickaxe_Iron, "Iron Pickaxe", "An Iron Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_351"), 4, BlockTypes.Hard, 4, 100) },
            { ItemIds.Pickaxe_Gold, new PrimaryTool(ItemIds.Pickaxe_Gold, "Gold Pickaxe", "A Gold Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_445"), 5, BlockTypes.Hard, 5, 30) },
            { ItemIds.Pickaxe_Diamond, new PrimaryTool(ItemIds.Pickaxe_Diamond, "Diamond Pickaxe", "A Diamond Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_504"), 6, BlockTypes.Hard, 6, 200) },

            { ItemIds.Stone, new PrimaryBlocks(ItemIds.Stone, "Stone", "A Stone", 64, 1, Globals.Sprites.GetSprite("3208_372"), ItemIds.Stone, TextureIds.Stone, BlockTypes.Hard, true, false, 16, 16, 2) },
            { ItemIds.Dirt, new PrimaryBlocks(ItemIds.Dirt, "Dirt", "A Dirt", 64, 1, Globals.Sprites.GetSprite("3208_162"), ItemIds.Dirt, TextureIds.Dirt, BlockTypes.Soft, true, false, 4, 4, 1) },
            { ItemIds.Grass, new PrimaryBlocks(ItemIds.Grass, "Grass", "A Grass", 64, 1, Globals.Sprites.GetSprite("3208_686"), ItemIds.Grass, TextureIds.GrassTop, BlockTypes.Soft, true, false, 4, 4, 1) },
            { ItemIds.Air, new PrimaryBlocks(ItemIds.Air, "Air", "Air", 64, 1, null, ItemIds.Air, 0, BlockTypes.Gas, false, false, 0, 0, 0)},
            { ItemIds.Wood, new PrimaryBlocks(ItemIds.Wood, "Wood", "A Wood", 64, 1, Globals.Sprites.GetSprite("3208_429"), ItemIds.Wood, TextureIds.WoodTop, BlockTypes.Wood, true, false, 12, 12, 1) },
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