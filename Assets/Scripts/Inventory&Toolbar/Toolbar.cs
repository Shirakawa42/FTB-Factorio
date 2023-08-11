using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar : MonoBehaviour
{
    public List<Slot> toolbarSlots = new();

    private GameObject Highlight;
    private Slot selectedSlot;
    private PlayerInputs _playerInputs = null;
    private readonly Item[] itemInSlots = new Item[Globals.ToolbarSlots];

    void Start()
    {
        _playerInputs = Globals.Player.GetComponent<PlayerInputs>();

        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Wood));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Stone));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Iron));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Gold));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Diamond));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Stone));

        SelectSlot(0);
    }

    public void SetHighlight(GameObject highlight)
    {
        Highlight = Instantiate(highlight);
    }

    public void AddItemToSlot(Item item, ushort slot)
    {
        if (slot + 1 > Globals.ToolbarSlots)
            return;

        toolbarSlots[slot].SetItem(item);
        itemInSlots[slot] = item;
    }

    public bool AddItemToExistingSlot(Item item) {
        for (ushort i = 0; i < Globals.ToolbarSlots; i++)
        {
            if (itemInSlots[i] != null && itemInSlots[i].Id == item.Id && itemInSlots[i].CurrentStack + item.CurrentStack <= itemInSlots[i].MaxStack)
            {
                itemInSlots[i].CurrentStack += item.CurrentStack;
                toolbarSlots[i].SetItem(itemInSlots[i]);

                return true;
            }
        }

        return false;
    }

    public bool AddItemToFirstEmptySlot(Item item)
    {
        for (ushort i = 0; i < Globals.ToolbarSlots; i++)
        {
            if (itemInSlots[i] == null)
            {
                AddItemToSlot(item, i);
                return true;
            }
        }

        return false;
    }

    private void SelectSlot(ushort index)
    {
        if (index + 1 > Globals.ToolbarSlots)
            return;

        if (itemInSlots[index] != null)
            _playerInputs.SetEquippedItem(itemInSlots[index]);
        else
            _playerInputs.SetEquippedItem(null);

        selectedSlot = toolbarSlots[index];

        Highlight.transform.SetParent(selectedSlot.transform);
        Highlight.transform.localScale = new Vector3(1, 1, 1);
        Highlight.transform.localPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SelectSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SelectSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SelectSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SelectSlot(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SelectSlot(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SelectSlot(8);
    }
}
