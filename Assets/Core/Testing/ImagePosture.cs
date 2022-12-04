using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Core.Testing{
	[RequireComponent(typeof(ARTrackedImageManager))]
	public class ImagePosture : MonoBehaviour{
		private ARTrackedImageManager _imageTracker;

		private void Start(){
			_imageTracker = GetComponent<ARTrackedImageManager>();
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

			foreach(var trackedImage in obj.removed){
				RemoveImage(trackedImage);
			}
		}

		private void UpdateImage(ARTrackedImage trackedImage){
			var trackedTransform = trackedImage.transform;
			var trackedName = trackedImage.name;
		}

		private void RemoveImage(ARTrackedImage trackedImage){
			var trackedTransform = trackedImage.transform;
			var trackedName = trackedImage.name;
		}
	}
}