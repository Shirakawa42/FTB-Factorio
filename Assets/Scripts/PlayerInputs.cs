using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private Item EquippedItem = null;
    private SpriteRenderer EquippedItemSpriteRenderer = null;
    private Animation _animation = null;
    private const float _cooldownMaxRight = 0.05f;
    private const float _cooldownMaxLeft = 0.05f;
    private float _cooldownRight = 0f;
    private float _cooldownLeft = 0f;
    private bool _inventoryStatus = false;
    private Inventory _inventory;

    void Start()
    {
        EquippedItemSpriteRenderer = transform.Find("EquippedItem").GetComponent<SpriteRenderer>();
        _animation = EquippedItemSpriteRenderer.GetComponent<Animation>();

        EquippedItem = ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Iron);

        _inventory = Globals.Canvas.GetComponent<Initializor>().Inventory.GetComponent<Inventory>();
    }

    void Update()
    {
        Inputs();
        if (EquippedItem != null)
            EquippedItem.EquippedEffect();
    }

    public void SetEquippedItem(Item item)
    {
        if (EquippedItem != null)
            EquippedItem.UnequippedEffect();

        EquippedItem = item;
        if (EquippedItem == null)
            EquippedItemSpriteRenderer.sprite = null;
        else
            EquippedItemSpriteRenderer.sprite = EquippedItem.Sprite;
    }

    private void Inputs()
    {
        if (_cooldownRight > 0)
            _cooldownRight -= Time.deltaTime;
        if (_cooldownLeft > 0)
            _cooldownLeft -= Time.deltaTime;

        if (Input.GetMouseButton(0) && EquippedItem != null && _cooldownLeft <= 0)
        {
            EquippedItem.LeftClick(_animation);
            _cooldownLeft = _cooldownMaxLeft;
        }
        else if (Input.GetMouseButton(1) && EquippedItem != null && _cooldownRight <= 0)
        {
            EquippedItem.RightClick(_animation);
            _cooldownRight = _cooldownMaxRight;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_inventoryStatus)
                _inventory.CloseInventory();
            else
                _inventory.OpenInventory();
            _inventoryStatus = !_inventoryStatus;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _inventoryStatus = false;
            _inventory.CloseInventory();
        }
    }
}
