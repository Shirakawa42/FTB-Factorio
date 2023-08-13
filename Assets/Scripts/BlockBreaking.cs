using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaking : MonoBehaviour
{
    public Sprite BreakingSprite1;
    public Sprite BreakingSprite2;
    public Sprite BreakingSprite3;
    public Sprite BreakingSprite4;
    public Sprite BreakingSprite5;
    public Sprite BreakingSprite6;

    private Vector2Int _lastAttackedBlockPosition = new(0, 0);
    private PrimaryBlocks _lastAttackedBlock = null;
    private WorldsIds _lastAttackedBlockWorldId = WorldsIds.overworld;
    private float _timeBeforeReset = 1.5f;
    private const float _timeBeforeResetMax = 1.5f;
    private SpriteRenderer _spriteRenderer;
    private GameObject _groundItemPrefab;

    private void GenerateGroundItem(Vector2Int blockWorldPositionInt, PrimaryBlocks block)
    {
        float randomOffsetX = Random.Range(-.25f, .25f) + .5f;
        float randomOffsetY = Random.Range(-.25f, .25f) + .5f;
        GameObject groundItem = Instantiate(_groundItemPrefab, new Vector3(blockWorldPositionInt.x + randomOffsetX, blockWorldPositionInt.y + randomOffsetY, 0), Quaternion.identity);
        groundItem.GetComponent<SpriteRenderer>().material = Globals.SpritesMaterial;
        groundItem.transform.SetParent(Globals.CurrentWorld.transform.Find("grounditems"));
        groundItem.GetComponent<GroundItem>().SetItem(ItemInfos.GenerateItemFromId(block.DropId));
    }

    public void AttackBlock(Vector3 blockWorldPosition, float miningPower, BlockTypes blockType, int miningLevel, WorldsIds worldId, ChunkTypes chunkType)
    {
        bool willLoot = true;
        Vector2Int blockWorldPositionInt = WorldsHelper.WorldPositionToVector2Int(blockWorldPosition);

        PrimaryBlocks block = WorldsHelper.GetBlockStats(blockWorldPosition, worldId, chunkType);

        if (block.Id == ItemIds.Air)
            return;

        if (_lastAttackedBlock != null && (_lastAttackedBlockPosition != blockWorldPositionInt || _lastAttackedBlockWorldId != worldId))
            _lastAttackedBlock.Hp = _lastAttackedBlock.HpMax;

        _lastAttackedBlockPosition = blockWorldPositionInt;
        _lastAttackedBlock = block;
        _lastAttackedBlockWorldId = worldId;
        _timeBeforeReset = _timeBeforeResetMax;
        if (blockType != block.BlockType || miningLevel < block.SolidityLevel)
        {
            willLoot = false;
            miningPower /= 4;
            miningPower = Mathf.Max(miningPower, 1f);
        }
        block.Hp -= (short)Mathf.FloorToInt(miningPower);

        Globals.BlockBreaking.transform.position = new Vector3(blockWorldPositionInt.x + .5f, blockWorldPositionInt.y + .5f, 0f);

        if (block.Hp <= 0)
        {
            _lastAttackedBlock.Hp = _lastAttackedBlock.HpMax;
            Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = null;
            WorldsHelper.SetBlock(blockWorldPosition, ItemIds.Air, worldId, chunkType);
            if (willLoot)
            {
                GenerateGroundItem(blockWorldPositionInt, block);
            }
        }
        else if (block.GetHpPercentage() <= 1f / 6f)
            Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = BreakingSprite6;
        else if (block.GetHpPercentage() <= 2f / 6f)
            Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = BreakingSprite5;
        else if (block.GetHpPercentage() <= 3f / 6f)
            Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = BreakingSprite4;
        else if (block.GetHpPercentage() <= 4f / 6f)
            Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = BreakingSprite3;
        else if (block.GetHpPercentage() <= 5f / 6f)
            Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = BreakingSprite2;
        else
            Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = BreakingSprite1;
    }

    void Start()
    {
        _spriteRenderer = Globals.BlockBreaking.GetComponent<SpriteRenderer>();
        _groundItemPrefab = Resources.Load<GameObject>("Prefabs/GroundItem");
    }

    void Update()
    {
        if (_spriteRenderer.sprite != null && _timeBeforeReset > 0)
        {
            _timeBeforeReset -= Time.deltaTime;
            if (_timeBeforeReset <= 0)
            {
                _timeBeforeReset = _timeBeforeResetMax;
                Globals.BlockBreaking.GetComponent<SpriteRenderer>().sprite = null;
                _lastAttackedBlock.Hp = _lastAttackedBlock.HpMax;
            }
        }
    }
}
