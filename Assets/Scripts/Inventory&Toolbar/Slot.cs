using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Item _item = null;
    private SpriteRenderer _spriteRenderer = null;

    void Start()
    {
        _spriteRenderer = transform.Find("Item").GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item)
    {
        _item = item;
        _spriteRenderer.sprite = item.Sprite;
    }
}
