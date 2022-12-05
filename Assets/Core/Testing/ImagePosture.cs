using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace Core.Testing{
	[RequireComponent(typeof(ARTrackedImageManager))]
	public class ImagePosture : MonoBehaviour{
		private ARTrackedImageManager _imageTracker;

		[Header("Spawn Object")] [SerializeField]
		private List<GameObject> spawnPrefabList;

		private readonly List<GameObject> _referenceList = new();

		private void Awake(){
			_imageTracker = GetComponent<ARTrackedImageManager>();
			foreach(var prefab in spawnPrefabList){
				var instantiate = Instantiate(prefab, Vector3.zero, Quaternion.identity);
				instantiate.name = prefab.name;
				instantiate.SetActive(false);
				_referenceList.Add(instantiate);
			}
		}

		private void OnEnable(){
			_imageTracker.trackedImagesChanged += OnTrackedImagesChanged;
		}

		private void OnDisable(){
			_imageTracker.trackedImagesChanged -= OnTrackedImagesChanged;
		}

		private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj){
			foreach(var trackedImage in obj.added){
				UpdateImage(trackedImage);
			}

			foreach(var trackedImage in obj.updated){
				UpdateImage(trackedImage);
			}
		}

		private void UpdateImage(ARTrackedImage trackedImage){
			var detectObject = _referenceList.Find(x => x.name == trackedImage.referenceImage.name);
			detectObject.SetActive(true);
			detectObject.transform.position = trackedImage.transform.position;
			foreach(var go in _referenceList){
				if(go.name != trackedImage.referenceImage.name){
					go.SetActive(false);
				}
			}
		}
	}
}