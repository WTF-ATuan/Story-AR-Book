using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Chapter_1.Interactive.Puzzle{
	public class PuzzlePlaceHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPuzzle{
		public GameObject pieceRoot;
		private List<Image> _piece;
		private bool _isMouseEnter;

		public Action<PuzzleData> StateChanged{ get; set; }

		private void Start(){
			_piece = pieceRoot.GetComponentsInChildren<Image>(true).ToList();
			_piece.ForEach(x => x.OnEndDragAsObservable().Subscribe(AssignToPlace));
		}

		private void AssignToPlace(PointerEventData data){
			if(!_isMouseEnter || transform.childCount > 0) return;
			var dragObj = data.pointerDrag.gameObject;
			dragObj.GetComponent<DragablePiece>().ResetPosition(transform);
			StateChanged?.Invoke(new PuzzleData(this, Complete()));
		}

		public void OnPointerEnter(PointerEventData eventData){
			_isMouseEnter = true;
		}

		public void OnPointerExit(PointerEventData eventData){
			_isMouseEnter = false;
		}

		private bool Complete(){
			return transform.childCount >= 1 && transform.GetChild(0).name.Equals(name);
		}
	}
}