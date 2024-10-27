using UnityEngine;
using UnityEngine.Splines;
using Bipolar;

[RequireComponent(typeof(SplineContainer))]
public class GrandpaPath : MonoBehaviour
{
	private SplineContainer _spline;
	public SplineContainer Spline => this.GetRequired(ref _spline);
	
	[SerializeField]
	private PathType type;
	public virtual PathType Type => type;

	private void Start()
	{
	}
}
