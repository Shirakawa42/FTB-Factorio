using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbar : MonoBehaviour
{
    [SerializeField] public List<Slot> toolbarSlots = new List<Slot>();
    
    private GameObject Highlight;
    private Slot selectedSlot;
    private PlayerInputs _playerInputs = null;

    Dictionary<ushort, ushort> itemIdInSlots = new Dictionary<ushort, ushort>();

    public void SetHighlight(GameObject highlight)
    {
        Highlight = Instantiate(highlight);
    }

    private void SelectSlot(ushort index) {
        if (index + 1 > Globals.ToolbarSlots) {
            return ;
        }

        if (itemIdInSlots.ContainsKey(index)) {
            _playerInputs.SetEquippedItem(ItemInfos.GetItemFromId(itemIdInSlots[index]));
        } else {
            _playerInputs.SetEquippedItem(null);
        }

        selectedSlot = toolbarSlots[index];

        Highlight.transform.SetParent(selectedSlot.transform);
        Highlight.transform.localPosition = new Vector3(0, 0, 0);
    }
    
    void Start() {
        _playerInputs = Globals.Player.GetComponent<PlayerInputs>();

        // Add Item in toolbar
        itemIdInSlots.Add(0, 0);
        itemIdInSlots.Add(1, 1);
        itemIdInSlots.Add(2, 2);

        // Set first selected slot
        SelectSlot(0);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectSlot(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectSlot(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectSlot(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectSlot(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectSlot(4);
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SelectSlot(5);
        } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SelectSlot(6);
        } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SelectSlot(7);
        } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SelectSlot(8);
        }
    }
}
