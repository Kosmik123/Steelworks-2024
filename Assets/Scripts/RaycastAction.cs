using UnityEngine;
using UnityEngine.Events;

public class RaycastAction : MonoBehaviour
{
	[SerializeField]
	private UnityEvent action;

	public void DoAction() => action.Invoke();
}
