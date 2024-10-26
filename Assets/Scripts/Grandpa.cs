using UnityEngine;
using UnityEngine.Splines;
using Bipolar;
using NaughtyAttributes;
using System.Collections.Generic;

public class GrandpaController : MonoBehaviour
{
	[SerializeField]
	private Grandpa grandpa;






}

public class Grandpa : MonoBehaviour
{
	private RoomObject _roomObject;
	public RoomObject RoomObject => this.GetRequired(ref _roomObject);

	[SerializeField]
	private SplineAnimate splineMovement;

	[SerializeField]
	private GrandpaPath currentPath;

	[SerializeField]
	private float movementSpeed;

	[Space, SerializeReference]
	private TransitionData currentTransition = null;
	[SerializeField, ReadOnly]
	private List<TransitionData> transitionsHistory;

	public bool IsTransitioning => currentTransition != null;

	[System.Serializable]
	private class TransitionData
	{
		public GrandpaPath startPath;
		public float startSplineProgress;
		public Vector3 startPosition;
		
		public Vector3 targetPosition;
		public GrandpaPath targetPath;
	}

	private void Reset()
	{
		splineMovement = GetComponent<SplineAnimate>();
	}

	private void Start()
	{
		splineMovement.ElapsedTime = 0;
	}

	private void Update()
	{
		float elapsedTime = splineMovement.ElapsedTime + RoomObject.Room.DeltaTime * movementSpeed;
		elapsedTime %= splineMovement.Duration;
		elapsedTime += splineMovement.Duration;
		elapsedTime %= splineMovement.Duration;
		splineMovement.ElapsedTime = elapsedTime;
	}
}
