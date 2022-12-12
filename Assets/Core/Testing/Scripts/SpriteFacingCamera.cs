using UnityEngine;

namespace Core.Testing{
	public class SpriteFacingCamera : MonoBehaviour{
		private void Update(){
			if(Camera.main is not null)
				transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
		}
	}
}