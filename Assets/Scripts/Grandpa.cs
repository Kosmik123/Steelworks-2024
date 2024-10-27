using UnityEngine;
using UnityEngine.Splines;
using Bipolar;
using NaughtyAttributes;
using System.Collections.Generic;

[RequireComponent(typeof(SplineMovement), typeof(RoomObject))]
public class Grandpa : MonoBehaviour
{
	private RoomObject _roomObject;
	public RoomObject RoomObject => this.GetRequired(ref _roomObject);
	public Room CurrentRoom => RoomObject.CurrentRoom;

	private SplineMovement _splineMovement;
	public SplineMovement SplineMovement => this.GetRequired(ref _splineMovement);

	[Space, SerializeReference]
	private TransitionData currentTransition = null;
	[SerializeField, ReadOnly]
	private List<TransitionData> transitionsHistory;

	public bool IsTransitioning => currentTransition != null;

	[SerializeField]
	private SplineContainer transitionsContainer;
	public SplineContainer TransitionContainer => transitionsContainer;

	[System.Serializable]
	private class TransitionData
	{
		public Room startRoom;
		public float startSplineProgress;
		public Vector3 startPosition;

		public Vector3 targetPosition;
		public Room targetRoom;
	}

	private void Start()
	{
		SetRoom(RoomObject.CurrentRoom);
	}

	public void SetRoom(Room path, float startOffset = 0)
	{
		RoomObject.CurrentRoom = path;
		float distance = SplineUtility.GetNearestPoint(CurrentRoom.Spline.Spline,
			CurrentRoom.transform.InverseTransformPoint(transform.position), out var nearest, out float normalizedProgress);
		SplineMovement.Path = CurrentRoom.Spline;
		SplineMovement.ElapsedTime = normalizedProgress * CurrentRoom.Spline.CalculateLength();
	}

	public void StartTransition(Room targetRoom) => StartTransition(targetRoom, targetRoom.Spline.EvaluatePosition(0));

	public void StartTransition(Room targetRoom, Vector3 targetPosition)
	{
		SplineMovement.Path = transitionsContainer;
		SplineMovement.ElapsedTime = 0;
		currentTransition = new TransitionData
		{
			startRoom = CurrentRoom,
			startPosition = transform.position,
			startSplineProgress = SplineMovement.ElapsedTime,
			targetPosition = targetPosition,
			targetRoom = targetRoom,
		};
		RoomObject.CurrentRoom = null;

		SplineMovement.OnPathEndReached += SplineMovement_OnTransitionEnded;
	}

	private void SplineMovement_OnTransitionEnded()
	{
		SplineMovement.OnPathEndReached -= SplineMovement_OnTransitionEnded;
		transitionsHistory.Insert(0, currentTransition);
		SetRoom(currentTransition.targetRoom);
		currentTransition = null;
		SplineMovement.enabled = true;
	}
}
