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

        AddItemToSlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Wood), 0);
        AddItemToSlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Stone), 1);
        AddItemToSlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Iron), 2);
        AddItemToSlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Gold), 3);
        AddItemToSlot(ItemInfos.GenerateItemFromId(ItemIds.Pickaxe_Diamond), 4);

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
