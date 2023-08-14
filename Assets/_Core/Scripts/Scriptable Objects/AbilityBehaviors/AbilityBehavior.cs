using System.Threading.Tasks;
using UnityEngine;

public abstract class AbilityBehavior : ScriptableObject
{
    public bool _waitUntil = true;

    [HideInInspector] public int SelectedID;

    public bool WaitUntil { get { return _waitUntil; } }

    public abstract Task Execute(float effectivenessFactor);

    public abstract void Initialize(AbilityController abilityController);
}
