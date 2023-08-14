using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Combo Ability", menuName ="Abilities/Combo Ability")]
public class ComboAbility : AbstractAbilityObject
{
    [SerializeField] private float _resetComboTime;
    [SerializeField] private List<AbstractAbilityObject> _abilitySequence;

    public int SequenceLength { get { return _abilitySequence.Count; } }
    public float ResetComboTime { get { return _resetComboTime; } }

    public AbstractAbilityObject ReturnAbility(int index)
    {
        return _abilitySequence[index];
    }

    public override void Activate(float charge)
    {
        ComboHandler.instance.AbilityToActivate(this).Activate(charge);
        // cooldown handled by ComboHandler
    }

    public override void Init(AbilityController player)
    {
        foreach (Ability ability in _abilitySequence)
        {
            ability.Init(player);
        }
    }
}
