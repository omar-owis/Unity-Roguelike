using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerJumpState : PlayerBaseState
    {
        private bool _readyToLevitate;
        public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
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
            else if(_readyToLevitate && Ctx.IsJumpPressed && Ctx.LeviationCDTimer <= 0)
            {
                SwitchStates(Factory.Levitate());
            }
        }

        public override void EnterState()
        {
            InitializeSubState();
            Ctx.ChangeDrag(0);
            HandleJump();
            Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
            _readyToLevitate = false;
        }

        public override void ExitState()
        {
            Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
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
            if(!Ctx.IsJumpPressed && ! _readyToLevitate)
            {
                _readyToLevitate = true;
            }
            CheckSwitchStates();
        }

        void HandleJump()
        {
            if (Ctx.IsJumpPressed && Ctx.ReadyToJump)
            {
                Ctx.ReadyToJump = false;

                Ctx.ExitingSlope = true;

                Ctx.Rb.velocity = new Vector3(Ctx.Rb.velocity.x, 0f, Ctx.Rb.velocity.z);

                Ctx.Rb.AddForce(Ctx.PlayerTransform.up * Ctx.JumpForce, ForceMode.Impulse);

                Ctx.Invoke(nameof(Ctx.ResetJump), Ctx.JumpCooldown);
            }
        }
    }
}