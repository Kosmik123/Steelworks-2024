using UnityEngine;

[RequireComponent(typeof(RemoteControl))]
public class RemoteControlTesting : MonoBehaviour
{
	private RemoteControl remoteControl;

	private void Awake()
	{
		remoteControl = GetComponent<RemoteControl>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
			remoteControl.FastForward();
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			remoteControl.Rewind();
		else if (Input.GetKeyDown(KeyCode.P))
			remoteControl.Play();
		else if (Input.GetKeyDown(KeyCode.O))
			remoteControl.Pause();
		else if (Input.GetKeyDown(KeyCode.UpArrow))
			remoteControl.ChangeProgramUp();
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			remoteControl.ChangeProgramDown();
		else
			for (int i = 0; i <= 9; i++)
				if (Input.GetKeyDown(KeyCode.Alpha0 + i))
					remoteControl.SetProgram(i);

	}
}
