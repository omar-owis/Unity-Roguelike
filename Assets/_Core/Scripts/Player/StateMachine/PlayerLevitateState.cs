using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerLevitateState : PlayerBaseState
    {
        public PlayerLevitateState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
        }

        public override void CheckSwitchStates()
        {
            if(Ctx.IsGrounded)
            {
                SwitchStates(Factory.Grounded());
            }
            else if(!Ctx.IsJumpPressed)
            {
                SwitchStates(Factory.Fall());
            }
            else if(Ctx.LeviationRate <= 0)
            {
                Ctx.LeviationCDTimer = Ctx.LeviationCD;
                SwitchStates(Factory.Fall());
            }
        }

        public override void EnterState()
        {
            InitializeSubState();
            Ctx.Rb.velocity = new Vector3(Ctx.Rb.velocity.x, Ctx.Rb.velocity.y / 16, Ctx.Rb.velocity.z);
            Ctx.LeviationIncreaseRate = 0;
            Ctx.Animator.SetBool(Ctx.IsLevitatingHash, true);
        }

        public override void ExitState()
        {
            Ctx.LeviationIncreaseRate = Ctx.LeviationChargeRate;
            Ctx.Animator.SetBool(Ctx.IsLevitatingHash, false);
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
            Ctx.Rb.AddForce(Vector3.up * Ctx.LeviationForce);
            if(Ctx.Rb.velocity.y > Ctx.LeviationMaxSpeed)
            {
                Ctx.Rb.velocity = new Vector3(Ctx.Rb.velocity.x, Ctx.LeviationMaxSpeed, Ctx.Rb.velocity.z);
            }
            Ctx.LeviationRate -= Ctx.LeviationConsumptionRate * Time.deltaTime;
            CheckSwitchStates();
        }
    }
}
