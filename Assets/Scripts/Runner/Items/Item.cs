using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
	#region Fields
	float speed;
	protected UnityEvent<float> speedChange = new UnityEvent<float>();
	#endregion

	#region Properties
	#endregion

	#region Methods
	protected virtual void Awake()
	{
		EventManager.WorldSpeedChanged.AddListener(ChangeSpeed);
		EventManager.SpeedChange.AddInvoker(speedChange);
	}
	protected virtual void Start()
	{
		speed = RunnerManager.Instance.CurrentSpeed;
	}
	private void Update()
	{
		transform.position += Vector3.left * speed * Time.deltaTime;
	}
	void ChangeSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
	#endregion
}
