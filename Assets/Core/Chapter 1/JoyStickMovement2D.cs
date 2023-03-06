using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core{
	public class JoyStickMovement2D : MonoBehaviour{
		[Required] public VariableJoystick variableJoystick;
		[Required] public Transform playerRoot;

		private Rigidbody _rigidbody;
		private Animator _animator;
		private Vector3 _movementDirection = Vector3.zero;

		[Inject] private PlayerMoveData _moveData;

		private static readonly int Walk = Animator.StringToHash("walk");
		private static readonly int FaceFront = Animator.StringToHash("isFaceFront");
		private float _startScaleX;

		private void Start(){
			_rigidbody = GetComponent<Rigidbody>();
			_animator = GetComponentInChildren<Animator>();
			_startScaleX = transform.localScale.x;
		}

		private void Update(){
			CalculateMoveDirection();
			Move();
			PlayAnimation();
			FlipDirection();
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

		private void PlayAnimation(){
			var isWalking = _movementDirection.magnitude > 0f;
			var isFacingFront = (_movementDirection * _moveData.Direction).z > 0;
			_animator.SetBool(Walk, isWalking);
			_animator.SetBool(FaceFront, isFacingFront);
		}

		private void FlipDirection(){
			var isFaceRight = (_movementDirection * _moveData.Direction).x > 0;
			if(isFaceRight){
				var root = transform;
				var localScale = root.localScale;
				localScale = new Vector3(_startScaleX, localScale.y, localScale.z);
				root.localScale = localScale;
			}
			else{
				var root = transform;
				var localScale = root.localScale;
				localScale = new Vector3(-_startScaleX, localScale.y, localScale.z);
				root.localScale = localScale;
			}
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