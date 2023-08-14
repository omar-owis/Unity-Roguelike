using UnityEngine;

namespace DungeonMan.Player.StateMachine
{

    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
        }

        public override void CheckSwitchStates()
        {
            if (Ctx.IsGrounded)
            {
                SwitchStates(Factory.Grounded());
            }
            if(Ctx.IsJumpPressed && Ctx.LeviationCDTimer <= 0)
            {
                SwitchStates(Factory.Levitate());
            }
        }

        public override void EnterState()
        {
            InitializeSubState();
            Ctx.ChangeDrag(0);
            Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.IsFallingHash, false);
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