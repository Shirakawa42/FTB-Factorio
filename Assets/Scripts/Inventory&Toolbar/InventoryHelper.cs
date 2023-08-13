using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryHelper {
    public static bool AddItemToPlayer(Item item)
    {
        if (Globals.Canvas.GetComponent<Initializor>().Toolbar.GetComponent<Toolbar>().AddItemToExistingSlot(item) ||
            Globals.Canvas.GetComponent<Initializor>().Inventory.GetComponent<Inventory>().AddItemToExistingSlot(item) ||
            Globals.Canvas.GetComponent<Initializor>().Toolbar.GetComponent<Toolbar>().AddItemToFirstEmptySlot(item) ||
            Globals.Canvas.GetComponent<Initializor>().Inventory.GetComponent<Inventory>().AddItemToFirstEmptySlot(item))
            return true;

        return false;
    }
}
