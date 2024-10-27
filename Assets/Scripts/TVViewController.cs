using Bipolar;
using NaughtyAttributes;
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
			if (rooms == null)
				return;
			
			int count = rooms.Length;
			if (count <= 0)
				return;

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
		rooms.Shuffle();	
		RefreshCameras();
	}

	private void RefreshCameras()
	{
		for (int i = 0; i < rooms.Length; i++)
			rooms[i].IsViewed = i == currentlyViewedRoomIndex;
	}

	[Button]
	[ContextMenu("Next Camera")]
	public void NextCamera() => CurrentlyViewedRoomIndex++;
	
	[Button]
	[ContextMenu("Previous Camera")]
	public void PreviousCamera() => CurrentlyViewedRoomIndex--;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;	
		if (rooms != null && rooms.Length > 0)
		{
			if (CurrentRoom)	
				Gizmos.DrawCube(CurrentRoom.transform.position, 2 * Vector3.one);
		}
	}

	private void OnValidate()
	{
		CurrentlyViewedRoomIndex = currentlyViewedRoomIndex;
	}
}
