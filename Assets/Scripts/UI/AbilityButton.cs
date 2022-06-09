using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : Button
{
    private Character character;
    private Ability ability;

    public void SetAbility(Character character, Ability ability)
    {
        this.character = character;
        this.ability = ability;
        this.image.sprite = ability.Sprite;
        onClick.AddListener(() => character.SendSelectedAbilityToParticipant(ability));
    }
}
