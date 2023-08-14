using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Data/Character")]
public class CharacterData : ScriptableObject, IDamageable
{
    public ModifiableStat maxHealth;
    public ModifiableStat Speed;
    public ModifiableStat Luck;
    public ModifiableStat Power;
    public ModifiableStat CooldownReduction;
    public ModifiableStat Defence;

    private float _currentHealth;

    public event Action OnCharacterDeath = delegate { };
    public event Action<float> OnCharacterDamage = delegate { };
    public event Action OnStatChangeCallback = delegate { };

    public float MaxHealth => maxHealth.Value;
    public float CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    private void OnEnable()
    {
        maxHealth.OnChangeCallback += InvokeCallback;
        Speed.OnChangeCallback += InvokeCallback;
        Luck.OnChangeCallback += InvokeCallback;
        Power.OnChangeCallback += InvokeCallback;
        CooldownReduction.OnChangeCallback += InvokeCallback;
        Defence.OnChangeCallback += InvokeCallback;
    }

    private void OnDisable()
    {
        maxHealth.OnChangeCallback -= InvokeCallback;
        Speed.OnChangeCallback -= InvokeCallback;
        Luck.OnChangeCallback -= InvokeCallback;
        Power.OnChangeCallback -= InvokeCallback;
        CooldownReduction.OnChangeCallback -= InvokeCallback;
        Defence.OnChangeCallback -= InvokeCallback;
    }

    public void Damage(float damage)
    {
        if (CurrentHealth <= 0)
        {
            OnCharacterDeath.Invoke();
            return; // character dead
        }
        CurrentHealth -= damage;
        OnCharacterDamage.Invoke(_currentHealth);
    }

    public void Heal(float amount)
    {
        if (CurrentHealth >= MaxHealth)
        {
            return; // player full hp
        }
        CurrentHealth += amount;
        OnCharacterDamage.Invoke(_currentHealth);
    }

    public void ResetData()
    {
        maxHealth.RemoveAllModifiers();
        Speed.RemoveAllModifiers();
        Luck.RemoveAllModifiers();
        Power.RemoveAllModifiers();
        CooldownReduction.RemoveAllModifiers();
        Defence.RemoveAllModifiers();
        _currentHealth = maxHealth.Value;
    }

    private void InvokeCallback()
    {
        OnStatChangeCallback.Invoke();
    }
}
