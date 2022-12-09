using EasyCharacterMovement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Testing{
	public class JoyStickMovement : MonoBehaviour{
		[Required] public VariableJoystick variableJoystick;
		[Required] public Camera playerCamera;
		public float rotationRate = 540.0f;

		public float maxSpeed = 5;

		public float acceleration = 20.0f;
		public float deceleration = 20.0f;

		public float groundFriction = 8.0f;
		public float airFriction = 0.5f;

		public float jumpImpulse = 6.5f;

		[Range(0.0f, 1.0f)] public float airControl = 0.3f;

		public Vector3 gravity = Vector3.down * 9.81f;

		private CharacterMovement _characterMovement;

		private Vector3 _movementDirection;

		private void Awake(){
			_characterMovement = GetComponent<CharacterMovement>();
		}

		private void Update(){
			// Read Input values

			var horizontal = variableJoystick.Horizontal;
			var vertical = variableJoystick.Vertical;

			// Create a movement direction vector (in world space)

			_movementDirection = Vector3.zero;
			_movementDirection += Vector3.forward * vertical;
			_movementDirection += Vector3.right * horizontal;
			_movementDirection.relativeTo(playerCamera.transform);

			// Make Sure it won't move faster diagonally

			_movementDirection = Vector3.ClampMagnitude(_movementDirection, 1.0f);

			// Jump
			if(_characterMovement.isGrounded && Input.GetButton($"Jump")){
				_characterMovement.PauseGroundConstraint();
				_characterMovement.LaunchCharacter(Vector3.up * jumpImpulse, true);
			}

			// Rotate towards movement direction

			_characterMovement.RotateTowards(_movementDirection, rotationRate * Time.deltaTime);

			// Perform movement

			var desiredVelocity = _movementDirection * maxSpeed;

			var actualAcceleration = _characterMovement.isGrounded ? acceleration : acceleration * airControl;
			var actualDeceleration = _characterMovement.isGrounded ? deceleration : 0.0f;

			var actualFriction = _characterMovement.isGrounded ? groundFriction : airFriction;

			_characterMovement.SimpleMove(desiredVelocity, maxSpeed, actualAcceleration, actualDeceleration,
				actualFriction, actualFriction, gravity);
		}
	}
}