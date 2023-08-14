using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName ="New Ability", menuName ="Abilities/Ability")]
public class Ability : AbstractAbilityObject
{
    [HideInInspector] public List<AbilityBehavior> _behaviors = new List<AbilityBehavior>();
    public override async void Activate(float charge)
    {
        Debug.Log("Ability Activate");
        foreach(AbilityBehavior command in _behaviors)
        {
            Task commandTask = command.Execute(1);
            if (command.WaitUntil) await commandTask;
        }
        CooldownHandler.instance.SetAbilityOnCooldown(this);
    }

    public override void Init(AbilityController controller)
    {
        foreach(AbilityBehavior command in _behaviors)
        {
            command.Initialize(controller);
        }
    }
}
