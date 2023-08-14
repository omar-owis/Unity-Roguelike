using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

        public override void CheckSwitchStates()
        {
            if (Ctx.MovementControl != Vector3.zero)
            {
                SwitchStates(Factory.Control());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsSprintPressed && Ctx.OpenWorldCam)
            {
                SwitchStates(Factory.Sprint());
            }
            else if (Ctx.IsMovementPressed && Ctx.IsSprintPressed && Ctx.MovementInputY >= 0)
            {
                SwitchStates(Factory.Sprint());
            }
            else if (Ctx.IsMovementPressed)
            {
                SwitchStates(Factory.Walk());
            }
        }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
            Ctx.Animator.SetBool(Ctx.IsSprintingHash, false);

            Ctx.MovementDirectionX = 0;
            Ctx.MovementDirectionZ = 0;
            Ctx.MoveSpeed = 0;
        }

        public override void ExitState()
        {

        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
    }
}