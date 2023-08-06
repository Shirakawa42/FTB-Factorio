using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private Item EquippedItem = null;
    private SpriteRenderer EquippedItemSpriteRenderer = null;
    private Animation _animation = null;
    private float _cooldownMax = 0.4f;
    private float _cooldown = 0f;

    void Start()
    {
        EquippedItemSpriteRenderer = transform.Find("EquippedItem").GetComponent<SpriteRenderer>();
        _animation = EquippedItemSpriteRenderer.GetComponent<Animation>();

        EquippedItem = ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Iron);
    }

    void Update()
    {
        Inputs();
    }

    public void SetEquippedItem(Item item)
    {
        EquippedItem = item;
        EquippedItemSpriteRenderer.sprite = item.Sprite;
    }

    private void Inputs()
    {
        if (_cooldown > 0)
            _cooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0) && EquippedItem != null && _cooldown <= 0)
        {
            EquippedItem.LeftClick(_animation);
            _cooldown = _cooldownMax;
        }
        else if (Input.GetMouseButton(1) && EquippedItem != null && _cooldown <= 0)
        {
            EquippedItem.RightClick(_animation);
            _cooldown = _cooldownMax;
        }
    }
}
