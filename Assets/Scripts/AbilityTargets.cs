using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargets
{
    private List<TargetType> types;
    private List<Character> targets = new();


    public List<Character> Targets => targets;


    public AbilityTargets(List<TargetType> types)
    {
        this.types = types;
    }

    public bool Add(Character character, TargetType  type)
    {
        if (IsReady()) return false;
        TargetType? neededTargetType = GetNextNeededTargetType();
        if (neededTargetType == null) return false;
        if (type == neededTargetType) 
        {
            targets.Add(character);
            return true;
        }
        else return false;
    }

    public bool IsReady()// => types.Count == targets.Count;
    {
        if (types.Count == targets.Count) return true;
        else return false;
    }

    public TargetType? GetNextNeededTargetType()// => IsReady()?null:types[targets.Count];
    {
        if (IsReady()) return null;
        return types[targets.Count];
    }
}
