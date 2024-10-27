using UnityEngine;
using UnityEngine.Splines;

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
		var spline = new Spline();
		ConnectSplinesNonAlloc(spline, startSplineContainer, startSplineIndex, startSplineNormalizedProgress, endSplineContainer, endSplineIndex);
		return spline;
	}

	public static void ConnectSplinesNonAlloc(Spline connection, SplineContainer startSplineContainer, Vector3 startPosition, SplineContainer endSplineContainer) =>
		ConnectSplinesNonAlloc(connection, startSplineContainer, 0, startPosition, endSplineContainer, 0);

	public static void ConnectSplinesNonAlloc(Spline connection, SplineContainer startSplineContainer, int startSplineIndex, Vector3 startPosition, SplineContainer endSplineContainer, int endSplineIndex)
	{
		var startSpline = startSplineContainer[startSplineIndex];
		SplineUtility.GetNearestPoint(startSpline, startPosition, out _, out float normalizedProgress);
		ConnectSplinesNonAlloc(connection, startSplineContainer, startSplineIndex, normalizedProgress, endSplineContainer, endSplineIndex);
	}

	public static void ConnectSplinesNonAlloc(Spline connection, SplineContainer startSplineContainer, int startSplineIndex, float startSplineNormalizedProgress, SplineContainer endSplineContainer, int endSplineIndex)
	{
		var startSpline = startSplineContainer.Splines[startSplineIndex];
		var endSpline = endSplineContainer.Splines[endSplineIndex];

		startSpline.Evaluate(startSplineNormalizedProgress, out var startPosition, out var startTangent, out var startUp);
		var startKnot = new BezierKnot
		{
			Position = startSplineContainer.transform.TransformPoint(startPosition),
			TangentOut = Vector3.forward,
			TangentIn = Vector3.back,
			Rotation = Quaternion.LookRotation(startTangent, startUp),
		};

		int bestEndKnotIndex = GetNearestKnotIndex(endSplineContainer, endSplineIndex, startKnot.Position);
		var endKnot = endSpline[bestEndKnotIndex];
		endKnot.Position = endSplineContainer.transform.TransformPoint(endKnot.Position);

		PopulateSpline(connection, startKnot, endKnot);
	}

	public static void ConnectSplinesThroughtPoint(Spline connection, SplineContainer startSplineContainer, int startSplineIndex,
		Vector3 startPosition, Transform intermediaryPoint, SplineContainer endSplineContainer, int endSplineIndex)
	{
		var spline = startSplineContainer[startSplineIndex];
		SplineUtility.GetNearestPoint(spline, startPosition, out _, out float normalizedProgress);
		ConnectSplinesThroughtPoint(connection, startSplineContainer, startSplineIndex, normalizedProgress, intermediaryPoint, endSplineContainer, endSplineIndex);
	}

	public static void ConnectSplinesThroughtPoint(Spline connection, SplineContainer startSplineContainer, int startSplineIndex,
		float startSplineNormalizedProgress, Transform intermediaryPoint, SplineContainer endSplineContainer, int endSplineIndex)
	{
		var startSpline = startSplineContainer.Splines[startSplineIndex];
		var endSpline = endSplineContainer.Splines[endSplineIndex];

		startSpline.Evaluate(startSplineNormalizedProgress, out var startPosition, out var startTangent, out var startUp);
		var startKnot = new BezierKnot
		{
			Position = startSplineContainer.transform.TransformPoint(startPosition),
			TangentOut = Vector3.forward,
			TangentIn = Vector3.back,
			Rotation = Quaternion.LookRotation(startTangent, startUp),
		};

		int bestEndKnotIndex = GetNearestKnotIndex(endSplineContainer, endSplineIndex, intermediaryPoint.position);
		var endKnot = endSpline[bestEndKnotIndex];
		endKnot.Position = endSplineContainer.transform.TransformPoint(endKnot.Position);

		bool isWithTheFlow = Vector3.Dot(endKnot.Position - startKnot.Position, intermediaryPoint.forward) > 0;
		var intermediaryKnot = new BezierKnot
		{
			Position = intermediaryPoint.position,
			TangentOut = isWithTheFlow ? Vector3.forward : Vector3.back,
			TangentIn = isWithTheFlow ? Vector3.back : Vector3.forward,
			Rotation = intermediaryPoint.rotation,
		};

		connection.Clear();
		connection.Add(startKnot, TangentMode.Continuous);
		connection.Add(intermediaryKnot, TangentMode.Continuous);
		connection.Add(endKnot, TangentMode.Continuous);
	}
}
