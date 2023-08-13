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
        Globals.TreeSpritePool = GetComponent<TreeSpritePool>();
        Globals.Canvas = GameObject.Find("Canvas");

        Globals.Sprites.InitSprites();
        ItemInfos.InitItems();
    }
}
