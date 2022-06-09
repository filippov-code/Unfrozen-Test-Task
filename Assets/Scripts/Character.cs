using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using Spine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int maxHealth;
    private int health;
    private bool isDead;
    private Ability selectedAbility;
    private ParticipantGame manager;
    private SkeletonAnimation skeletonAnimation;
    [Header("Ссылки")]
    public ShowingHealthChanges showingHealthChanges;
    public Transform showingHealthChangesPoint;
    public Ability[] abilities = new Ability[2];
    public Transform abilitiesPanel;
    public Transform abilitiesContainer;
    public AbilityButton abilityButton;


    public int Health
    { 
        get => health;
        set
        {
            if (isDead) return;
            int difference = value - health;
            if (difference == 0) return;
            var shc = Instantiate(showingHealthChanges, showingHealthChangesPoint.position, Quaternion.identity);
            shc.SetHealthChanges(difference);
            if (difference < 0)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Damage", false);
                skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 0);
            }

            health = Mathf.Clamp(value, 0, maxHealth);
            if (health == 0) IsDead = true;
            OnHealthChanged?.Invoke();
        }
    }

    public int MaxHealth => maxHealth;

    public bool IsDead 
    {
        get => isDead;
        private set
        {
            isDead = value;
            if (isDead) 
            {
                skeletonAnimation.Skeleton.SetSkin("blood");
                skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                skeletonAnimation.LateUpdate();
                TrackEntry track = skeletonAnimation.AnimationState.SetAnimation(0,"Pull", false);
                track.Complete += OnDeadAnimationComplete;
                OnDead?.Invoke(this);
            }
        }
    }

    public bool IsTired {get; set;}
    
    public Action OnHealthChanged {get; set;}

    public Action<Character> OnDead {get; set;}

    public Vector3 Place {get; private set;}

    public Vector3 PodiumPlace => manager.Podium.position;

    public SkeletonAnimation SkeletonAnimation => skeletonAnimation;


    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        SetActiveAbilitiesPanel(false);
        FillAbilitiesContainer();
        Place = transform.position;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        health = maxHealth;
    }

    private void FillAbilitiesContainer()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            var temp = abilities[i];
            abilities[i] = Instantiate(temp, transform);
            abilities[i].gameObject.SetActive(false);

            var newAbilityButton = Instantiate(abilityButton, abilitiesContainer);
            newAbilityButton.SetAbility(this, abilities[i]);
            //if (abilities[i].owner != null) Debug.Log($"{name}:opaa {abilities[i].name}/{abilities[i].owner.name}");
            abilities[i].SetOwner(this);
        }
    }

    public void SetActiveAbilitiesPanel(bool active)
    {
        abilitiesPanel.gameObject.SetActive(active);
    }

    public void SendSelectedAbilityToParticipant(Ability ability) => manager.SetSelectedAbility(ability);

    public void SetManager(ParticipantGame manager)
    {
        this.manager = manager;
    }  

    private void OnDeadAnimationComplete(TrackEntry track) => Disappear();

    private void Disappear() => Destroy(gameObject);
}
