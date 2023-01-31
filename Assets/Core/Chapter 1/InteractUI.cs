using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Chapter_1{
	public class InteractUI : MonoBehaviour{
		[TitleGroup("Data Holder")] [ReadOnly] [SerializeField]
		private Sprite image;

		[ReadOnly] [SerializeField] private AnimationClip animationClip;
		[ReadOnly] [SerializeField] private AudioClip audioClip;

		[TitleGroup("Components")] [SerializeField] [Required]
		private Image imageComponent;

		[SerializeField] [Required] private Animation animationComponent;
		[SerializeField] [Required] private Button buttonComponent;
		[SerializeField] [Required] private AudioSource audioComponent;


		private string _currentObjID;

		[Inject] private readonly InteractDataSet _interactDataSet;
		[Inject] private readonly PlayerData _playerData;

		private void Start(){
			image = imageComponent.sprite;
			animationClip = animationComponent.clip;
			audioClip = audioComponent.clip;
			buttonComponent.OnClickAsObservable().Subscribe(x => OnInteractButtonClick());
		}

		[Button]
		public void ChangeUIData(string objID){
			var interactData = _interactDataSet.FindData(objID);
			image = interactData.image;
			animationClip = interactData.animationClip;
			audioClip = interactData.audioClip;
			_currentObjID = objID;
			SetDataToComponent();
			SetComponentActive(true);
		}

		private void SetComponentActive(bool active){
			imageComponent.gameObject.SetActive(active);
			animationComponent.gameObject.SetActive(active);
			buttonComponent.gameObject.SetActive(active);
			audioComponent.gameObject.SetActive(active);
		}

		private void SetDataToComponent(){
			imageComponent.sprite = image;
			animationComponent.clip = animationClip;
			animationComponent.Play(_currentObjID);
			//Button .....
			audioComponent.clip = audioClip;
		}

		private void OnInteractButtonClick(){
			if(_interactDataSet.CheckCorrect(_currentObjID)){
				Debug.Log("找到錯誤音效! 給予一個 '通關條件'");
				_playerData.SaveSuccessResult(_currentObjID);
			}
			else{
				Debug.Log("這是正確的音效，沒有問題");
			}

			SetComponentActive(false);
		}
	}
}