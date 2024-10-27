using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField]
	private Room room1;
	[SerializeField]
	private Room room2;

	public Room GetOtherRoom(Room room)
	{
		if (room == room1)
			return room2;

		if (room == room2)
			return room1;

		return null;
	}

	private void Awake()
	{
		room1.AddDoor(this);
		room2.AddDoor(this);
	}
}
