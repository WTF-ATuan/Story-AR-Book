using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Core.Testing{
	[RequireComponent(typeof(ARTrackedImageManager))]
	public class ImagePosture : MonoBehaviour{
		private ARTrackedImageManager _imageTracker;

		[Header("Spawn Object")] [SerializeField]
		private List<GameObject> placeablePrefabs;

		private Dictionary<string, GameObject> _spawnedObjectFinder = new();

		private void Awake(){
			_imageTracker = GetComponent<ARTrackedImageManager>();
			foreach(var prefab in placeablePrefabs){
				var newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
				newPrefab.name = prefab.name;
				_spawnedObjectFinder.Add(prefab.name, newPrefab);
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

			foreach(var trackedImage in obj.removed){
				_spawnedObjectFinder[trackedImage.name].SetActive(false);
			}
		}

		private void UpdateImage(ARTrackedImage trackedImage){
			var imageName = trackedImage.referenceImage.name;
			var position = trackedImage.transform.position;

			var prefab = _spawnedObjectFinder[imageName];
			prefab.transform.position = position;
			prefab.SetActive(true);
			foreach(var go in _spawnedObjectFinder.Values.Where(go => go.name != imageName)){
				go.SetActive(false);
			}
		}
	}
}