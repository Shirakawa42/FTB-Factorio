using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites
{
    private readonly Dictionary<string, Sprite> _SpritesDict = new();

    public void InitSprites()
    {
        Sprite[] SpritesArray = Resources.LoadAll<Sprite>("Textures/3208");
        foreach (Sprite sprite in SpritesArray)
            _SpritesDict.Add(sprite.name, sprite);
    }

    public Sprite GetSprite(string name)
    {
        if (_SpritesDict.ContainsKey(name))
            return _SpritesDict[name];
        else
            return null;
    }
}
