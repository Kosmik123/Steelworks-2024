using UnityEngine;
using Bipolar;

[RequireComponent(typeof(Room))]
public class RoomController : MonoBehaviour
{
	private Room _room;
	public Room Room => this.GetRequired(ref _room);

	[SerializeField]
	private RoomControlSettings settings;
}
