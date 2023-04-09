using System.Collections.Generic;
using UnityEngine;

namespace Core.Chapter_1{
	public class OutlineRepository : MonoBehaviour{
		public List<GameObject> outlineObjects;

		public void ActiveOutline(string id){
			DisableAllOutline();
			var foundObject = outlineObjects.Find(x => x.name == id);
			if(!foundObject){
				throw new System.Exception($"can,t find {id} outline object");
			}

			foundObject.SetActive(true);
		}
		public void DisableOutline(string id){
			var foundObject = outlineObjects.Find(x => x.name == id);
			if(!foundObject){
				throw new System.Exception($"can,t find {id} outline object");
			}

			foundObject.SetActive(false);
		}
		public void DisableAllOutline(){
			foreach(var obj in outlineObjects){
				obj.SetActive(false);
			}
		}
	}
}