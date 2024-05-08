using UnityEngine;
using UnityEngine.Events;

public class Paint : Item
{
	#region Fields
	[SerializeField]
	Renderer[] paintRenderers;

	UnityEvent<Vector3> paintPickedUp = new UnityEvent<Vector3>();
	#endregion
	protected override void Awake()
	{
		base.Awake();
		EventManager.PickedUpPaint.AddInvoker(paintPickedUp);
	}
	protected override void Start()
	{
		base.Start();
		Color _newColor = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		foreach (Renderer _renderer in paintRenderers)
		{
			_renderer.material.color = _newColor;
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			paintPickedUp.Invoke(transform.position);
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "AnihilatorPlane")
		{
			Destroy(gameObject);
		}
	}
}
