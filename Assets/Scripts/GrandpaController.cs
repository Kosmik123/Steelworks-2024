using UnityEngine;

public class GrandpaController : MonoBehaviour
{
	[SerializeField]
	private Grandpa grandpa;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			ExitThroughtNearestDoor();
	}

	[ContextMenu("Exit Through Door")]
	public void ExitThroughtNearestDoor()
	{
		var currentRoom = grandpa.RoomObject.CurrentRoom;
		if (currentRoom == null)
			return;

		var door = currentRoom.GetNearestDoor(grandpa.transform.position);
		if (door == null) 
			return;

		var nextRoom = door.GetOtherRoom(currentRoom);
		if (nextRoom == null)
			return;

		SplineConnectionUtility.ConnectSplinesThroughtPoint(grandpa.TransitionContainer.Spline, grandpa.CurrentRoom.Spline, 0,
			grandpa.transform.position, door.transform, nextRoom.Spline, 0);
		grandpa.StartTransition(nextRoom);
	}
}
