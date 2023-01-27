using Zenject;

namespace Core.Chapter_1{
	public class StoryInstaller : MonoInstaller<StoryInstaller>{
		public override void InstallBindings(){
			Container.Bind<PlayerData>().AsSingle().NonLazy();
		}
	}
}