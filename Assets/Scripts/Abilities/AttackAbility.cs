using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : Ability
{
    [SerializeField]
    private int damage;

    public override List<TargetType> TargetsTypes => new List<TargetType>(){ TargetType.Opponent };

    public override void Execute(List<Character> targets, Spine.Event e)
    {
        if (e == null || e.Data.Name == "Hit")
            foreach(var target in targets)
                target.Health -= damage;
    }
}
