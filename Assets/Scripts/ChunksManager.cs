using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChunksManager : MonoBehaviour
{
    private Vector2Int _playerChunkPosition;
    private WorldsIds _oldWorldId = WorldsIds.overworld;

    void Start()
    {
        _playerChunkPosition = new Vector2Int(int.MaxValue, int.MaxValue);
    }

    private void LoadAroundPlayer()
    {
        Vector3 _playerPosition = Globals.Player.transform.position;
        Vector2Int chunkPosition = new((int)_playerPosition.x / Globals.ChunkSize, (int)_playerPosition.y / Globals.ChunkSize);

        if (chunkPosition != _playerChunkPosition || Globals.CurrentWorldId != _oldWorldId)
        {
            _oldWorldId = Globals.CurrentWorldId;
            _playerChunkPosition = chunkPosition;
            Map.LoadAroundChunkPosition(_playerChunkPosition, Globals.CurrentWorldId);
        }
    }

    void Update()
    {
        LoadAroundPlayer();
    }
}
