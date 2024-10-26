using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class GrandpaController : MonoBehaviour
{
	[SerializeField]
	private SplineAnimate splineMovement;

	[SerializeField]
	private SplineContainer start;
	[SerializeField]
	private SplineContainer end;

	private void Reset()
	{
		splineMovement = GetComponent<SplineAnimate>();	
	}


	[ContextMenu("Create Spline Test")]
	private void Test()
	{
		var splineContainer = new GameObject("A").AddComponent<SplineContainer>();
		var spline = SplineConnectionUtility.ConnectSplines(start, end);
		splineContainer.RemoveSplineAt(0);
		splineContainer.AddSpline(spline);
	}
}

public static class SplineConnectionUtility
{
	public static Spline ConnectSplines(SplineContainer startSplineContainer, SplineContainer endSplineContainer) =>
		ConnectSplines(startSplineContainer, 0, endSplineContainer, 0);

	public static Spline ConnectSplines(SplineContainer startSplineContainer, int startSplineIndex, SplineContainer endSplineContainer, int endSplineIndex)
	{
		var startSpline = startSplineContainer.Splines[startSplineIndex];
		var endSpline = endSplineContainer.Splines[endSplineIndex];

		int bestStartKnotIndex = GetNearestKnotIndex(startSplineContainer, startSplineIndex, endSplineContainer.transform.position);
		int bestEndKnotIndex = GetNearestKnotIndex(endSplineContainer, endSplineIndex, startSplineContainer.transform.position);

		var startKnot = startSpline[bestStartKnotIndex];
		startKnot.Position = startSplineContainer.transform.TransformPoint(startKnot.Position);
		var endKnot = endSpline[bestEndKnotIndex];
		endKnot.Position = endSplineContainer.transform.TransformPoint(endKnot.Position);

		var knots = new BezierKnot[] { startKnot, endKnot };
		return new Spline(knots);
	}


	public static int GetNearestKnotIndex(SplineContainer splineContainer, Vector3 position) => GetNearestKnotIndex(splineContainer, 0, position);
	public static int GetNearestKnotIndex(SplineContainer splineContainer, int splineIndex, Vector3 position)
	{
		var spline = splineContainer.Splines[splineIndex];
		int nearestKnotIndex = 0;
		float nearestKnotSquareDistance = (position - splineContainer.transform.TransformPoint(spline[nearestKnotIndex].Position)).sqrMagnitude;
		for (int i = 0; i < spline.Count; i++)
			if ((position - splineContainer.transform.TransformPoint(spline[i].Position)).sqrMagnitude < nearestKnotSquareDistance)
				nearestKnotIndex = i;

		return nearestKnotIndex;
	}
}
