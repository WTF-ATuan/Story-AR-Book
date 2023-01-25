using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vuforia;
using Zenject;

namespace Core.Chapter_One{
	public class PlatformSwitcher : MonoBehaviour{
		[TitleGroup("AR component")] public VuforiaBehaviour arCamera;
		public AreaTargetBehaviour areaTarget;
		[TitleGroup("Editor")] public Camera editorCamera;

		[TitleGroup("Mobile")] public Camera mobileCamera;

		[TitleGroup("Switch Object")] [InlineButton("GetFromChild")]
		public List<GameObject> switchObjectList;

		[Inject] private PlatformType _platformType;

		private void Awake(){
			SwitchPlatform();
		}

		private void GetFromChild(){
			for(var i = 0; i < transform.childCount; i++){
				var child = transform.GetChild(i);
				if(!switchObjectList.Contains(child.gameObject)){
					switchObjectList.Add(child.gameObject);
				}
			}
		}

		[Button]
		private void SwitchPlatform(){
			switch(_platformType){
				case PlatformType.ArBuild:
					SetArActive(true);
					SetEditorActive(false);
					SetMobileActive(false);
					foreach(var switchObject in switchObjectList){
						switchObject.transform.SetParent(areaTarget.transform);
					}

					break;
				case PlatformType.MobileBuild:
					SetArActive(false);
					SetEditorActive(false);
					SetMobileActive(true);
					foreach(var switchObject in switchObjectList){
						switchObject.transform.SetParent(null);
					}

					break;
				case PlatformType.Editor:
					SetArActive(false);
					SetEditorActive(true);
					SetMobileActive(false);
					foreach(var switchObject in switchObjectList){
						switchObject.transform.SetParent(transform);
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void SetArActive(bool active){
			arCamera.gameObject.SetActive(active);
			areaTarget.gameObject.SetActive(active);
		}

		private void SetEditorActive(bool active){
			editorCamera.gameObject.SetActive(active);
		}

		private void SetMobileActive(bool active){
			mobileCamera.gameObject.SetActive(active);
		}
	}
}