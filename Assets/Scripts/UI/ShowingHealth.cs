using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowingHealth : MonoBehaviour
{
    [SerializeField]
    private Image healthStrip;
    private Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
        character.OnHealthChanged += UpdateHealthStrip;
        character.OnDead += Disappear;
    }

    public void UpdateHealthStrip()
    {
        healthStrip.fillAmount = character.Health/(float)character.MaxHealth;
    }

    public void Disappear(Character character) => Destroy(gameObject);
    
}
