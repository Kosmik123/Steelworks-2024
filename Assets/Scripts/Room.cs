using Bipolar;
using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class Room : MonoBehaviour
{
	public event System.Action OnLocalTimeSpeedChanged;

	[SerializeField]
	private float localTimeSpeed = 1;
	public float LocalTimeSpeed
	{
		get => localTimeSpeed;
		set
		{
			if (localTimeSpeed != value)
			{
				localTimeSpeed = value;
				OnLocalTimeSpeedChanged?.Invoke();
			}
		}
	}

	private SplineContainer _path;
	public SplineContainer Path => this.GetRequired(ref _path);

	[SerializeField]
	private CinemachineVirtualCamera virtualCamera;
	public CinemachineVirtualCamera VirtualCamera => virtualCamera;

	private readonly List<Door> doors = new List<Door>();

	public float DeltaTime => localTimeSpeed * Time.deltaTime;



	public void AddDoor(Door door) => doors.Add(door);

	public bool IsViewed
	{
		set
		{
			if (virtualCamera)
				virtualCamera.enabled = value;
			if (value == false)
				LocalTimeSpeed = 1;
		}
	}

	public Door GetNearestDoor(Vector3 position)
	{
		if (doors.Count < 1)
			return null;

		var nearestDoor = doors[0];
		float nearestDoorSquareDistance = (nearestDoor.transform.position - position).sqrMagnitude;
		for (int i = 1; i < doors.Count; i++) 
		{
			var door = doors[i];
			float squreDistance = (door.transform.position - position).sqrMagnitude;
			if (squreDistance < nearestDoorSquareDistance)
				nearestDoor = door;
		}

		return nearestDoor;
	}
}
