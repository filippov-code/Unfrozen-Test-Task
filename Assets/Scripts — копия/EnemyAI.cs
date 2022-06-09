using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : ParticipantGame
{
    public override bool MyTurn
    {
        get => myTurn;
        set
        {
            myTurn = value;
            if (myTurn) MakeMove();
        }
    }


    IEnumerator Start()
    {
        SetManagerToCharacters();
        yield return new WaitForSeconds(1);
        RegistrationInGame();
    }

    private void SetManagerToCharacters()
    {
        foreach (var character in army)  
        {
            character.SetManager(this);
            character.OnDead += OnCharacterDead;
        } 
    }


    public override void SetSelectedAbility(Ability ability)
    {
        
    }

    private void MakeMove()
    {
        Character attacker = army.First(x => !x.IsTired);
        Ability ability = attacker.abilities.Random();
        AbilityTargets abilityTargets = ability.GetTargetsForFill();

        while(!abilityTargets.IsReady())
        {
            TargetType? neededType = abilityTargets.GetNextNeededTargetType();
            if (neededType == null) break;
            if (neededType == TargetType.Ally)
            {
                Character ally = Army.Random();
                abilityTargets.Add(ally, neededType.Value);
            }
            else if (neededType == TargetType.Opponent)
            {
                Character opponent = Game.GetOpponent(this).Army.Random();
                abilityTargets.Add(opponent, neededType.Value);
            }
        }

        Game.MakeBattle(attacker, ability, abilityTargets);
    }
}
