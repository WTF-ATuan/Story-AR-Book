using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Chapter_1{
	public class UITransition : MonoBehaviour{
		public void ImageTransition(Image image){
			image.DOFade(0, 0.5f).OnComplete(() => image.gameObject.SetActive(false));
		}

		public void TextTransition(Text text){
			text.DOFade(0, 0.5f).OnComplete(() => text.transform.parent.gameObject.SetActive(false));
		}
	}
}