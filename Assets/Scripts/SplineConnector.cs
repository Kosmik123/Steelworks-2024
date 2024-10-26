using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Splines;

public class SplineConnector : MonoBehaviour
{
	public SplineContainer start;

	public SplineContainer end;

	public Transform grandpa;

	public SplineContainer connectionContainer;

	[ContextMenu("Connect")]
	private void TestConnect()
	{
		var connection = new GameObject("Connection").AddComponent<SplineContainer>();
		connection.RemoveSplineAt(0);
		var spline = SplineConnectionUtility.ConnectSplines(start, grandpa.position, end);
		connection.AddSpline(spline);
	}

	private void Update()
	{
		connectionContainer.RemoveSplineAt(0);
		var spline = SplineConnectionUtility.ConnectSplines(start, grandpa.position, end);
		connectionContainer.AddSpline(spline);
	}
}
