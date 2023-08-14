using System.Collections.Generic;

namespace DungeonMan.Player.StateMachine
{
    enum PlayerStates
    {
        jump,
        grounded,
        fall,
        levitate,
        idle,
        walk,
        sprint,
        dash,
        control
    }

    public class PlayerStateFactory
    {
        PlayerStateMachine _context;

        Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext;

            _states[PlayerStates.jump] = new PlayerJumpState(_context, this);
            _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
            _states[PlayerStates.fall] = new PlayerFallState(_context, this);
            _states[PlayerStates.levitate] = new PlayerLevitateState(_context, this);
            _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
            _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
            _states[PlayerStates.sprint] = new PlayerSprintState(_context, this);
            _states[PlayerStates.dash] = new PlayerDashState(_context, this);
            _states[PlayerStates.control] = new PlayerControlState(_context, this);
        }

        public PlayerBaseState Idle()
        {
            return _states[PlayerStates.idle];
        }
        public PlayerBaseState Walk()
        {
            return _states[PlayerStates.walk];
        }
        public PlayerBaseState Sprint()
        {
            return _states[PlayerStates.sprint];
        }
        public PlayerBaseState Dash()
        {
            return _states[PlayerStates.dash];
        }
        public PlayerBaseState Control()
        {
            return _states[PlayerStates.control];
        }
        public PlayerBaseState Jump()
        {
            return _states[PlayerStates.jump];
        }
        public PlayerBaseState Grounded()
        {
            return _states[PlayerStates.grounded];
        }
        public PlayerBaseState Fall()
        {
            return _states[PlayerStates.fall];
        }
        public PlayerBaseState Levitate()
        {
            return _states[PlayerStates.levitate];
        }
    }
}