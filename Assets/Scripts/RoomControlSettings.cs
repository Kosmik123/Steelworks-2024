using UnityEngine;

[CreateAssetMenu]
public class RoomControlSettings : ScriptableObject
{
	[SerializeField]
	private float normalTimeSpeed = 1;
	public float NormalTimeSpeed => normalTimeSpeed;

	[SerializeField]
	private float fastForwardTimeSpeed = 2;
	public float FastForwardTimeSpeed => fastForwardTimeSpeed;
	
	[SerializeField]
	private float rewindTimeSpeed = -1.5f;
	public float RewindTimeSpeed => rewindTimeSpeed;
}
