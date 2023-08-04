using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllBlocksStats
{
    public static BlockStats Stone = new(BlockIds.Stone, "Stone", true, false, 10, 1, BlockTypes.Hard, 0);
    public static BlockStats Dirt = new(BlockIds.Dirt, "Dirt", true, false, 10, 1, BlockTypes.Soft, 1);
    public static BlockStats Grass = new(BlockIds.Grass, "Grass", true, false, 10, 1, BlockTypes.Soft, 7);
    public static BlockStats Sand = new(BlockIds.Sand, "Sand", true, false, 10, 1, BlockTypes.Soft, 10);
    public static BlockStats Water = new(BlockIds.Water, "Water", false, true, 10, 1, BlockTypes.Liquid, 9999);
    public static BlockStats Coal = new(BlockIds.Coal, "Coal", true, false, 15, 1, BlockTypes.Hard, 9999);
    public static BlockStats Iron = new(BlockIds.Iron, "Iron", true, false, 20, 2, BlockTypes.Hard, 9999);
    public static BlockStats Gold = new(BlockIds.Gold, "Gold", true, false, 25, 3, BlockTypes.Hard, 9999);
    public static BlockStats Diamond = new(BlockIds.Diamond, "Diamond", true, false, 30, 4, BlockTypes.Hard, 9999);

    public static BlockStats GetBlockStatsFromId(ushort id)
    {
        if (id == BlockIds.Stone)
            return Stone;
        else if (id == BlockIds.Dirt)
            return Dirt;
        else if (id == BlockIds.Grass)
            return Grass;
        else if (id == BlockIds.Sand)
            return Sand;
        else if (id == BlockIds.Water)
            return Water;
        else if (id == BlockIds.Coal)
            return Coal;
        else if (id == BlockIds.Iron)
            return Iron;
        else if (id == BlockIds.Gold)
            return Gold;
        else if (id == BlockIds.Diamond)
            return Diamond;
        else
            throw new System.Exception("Block id does not exist");
    }
}
