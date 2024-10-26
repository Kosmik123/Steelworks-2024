using UnityEngine;

public class RoomObject : MonoBehaviour
{
	[SerializeField]
	private Room room;
	public Room Room
	{
		get => room;
		set => room = value;
	}
}
