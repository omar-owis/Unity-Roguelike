using UnityEngine;

namespace DungeonMan.Player.StateMachine
{
    public class PlayerStateMachine : MonoBehaviour
    {
        Rigidbody _rb;
        [SerializeField] InputManager _inputManager = default;
        Animator _animator;

        int _isWalkingHash;
        int _isSprintingHash;
        int _isLevitateHash;
        int _isJumpingHash;
        int _isFallingHash;
        int _moveDirXHash;
        int _moveDirYHash;

        private Vector2 _movementInput;
        private Vector3 _movementDir;
        private Vector3 _movementControl;

        private bool _isMovementPressed = false;
        private bool _sprintToggle = false;
        private bool _isJumpPressed = false;
        private bool _isSprintPressed = false;
        private bool _isDashPressed = false;
        private bool _moveRelativeToPlayer;

        [SerializeField] float _turnVelocity = 0.03f;
        float _targetAdditonalAngle = 0;
        float _turnSmoothVelocity;
        [SerializeField] bool _allowRotation = true;
        [SerializeField] bool _openWorldCam;

        public float _groundDrag;

        [Header("Movement")]
        private float _moveSpeed;
        [SerializeField] float _walkSpeed;
        [SerializeField] float _sprintSpeed;
        [SerializeField] float _maxSlopeAngle;
        private RaycastHit _slopeHit;

        [Header("Ground Check")]
        [SerializeField] float _playerHeight;
        public LayerMask _whatIsGround;
        bool _isGrounded;

        [Header("Jumping")]
        [SerializeField] float _jumpForce;
        [SerializeField] float _jumpCooldown;
        [SerializeField] float _airMultiplier;
        bool _readyToJump;
        bool _exitingSlope;

        [Header("Dashing")]
        [SerializeField] float _dashDuration;
        [SerializeField] float _dashCD;
        [SerializeField] float _dashSpeed;
        [SerializeField] float _decelerationDuration;
        private float _dashCDTimer;
        private bool _dashing;
        private bool _isDoneLerping;

        [Header("Levitation")]
        [SerializeField] CircularProgressBar _levitationUI;
        [SerializeField] float _leviationForce;
        [SerializeField] float _leviationMaxSpeed;
        [SerializeField] float _leviationChargeRate;
        [SerializeField] float _leviationConsumptionRate;
        [SerializeField] float _leviationCD;
        private float _leviationRate = 1;
        private float _levitationIncreaseRate;
        private float _leviationCDTimer;

        PlayerBaseState _currentState;
        PlayerStateFactory _states;

        // getters and setters
        public Rigidbody Rb { get { return _rb; } }
        public Animator Animator { get { return _animator; } }
        public InputManager InputManager { get { return _inputManager; } }
        public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
        public bool IsJumpPressed { get { return _isJumpPressed; } }
        public bool IsMovementPressed { get { return _isMovementPressed; } }
        public bool IsSprintPressed { get { return _isSprintPressed; } }
        public bool SprintToggle { get { return _sprintToggle; } set { _sprintToggle = value; } }
        public bool IsDashPressed { get { return _isDashPressed; } }
        public bool ExitingSlope { get { return _exitingSlope; } set { _exitingSlope = value; } }
        public bool ReadyToJump { get { return _readyToJump; } set { _readyToJump = value; } }
        public bool MoveRelativeToPlayer { get { return _moveRelativeToPlayer; } set { _moveRelativeToPlayer = value; } }
        public bool IsGrounded { get { return _isGrounded; } }
        public bool AllowRotation { get { return _allowRotation; } set { _allowRotation = value; } }
        public bool OpenWorldCam { get { return _openWorldCam; } set { _openWorldCam = value; } }
        public float JumpForce { get { return _jumpForce; } }
        public Transform PlayerTransform { get { return this.transform; } }
        public float JumpCooldown { get { return _jumpCooldown; } }
        public int IsWalkingHash { get { return _isWalkingHash; } }
        public int IsSprintingHash { get { return _isSprintingHash; } }
        public int IsLevitatingHash { get { return _isLevitateHash; } }
        public int IsJumpingHash { get { return _isJumpingHash; } }
        public int IsFallingHash { get { return _isFallingHash; } }
        public Vector3 MovementControl { get { return _movementControl; } set { _movementControl = value; } }
        public float MovementInputX { get { return _movementInput.x; } }
        public float MovementInputY { get { return _movementInput.y; } }
        public float MovementDirectionX { get { return _movementDir.x; } set { _movementDir.x = value; } }
        public float MovementDirectionZ { get { return _movementDir.z; } set { _movementDir.z = value; } }
        public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
        public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
        public float GroundDrag { get { return _groundDrag; } }
        public float SprintSpeed { get { return _sprintSpeed; } set { _sprintSpeed = value; } }
        public float DashSpeed { get { return _dashSpeed; } }
        public float DashCDTimer { get { return _dashCDTimer; } set { _dashCDTimer = value; } }
        public float DashCD { get { return _dashCD; } }
        public bool Dashing { get { return _dashing; } set { _dashing = value; } }
        public bool IsDoneLerping { get { return _isDoneLerping; } set { _isDoneLerping = value; } }
        public float DashDuration { get { return _dashDuration; } }
        public float DecelerationDuration { get { return _decelerationDuration; } }
        public bool OpenWorldCamera { get { return _openWorldCam; } }
        public float LeviationForce { get { return _leviationForce; } }
        public float LeviationMaxSpeed { get { return _leviationMaxSpeed; } }
        public float LeviationChargeRate { get { return _leviationChargeRate; } }
        public float LeviationConsumptionRate { get { return _leviationConsumptionRate; } }
        public float LeviationRate { get { return _leviationRate; } set { _leviationRate = value; } }
        public float LeviationIncreaseRate { set { _levitationIncreaseRate = value; } }
        public float LeviationCD { get { return _leviationCD; } }

