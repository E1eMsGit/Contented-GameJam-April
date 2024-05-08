using UnityEngine;

public class Prop : MonoBehaviour
{
	#region Fields
	[SerializeField]
	int iD;
	[SerializeField]
	bool isStatic = false;
	[SerializeField]
	Material unpaintedMaterial;

	float speed;
	bool isPainted = true;
	Material[] propMaterials;
	Material[] unpaintedMaterials;
	MeshRenderer propRenderer;

	#endregion

	#region Properties
	public int ID { get => iD; }
	#endregion

	#region Methods
	private void Awake()
	{
		EventManager.WorldSpeedChanged.AddListener(ChangeSpeed);
		propRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
		propMaterials = propRenderer.materials;
		unpaintedMaterials = new Material[propRenderer.materials.Length];
		for (int _i = 0; _i < propMaterials.Length; _i++)
		{
			unpaintedMaterials[_i] = unpaintedMaterial;
		}
	}
	private void Start()
	{
		speed = RunnerManager.Instance.CurrentSpeed;
	}
	private void Update()
	{
		if (!isStatic)
		{
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
	}
	void ChangeSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
	public void TogglePaint()
	{
		if (isPainted)
		{
			propRenderer.materials = unpaintedMaterials;
			isPainted = false;
		}
		else
		{
			propRenderer.materials = propMaterials;
			isPainted = true;
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "AnihilatorPlane")
		{
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "PaintMissile")
		{
			if (other.GetComponent<PaintMissile>().Target == this)
			{
				TogglePaint();
				Destroy(other.gameObject);
			}
		}
	}
	#endregion
}
