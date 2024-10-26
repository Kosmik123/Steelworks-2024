using UnityEngine;
using UnityEngine.Splines;
using Bipolar;

[RequireComponent(typeof(SplineContainer))]
public class GrandpaPath : MonoBehaviour
{
	private SplineComponent _spline;
	public SplineComponent Spline => this.GetRequired(ref _spline);

	[SerializeField]
	private PathType type;
	public PathType Type => type;
}
