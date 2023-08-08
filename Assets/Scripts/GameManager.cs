using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Vector3 _playerPosition;
    private Vector2Int _playerChunkPosition;

    void Awake()
    {
        Globals.Player = GameObject.Find("Player");
        Globals.BlockBreaking = GameObject.Find("BlockBreaking");
        _playerChunkPosition = new Vector2Int(int.MaxValue, int.MaxValue);
    }

    private void LoadAroundPlayer()
    {
        // _playerPosition = Globals.Player.transform.position;
        Vector2Int chunkPosition = new((int)_playerPosition.x / Globals.ChunkSize, (int)_playerPosition.y / Globals.ChunkSize);

        if (chunkPosition != _playerChunkPosition)
        {
            _playerChunkPosition = chunkPosition;
            Map.LoadAroundChunkPosition(_playerChunkPosition);
        }
    }

    void Update()
    {
        LoadAroundPlayer();
    }
}
