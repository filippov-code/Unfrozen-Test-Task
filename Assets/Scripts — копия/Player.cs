using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : ParticipantGame
{
    private Character selectedCharacter;
    private Ability selectedAbility;
    private AbilityTargets targets;
  
    public override bool MyTurn 
    {
        get => myTurn;
        set 
        {
            myTurn = value;
            if (!myTurn) selectedCharacter?.SetActiveAbilitiesPanel(false);
        }
    }


    void Start()
    {
        SetManagerToCharacters();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && MyTurn)
        {
            Vector3 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector3.back);
            if (hit.collider != null)
            {
                if (hit.transform.TryGetComponent<Character>(out var character))
                {
                    if(selectedAbility != null)
                    {
                        //выбрали цель для способности
                        TargetType targetType = army.Contains(character)?
                                                TargetType.Ally:
                                                TargetType.Opponent;

                        targets.Add(character, targetType);

                        TryMakeBattle();
                        return;
                        
                    }
                    else if (!army.Contains(character)) return;

                    if (character.IsTired) return;
                    if (selectedCharacter != null)
                    {
                        if (selectedCharacter != character)
                        {
                            //выбрали нового перснажа
                            selectedCharacter.SetActiveAbilitiesPanel(false);
                            selectedCharacter = character;
                            character.SetActiveAbilitiesPanel(true);
                        }
                        else
                        {
                            //выбрали старого персонажа
                            selectedCharacter.SetActiveAbilitiesPanel(false);
                            selectedCharacter = null;
                        }
                    }
                    else 
                    {
                        //выбрали первого персонажа
                        
                        selectedCharacter = character;
                        selectedCharacter.SetActiveAbilitiesPanel(true);
                    }
                    
                }
            }
        }
    }

    public override void SetSelectedAbility(Ability ability)
    {
        selectedAbility = ability;
        targets = ability.GetTargetsForFill();

        TryMakeBattle();
    }

    private void TryMakeBattle()
    {
        if (targets.IsReady()) 
        {
            Game.MakeBattle(selectedCharacter, selectedAbility, targets);
            SelectionToNull();
        }
    }

    private void SelectionToNull()
    {
        selectedCharacter?.SetActiveAbilitiesPanel(false);
        selectedCharacter = null;
        selectedAbility = null;
        targets = null;
    }
}
