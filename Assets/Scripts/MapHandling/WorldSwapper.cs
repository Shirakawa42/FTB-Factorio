using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldSwapper : MonoBehaviour
{
    private GameObject _worlds;

    void Awake()
    {
        _worlds = GameObject.Find("Worlds");
        Globals.CurrentWorld = _worlds.transform.Find("overworld").gameObject;
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
    }
}
