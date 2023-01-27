using System;
using System.Collections.Generic;
using ModestTree;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core{
	public class InteractRepository : MonoBehaviour{
		public List<Collider> interactObject;


		public void RegisterWithName(string id, Action<Collider, bool> callback){
			var foundCollider = interactObject.Find(x => x.name == id);
			if(!foundCollider){
				throw new Exception($"can,t find {id}");
			}

			foundCollider.OnTriggerEnterAsObservable().Subscribe(x => callback(x, true));
			foundCollider.OnTriggerExitAsObservable().Subscribe(x => callback(x, false));
		}

		public void RegisterWithName(string id, bool enterOrExit, Action<Collider, string> callback){
			var foundCollider = interactObject.Find(x => x.name == id);
			if(!foundCollider){
				throw new Exception($"can,t find {id}");
			}

			if(enterOrExit){
				foundCollider.OnTriggerEnterAsObservable().Subscribe(x => callback(x, foundCollider.name));
			}
			else{
				foundCollider.OnTriggerExitAsObservable().Subscribe(x => callback(x, foundCollider.name));
			}
		}

		public void RegisterWithTag(string objectTag, bool enterOrExit, Action<Collider, string> callback){
			var foundCollider = interactObject.FindAll(x => x.CompareTag(objectTag));
			if(foundCollider.IsEmpty()){
				throw new Exception($"can,t find {objectTag}");
			}

			foreach(var collider in foundCollider){
				if(enterOrExit){
					collider.OnTriggerEnterAsObservable().Subscribe(x => callback(x, collider.name));
				}
				else{
					collider.OnTriggerExitAsObservable().Subscribe(x => callback(x, collider.name));
				}
			}
		}
	}
}