using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        new Chunk(new Vector2Int(0, 0), WorldsIds.overworld, ChunkTypes.Floor);
    }
}
