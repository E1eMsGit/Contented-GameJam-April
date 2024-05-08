using UnityEngine;

public class RunnerController : MonoBehaviour
{
	#region Fields
	int numOfLanes;
	int lanePosition;
	#endregion

	#region Properties

	#endregion

	#region Methods
	public void Initialize(int numOfLanes, int lanePosition)
	{
		this.numOfLanes = numOfLanes;
		this.lanePosition = lanePosition;
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			MoveUp();
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			MoveDown();
		}

	}
	public void MoveUp()
	{
		if (lanePosition < numOfLanes - 1)
		{
			transform.position += Vector3.forward;
			lanePosition++;
		}
	}
	public void MoveDown()
	{
		if (lanePosition > 0)
		{
			transform.position -= Vector3.forward;
			lanePosition--;
		}
	}
	#endregion
}
