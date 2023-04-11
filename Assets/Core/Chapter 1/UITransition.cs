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
		public void TextTransitionFadeIn(Text text){
			text.DOFade(0, 0f);
			text.DOFade(1, 1f);
		}

		//move image to offset
		public void ImageMove(Image image, Vector2 offset){
			image.transform.DOLocalMove(offset, 0.5f);
		}

		//move text to offset
		public void TextMove(Text text, Vector2 offset){
			text.transform.parent.DOLocalMove(offset, 0.5f);
		}
	}
}