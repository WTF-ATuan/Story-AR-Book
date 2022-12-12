using System.Collections;
using EasyCharacterMovement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Testing{
	public sealed class JoyStickAdvancedMovement : MonoBehaviour{
		[Required] public VariableJoystick variableJoystick;
		[Required] public Camera playerCamera;

		#region EDITOR EXPOSED FIELDS

		[Space(15f)] [SerializeField] private float _rotationRate;

		[Space(15f)] [SerializeField] private float _maxSpeed;

		[SerializeField] private float _minAnalogSpeed;

		[SerializeField] private float _maxAcceleration;

		[SerializeField] private float _groundBrakingDeceleration;

		[SerializeField] private float _groundFriction;

		[Space(15f)] [SerializeField] private float _airBrakingDeceleration;

		[SerializeField] private float _airFriction;

		[Range(0.0f, 1.0f)] [SerializeField] private float _airControl;

		[Space(15f)] [SerializeField] private bool _useSeparateBrakingFriction;

		[SerializeField] private float _brakingFriction;

		[Space(15f)] [SerializeField] private bool _canEverCrouch;

		[SerializeField] private float _unCrouchedHeight;

		[SerializeField] private float _crouchedHeight;

		[SerializeField] [Range(0.0f, 1.0f)] private float _crouchedSpeedModifier;

		[Space(15f)] [Tooltip("The max number of jumps the Character can perform.")] [SerializeField]
		private int _jumpMaxCount;

		[Tooltip("Initial velocity (instantaneous vertical velocity) when jumping.")] [SerializeField]
		private float _jumpImpulse;

		[Tooltip("The maximum time (in seconds) to hold the jump. eg: Variable height jump.")] [SerializeField]
		private float _jumpMaxHoldTime;

		[Tooltip("How early before hitting the ground you can trigger a jump (in seconds).")] [SerializeField]
		private float _jumpPreGroundedTime;

		[Tooltip("How long after leaving the ground you can trigger a jump (in seconds).")] [SerializeField]
		private float _jumpPostGroundedTime;

		[Space(15f)] [SerializeField] private Vector3 _gravity;

		[SerializeField] private float _gravityScale;

		#endregion

		#region FIELDS

		private bool _enableLateFixedUpdateCoroutine;
		private Coroutine _lateFixedUpdateCoroutine;

		private Transform _transform;
		private CharacterMovement _characterMovement;

		private bool _isCrouching;
		private bool _crouchButtonPressed;

		private bool _jumpButtonPressed;
		private float _jumpButtonHeldDownTime;
		private float _jumpHoldTime;
		private int _jumpCount;
		private bool _isJumping;

		private float _fallingTime;
		private bool _useGravity = true;

		private Vector3 _movementDirection;

		#endregion

		#region PROPERTIES

		public new Transform transform{
			get{
				#if UNITY_EDITOR
				if(_transform == null)
					_transform = GetComponent<Transform>();
				#endif

				return _transform;
			}
		}

		private CharacterMovement characterMovement{
			get{
				#if UNITY_EDITOR
				if(_characterMovement == null)
					_characterMovement = GetComponent<CharacterMovement>();
				#endif

				return _characterMovement;
			}
		}

		public float rotationRate{
			get => _rotationRate;
			set => _rotationRate = Mathf.Max(0.0f, value);
		}

		public float maxSpeed{
			get => _maxSpeed;
			set => _maxSpeed = Mathf.Max(0.0f, value);
		}

		public float minAnalogSpeed{
			get => _minAnalogSpeed;
			set => _minAnalogSpeed = Mathf.Max(0.0f, value);
		}

		public float maxAcceleration{
			get => _maxAcceleration;
			set => _maxAcceleration = Mathf.Max(0.0f, value);
		}

		public float groundBrakingDeceleration{
			get => _groundBrakingDeceleration;
			set => _groundBrakingDeceleration = Mathf.Max(0.0f, value);
		}

		public float groundFriction{
			get => _groundFriction;
			set => _groundFriction = Mathf.Max(0.0f, value);
		}

		public float airBrakingDeceleration{
			get => _airBrakingDeceleration;
			set => _airBrakingDeceleration = Mathf.Max(0.0f, value);
		}

		public float airFriction{
			get => _airFriction;
			set => _airFriction = Mathf.Max(0.0f, value);
		}

		public float airControl{
			get => _airControl;
			set => _airControl = Mathf.Max(0.0f, value);
		}

		public bool useSeparateBrakingFriction{
			get => _useSeparateBrakingFriction;
			set => _useSeparateBrakingFriction = value;
		}

		public float brakingFriction{
			get => _brakingFriction;
			set => _brakingFriction = Mathf.Max(0.0f, value);
		}

		public bool canEverCrouch{
			get => _canEverCrouch;
			set => _canEverCrouch = value;
		}

		public float unCrouchedHeight{
			get => _unCrouchedHeight;
			set => _unCrouchedHeight = Mathf.Max(0.0f, value);
		}

		public float crouchedHeight{
			get => _crouchedHeight;
			set => _crouchedHeight = Mathf.Max(0.0f, value);
		}

		public float crouchedSpeedModifier{
			get => _crouchedSpeedModifier;
			set => _crouchedSpeedModifier = Mathf.Clamp01(value);
		}

		public int jumpMaxCount{
			get => _jumpMaxCount;
			set => _jumpMaxCount = Mathf.Max(0, value);
		}

		public float jumpImpulse{
			get => _jumpImpulse;
			set => _jumpImpulse = Mathf.Max(0.0f, value);
		}

		public float jumpMaxHoldTime{
			get => _jumpMaxHoldTime;
			set => _jumpMaxHoldTime = Mathf.Max(0.0f, value);
		}

		public float jumpPreGroundedTime{
			get => _jumpPreGroundedTime;
			set => _jumpPreGroundedTime = Mathf.Max(0.0f, value);
		}

		public float jumpPostGroundedTime{
			get => _jumpPostGroundedTime;
			set => _jumpPostGroundedTime = Mathf.Max(0.0f, value);
		}

		public Vector3 gravity{
			get => _gravity;
			set => _gravity = value;
		}

		public float gravityScale{
			get => _gravityScale;
			set => _gravityScale = value;
		}

		public bool useGravity{
			get => _useGravity;
			set => _useGravity = value;
		}

		public bool enableLateFixedUpdate{
			get => _enableLateFixedUpdateCoroutine;
			set{
				_enableLateFixedUpdateCoroutine = value;
				EnableLateFixedUpdate(_enableLateFixedUpdateCoroutine);
			}
		}

		#endregion

		#region METHODS

		private void EnableLateFixedUpdate(bool enable){
			if(enable){
				if(_lateFixedUpdateCoroutine != null)
					StopCoroutine(_lateFixedUpdateCoroutine);

				_lateFixedUpdateCoroutine = StartCoroutine(LateFixedUpdate());
			}
			else{
				if(_lateFixedUpdateCoroutine != null)
					StopCoroutine(_lateFixedUpdateCoroutine);
			}
		}

		public float GetMaxSpeed(){
			float actualMaxSpeed = maxSpeed;
			if(IsCrouching())
				actualMaxSpeed *= crouchedSpeedModifier;

			return actualMaxSpeed;
		}

		public float GetMaxAcceleration(){
			if(characterMovement.isGrounded)
				return maxAcceleration;

			return maxAcceleration * airControl;
		}

		public float GetMaxBrakingDeceleration(){
			return characterMovement.isGrounded ? groundBrakingDeceleration : airBrakingDeceleration;
		}

		public float GetMinAnalogSpeed(){
			return minAnalogSpeed;
		}

		public float GetFriction(){
			return characterMovement.isGrounded ? groundFriction : airFriction;
		}

		public Vector3 GetVelocity(){
			return characterMovement.velocity;
		}

		public Vector3 GetPosition(){
			return characterMovement.position;
		}

		public void SetPosition(Vector3 newPosition, bool updateGround){
			characterMovement.SetPosition(newPosition, updateGround);
		}

		public Vector3 GetUpVector(){
			return transform.up;
		}

		public Vector3 GetRightVector(){
			return transform.right;
		}

		public Vector3 GetForwardVector(){
			return transform.forward;
		}

		public Quaternion GetRotation(){
			return characterMovement.rotation;
		}

		public void SetRotation(Quaternion newRotation){
			characterMovement.rotation = newRotation;
		}

		private void RotateTowards(Vector3 worldDirection, bool isPlanar = true){
			Vector3 characterUp = transform.up;

			if(isPlanar)
				worldDirection = worldDirection.projectedOnPlane(characterUp);

			if(worldDirection.isZero())
				return;

			Quaternion targetRotation = Quaternion.LookRotation(worldDirection, characterUp);

			characterMovement.rotation =
					Quaternion.Slerp(characterMovement.rotation, targetRotation,
						rotationRate * Mathf.Deg2Rad * Time.deltaTime);
		}

		private void UpdateRotation(){
			RotateTowards(GetMovementDirection());
		}

		public void SetMovementDirection(Vector3 movementDirection){
			_movementDirection = movementDirection;
		}

		public Vector3 GetMovementDirection(){
			return _movementDirection;
		}

		public Vector3 GetDesiredVelocity(){
			return GetMovementDirection() * GetMaxSpeed();
		}

		public Vector3 GetGravity(){
			return gravity * gravityScale;
		}

		public void Crouch(){
			_crouchButtonPressed = true;
		}

		public void StopCrouching(){
			_crouchButtonPressed = false;
		}

		public bool IsCrouching(){
			return _isCrouching;
		}

		private bool CanCrouch(){
			if(!canEverCrouch)
				return false;

			return characterMovement.isGrounded;
		}

		public bool CanUnCrouch(){
			// Check if there's room to expand capsule

			bool overlapped = characterMovement.CheckHeight(unCrouchedHeight);

			return !overlapped;
		}

		private void Crouching(){
			if(_crouchButtonPressed && !IsCrouching()){
				if(!CanCrouch())
					return;

				characterMovement.SetHeight(crouchedHeight);
				_isCrouching = true;
			}
			else if(IsCrouching() && _crouchButtonPressed == false){
				if(!CanUnCrouch())
					return;

				characterMovement.SetHeight(unCrouchedHeight);
				_isCrouching = false;
			}
		}

		public void Jump(){
			_jumpButtonPressed = true;
		}

		public void StopJumping(){
			// Input state

			_jumpButtonPressed = false;
			_jumpButtonHeldDownTime = 0.0f;

			// Jump holding state

			_isJumping = false;
			_jumpHoldTime = 0.0f;
		}

		public bool IsJumping(){
			return _isJumping;
		}

		public int GetJumpCount(){
			return _jumpCount;
		}

		private bool CanJump(){
			// Do not allow to jump while crouched

			if(IsCrouching())
				return false;

			// Cant jump if no jumps available

			if(jumpMaxCount == 0 || _jumpCount >= jumpMaxCount)
				return false;

			// Is fist jump ?

			if(_jumpCount == 0){
				// On first jump,
				// can jump if is grounded or is falling (e.g. not grounded) BUT withing post grounded time

				bool canJump = characterMovement.isGrounded ||
							   !characterMovement.isGrounded && jumpPostGroundedTime > 0.0f &&
							   _fallingTime < jumpPostGroundedTime;

				// Missed post grounded time ?

				if(!characterMovement.isGrounded && !canJump){
					// Missed post grounded time,
					// can jump if have any 'in-air' jumps but the first jump counts as the in-air jump

					canJump = jumpMaxCount > 1;
					if(canJump)
						_jumpCount++;
				}

				return canJump;
			}

			// In air jump conditions

			return !characterMovement.isGrounded;
		}

		private Vector3 CalcJumpImpulse(){
			Vector3 characterUp = GetUpVector();

			float verticalSpeed = Vector3.Dot(GetVelocity(), characterUp);
			float actualJumpImpulse = Mathf.Max(verticalSpeed, jumpImpulse);

			return characterUp * actualJumpImpulse;
		}

		private void DoJump(){
			// Update held down timer

			if(_jumpButtonPressed)
				_jumpButtonHeldDownTime += Time.deltaTime;

			// Wants to jump and not already jumping..

			if(_jumpButtonPressed && !IsJumping()){
				// If jumpPreGroundedTime is enabled,
				// allow to jump only if held down time is less than tolerance

				if(jumpPreGroundedTime > 0.0f){
					bool canJump = _jumpButtonHeldDownTime <= jumpPreGroundedTime;
					if(!canJump)
						return;
				}

				// Can perform the requested jump ?

				if(!CanJump())
					return;

				// Jump!

				characterMovement.PauseGroundConstraint();
				characterMovement.LaunchCharacter(CalcJumpImpulse(), true);

				_jumpCount++;
				_isJumping = true;
			}
		}

		private void Jumping(){
			// Check jump input state and attempts to do the requested jump

			DoJump();

			// Perform jump hold, applies an opposite gravity force proportional to _jumpHoldTime.

			if(IsJumping() && _jumpButtonPressed && jumpMaxHoldTime > 0.0f && _jumpHoldTime < jumpMaxHoldTime){
				Vector3 actualGravity = GetGravity();

				float actualGravityMagnitude = actualGravity.magnitude;
				Vector3 actualGravityDirection = actualGravityMagnitude > 0.0f
						? actualGravity / actualGravityMagnitude
						: Vector3.zero;

				float jumpProgress = Mathf.InverseLerp(0.0f, jumpMaxHoldTime, _jumpHoldTime);
				float proportionalForce = Mathf.LerpUnclamped(actualGravityMagnitude, 0.0f, jumpProgress);

				Vector3 proportionalJumpForce = -actualGravityDirection * proportionalForce;
				characterMovement.AddForce(proportionalJumpForce);

				_jumpHoldTime += Time.deltaTime;
			}

			// If 'falling' update falling time

			if(!characterMovement.isGrounded)
				_fallingTime += Time.deltaTime;
			else if(!characterMovement.wasGrounded){
				// If landed, reset jump info

				_jumpCount = 0;
				_fallingTime = 0.0f;
			}
		}

		private void Move(){
			// Get desired velocity

			Vector3 desiredVelocity = GetDesiredVelocity();

			// Handle crouching state

			Crouching();

			// Handle Jumping state

			Jumping();

			// Perform character movement using the CharacterMovement built-in friction based movement

			float actualBrakingFriction = useSeparateBrakingFriction ? brakingFriction : GetFriction();

			characterMovement.SimpleMove(desiredVelocity, GetMaxSpeed(), GetMaxAcceleration(),
				GetMaxBrakingDeceleration(), GetFriction(), actualBrakingFriction, GetGravity());
		}

		private void HandleInput(){
			// Movement input (in world space)

			var movementDirection = Vector3.zero;

			movementDirection += Vector3.right * variableJoystick.Horizontal;
			movementDirection += Vector3.forward * variableJoystick.Vertical;
			movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);
			movementDirection.relativeTo(playerCamera.transform);
			SetMovementDirection(movementDirection);

			// Jump input

			if(Input.GetButtonDown($"Jump"))
				Jump();
			else if(Input.GetButtonUp($"Jump"))
				StopJumping();

			// Crouch input

			if(Input.GetKeyDown(KeyCode.C))
				Crouch();
			else if(Input.GetKeyUp(KeyCode.C))
				StopCrouching();
		}

		private void OnReset(){
			_rotationRate = 540.0f;

			_maxSpeed = 6.0f;
			_minAnalogSpeed = 0.0f;
			_maxAcceleration = 20.0f;
			_groundBrakingDeceleration = 20.0f;
			_groundFriction = 8.0f;

			_useSeparateBrakingFriction = false;
			_brakingFriction = 0.0f;

			_airBrakingDeceleration = 0.0f;
			_airFriction = 0.5f;
			_airControl = 0.3f;

			_canEverCrouch = false;
			_unCrouchedHeight = 2.0f;
			_crouchedHeight = 1.0f;
			_crouchedSpeedModifier = 0.5f;

			_jumpMaxCount = 1;
			_jumpImpulse = 6.0f;
			_jumpMaxHoldTime = 0.35f;
			_jumpPreGroundedTime = 0.15f;
			_jumpPostGroundedTime = 0.15f;

			_gravity = Physics.gravity;
			_gravityScale = 1.0f;
		}

		private void OnOnValidate(){
			rotationRate = _rotationRate;

			maxSpeed = _maxSpeed;
			minAnalogSpeed = _minAnalogSpeed;
			maxAcceleration = _maxAcceleration;
			groundBrakingDeceleration = _groundBrakingDeceleration;
			groundFriction = _groundFriction;

			brakingFriction = _brakingFriction;

			airBrakingDeceleration = _airBrakingDeceleration;
			airFriction = _airFriction;
			airControl = _airControl;

			unCrouchedHeight = _unCrouchedHeight;
			crouchedHeight = _crouchedHeight;
			crouchedSpeedModifier = _crouchedSpeedModifier;

			jumpMaxCount = _jumpMaxCount;
			jumpImpulse = _jumpImpulse;
			jumpMaxHoldTime = _jumpMaxHoldTime;
			jumpPreGroundedTime = _jumpPreGroundedTime;
			jumpPostGroundedTime = _jumpPostGroundedTime;

			gravityScale = _gravityScale;
		}

		private void OnAwake(){
			// Cache components

			_transform = GetComponent<Transform>();
			_characterMovement = GetComponent<CharacterMovement>();

			// By default enable late fixed update

			enableLateFixedUpdate = true;

			// Enable platform and physics interactions

			characterMovement.impartPlatformMovement = true;
			characterMovement.impartPlatformRotation = true;
			characterMovement.impartPlatformVelocity = true;

			characterMovement.enablePhysicsInteraction = true;
		}

		private void OnStart(){
			// DEFAULT EMPTY
		}

		private void OnOnEnable(){
			if(_enableLateFixedUpdateCoroutine)
				EnableLateFixedUpdate(true);
		}

		private void OnOnDisable(){
			if(_enableLateFixedUpdateCoroutine)
				EnableLateFixedUpdate(false);
		}

		private void OnFixedUpdate(){
			// DEFAULT EMPTY
		}

		private void OnLateFixedUpdate(){
			UpdateRotation();

			Move();
		}

		private void OnUpdate(){
			HandleInput();
		}

		#endregion

		#region MONOBEHAVIOUR

		private void Reset(){
			OnReset();
		}

		private void OnValidate(){
			OnOnValidate();
		}

		private void Awake(){
			OnAwake();
		}

		private void Start(){
			OnStart();
		}

		private void OnEnable(){
			OnOnEnable();
		}

		private void OnDisable(){
			OnOnDisable();
		}

		private void FixedUpdate(){
			OnFixedUpdate();
		}

		private void Update(){
			OnUpdate();
		}

		private IEnumerator LateFixedUpdate(){
			WaitForFixedUpdate waitTime = new WaitForFixedUpdate();

			while(true){
				yield return waitTime;

				OnLateFixedUpdate();
			}
		}

		#endregion
	}
}