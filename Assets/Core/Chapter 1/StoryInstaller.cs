using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Chapter_1{
	public class StoryInstaller : MonoInstaller<StoryInstaller>{
		[SerializeField, ReadOnly] private InteractRepository interactRepository;
		[SerializeField, ReadOnly] private InteractUI interactUI;

		public override void InstallBindings(){
			GetSceneData();
			Container.Bind<PlayerData>().AsSingle().NonLazy();
			Container.Bind<InteractRepository>().FromInstance(interactRepository).AsSingle();
			Container.Bind<InteractUI>().FromInstance(interactUI).AsSingle();
		}

		private void GetSceneData(){
			interactRepository = FindObjectOfType<InteractRepository>();
			interactUI = FindObjectOfType<InteractUI>(true);
		}
	}
}