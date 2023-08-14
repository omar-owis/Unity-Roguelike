using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

        public override void CheckSwitchStates()
        {

            if (Ctx.MovementControl != Vector3.zero)
            {
                SwitchStates(Factory.Control());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsDashPressed && Ctx.DashCDTimer <= 0)
            {
                SwitchStates(Factory.Dash());
            }
            else if(Ctx.IsMovementPressed && Ctx.IsSprintPressed && Ctx.OpenWorldCam)
            {
                SwitchStates(Factory.Sprint());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsSprintPressed && Ctx.MovementInputY >= 0)
            {
                SwitchStates(Factory.Sprint());
            }
            else if (!Ctx.IsMovementPressed)
            {
                SwitchStates(Factory.Idle());
            }
        }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
            Ctx.Animator.SetBool(Ctx.IsSprintingHash, false);

            Ctx.MoveSpeed = Ctx.WalkSpeed;
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            Ctx.MovementDirectionX = Ctx.MovementInputX;
            Ctx.MovementDirectionZ = Ctx.MovementInputY;
            CheckSwitchStates();
        }
    }
}