        public float LeviationCDTimer { get { return _leviationCDTimer; } set { _leviationCDTimer = value; } }

        private void OnEnable()
        {
            _inputManager.moveEvent += OnMovementInput;
            _inputManager.sprintStartedEvent += OnSprintStarted;
            _inputManager.sprintCanceledEvent += OnSprintCanceled;
            _inputManager.jumpStartedEvent += OnJumpStarted;
            _inputManager.jumpCanceledEvent += OnJumpCanceled;
            _inputManager.dashStartedEvent += OnDashStarted;
            _inputManager.dashCanceledEvent += OnDashCanceled;
        }

        private void Awake()
        {
            ResetJump();
            _dashing = false;
            _openWorldCam = true;
            _moveRelativeToPlayer = false;

            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();

            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterState();

            _isWalkingHash = Animator.StringToHash("isWalking");
            _isSprintingHash = Animator.StringToHash("isSprinting");
            _isLevitateHash = Animator.StringToHash("isLevitating");
            _isJumpingHash = Animator.StringToHash("isJumping");
            _isFallingHash = Animator.StringToHash("isFalling");
            _moveDirXHash = Animator.StringToHash("InputX");
            _moveDirYHash = Animator.StringToHash("InputY");
        }

        private void OnDisable()
        {
            _inputManager.moveEvent -= OnMovementInput;
            _inputManager.sprintStartedEvent -= OnSprintStarted;
            _inputManager.sprintCanceledEvent -= OnSprintCanceled;
            _inputManager.jumpStartedEvent -= OnJumpStarted;
            _inputManager.jumpCanceledEvent -= OnJumpCanceled;
            _inputManager.dashStartedEvent -= OnDashStarted;
            _inputManager.dashCanceledEvent -= OnDashCanceled;
        }

        private void Update()
        {
            // ground check
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.1f, _whatIsGround);
            
            // movement animation handling
            if(!_openWorldCam)
            {
                _animator.SetFloat(_moveDirXHash, _movementInput.x, 0.1f, Time.deltaTime);
                _animator.SetFloat(_moveDirYHash, _movementInput.y, 0.1f, Time.deltaTime);
            }
            else
            {
                if (_isMovementPressed)
                {
                    _animator.SetFloat(_moveDirXHash, 0, 0.1f, Time.deltaTime);
                    _animator.SetFloat(_moveDirYHash, 1, 0.1f, Time.deltaTime);
                }
            }

            


            CurrentState.UpdateStates();
            SpeedConstraint();
            HandleCooldowns();

            if(_leviationRate < 1 && _isGrounded) _leviationRate += _levitationIncreaseRate * Time.deltaTime;
            _levitationUI.UpdateProgressBar(_leviationRate);

            //Debug.Log("Current Super State: " + _currentState);
            //Debug.Log("Current Sub State: " + _currentState.CurrentSubState);
            //Debug.Log("Current Super State of Sub State: " + _currentState.CurrentSubState.CurrentSuperState);
        }

        void FixedUpdate()
        {
            Move();
            if (_allowRotation) HandleRotation();
        }

