using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerDashState : PlayerBaseState
    {
        private float _dashTimeRemaining;
        private float _elapsedTime;
        public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
        public override void CheckSwitchStates()
        {
            if (!Ctx.Dashing && Ctx.IsDoneLerping)
            {
                SwitchStates(Factory.Sprint());
            }
        }

        public override void EnterState()
        {
            _dashTimeRemaining = Ctx.DashDuration;
            Ctx.Dashing = true;
            Ctx.IsDoneLerping = false;
            Ctx.Rb.useGravity = true;
            Ctx.AllowRotation = false;
            Ctx.MoveRelativeToPlayer = true;
            Ctx.MoveSpeed = Ctx.DashSpeed;

            Ctx.MovementDirectionX = Ctx.MovementInputX;
            Ctx.MovementDirectionZ = Ctx.MovementInputY;

            // Disable all character inputs
            Ctx.InputManager.DisableCharacter();

        }

        public override void ExitState()
        {
            Ctx.AllowRotation = true;
            Ctx.MoveRelativeToPlayer = false;
            Ctx.DashCDTimer = Ctx.DashCD;

            // Enable all character inputs
            Ctx.InputManager.EnableCharacter();
        }

        public override void InitializeSubState()
        {

        }

        public override void UpdateState()
        {
            DashDurationTimer();
            CheckSwitchStates();
        }

        private void DashDurationTimer()
        {
            if (_dashTimeRemaining > 0) _dashTimeRemaining -= Time.deltaTime;
            else
            {
                Ctx.Dashing = false;
                _elapsedTime += Time.deltaTime;
                float percentageComplete = _elapsedTime / Ctx.DecelerationDuration;
                Ctx.MoveSpeed = Mathf.Lerp(Ctx.DashSpeed, Ctx.SprintSpeed, percentageComplete);
            }

            if (Ctx.MoveSpeed <= Ctx.SprintSpeed) Ctx.IsDoneLerping = true;
        }
    }
}