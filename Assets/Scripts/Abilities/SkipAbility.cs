using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipAbility : Ability
{
    [SerializeField]
    private int heal;

    public override List<TargetType> TargetsTypes => new List<TargetType>();

    public override void Execute(List<Character> targets, Spine.Event e)
    {
        Owner.Health += heal;
    }
}
