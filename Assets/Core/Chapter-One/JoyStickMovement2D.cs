using EasyCharacterMovement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Testing{
	public class JoyStickMovement2D : MonoBehaviour{
		[Required] public VariableJoystick variableJoystick;
		public float maxSpeed = 5;

		public float acceleration = 20.0f;
		public float deceleration = 20.0f;

		private void Update(){
			// Read Input values

			var horizontal = variableJoystick.Horizontal;
			var vertical = variableJoystick.Vertical;

			// Create a movement direction vector (in world space)

			var movementDirection = Vector3.zero;
			movementDirection += transform.forward * vertical;
			movementDirection += transform.right * horizontal;

			// Make Sure it won't move faster diagonally

			movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);
			
			// Perform movement

			var desiredVelocity = movementDirection * maxSpeed * Time.fixedDeltaTime;
			transform.localPosition += desiredVelocity;
		}
	}
}