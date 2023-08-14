using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName ="InputManager", menuName ="Managers/InputManager")]
public class InputManager : ScriptableObject, PlayerInput.ICharacterMovementActions, PlayerInput.ICharacterAbilitiesActions,
    PlayerInput.ICharacterUIActions
{
    public event Action<Vector2> moveEvent = delegate { };
    public event Action sprintStartedEvent = delegate { };
    public event Action sprintCanceledEvent = delegate { };
    public event Action jumpStartedEvent = delegate { };
    public event Action jumpCanceledEvent = delegate { };
    public event Action dashStartedEvent = delegate { };
    public event Action dashCanceledEvent = delegate { };

    public event Action<bool> autoattackEvent = delegate { };
    public event Action<bool> heavyattackEvent = delegate { };
    public event Action<bool> OA1Event = delegate { };
    public event Action<bool> OA2Event = delegate { };

    public event Action inventoryEvent = delegate { };
    public event Action use1Event = delegate { };
    public event Action use2Event = delegate { };


    private PlayerInput playerInput;
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.CharacterMovement.SetCallbacks(this);
            playerInput.CharacterAbilities.SetCallbacks(this);
            playerInput.CharacterUI.SetCallbacks(this);
        }

        playerInput.CharacterMovement.Enable();
        playerInput.CharacterAbilities.Enable();
        playerInput.CharacterUI.Enable();
        
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        switch(context.phase)
        {
            case InputActionPhase.Started:
                dashStartedEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                dashCanceledEvent.Invoke();
                break;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                jumpStartedEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                jumpCanceledEvent.Invoke();
                break;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent.Invoke(context.ReadValue<Vector2>());
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                sprintStartedEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                sprintCanceledEvent.Invoke();
                break;
        }
    }
    public void OnAutoAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                autoattackEvent.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                autoattackEvent.Invoke(false);
                break;
        }    
    }
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                heavyattackEvent.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                heavyattackEvent.Invoke(false);
                break;
        }
    }
    public void OnOA1(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                OA1Event.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                OA1Event.Invoke(false);
                break;
        }
    }
    public void OnOA2(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                OA2Event.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                OA2Event.Invoke(false);
                break;
        }
    }
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            inventoryEvent.Invoke();
    }
    public void OnUseItem1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            use1Event.Invoke();
    }
    public void OnUseItem2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            use2Event.Invoke();
    }
    public void EnableCharacter()
    {
        playerInput.CharacterMovement.Enable();
        playerInput.CharacterAbilities.Enable();
    }
    public void DisableCharacter()
    {
        playerInput.CharacterMovement.Disable();
        playerInput.CharacterAbilities.Disable();
    }

    public void EnableMovement()
    {
        playerInput.CharacterMovement.Enable();
    }

    public void DisableMovement()
    {
        playerInput.CharacterMovement.Disable();
    }
    public void DisableAllInputs()
    {
        playerInput.CharacterAbilities.Disable();
        playerInput.CharacterMovement.Disable();
        playerInput.CharacterUI.Disable();
    }
}
