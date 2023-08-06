using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemInfos
{
    public static List<Item> Items = new()
    {
        new PrimaryTool(0, "Wooden Pickaxe", "A Wooden Pickaxe", 1, 1, Resources.Load<Sprite>("Textures/3208_351"), 2, BlockTypes.Hard, 1, 30),
        new PrimaryTool(1, "Stone Pickaxe", "A Stone Pickaxe", 1, 1, Resources.Load<Sprite>("Textures/3208_351"), 3, BlockTypes.Hard, 2, 50),
        new PrimaryTool(2, "Iron Pickaxe", "An Iron Pickaxe", 1, 1, Resources.Load<Sprite>("Textures/3208_351"), 4, BlockTypes.Hard, 3, 100),
        new PrimaryTool(3, "Gold Pickaxe", "A Gold Pickaxe", 1, 1, Resources.Load<Sprite>("Textures/3208_351"), 5, BlockTypes.Hard, 4, 30),
        new PrimaryTool(4, "Diamond Pickaxe", "A Diamond Pickaxe", 1, 1, Resources.Load<Sprite>("Textures/3208_351"), 6, BlockTypes.Hard, 5, 200)
    };

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

public static class ItemIds
{
    public const ushort Pickaxe_Wood = 0;
    public const ushort Pickaxe_Stone = 1;
    public const ushort Pickaxe_Iron = 2;
    public const ushort Pickaxe_Gold = 3;
    public const ushort Pickaxe_Diamond = 4;
}
