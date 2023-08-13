using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    public List<Slot> inventorySlots = new();

    private readonly Item[] itemInSlots = new Item[Globals.InventorySlots];

    public void RemoveItemFromInventory(ushort slot)
    {
        if (slot + 1 > Globals.InventorySlots)
            return;

        inventorySlots[slot].SetItem(null);
        itemInSlots[slot] = null;
    }

    public void AddItemToInventory(Item item, ushort slot)
    {
        if (slot + 1 > Globals.InventorySlots)
            return;

        inventorySlots[slot].SetItem(item);
        itemInSlots[slot] = item;
    }

    public bool AddItemToExistingSlot(Item item) {
        for (ushort i = 0; i < Globals.InventorySlots; i++)
        {
            if (itemInSlots[i] != null && itemInSlots[i].Id == item.Id && itemInSlots[i].CurrentStack + item.CurrentStack <= itemInSlots[i].MaxStack)
            {
                itemInSlots[i].CurrentStack += item.CurrentStack;
                inventorySlots[i].SetItem(itemInSlots[i]);

                return true;
            }
        }

        return false;
    }

    public bool AddItemToFirstEmptySlot(Item item)
    {
        for (ushort i = 0; i < Globals.InventorySlots; i++)
        {
            if (itemInSlots[i] == null)
            {
                AddItemToInventory(item, i);
                return true;
            }
        }

        return false;
    }

    public void TMPAddItemsToInventory()
    {
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Wood));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Wood));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Wood));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Wood));
        AddItemToFirstEmptySlot(ItemInfos.GenerateItemFromId(ItemIds.Stone));
        RemoveItemFromInventory(2);
    }

    public void Init(GameObject inventoryGO)
    {
        inventory = inventoryGO;
        inventory.SetActive(false);
    }

    public void CloseInventory()
    {
        inventory.SetActive(false);
    }

    public void OpenInventory()
    {
        inventory.SetActive(true);
    }

}
