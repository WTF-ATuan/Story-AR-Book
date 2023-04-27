using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Chapter_1.Interactive.Puzzle{
	public class SwitchPuzzle : MonoBehaviour, IPuzzle{
		private List<Image> _images;
		[SerializeField] private List<Vector3> _piecePositions;
		private Transform _currentEnterPiece;
		private Transform _currentDragPiece;

		private void Start(){
			_images = new List<Image>(GetComponentsInChildren<Image>());
			_piecePositions = _images.Select(x => x.transform.position).ToList();
			_images.ForEach(x => x.OnPointerDownAsObservable().Subscribe(DetectDrag));
			_images.ForEach(x => x.OnPointerUpAsObservable().Subscribe(Switch));
			_images.ForEach(x => x.OnPointerEnterAsObservable().Subscribe(DetectPointEnter));
			RandomSwitchPosition();
		}

		private void RandomSwitchPosition(){
			// random pick image and switch position with other image in _images 
			foreach(var image in _images){
				var randomIndex = Random.Range(0, _images.Count);
				var imageTransform = image.transform;
				(imageTransform.position, _images[randomIndex].transform.position) = (
					_images[randomIndex].transform.position, imageTransform.position);
			}
		}

		private void DetectDrag(PointerEventData obj){
			var pointerObj = obj.pointerCurrentRaycast.gameObject;
			_currentDragPiece = pointerObj.transform;
			pointerObj.GetComponent<CanvasGroup>().alpha = 0.4f;
		}

		private void DetectPointEnter(PointerEventData obj){
			var pointerObj = obj.pointerEnter.gameObject;
			_currentEnterPiece = pointerObj.transform;
		}

		private void Switch(PointerEventData obj){
			if(!_currentEnterPiece || !_currentEnterPiece) return;
			var temp = _currentEnterPiece.position;
			_currentEnterPiece.DOMove(_currentDragPiece.position, 0.2f);
			_currentDragPiece.DOMove(temp, 0.2f).OnComplete(
						() => StateChanged?.Invoke(new PuzzleData(this, Complete())));
			_currentDragPiece.GetComponent<CanvasGroup>().alpha = 1f;
		}

		public Action<PuzzleData> StateChanged{ get; set; }

		private bool Complete(){
			var positions = _images.Select(x => x.transform.position).ToList();
			// compare positions with _piecePositions
			return positions.SequenceEqual(_piecePositions);
		}
	}
}