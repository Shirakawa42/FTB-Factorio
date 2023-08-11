using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Sprite DefaultSlotSprite = null;
    public Item _item = null;
    private Image _Image = null;

    public void Init()
    {
        _Image = transform.Find("Item").GetComponent<Image>();
        DefaultSlotSprite = _Image.sprite;
    }

    public void SetItem(Item item)
    {
        if (item == null) {
            _item = null;
            _Image.sprite = DefaultSlotSprite;
            return;
        }

        _item = item;
        _Image.sprite = item.Sprite;
    }
}
