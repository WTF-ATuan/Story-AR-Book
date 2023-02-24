using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core{
	public class JoyStickMovement2D : MonoBehaviour{
		[Required] public VariableJoystick variableJoystick;
		[Required] public Transform playerRoot;

		private Rigidbody _rigidbody;
		private Vector3 _movementDirection = Vector3.zero;

		[Inject] private PlayerMoveData _moveData;

		private void Start(){
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Update(){
			CalculateMoveDirection();
			Move();
		}

		private void CalculateMoveDirection(){
			var horizontal = variableJoystick.Horizontal;
			var vertical = variableJoystick.Vertical;
			if(horizontal != 0 || vertical != 0){
				_movementDirection += playerRoot.forward * _moveData.Direction * vertical;
				_movementDirection += playerRoot.right * _moveData.Direction * horizontal;
				_movementDirection = _movementDirection * _moveData.Acceleration * Time.deltaTime;
				_movementDirection = Vector3.ClampMagnitude(_movementDirection, _moveData.MoveClamp);
			}
			else{
				_movementDirection.x =
						Mathf.MoveTowards(_movementDirection.x, 0, _moveData.DeAcceleration * Time.deltaTime);
				_movementDirection.z =
						Mathf.MoveTowards(_movementDirection.z, 0, _moveData.DeAcceleration * Time.deltaTime);
			}
		}

		private void Move(){
			_rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(_movementDirection));
		}

		private void OnDrawGizmos(){
			if(!(_moveData is{ EnableGizmos: true })){
				return;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawRay(playerRoot.position, _movementDirection.normalized * _moveData.DetectRange);
		}
	}
}