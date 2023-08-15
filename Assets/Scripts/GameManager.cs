using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        Globals.Player = GameObject.Find("Player");
        Globals.BlockBreaking = GameObject.Find("BlockBreaking");
        Globals.Sprites = new Sprites();
        Globals.SpritePool = GetComponent<SpritePool>();
        Globals.Canvas = GameObject.Find("Canvas");
        Globals.ChunksManager = GetComponent<ChunksManager>();
        Globals.ChunkMaterialFloor = Resources.Load<Material>("Materials/BlockMaterialFloor");
        Globals.ChunkMaterialSolid = Resources.Load<Material>("Materials/BlockMaterialSolid");
        Globals.SpritesMaterial = Resources.Load<Material>("Materials/Sprites");

        Application.targetFrameRate = 6000000;

        Globals.Sprites.InitSprites();
        ItemInfos.InitItems();
    }
}
