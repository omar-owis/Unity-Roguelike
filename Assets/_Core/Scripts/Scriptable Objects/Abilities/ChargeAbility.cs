using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonMan
{
    [CreateAssetMenu(fileName = "New Charge Ability", menuName = "Abilities/Charge Ability")]
    public class ChargeAbility : Ability
    {
        [SerializeField] private float _chargeRate;
        [SerializeField] private float _threshold;
        [SerializeField] private bool _autoActivate;
        [SerializeField] private AnimationCurve _effectivenessCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float ChargeRate { get { return _chargeRate; } }
        public bool AutoActivate { get { return _autoActivate; } }

        private void Awake()
        {
            _isChargable = true;
        }
        public override async void Activate(float charge)
        {
            if(charge >= _threshold)
            {
                Debug.Log("Ability Activate");
                foreach (AbilityBehavior command in _behaviors)
                {
                    Task commandTask = command.Execute(_effectivenessCurve.Evaluate(charge));
                    if (command.WaitUntil) await commandTask;
                }
                CooldownHandler.instance.SetAbilityOnCooldown(this);
            }
        }

        private void OnValidate()
        {
            _chargeRate = Mathf.Clamp01(_chargeRate);
            _threshold = Mathf.Clamp01(_threshold);
        }
    }
}
