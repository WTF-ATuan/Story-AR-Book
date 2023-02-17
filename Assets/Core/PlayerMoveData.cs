using UnityEngine;

namespace Core{
	[System.Serializable]
	public class PlayerMoveData{
		[field: SerializeField]
		public float Acceleration{ get; set; }

		[field: SerializeField]
		public float DeAcceleration{ get; set; }

		[field: SerializeField]
		public float MoveClamp{ get; set; }

		[field: SerializeField]
		public float DetectRange{ get; set; }

		[field: SerializeField]
		public bool EnableGizmos{ get; set; }
	}
}