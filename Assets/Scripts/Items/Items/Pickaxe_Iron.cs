using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe_Iron : PrimaryTool
{
    public override ushort Id { get => ItemIds.Pickaxe_Iron; }
    public override string Name { get => "Iron Pickaxe"; }
    public override string Description { get => "An Iron Pickaxe"; }
    public override int MaxStack { get => 1; }
    public override int CurrentStack { get => 1; set => CurrentStack = value; }
    public override Texture2D Texture2D { get => Resources.Load<Texture2D>("Textures/Items/Pickaxe_Iron"); }
    public override float MiningPower { get => 5; set => MiningPower = value; }
    public override BlockTypes BlockTypes { get => BlockTypes.Hard; }
    public override int MiningLevel { get => 3; }
}
