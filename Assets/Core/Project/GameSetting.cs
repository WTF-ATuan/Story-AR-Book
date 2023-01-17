using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.Project{
	[CreateAssetMenu(fileName = "GameSetting", menuName = "Setting/GameSetting", order = 0)]
	public class GameSetting : ScriptableObject{
		[EnumToggleButtons] [HideLabel] public PlatformType platform = PlatformType.ArBuild;
		public SoundDataSet soundDataSet;
	}

	public enum PlatformType{
		ArBuild,
		MobileBuild,
		Editor,
	}
}