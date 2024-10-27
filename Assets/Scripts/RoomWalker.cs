using UnityEngine;
using UnityEngine.Splines;
using Bipolar;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SplineMovement), typeof(RoomObject))]
public class RoomWalker : MonoBehaviour
{
	private RoomObject _roomObject;
	public RoomObject RoomObject => this.GetRequired(ref _roomObject);

	private SplineMovement _splineMovement;
	public SplineMovement SplineMovement => this.GetRequired(ref _splineMovement);

	public Room CurrentRoom => RoomObject.CurrentRooms.First();

	[SerializeField]
	private float movementSpeed;

	[SerializeField]
	private SplineContainer currentPath;
	public SplineContainer CurrentPath => currentPath;

	[Space, SerializeReference]
	private TransitionData currentTransition = null;
	[SerializeField, ReadOnly]
	private List<TransitionData> transitionsHistory;

	public bool IsTransitioning => currentTransition != null;

	private SplineContainer transitionsContainer;
	public SplineContainer TransitionContainer => transitionsContainer;

	[System.Serializable]
	private class TransitionData
	{
		public SplineContainer startPath;
		public float startSplineProgress;
		public Vector3 startPosition;

		public Vector3 targetPosition;
		public SplineContainer targetPath;
	}

	private void Awake()
	{
		transitionsContainer = new GameObject($"{name} Transtions Container").AddComponent<SplineContainer>();
	}

	private void Start()
	{
		SetPath(currentPath);
	}

	private void Update()
	{
		float speed = movementSpeed;
		foreach(var room in RoomObject.CurrentRooms)
			speed *= room.LocalTimeSpeed;

		SplineMovement.Speed = speed;
	}

	public void SetPath(SplineContainer path, float startOffset = 0)
	{
		currentPath = path;
		float distance = SplineUtility.GetNearestPoint(currentPath.Spline,
			currentPath.transform.InverseTransformPoint(transform.position), out var nearest, out float normalizedProgress);
		SplineMovement.Path = currentPath;
		SplineMovement.ElapsedTime = normalizedProgress * currentPath.CalculateLength();
	}

	public void StartTransition(SplineContainer targetPath) => StartTransition(targetPath, targetPath.EvaluatePosition(0));

	public void StartTransition(SplineContainer targetPath, Vector3 targetPosition)
	{
		SplineMovement.Path = transitionsContainer;
		SplineMovement.ElapsedTime = 0;
		currentTransition = new TransitionData
		{
			startPath = currentPath,
			startPosition = transform.position,
			startSplineProgress = SplineMovement.ElapsedTime,
			targetPosition = targetPosition,
			targetPath = targetPath,
		};
		currentPath = null;

		SplineMovement.OnPathEndReached += SplineMovement_OnTransitionEnded;
	}

	private void SplineMovement_OnTransitionEnded()
	{
		SplineMovement.OnPathEndReached -= SplineMovement_OnTransitionEnded;

		var newPath = SplineMovement.Speed < 0
			? currentTransition.startPath
			: currentTransition.targetPath;
		SetPath(newPath);

		//transitionsHistory.Insert(0, currentTransition);
		currentTransition = null;
		SplineMovement.enabled = true;
	}
}
