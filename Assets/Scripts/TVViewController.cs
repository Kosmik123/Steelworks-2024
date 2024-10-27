using UnityEngine;

public class TVViewController : MonoBehaviour
{
	[SerializeField]
	private Room[] rooms;
	
	[SerializeField]
	private int currentlyViewedRoomIndex;
	public int CurrentlyViewedRoomIndex
	{
		get => currentlyViewedRoomIndex;
		set
		{
			int count = rooms.Length;
			value %= count;
			value += count; 
			value %= count;	
			currentlyViewedRoomIndex = value;
			RefreshCameras();
		}
	}

	public Room CurrentRoom => rooms[currentlyViewedRoomIndex];

	private void Awake()
	{
		RefreshCameras();
	}

	private void RefreshCameras()
	{
		for (int i = 0; i < rooms.Length; i++)
			rooms[i].IsViewed = i == currentlyViewedRoomIndex;
	}

	[ContextMenu("Next Camera")]
	public void NextCamera() => CurrentlyViewedRoomIndex++;
	
	[ContextMenu("Previous Camera")]
	public void PreviousCamera() => CurrentlyViewedRoomIndex--;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;	
		if (rooms != null && rooms.Length > 0 )
		{
			Gizmos.DrawCube(CurrentRoom.transform.position, 2 * Vector3.one);
		}
	}
}
