using System.Collections.Generic;
using UnityEngine;

public class SceneryManager : MonoBehaviour
{
	#region Fields
	public static SceneryManager Instance;

	[SerializeField]
	Transform RayAnchor;
	[SerializeField]
	List<Prop> propsToSpawn;
	[SerializeField]
	List<Prop> preplacedProps;
	[SerializeField]
	float minDistance = 10;
	[SerializeField]
	float maxDistance = 250;
	[SerializeField]
	float propsPerSecond = 5;
	[SerializeField]
	AnimationCurve spawnCurve;
	[SerializeField]
	PaintMissile paintMissile;

	Dictionary<Prop, bool> paintedProps = new Dictionary<Prop, bool>();
	List<Prop> propTypes;
	List<Prop> propsList = new List<Prop>();
	float speed;
	float spawnTimer = 0;
	float spawnCooldown;
	Ray ray;
	float howManyPaintsToPaintProp;
	float currentPaintProgress;
	#endregion

	#region Properties

	#endregion

	#region Methods
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			Instance = this;
		}
		spawnCooldown = 1 / propsPerSecond;
		ray = new Ray(RayAnchor.transform.position, RayAnchor.transform.forward);
		EventManager.WorldSpeedChanged.AddListener(ChangeSpeed);
		EventManager.PickedUpPaint.AddListener(CheckPainting);
	}
	static void FisherYatesShuffle<T>(List<T> list)
	{
		System.Random random = new System.Random();
		int n = list.Count;

		// Start from the end and swap elements with a random one
		for (int i = n - 1; i > 0; i--)
		{
			int j = random.Next(0, i + 1);
			T temp = list[i];
			list[i] = list[j];
			list[j] = temp;
		}
	}
	private void Start()
	{
		propTypes = new List<Prop>();
		foreach (Prop _prop in preplacedProps)
		{
			_prop.TogglePaint();
			propTypes.Add(_prop);
			propsList.Add(_prop);
		}
		propTypes.AddRange(propsToSpawn);
		FisherYatesShuffle(propTypes);
		foreach (Prop _prop in propTypes)
		{
			paintedProps.Add(_prop, false);
		}

		speed = RunnerManager.Instance.CurrentSpeed;
		howManyPaintsToPaintProp = RunnerManager.Instance.PaintForWin / propTypes.Count;
	}
	private void Update()
	{
		spawnTimer += Time.deltaTime * speed;
		if (spawnTimer >= spawnCooldown)
		{
			spawnTimer = 0;
			float _spawnSeed = spawnCurve.Evaluate(Random.Range(0f, 1f));
			float _disntaceToSpawn = (maxDistance - minDistance) * _spawnSeed + minDistance;
			Vector3 _placeToSpawn = ray.GetPoint(_disntaceToSpawn);
			Prop _propToSpawn = propsToSpawn[Random.Range(0, propsToSpawn.Count)];
			Collider[] _collisions = Physics.OverlapSphere(_placeToSpawn, 3f);
			if (_collisions.Length == 0)
			{
				Prop _newProp = Instantiate(_propToSpawn, _placeToSpawn, Quaternion.identity * Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
				if (!paintedProps[_propToSpawn])
				{
					_newProp.TogglePaint();
				}
				propsList.Add(_newProp);
			}
		}
	}
	void ChangeSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
	void CheckPainting(Vector3 paintPosition)
	{
		currentPaintProgress++;
		if (currentPaintProgress >= howManyPaintsToPaintProp)
		{
			for (int _i = 0; _i < propTypes.Count; _i++)
			{
				if (!paintedProps[propTypes[_i]])
				{
					paintedProps[propTypes[_i]] = true;
					foreach (Prop _prop in propsList)
					{
						if (_prop != null && _prop.ID == propTypes[_i].ID)
						{
							PaintMissile _temp = Instantiate(paintMissile, paintPosition, Quaternion.identity, transform);
							_temp.Initilize(new Vector3(Random.Range(-.5f, .5f), 1, Random.Range(-.5f, .5f)), 10, _prop);
						}
					}
					break;
				}
			}
			currentPaintProgress = 0;
		}
	}
	#endregion
}
