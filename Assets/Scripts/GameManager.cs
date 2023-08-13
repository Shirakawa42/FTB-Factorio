using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 _playerPosition;
    private Vector2Int _playerChunkPosition;
    private WorldsIds _oldWorldId = WorldsIds.overworld;

    private GameObject _worlds;
    private float _DayLight = 1.0f;
    private bool _DayLightIncreasing = false;

    void Awake()
    {
        Globals.Player = GameObject.Find("Player");
        Globals.BlockBreaking = GameObject.Find("BlockBreaking");
        _worlds = GameObject.Find("Worlds");
        Globals.CurrentWorld = _worlds.transform.Find("overworld").gameObject;
        Globals.ChunkMaterialFloor = Resources.Load<Material>("Materials/BlockMaterialFloor");
        Globals.ChunkMaterialSolid = Resources.Load<Material>("Materials/BlockMaterialSolid");
        Globals.SpritesMaterial = Resources.Load<Material>("Materials/Sprites");
        Globals.ChunkMaterialFloor.SetFloat("_Daylight", 1.0f);
        Globals.ChunkMaterialSolid.SetFloat("_Daylight", 1.0f);
        Globals.SpritesMaterial.SetFloat("_Daylight", 1.0f);
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

    void EnableWorld(string world, int underground)
    {
            DisableWorlds();
            Globals.CurrentWorld = _worlds.transform.Find(world).gameObject;
            Globals.ChunkMaterialFloor.SetInt("_IsUnderground", underground);
            Globals.ChunkMaterialSolid.SetInt("_IsUnderground", underground);
            Globals.CurrentWorld.SetActive(true);
    }

    void Update()
    {
        LoadAroundPlayer();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            EnableWorld("overworld", 0);
            Globals.CurrentWorldId = WorldsIds.overworld;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            EnableWorld("low_depth", 1);
            Globals.CurrentWorldId = WorldsIds.low_depth;
        }

        UpdateDayLight();
    }

    void UpdateDayLight()
    {
        if (_DayLightIncreasing)
            SetDaylight(_DayLight + Time.deltaTime * 0.1f);
        else
            SetDaylight(_DayLight - Time.deltaTime * 0.1f);
    }

    void SetDaylight(float daylight)
    {
        Globals.ChunkMaterialFloor.SetFloat("_Daylight", daylight);
        Globals.ChunkMaterialSolid.SetFloat("_Daylight", daylight);
        Globals.SpritesMaterial.SetFloat("_Daylight", daylight);
        
        _DayLight = daylight;

        if (_DayLight <= 0.0f)
        {
            _DayLight = 0.0f;
            _DayLightIncreasing = true;
        }
        if (_DayLight >= 1.0f)
        {
            _DayLight = 1.0f;
            _DayLightIncreasing = false;
        }
    }
}
