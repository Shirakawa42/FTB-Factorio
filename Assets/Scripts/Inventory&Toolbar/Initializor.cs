using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializor : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Toolbar;

    private GameObject _SlotPrefab;
    private GameObject _HighlightPrefab;
    private GameObject _ToolbarPrefab;
    private GameObject _InventoryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _SlotPrefab = Resources.Load<GameObject>("Prefabs/Slot");
        _HighlightPrefab = Resources.Load<GameObject>("Prefabs/Highlight");
        _ToolbarPrefab = Resources.Load<GameObject>("Prefabs/Toolbar");
        _InventoryPrefab = Resources.Load<GameObject>("Prefabs/Inventory");

        Toolbar = Instantiate(_ToolbarPrefab, transform);
        Inventory = Instantiate(_InventoryPrefab, transform);

        Inventory.GetComponent<Inventory>().Init(Inventory);

        InitSlots();

        Toolbar.GetComponent<Toolbar>().SetHighlight(_HighlightPrefab);
    }

    private void InitSlots()
    {
        float slotWidth = _SlotPrefab.GetComponent<RectTransform>().rect.width;
        float leftBorderX = Toolbar.GetComponent<RectTransform>().anchoredPosition.x - Toolbar.GetComponent<RectTransform>().rect.width / 2f;
        float toolbarWidth = slotWidth * Globals.ToolbarSlots;
        float spacing = toolbarWidth / Globals.ToolbarSlots;

        List<Slot> toolbarSlots = Toolbar.GetComponent<Toolbar>().toolbarSlots;
        for (int i = 0; i < Globals.ToolbarSlots; i++)
        {
            GameObject _slot = Instantiate(_SlotPrefab, Toolbar.transform);
            _slot.name = "Slot " + i;
            _slot.transform.SetParent(Toolbar.transform);
            _slot.transform.localPosition = new Vector3(spacing / 2 + leftBorderX + spacing * i, 0, 0);
            toolbarSlots.Add(_slot.GetComponent<Slot>());
            toolbarSlots[i].Init();
        }

        float slotHeight = _SlotPrefab.GetComponent<RectTransform>().rect.height;

        List<Slot> inventorySlots = Inventory.GetComponent<Inventory>().inventorySlots;
        for (int i = 0; i < Globals.InventorySlots; i++)
        {
            GameObject _slot = Instantiate(_SlotPrefab, Inventory.transform);
            _slot.name = "Slot " + i;
            _slot.transform.SetParent(Inventory.transform);
            _slot.transform.localPosition = new Vector3(spacing / 2 + leftBorderX + spacing * (i % Globals.ToolbarSlots), -(Mathf.Floor(i / Globals.ToolbarSlots) * slotHeight), 0);
            inventorySlots.Add(_slot.GetComponent<Slot>());
            inventorySlots[i].Init();
        }

        Inventory.GetComponent<Inventory>().TMPAddItemsToInventory();
    }
}