        void Move()
        {
            if (_isMovementPressed || _dashing || _movementControl != Vector3.zero)
            {
                Vector3 moveDir = _moveRelativeToPlayer ? RelativeToPlayer(_movementDir) : RelativeToCamera(_movementDir);
                float angleOfSlope = SlopeAngle();

                if ((angleOfSlope <= _maxSlopeAngle && angleOfSlope != 0) && !_exitingSlope)
                {
                    _rb.AddForce(GetSlopeAngle(moveDir) * _moveSpeed * 20f, ForceMode.Force);

                    if (Mathf.Abs(_rb.velocity.y) > 0)
                    {
                        _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                    }
                }
                else if (angleOfSlope > _maxSlopeAngle)
                {
                    _rb.AddForce(GetSlopeAngle(Vector3.down) * 100f, ForceMode.Force);
                }

                if (_isGrounded)
                    _rb.AddForce(moveDir.normalized * _moveSpeed * 10f, ForceMode.Force);
                else
                    _rb.AddForce(moveDir.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
            }

            _rb.useGravity = !(SlopeAngle() <= _maxSlopeAngle && SlopeAngle() != 0 && !Dashing);

        }

        void HandleRotation()
        {
            if (!_openWorldCam)
            {
                float yawCam = Helper.MainCamera.transform.rotation.eulerAngles.y + _targetAdditonalAngle;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCam, 0), _turnVelocity * 1500 * Time.fixedDeltaTime);

                if (_movementInput.x == -1)
                {
                    //targetAdditonalAngle = -90 + transform.rotation.y;
                    if (_targetAdditonalAngle > -90)
                    {
                        transform.rotation *= Quaternion.Euler(0, -10, 0);
                        _targetAdditonalAngle -= 10;
                    }
                }
                else if (_movementInput.x == 1)
                {
                    //targetAdditonalAngle = 90 + transform.rotation.y;

                    if (_targetAdditonalAngle < 90)
                    {
                        transform.rotation *= Quaternion.Euler(0, 10, 0);
                        _targetAdditonalAngle += 10;
                    }
                }
                else
                {
                    if(_targetAdditonalAngle != 0)
                    {
                        if (_targetAdditonalAngle > 0) _targetAdditonalAngle -= 10;
                        else _targetAdditonalAngle += 10;
                        transform.rotation *= Quaternion.Euler(0, 10, 0);
                    }
                }

                //float additionalAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAdditonalAngle, ref _turnSmoothVelocity, _turnVelocity);
                //transform.rotation = Quaternion.Euler(0f, targetAdditonalAngle, 0f);
            }
            else
            {
                if (_isMovementPressed)
                {
                    float targetAngle = Mathf.Atan2(_movementDir.x, _movementDir.z) * Mathf.Rad2Deg + Helper.MainCamera.transform.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnVelocity);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }
        }

        private void SpeedConstraint()
        {
            float angleOfSlope = SlopeAngle();
            if ((angleOfSlope <= _maxSlopeAngle && angleOfSlope != 0) && !_exitingSlope)
            {
                if (_rb.velocity.magnitude > _moveSpeed)
                    _rb.velocity = _rb.velocity.normalized * _moveSpeed;
            }

            else
            {
                Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

                if (flatVelocity.magnitude > _moveSpeed)
                {
                    Vector3 limitedVelocity = flatVelocity.normalized * _moveSpeed;
                    _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
                }
            }
        }

        private float SlopeAngle()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.15f))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                return angle;
            }

            return 0;
        }

        private Vector3 GetSlopeAngle(Vector3 moveDir)
        {
            return Vector3.ProjectOnPlane(moveDir, _slopeHit.normal).normalized;
        }

        private Vector3 RelativeToCamera(Vector3 dir)
        {
            Vector3 camForward = Helper.MainCamera.transform.forward;
            Vector3 camRight = Helper.MainCamera.transform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward = camForward.normalized;
            camRight = camRight.normalized;

            return (dir.z * camForward) + (dir.y * Vector3.up) + (dir.x * camRight);
        }

        private Vector3 RelativeToPlayer(Vector3 dir)
        {
            Vector3 playerForward = transform.forward;
            Vector3 playerRight = transform.right;

            playerForward.y = 0;
            playerRight.y = 0;
            playerForward = playerForward.normalized;
            playerRight = playerRight.normalized;

            return (dir.z * playerForward) + (dir.y * Vector3.up) + (dir.x * playerRight);
        }

        public void OnMovementInput(Vector2 movement)
        {
            _movementInput = movement;
            //_movementDir = new Vector3(_movementInput.x, _movementDir.y, _movementInput.y);
            _isMovementPressed = _movementInput != new Vector2(0, 0);
        }

        void OnSprintStarted()
        {
            _isSprintPressed = true;

            if(_sprintToggle)
            {
                _sprintToggle = false;
            }
        }

        void OnSprintCanceled()
        {
            _isSprintPressed = false;
        }

        void OnJumpStarted()
        {
            _isJumpPressed = true;
        }

        void OnJumpCanceled()
        {
            _isJumpPressed = false;
        }

        void OnDashStarted()
        {
            _isDashPressed = true;
        }

        void OnDashCanceled()
        {
            _isDashPressed = false;
        }

        private void HandleCooldowns()
        {
            if (_dashCDTimer > 0)
                _dashCDTimer -= Time.deltaTime;
            if (_leviationCDTimer > 0)
                _leviationCDTimer -= Time.deltaTime;
        }

        public void ResetJump()
        {
            ReadyToJump = true;

            ExitingSlope = false;
        }

        public void ChangeDrag(float newDrag)
        {
            _rb.drag = newDrag;
        }

        public void SetMovementDirection(Vector3 newDir)
        {
            _movementDir = newDir;
        }
    }
}