using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 _playerPosition;
    private Vector2Int _playerChunkPosition;
    private WorldsIds _oldWorldId = WorldsIds.overworld;

    private GameObject _worlds;

    void Awake()
    {
        Globals.Player = GameObject.Find("Player");
        Globals.BlockBreaking = GameObject.Find("BlockBreaking");
        _worlds = GameObject.Find("Worlds");
        Globals.CurrentWorld = _worlds.transform.Find("overworld").gameObject;
        Globals.ChunkMaterialFloor = Resources.Load<Material>("Materials/BlockMaterialFloor");
        Globals.ChunkMaterialSolid = Resources.Load<Material>("Materials/BlockMaterialSolid");
        Globals.Sprites = new Sprites();
        Globals.Sprites.InitSprites();
        Globals.TreeSpritePool = GetComponent<TreeSpritePool>();
        ItemInfos.InitItems();
        _playerChunkPosition = new Vector2Int(int.MaxValue, int.MaxValue);
        Globals.Canvas = GameObject.Find("Canvas");
    }

    private void LoadAroundPlayer()
    {
        _playerPosition = Globals.Player.transform.position;
        Vector2Int chunkPosition = new((int)_playerPosition.x / Globals.ChunkSize, (int)_playerPosition.y / Globals.ChunkSize);

        if (chunkPosition != _playerChunkPosition || Globals.CurrentWorldId != _oldWorldId)
        {
            _oldWorldId = Globals.CurrentWorldId;
            _playerChunkPosition = chunkPosition;
            Map.LoadAroundChunkPosition(_playerChunkPosition, Globals.CurrentWorldId);
        }
    }

    void DisableWorlds()
    {
        foreach (Transform child in _worlds.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        LoadAroundPlayer();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DisableWorlds();
            Globals.CurrentWorld = _worlds.transform.Find("overworld").gameObject;
            Globals.CurrentWorld.SetActive(true);
            Globals.CurrentWorldId = WorldsIds.overworld;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            DisableWorlds();
            Globals.CurrentWorld = _worlds.transform.Find("low_depth").gameObject;
            Globals.CurrentWorld.SetActive(true);
            Globals.CurrentWorldId = WorldsIds.low_depth;
        }
    }
}
