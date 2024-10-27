using NaughtyAttributes;
using UnityEngine;

public class RemoteControl : MonoBehaviour
{
	[SerializeField]
	private GrandpaController grandpaController;
	[SerializeField]
	private TVViewController viewControlller;
	[SerializeField]
	private RoomControlSettings roomControlSettings;

	[Button]
	public void Play()
	{
		viewControlller.CurrentRoom.LocalTimeSpeed = roomControlSettings.NormalTimeSpeed;
	}

	[Button]
	public void Pause()
	{
		viewControlller.CurrentRoom.LocalTimeSpeed = 0;
	}

	[Button]
	public void FastForward()
	{
		viewControlller.CurrentRoom.LocalTimeSpeed = roomControlSettings.FastForwardTimeSpeed;
	}

	[Button]
	public void Rewind()
	{
		viewControlller.CurrentRoom.LocalTimeSpeed = roomControlSettings.RewindTimeSpeed;
	}

	[Button]
	public void Exit()
	{
		grandpaController.ExitThroughtNearestDoor();
	}

	public void SetProgram(int program)
	{
		viewControlller.CurrentlyViewedRoomIndex = program;
	}

	[Button]
	public void ChangeProgramUp()
	{
		viewControlller.CurrentlyViewedRoomIndex++;
	}

	[Button]
	public void ChangeProgramDown()
	{
		viewControlller.CurrentlyViewedRoomIndex--;
	}

	[Button]
	public void Info()
	{
		// view map
	}

	[Button]
	public void Open()
	{
		// open all doors in current room
	}

}
