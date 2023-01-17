using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Testing{
	public class JoyStickMovement2D : MonoBehaviour{
		[Required] public VariableJoystick variableJoystick;
		[SerializeField] private Transform playerRoot;
		public float maxSpeed = 5;

		private Rigidbody _rigidbody;

		private void Start(){
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Update(){
			var horizontal = variableJoystick.Horizontal;
			var vertical = variableJoystick.Vertical;

			var movementDirection = Vector3.zero;
			movementDirection += playerRoot.forward * vertical;
			movementDirection += playerRoot.right * horizontal;


			movementDirection = Vector3.ClampMagnitude(movementDirection, 1.0f);

			// Perform movement

			var desiredVelocity = movementDirection * maxSpeed * Time.fixedDeltaTime;
			_rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(desiredVelocity));
		}
	}
}