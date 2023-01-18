using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core{
	[System.Serializable]
	public class SoundDataSet{
		public List<SoundData> soundDataList = new List<SoundData>();

		[HideLabel] [FoldoutGroup("Get Sound Effect Function")] [FolderPath] [InlineButton("GetSoundData")]
		public string dataPath;

		[System.Serializable]
		public class SoundData{
			public string soundID;
			public AudioClip soundEffect;
		}

		public void GetSoundData(){
			var audioClips = Resources.LoadAll<AudioClip>(dataPath);
			if(audioClips.Length < 1){
				Debug.Log($"{dataPath} have no AudioClip");
				return;
			}

			foreach(var audio in audioClips){
				var soundData = new SoundData{
					soundID = audio.name,
					soundEffect = audio
				};
				if(!soundDataList.Contains(soundData)){
					soundDataList.Add(soundData);
				}
			}
		}
	}
}