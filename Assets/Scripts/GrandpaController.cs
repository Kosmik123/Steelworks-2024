using UnityEngine;

public class GrandpaController : MonoBehaviour
{
	[SerializeField]
	private RoomWalker grandpa;
	public RoomWalker Grandpa => grandpa;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			ExitThroughtNearestDoor();
	}

	[ContextMenu("Exit Through Door")]
	public void ExitThroughtNearestDoor()
	{
		var currentPath = grandpa.CurrentPath;
		if (currentPath == null)
			return;

		if (currentPath.TryGetComponent<Room>(out var currentRoom) == false)
			return;

		var door = currentRoom.GetNearestDoor(grandpa.transform.position);
		if (door == null) 
			return;

		var nextRoom = door.GetOtherRoom(currentRoom);
		if (nextRoom == null)
			return;

		SplineConnectionUtility.ConnectSplinesThroughtPoint(grandpa.TransitionContainer.Spline, grandpa.CurrentPath, 0,
			grandpa.transform.position, door.transform, nextRoom.Path, 0);
		grandpa.StartTransition(nextRoom.Path);
	}
}
