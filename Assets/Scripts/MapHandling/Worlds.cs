using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldsIds
{
    overworld = 0,
    low_depth = 1,
    medium_depth = 2,
    deep_depth = 3,
    hardcore_depth = 4
}

public static class Worlds
{
    public static GameObject GetWorldFromId(WorldsIds id)
    {
        if (id == WorldsIds.overworld)
            return GameObject.Find("overworld");
        else if (id == WorldsIds.low_depth)
            return GameObject.Find("low_depth");
        else if (id == WorldsIds.medium_depth)
            return GameObject.Find("medium_depth");
        else if (id == WorldsIds.deep_depth)
            return GameObject.Find("deep_depth");
        else if (id == WorldsIds.hardcore_depth)
            return GameObject.Find("hardcore_depth");
        else
            throw new System.Exception("World id does not exist");
    }
}
