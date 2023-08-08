using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item _item = null;
    private Image _Image = null;

    public void Init()
    {
        _Image = transform.Find("Item").GetComponent<Image>();
    }

    public void SetItem(Item item)
    {
        _item = item;
        _Image.sprite = item.Sprite;
    }
}
