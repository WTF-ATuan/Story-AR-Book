using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Chapter_1{
	public class SimpleUIMovement : MonoBehaviour{
		[SerializeField] private float offset = 2;
		[SerializeField] private float duration = 1;
		[SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

		private Vector3 _originPos;
		private Vector3 _targetPos;

		private void Start(){
			_originPos = transform.localPosition;
			_targetPos = _originPos + Vector3.up * offset;
			transform.DOLocalMove(_targetPos, duration)
					.SetEase(curve)
					.SetLoops(-1, LoopType.Yoyo);
			
		}
	}
}