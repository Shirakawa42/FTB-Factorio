using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemInfos
{
    public static List<Item> Items;

    public static void InitItems()
    {
        Items = new()
        {
            new PrimaryTool(ItemIds.Pickaxe_Wood, "Wooden Pickaxe", "A Wooden Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_594"), 2, BlockTypes.Hard, 1, 30),
            new PrimaryTool(ItemIds.Pickaxe_Stone, "Stone Pickaxe", "A Stone Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_586"), 3, BlockTypes.Hard, 2, 50),
            new PrimaryTool(ItemIds.Pickaxe_Iron, "Iron Pickaxe", "An Iron Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_351"), 4, BlockTypes.Hard, 3, 100),
            new PrimaryTool(ItemIds.Pickaxe_Gold, "Gold Pickaxe", "A Gold Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_445"), 5, BlockTypes.Hard, 4, 30),
            new PrimaryTool(ItemIds.Pickaxe_Diamond, "Diamond Pickaxe", "A Diamond Pickaxe", 1, 1, Globals.Sprites.GetSprite("3208_504"), 6, BlockTypes.Hard, 5, 200)
        };
    }

    public static Item GetItemFromId(ushort id)
    {
        foreach (Item item in Items)
            if (item.Id == id)
                return item;
        return null;
    }

    public static Item GenerateItemFromId(ushort id)
    {
        foreach (Item item in Items)
            if (item.Id == id)
                return item.Clone();
        return null;
    }
}