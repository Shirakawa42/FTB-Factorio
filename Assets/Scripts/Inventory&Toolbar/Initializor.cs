using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializor : MonoBehaviour
{
    public GameObject Highlight;
    public GameObject Slot;
    private GameObject Toolbar;

    // Start is called before the first frame update
    void Start() {
        float slotWidth = Slot.GetComponent<RectTransform>().rect.width;
        float toolbarWidth = slotWidth * Globals.ToolbarSlots;

        Toolbar = new GameObject("Toolbar");
        Toolbar.transform.SetParent(this.transform);
        Toolbar.transform.localPosition = new Vector3(0, -750, 0);
        Toolbar.AddComponent<RectTransform>();
        Toolbar.AddComponent<Toolbar>();
        Toolbar.GetComponent<RectTransform>().sizeDelta = new Vector2(toolbarWidth, 80);

        float leftBorderX = Toolbar.GetComponent<RectTransform>().anchoredPosition.x - Toolbar.GetComponent<RectTransform>().rect.width / 2f;
        float spacing = toolbarWidth / Globals.ToolbarSlots;

        List<Slot> toolbarSlots = Toolbar.GetComponent<Toolbar>().toolbarSlots;

        for (int i = 0; i < Globals.ToolbarSlots; i++) {
            // create a new slot and assign it to the toolbar, use it like it should be centered
            GameObject _slot = Instantiate(Slot, Toolbar.transform);
            _slot.name = "Slot " + i;
            _slot.transform.SetParent(Toolbar.transform);

            // Calculate the position for the slot
            _slot.transform.localPosition = new Vector3(spacing/2 + leftBorderX + spacing * i, 0, 0);

            toolbarSlots.Add(_slot.GetComponent<Slot>());
        }

        Toolbar.GetComponent<Toolbar>().SetHighlight(Highlight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
