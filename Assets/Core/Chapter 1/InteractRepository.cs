using System;
using System.Collections.Generic;
using ModestTree;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core{
	public class InteractRepository : MonoBehaviour{
		public List<Collider> interactObject;


		public void RegisterWithName(string id, Action<string, bool> callback){
			var foundCollider = interactObject.Find(x => x.name == id);
			if(!foundCollider){
				throw new Exception($"can,t find {id}");
			}

			foundCollider.OnTriggerEnterAsObservable().Subscribe(x => callback(x.name, true));
			foundCollider.OnTriggerExitAsObservable().Subscribe(x => callback(x.name, false));
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

		public void RegisterWithTag(string objectTag, Action<string, bool> callback){
			var foundCollider = interactObject.FindAll(x => x.CompareTag(objectTag));
			if(foundCollider.IsEmpty()){
				throw new Exception($"can,t find {objectTag}");
			}

			foreach(var collider in foundCollider){
				collider.OnTriggerEnterAsObservable().Subscribe(x => callback(collider.name, true));
				collider.OnTriggerExitAsObservable().Subscribe(x => callback(collider.name, false));
			}
		}
	}
}