using UnityEngine;

namespace Core.Project{
	[System.Serializable]
	public class PlayerMoveData{
		[field: SerializeField]
		public float Acceleration{ get; set; }

		[field: SerializeField]
		public float DeAcceleration{ get; set; }

		[field: SerializeField]
		public float MoveClamp{ get; set; }
	}
}