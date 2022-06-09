using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ParticipantGame : MonoBehaviour
{
    [SerializeField]
    protected string nickname;
    [SerializeField]
    protected ParticipantSide side;
    [SerializeField]
    protected List<Character> army = new();
    [SerializeField]
    protected Transform podium;
    protected bool myTurn;

    public Transform Podium => podium;

    public List<Character> Army => army;

    public string Nickname => nickname;

    public bool ImTired => army.Where(x => !x.IsTired).ToList().Count == 0;


    protected void RegistrationInGame() => Game.RegistrationParticipant(this, side);

    public void OnCharacterDead(Character character) => army.Remove(character);

    public void RemoveTired() => army.ForEach(x => x.IsTired = false);

    public abstract void SetSelectedAbility(Ability ability);

    public abstract bool MyTurn {get; set;}



}
