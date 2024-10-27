using UnityEngine;

public class RemoteControl : MonoBehaviour
{
	[SerializeField]
	private GrandpaController grandpaController;
	[SerializeField]
	private CamerasManager camerasManager;

	public void Play()
	{

	}

	public void Pause()
	{

	}

	public void FastForward()
	{

	}

	public void Rewind()
	{

	}

	public void Exit()
	{
		grandpaController.ExitThroughtNearestDoor();
	}

	public void SetProgram(int program)
	{

	}

	public void ChangeProgramUp()
	{

	}

	public void ChangeProgramDown()
	{

	}

	public void Info()
	{
		// view map
	}

	public void Open()
	{
		// open all doors in current room
	}

}
