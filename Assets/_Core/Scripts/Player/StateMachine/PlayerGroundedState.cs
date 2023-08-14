using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
        }

        public override void CheckSwitchStates()
        {
            if (Ctx.IsJumpPressed)
            {
                SwitchStates(Factory.Jump());
            }
            else if (!Ctx.IsGrounded)
            {
                SwitchStates(Factory.Fall());
            }
        }

        public override void EnterState()
        {
            InitializeSubState();
            Ctx.ChangeDrag(Ctx.GroundDrag);
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {
            if (Ctx.MovementControl != Vector3.zero)
            {
                SetSubState(Factory.Control());
            }
            else if (Ctx.Dashing)
            {
                SetSubState(Factory.Dash());
            }
            else if (!Ctx.IsMovementPressed && !Ctx.IsSprintPressed)
            {
                SetSubState(Factory.Idle());
            }
            else if (Ctx.IsMovementPressed && !Ctx.IsSprintPressed)
            {
                SetSubState(Factory.Walk());
            }
            else // if (Ctx.IsMovementPressed && Ctx.IsSprintPressed)
            {
                SetSubState(Factory.Sprint());
            }
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
    }
}