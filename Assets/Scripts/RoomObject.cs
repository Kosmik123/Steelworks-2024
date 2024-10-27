using UnityEngine;

public class RoomObject : MonoBehaviour
{
	[SerializeField]
	private Room currentRoom;
	public Room CurrentRoom
	{
		get => currentRoom;
		set => currentRoom = value;
	}
}
