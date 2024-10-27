using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
	[System.Serializable]
	private class RoomVisits
	{
		public Room room;
		public int enteredTriggersCount;

		public RoomVisits(Room room, int enteredTriggersCount)
		{
			this.room = room;
			this.enteredTriggersCount = enteredTriggersCount;
		}
	}

	[SerializeField, ReadOnly]
	private List<RoomVisits> enteredRoomTriggersCounts = new List<RoomVisits>();

	public IEnumerable<Room> CurrentRooms => enteredRoomTriggersCounts.Select(visit => visit.room);

	public bool IsInRoom(Room room) => CurrentRooms.Contains(room);

	public float TimeScale
	{
		get
		{
			float scale = 1f;
			foreach (var room in CurrentRooms)
				scale *= room.LocalTimeSpeed;
			return scale;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var room = other.GetComponentInParent<Room>();
		if (room == null)
			return;

		var visit = enteredRoomTriggersCounts.FirstOrDefault(visit => visit.room == room);
		if (visit == null)
			enteredRoomTriggersCounts.Add(new RoomVisits(room, 1));
		else
			visit.enteredTriggersCount++;
	}

	private void OnTriggerExit(Collider other)
	{
		var room = other.GetComponentInParent<Room>();
		if (room == null)
			return;

		var visit = enteredRoomTriggersCounts.FirstOrDefault(visit => visit.room == room);
		if (visit == null)
			return;

		visit.enteredTriggersCount--;
		if (visit.enteredTriggersCount <= 0)
			enteredRoomTriggersCounts.Remove(visit);
	}
}
