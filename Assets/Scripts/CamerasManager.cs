using UnityEngine;

public class CamerasManager : MonoBehaviour
{
	[SerializeField]
	private Camera[] allCameras;
	[SerializeField]
	private RenderTexture renderTexture;

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
		}
	}

	[ContextMenu("Test")]
	private void Test()
	{
		CameraIndex = 200;
	}



}
