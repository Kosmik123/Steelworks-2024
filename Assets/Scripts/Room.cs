using UnityEngine;

public class Room : MonoBehaviour
{
	[SerializeField]
	private float localTimeSpeed = 1;
	public float LocalTimeSpeed
	{
		get => localTimeSpeed;
		set => localTimeSpeed = value;
	}

	public float DeltaTime => localTimeSpeed * Time.deltaTime;
}
