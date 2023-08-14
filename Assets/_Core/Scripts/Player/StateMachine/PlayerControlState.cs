using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerControlState : PlayerBaseState
    {
        public PlayerControlState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }


        public override void CheckSwitchStates()
        {
            // if no movement control then return to sprint state
            if (Ctx.MovementControl == Vector3.zero)
            {
                SwitchStates(Factory.Sprint());
            }
        }

        public override void EnterState()
        {
            // animation

            Ctx.MoveRelativeToPlayer = true;
            Ctx.AllowRotation = false;
            Ctx.InputManager.DisableMovement();
        }

        public override void ExitState()
        {
            Ctx.MoveRelativeToPlayer = false;
            Ctx.AllowRotation = true;
            Ctx.InputManager.EnableMovement();
        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            Ctx.SetMovementDirection(Ctx.MovementControl);
            CheckSwitchStates();
        }
    }
}