using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePool : MonoBehaviour
{
    private readonly Stack<GameObject> _spritePool = new();
    private Transform _parent;
    private int _treeCount = 0;

    private GameObject GenerateSpriteObj(bool active, Transform parent)
    {
        GameObject spriteObj = new("Tree");
        spriteObj.SetActive(active);
        SpriteRenderer spriteRenderer = spriteObj.AddComponent<SpriteRenderer>();
        spriteRenderer.material = Globals.SpritesMaterial;
        spriteRenderer.sortingOrder = _treeCount;
        spriteObj.transform.SetParent(parent);
        spriteObj.transform.localScale = new Vector3(2, 2, 1);
        _treeCount++;
        
        return spriteObj;
    }

    void Awake()
    {
        _parent = GameObject.Find("SpritePool").transform;

        for (int i = 0; i < 500; i++)
            _spritePool.Push(GenerateSpriteObj(false, _parent));
    }

    public GameObject GetSpriteObj(Sprite sprite, Vector3 localPosition, Transform parent, float scale, Vector2 offset, string layer)
    {
        GameObject spriteObj;
        if (_spritePool.Count == 0)
            spriteObj = GenerateSpriteObj(true, parent);
        else
            spriteObj = _spritePool.Pop();
        
        spriteObj.transform.SetParent(parent);
        spriteObj.transform.localPosition = localPosition + new Vector3(0.5f + offset.x, 0.5f + offset.y, 0);
        spriteObj.transform.localScale = new Vector3(scale, scale, 1);
        spriteObj.GetComponent<SpriteRenderer>().sprite = sprite;
        spriteObj.GetComponent<SpriteRenderer>().sortingLayerName = layer;
        spriteObj.SetActive(true);
        return spriteObj;
    }

    public void ReturnSprite(GameObject spriteObj)
    {
        spriteObj.SetActive(false);
        spriteObj.transform.SetParent(_parent);
        _spritePool.Push(spriteObj);
    }
}