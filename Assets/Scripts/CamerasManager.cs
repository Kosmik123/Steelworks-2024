using Cinemachine;
using UnityEngine;

public class CamerasManager : MonoBehaviour
{
	[SerializeField]
	private CinemachineVirtualCamera[] allCameras;
	
	[SerializeField]
	private int cameraIndex;
	public int CameraIndex
	{
		get => cameraIndex;
		set
		{
			int count = allCameras.Length;
			value %= count;
			value += count; 
			value %= count;	
			cameraIndex = value;
			RefreshCameras();
		}
	}

	private void Awake()
	{
		RefreshCameras();
	}

	private void RefreshCameras()
	{
		for (int i = 0; i < allCameras.Length; i++)
			allCameras[i].enabled = i == cameraIndex;
	}

	[ContextMenu("Next Camera")]
	public void NextCamera() => CameraIndex++;
	
	[ContextMenu("Previous Camera")]
	public void PreviousCamera() => CameraIndex--;
}
