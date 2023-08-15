using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public const int ChunkSize = 64;
    public const int LoadDistance = 1;
    public const int PreloadDistance = LoadDistance + 1;
    public const int Seed = 56231;
    public const int ToolbarSlots = 9;
    public const int InventorySlots = 27;
    public const int NbTreeSprites = 3;
    public const int MaxItemID = 512;

    public static WorldsIds CurrentWorldId = WorldsIds.overworld;
    public static GameObject CurrentWorld;
    public static GameObject Player;
    public static GameObject BlockBreaking;
    public static GameObject Canvas;
    public static Sprites Sprites;
    public static SpritePool SpritePool;
    public static Material ChunkMaterialFloor;
    public static Material ChunkMaterialSolid;
    public static Material SpritesMaterial;
    public static ChunksManager ChunksManager;
}
