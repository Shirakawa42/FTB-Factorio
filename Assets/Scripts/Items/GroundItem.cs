using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    private Item _item = null;
    private SpriteRenderer _spriteRenderer = null;
    private const float _rotateSpeed = 6f;

    public void SetItem(Item item)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _item = item;
        _spriteRenderer.sprite = item.Sprite;
    }

    public Item GetItem()
    {
        return _item;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Player picked up ");
        if (col.tag == "Player") {
            if (InventoryHelper.AddItemToPlayer(_item))
                Destroy();
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);


    }
}
