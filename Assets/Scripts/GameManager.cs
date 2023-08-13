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

        Globals.Sprites.InitSprites();
        ItemInfos.InitItems();
    }
}
