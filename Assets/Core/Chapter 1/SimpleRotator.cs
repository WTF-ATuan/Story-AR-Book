using DG.Tweening;
using UnityEngine;

namespace Core.Chapter_1{
	public class SimpleRotator : MonoBehaviour{
		
		public void RotateOffsetZ(float zOffset){
			transform.DOLocalRotate(new Vector3(0, 0, zOffset), 0.25f);
		}
	}
}