using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpritePool : MonoBehaviour
{
    private Sprite[] _sprites;
    private readonly Stack<GameObject> _treePool = new();
    private Transform _parent;
    private int _treeCount = 0;

    private GameObject GenerateTree(bool active, Transform parent, int spriteId)
    {
        GameObject tree = new("Tree");
        tree.SetActive(active);
        SpriteRenderer spriteRenderer = tree.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _sprites[spriteId];
        spriteRenderer.material = Globals.SpritesMaterial;
        spriteRenderer.sortingLayerName = "Tree";
        spriteRenderer.sortingOrder = _treeCount;
        tree.transform.SetParent(parent);
        tree.transform.localScale = new Vector3(2, 2, 1);
        _treeCount++;
        
        return tree;
    }

    void Awake()
    {
        _sprites = Resources.LoadAll<Sprite>("Textures/Trees");
        _parent = GameObject.Find("TreePool").transform;

        for (int i = 0; i < 500; i++)
            _treePool.Push(GenerateTree(false, _parent, 0));
    }

    public GameObject GetTree(int spriteId, Vector3 localPosition, Transform parent)
    {
        GameObject tree;
        if (_treePool.Count == 0)
            tree = GenerateTree(true, parent, spriteId);
        else
            tree = _treePool.Pop();
        
        tree.transform.SetParent(parent);
        tree.transform.localPosition = localPosition;
        tree.transform.localScale = new Vector3(2, 2, 1);
        tree.GetComponent<SpriteRenderer>().sprite = _sprites[spriteId];
        tree.SetActive(true);
        return tree;
    }

    public void ReturnTree(GameObject tree)
    {
        tree.SetActive(false);
        tree.transform.SetParent(_parent);
        _treePool.Push(tree);
    }
}