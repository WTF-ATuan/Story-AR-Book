using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Chapter_1.Interactive.Puzzle{
	public class PuzzlePlaceHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
		public GameObject pieceRoot;
		private List<Image> _piece;
		private bool _isMouseEnter;

		private void Start(){
			_piece = pieceRoot.GetComponentsInChildren<Image>(true).ToList();
			_piece.ForEach(x => x.OnEndDragAsObservable().Subscribe(AssignToPlace));
		}

		private void AssignToPlace(PointerEventData data){
			if(!_isMouseEnter || transform.childCount > 0) return;
			var dragObj = data.pointerDrag.gameObject;
			dragObj.GetComponent<DragablePiece>().ResetPosition(transform);
		}

		public void OnPointerEnter(PointerEventData eventData){
			_isMouseEnter = true;
		}

		public void OnPointerExit(PointerEventData eventData){
			_isMouseEnter = false;
		}
	}
}