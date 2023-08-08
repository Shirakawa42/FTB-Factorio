using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites
{
    public Sprite[] SpritesArray = Resources.LoadAll<Sprite>("Textures/3208");

    public Sprite GetSprite(string name)
    {
        foreach (Sprite sprite in SpritesArray)
            //if (sprite.name == id.ToString())
                return sprite;
        return null;
    }
}
