using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class ShadowDetection : MonoBehaviour{
		private ParticleSystem _particle;
		[Inject] private readonly UIPresenter _presenter;

		private const float DangerDistance = 0.04f;
		[SerializeField] private float detectRange = 0.08f;

		private bool _isDetecting;

		private void Start(){
			_particle = GetComponentInChildren<ParticleSystem>();
		}

		private void OnTriggerExit(Collider other) => _isDetecting = false;

		private void OnTriggerStay(Collider other){
			if(!other.CompareTag("Player")) return;
			var distance = Vector3.Distance(other.transform.position, transform.position);
			if(distance <= DangerDistance && !_isDetecting){
				_presenter.CameraFade(() => EventAggregator.Publish(new StoryEvent("Scary")));
				_isDetecting = true;
			}

			ScaleShadow(distance);
		}

		// function scaling shadow and color with distance
		private void ScaleShadow(float distance){
			var mainModule = _particle.main;
			var emission = _particle.emission;
			// percentage is the distance between detectRange and DangerDistance detectRange is 100% and DangerDistance is 0%
			var percentage = Mathf.Clamp01((distance - DangerDistance) / (detectRange - DangerDistance));
			mainModule.startSpeed = Mathf.Lerp(0.1f, 0.3f, 1 - percentage);
			mainModule.startSize = Mathf.Lerp(1.5f, 2f, 1 - percentage);
			emission.rateOverTime = Mathf.Lerp(25f, 30f, 1 - percentage);
		}

		private void OnDrawGizmos(){
			//if application is not playing, return
			if(!Application.isPlaying) return;
			Gizmos.color = Color.red;
			var position = transform.position;
			Gizmos.DrawWireSphere(position, DangerDistance);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(position, detectRange);
		}
	}
}