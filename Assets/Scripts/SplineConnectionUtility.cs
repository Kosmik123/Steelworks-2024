using UnityEngine;
using UnityEngine.Splines;
using static Unity.Mathematics.math;

public static class SplineConnectionUtility
{
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

		return CreateSpline(startKnot, endKnot);
	}

	private static Spline CreateSpline(BezierKnot startKnot, BezierKnot endKnot)
	{
		var spline = new Spline();
		PopulateSpline(spline, startKnot, endKnot);
		return spline;
	}

	public static void PopulateSpline(Spline spline, BezierKnot startKnot, BezierKnot endKnot)
	{
		spline.Clear();
		spline.Add(startKnot, TangentMode.Continuous);
		spline.Add(endKnot, TangentMode.Continuous);
	}

	public static int GetNearestKnotIndex(SplineContainer splineContainer, int splineIndex, Vector3 position)
	{
		var spline = splineContainer.Splines[splineIndex];
		int nearestKnotIndex = 0;
		float nearestKnotSquareDistance = (splineContainer.transform.TransformPoint(spline[nearestKnotIndex].Position) - position).sqrMagnitude;
		for (int i = 0; i < spline.Count; i++)
			if ((splineContainer.transform.TransformPoint(spline[i].Position) - position).sqrMagnitude < nearestKnotSquareDistance)
				nearestKnotIndex = i;

		return nearestKnotIndex;
	}

	public static Spline ConnectSplines(SplineContainer startSplineContainer, Vector3 startPosition, SplineContainer endSplineContainer) =>
		ConnectSplines(startSplineContainer, 0, startPosition, endSplineContainer, 0);

	public static Spline ConnectSplines(SplineContainer startSplineContainer, int startSplineIndex, Vector3 startPosition, SplineContainer endSplineContainer, int endSplineIndex)
	{
		var spline = startSplineContainer[startSplineIndex];
		SplineUtility.GetNearestPoint(spline, startPosition, out _, out float normalizedProgress);
		return ConnectSplines(startSplineContainer, startSplineIndex, normalizedProgress, endSplineContainer, endSplineIndex);
	}

	public static Spline ConnectSplines(SplineContainer startSplineContainer, int startSplineIndex, float startSplineNormalizedProgress, SplineContainer endSplineContainer, int endSplineIndex)
	{
		var startSpline = startSplineContainer.Splines[startSplineIndex];
		var endSpline = endSplineContainer.Splines[endSplineIndex];

		startSpline.Evaluate(startSplineNormalizedProgress, out var startPosition, out var startTangent, out var startUp);
		var startKnot = new BezierKnot
		{
			Position = startPosition, // prawdopodobnie trzeba będzie to przetransformować
			TangentOut = Vector3.forward,
			TangentIn = Vector3.back,
			Rotation = Quaternion.LookRotation(startTangent, startUp),
		};

		int bestEndKnotIndex = GetNearestKnotIndex(endSplineContainer, endSplineIndex, startKnot.Position);
		var endKnot = endSpline[bestEndKnotIndex];
		endKnot.Position = endSplineContainer.transform.TransformPoint(endKnot.Position);

		return CreateSpline(startKnot, endKnot);
	}





}
