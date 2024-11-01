using Bipolar.RaycastSystem;
using UnityEngine;

public class RaycastActionsController : MonoBehaviour
{
	[SerializeField]
	private RaycastController controller;

	private void Update()
	{
        if (Input.GetMouseButtonDown(0))
        {
			if (controller)
			{
				if (controller.Target != null)
				{
					if (controller.Target.TryGetComponent<RaycastAction>(out var action))
					{
						action.DoAction();
					}

				}
			}
        }
    }
}
