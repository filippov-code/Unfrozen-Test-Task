using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;
using Spine;

public abstract class Ability : MonoBehaviour
{   
    [SerializeField]
    private AnimationReferenceAsset[] animations = new AnimationReferenceAsset[0];
    [SerializeField]
    private string title;
    private Sprite sprite;
    private List<Character> targets = new();
    private Character owner;

    public string Title => title;

    public Sprite Sprite
    {
        get 
        {
            if (sprite == null)
            {
                var spriteR = GetComponent<SpriteRenderer>();
                if (spriteR == null) throw new NullReferenceException();
                sprite = spriteR.sprite;
            }

            return sprite;
        }
    }
    
    public Character Owner => owner;

    public AnimationReferenceAsset[] Animations => animations;

    public abstract List<TargetType> TargetsTypes{get;}

    public abstract void Execute(List<Character> targets, Spine.Event e);

    public void SetOwner(Character character) => owner = character;

    public AbilityTargets GetTargetsForFill() => new AbilityTargets(TargetsTypes);
    
}

public enum TargetType
{
    Ally,
    Opponent
}
