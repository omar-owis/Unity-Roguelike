using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerSprintState : PlayerBaseState
    {
        public PlayerSprintState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
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
            else if ((Ctx.IsMovementPressed && !Ctx.SprintToggle && !Ctx.IsSprintPressed))
            {
                SwitchStates(Factory.Walk());
            }
            else if (!Ctx.IsMovementPressed)
            {
                SwitchStates(Factory.Idle());
            }

            if(Ctx.MovementInputY < 0 && !Ctx.OpenWorldCam)
            {
                SwitchStates(Factory.Walk());
            }
        }

        public override void EnterState()
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
            Ctx.Animator.SetBool(Ctx.IsSprintingHash, true);

            Ctx.MoveSpeed = Ctx.SprintSpeed;
            Ctx.SprintToggle = true;
        }

        public override void ExitState()
        {
            Ctx.SprintToggle = false;
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