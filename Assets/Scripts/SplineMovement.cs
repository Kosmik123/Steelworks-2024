using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMovement : MonoBehaviour
{
	public event System.Action OnPathEndReached;

	[SerializeField]
	private SplineContainer path;
	public SplineContainer Path
	{
		get => path;
		set => path = value;
	}

	[SerializeField]
	private float speed;
	public float Speed
	{
		get => speed;
		set => speed = value;
	}

	[SerializeField]
	private float currentElapsedTime;
	public float ElapsedTime
	{
		get => currentElapsedTime;
		set => currentElapsedTime = value;
	}

	[SerializeField, ReadOnly]
	private float currentProgress;
	public float CurrentProgress => currentProgress;

	private void FixedUpdate()
	{
		if (path == null || path.Splines.Count < 1)
			return;

		float dt = Time.deltaTime;
		currentElapsedTime += speed * dt;
		float length = path.CalculateLength();
		bool isPathClosed = path.Spline.Closed;
		if (isPathClosed)
		{
			currentElapsedTime %= length;
			currentElapsedTime += length;
			currentElapsedTime %= length;
		}
		currentProgress = Mathf.Clamp01(currentElapsedTime / length);
		path.Evaluate(currentProgress, out var position, out var forward, out var up);
		transform.SetPositionAndRotation(position, Quaternion.LookRotation(forward, up));
		
		if (isPathClosed == false)
		{
			if (speed > 0 && currentProgress == 1 || speed < 0 && currentProgress == 0)
			{
				enabled = false;
				OnPathEndReached?.Invoke();
			}
		}
	}
}
