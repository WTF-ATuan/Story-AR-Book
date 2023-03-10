using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Core.Chapter_1.Interactive.Puzzle{
	public class RotatePuzzle : MonoBehaviour, IPointerClickHandler, IPuzzle{
		[SerializeField] private bool randomRotate = true;
		public Action<PuzzleData> StateChanged{ get; set; }
		private bool _freezeFlag;


		private void Start(){
			if(!randomRotate) return;
			var range = Random.Range(1, 4);
			var localEulerAngles = transform.localEulerAngles;
			transform.DOLocalRotate(new Vector3(localEulerAngles.x, localEulerAngles.y, range * 90), 0.5f);
		}

		public void OnPointerClick(PointerEventData eventData){
			if(_freezeFlag) return;
			_freezeFlag = true;
			transform.DOLocalRotate(new Vector3(0, 0, transform.localEulerAngles.z + 90), 0.5f)
					.OnComplete(() => _freezeFlag = false);
			StateChanged?.Invoke(new PuzzleData(this, Complete()));
		}

		public bool Complete(){
			return transform.localEulerAngles.z == 270;
		}
	}
}