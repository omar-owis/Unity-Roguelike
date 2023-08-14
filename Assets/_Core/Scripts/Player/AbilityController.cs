using DungeonMan;
using DungeonMan.Player.StateMachine;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PlayerStateMachine))]
public class AbilityController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    private Character _character;
    private PlayerStateMachine _playerState;
    
    [SerializeField] private PlayerAbilities _abilities;
    [SerializeField] private float _globalCooldown;
    private float _globalTimeRemaining;

    private bool _pressed;
    private float _abilityCharge;

    public CharacterData CharachterData { get { return _character.CharacterData; } }
    public Vector3 MovementControl { get { return _playerState.MovementControl; } set { _playerState.MovementControl = value; } }
    public float MoveSpeed { get { return _playerState.MoveSpeed; } set { _playerState.MoveSpeed = value; } }

    private void OnEnable()
    {
        _inputManager.autoattackEvent += OnAutoAttack;
        _inputManager.heavyattackEvent += OnHeavyAttack;
        _inputManager.OA1Event += OnAttack1;
        _inputManager.OA2Event += OnAttack2;
    }
    private void Start()
    {
        _character = GetComponent<Character>();
        _playerState = GetComponent<PlayerStateMachine>();

        _abilities.Initialize(this);
        _globalTimeRemaining = 0;
    }
    private void OnDisable()
    {
        _inputManager.autoattackEvent -= OnAutoAttack;
        _inputManager.heavyattackEvent -= OnHeavyAttack;
        _inputManager.OA1Event -= OnAttack1;
        _inputManager.OA2Event -= OnAttack2;
    }

    private void Update()
    {
        _abilityCharge = Mathf.Clamp01(_abilityCharge);
        if (_globalTimeRemaining > 0)
        {
            _globalTimeRemaining -= Time.deltaTime;
        }
    }

    void OnAutoAttack(bool pressed)
    {
        _pressed = pressed;
        if(pressed) 
            StartCoroutine(TriggerAbility(_abilities.container[0]));
    }

    void OnHeavyAttack(bool pressed)
    {
        _pressed = pressed;
        if (pressed)
            StartCoroutine(TriggerAbility(_abilities.container[1]));
    }

    void OnAttack1(bool pressed)
    {
        _pressed = pressed;
        if (pressed)
            StartCoroutine(TriggerAbility(_abilities.container[3]));
    }

    void OnAttack2(bool pressed)
    {
        _pressed = pressed;
        if(pressed) 
            StartCoroutine(TriggerAbility(_abilities.container[4]));
    }

    IEnumerator TriggerAbility(AbstractAbilityObject ability)
    {
        if (ability == null) yield break;

        if (!CooldownHandler.instance.OnCooldown(ability) && _globalTimeRemaining <= 0)
        {
            if (ability.IsChargable)
            {
                ChargeAbility chargeAbility = (ChargeAbility)ability;
                while (_pressed)
                {
                    _abilityCharge += chargeAbility.ChargeRate * Time.deltaTime;
                    if (chargeAbility.AutoActivate && _abilityCharge >= 1) break;
                    yield return null;
                }
            }

            ability.Activate(_abilityCharge);
            _globalTimeRemaining = _globalCooldown; // start global cooldown
            _abilityCharge = 0;
        }
    }

    // for changeing obtainable ability one
    public void ChangeAbility1(AbstractAbilityObject ability)
    {
        _abilities.container[3] = ability;
        _abilities.container[3].Init(this);
    }

    // for changeing obtainable ability two
    public void ChangeAbility2(AbstractAbilityObject ability)
    {
        _abilities.container[4] = ability;
        _abilities.container[4].Init(this);
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity);
    }

    public Transform GetShootPoint(Hand hand)
    {
        switch (hand)
        {
            case Hand.left:
                return GameObject.Find("Player/GFX/Left ShootPoint").transform;
            case Hand.right:
                return GameObject.Find("Player/GFX/Right ShootPoint").transform;
            default:
                Debug.Log("Shoot Point Not Implemented");
                break;
        }
        return null;
    }
}

[Serializable]
public class PlayerAbilities
{
    public AbstractAbilityObject[] container = new AbstractAbilityObject[5]; // lightattack, heavyattack, dash, OA1, OA2


    public void Initialize(AbilityController player)
    {
        for (int i = 0; i < 5; i++)
        {
            container[i]?.Init(player);
        }
    }
}