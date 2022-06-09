using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;
using System.Linq;

public enum ParticipantSide
{
    Left,
    Right
}

public class Game : MonoBehaviour
{
    [SerializeField]
    private float timeForPrepareAndRemove;
    [SerializeField]
    private Image curtains;
    [SerializeField]
    private Text titleText;
    
    private static Game game;
    private static ParticipantGame leftParticipant;
    private static ParticipantGame rightParticipant;
    private static Character attacker;
    private static Ability ability;
    private static AbilityTargets targets;
    private static ParticipantGame winner;
    private static ParticipantGame mover;
    private const int TIME_DIVISIONS = 22;


    void Start()
    {
        game = this;
        TryStartGame();
    }

    public static void RegistrationParticipant(ParticipantGame participant, ParticipantSide side)
    {
        switch (side)
        {
            case ParticipantSide.Left:
                leftParticipant = participant;
                break;
            case ParticipantSide.Right:
                rightParticipant = participant;
                break;
        }
        Debug.Log($"{participant.Nickname} зашел за {side} сторону");
        TryStartGame();
    }

    private static void TryStartGame()
    {
        if (leftParticipant != null && rightParticipant != null && game != null) NextMove();
    }

    private static void NextMove()
    {
        mover?.RemoveTired();
        mover = Random.Range(0, 2) == 0? leftParticipant: rightParticipant;
        mover.MyTurn = true;
        game.titleText.text = $"{mover.Nickname} ходит";
    }

    public static void MakeBattle(Character _attacker, Ability _ability, AbilityTargets _targets)
    {
        attacker = _attacker;
        ability = _ability;
        targets = _targets;
        game.StartBattle();
    }

    public void StartBattle()
    {
        StartCoroutine(PrepareToBattle());
    }

    private IEnumerator PrepareToBattle()
    {
        mover.MyTurn = false;

        Color startCurtainsColor = curtains.color;
        Color finishCurtainsColor = new Color(curtains.color.r, curtains.color.g, curtains.color.b, 0.6f);

        attacker.transform.ZPositionTo(attacker.PodiumPlace.z);
        foreach (var target in targets.Targets)
            target.transform.ZPositionTo(target.PodiumPlace.z);
        float duration = timeForPrepareAndRemove/TIME_DIVISIONS;
        float timer = 0, progress = 0;
        for (timer = 0; timer < timeForPrepareAndRemove; timer += duration)
        {
            progress = timer/timeForPrepareAndRemove;

            curtains.color = Color.Lerp(startCurtainsColor, finishCurtainsColor, progress);
            attacker.transform.position = Vector3.Lerp(attacker.Place, attacker.PodiumPlace, progress);
            foreach (var target in targets.Targets) 
                target.transform.position = Vector3.Lerp(target.Place, target.PodiumPlace, progress);

            yield return new WaitForSeconds(duration);
        }
        ExecuteBattle();
    }

    private void  ExecuteBattle()
    {
        if (ability.Animations.Length > 0)
        {
            var abilityAnimation = ability.Animations[Random.Range(0, ability.Animations.Length)];
            TrackEntry track = attacker.SkeletonAnimation.AnimationState.SetAnimation(0, abilityAnimation, false);
            track.Event += OnAbilityExecute;
            track.Complete += OnAbilityEnd;
        }
        else
        {
            ability.Execute(targets.Targets, null);
            StartCoroutine(RemoveBattle());
        }

        attacker.IsTired = true;
    }

    private void OnAbilityExecute(TrackEntry track, Spine.Event e)
    {
        ability.Execute(targets.Targets, e);
    }

    private void OnAbilityEnd(TrackEntry track)
    {
        track.Event -= OnAbilityExecute;
        track.Complete -= OnAbilityEnd;
        attacker.SkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        StartCoroutine(RemoveBattle());
    }

    private IEnumerator RemoveBattle()
    {
        Color startCurtainsColor = curtains.color;
        Color finishCurtainsColor = new Color(curtains.color.r, curtains.color.g, curtains.color.b, 0);

        float duration = timeForPrepareAndRemove/TIME_DIVISIONS;
        float timer = 0, progress = 0;
        for (timer = 0; timer <= timeForPrepareAndRemove; timer += duration)
        {
            progress = timer/timeForPrepareAndRemove;

            curtains.color = Color.Lerp(startCurtainsColor, finishCurtainsColor, progress);
            attacker.transform.position = Vector3.Lerp(attacker.PodiumPlace, attacker.Place, progress);
            foreach (var target in targets.Targets) 
                if (target != null)target.transform.position = Vector3.Lerp(target.PodiumPlace, target.Place, progress);


            yield return new WaitForSeconds(duration);
        }
        attacker.transform.ZPositionTo(attacker.Place.z);
        foreach (var target in targets.Targets)
            if (target != null) target.transform.ZPositionTo(target.Place.z);


        ContinueGame();
    }

    private static void ContinueGame()
    {
        if (leftParticipant.Army.Count == 0)
        {
            winner = rightParticipant;
            EndGame();
        }
        else if (rightParticipant.Army.Count == 0)
        {
            winner = leftParticipant;
            EndGame();
        }
        else if (mover.ImTired)
        {
            NextMove();
        }
        else mover.MyTurn = true;
    }

    private static void EndGame()
    {
        game.titleText.text = $"Победитель {winner.Nickname}";
        Debug.Log($"Игра закончена. Победитель {winner.Nickname}");
    }

    private static void RemoveParticipantsMoves()
    {
        leftParticipant.MyTurn = false;
        rightParticipant.MyTurn = false;
    }

    public static ParticipantGame GetOpponent(ParticipantGame participant)
    {
        Debug.Log(leftParticipant.Nickname);
        Debug.Log(rightParticipant.Nickname);
        Debug.Log(participant.Nickname);
        Debug.Log($"у {participant.Nickname} враг {(participant == leftParticipant? rightParticipant: leftParticipant).Nickname}");
        return participant == leftParticipant? rightParticipant: leftParticipant;
    }
}
