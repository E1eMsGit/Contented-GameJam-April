using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RunnerManager : MonoBehaviour
{
	#region Filds
	public static RunnerManager Instance;

	[Header("Setup")]

	[SerializeField]
	int numOfLanes = 3;
	[SerializeField]
	int runnerStartPosition = 1;
	[SerializeField]
	RunnerController runner;
	[SerializeField]
	Transform runnerSpawnLocation;
	[SerializeField]
	int paintForWin = 50;

	[Header("Speed Controls")]

	[SerializeField]
	float initialRunSpeed = 3f;
	[SerializeField, Range(.1f, 10f)]
	float minSpeed = 1f;
	[SerializeField, Range(10f, 50f)]
	float maxSpeed = 10f;
	[SerializeField]
	float speedUpTime = 1f;
	[SerializeField]
	AnimationCurve speedUpCurve;
	[SerializeField]
	float speedDownTime = 1f;
	[SerializeField]
	AnimationCurve speedDownCurve;

	[Header("Spawn Controls")]

	[SerializeField]
	Transform itemSpawnLocation;
	[SerializeField]
	Item paint;
	[SerializeField]
	Item boost;
	[SerializeField]
	Item obsticle;
	[Header("Spawn rate is per second per speed")]
	[SerializeField]
	float paintSpawnRate = .25f;
	[SerializeField]
	float boostSpawnRate = .5f;
	[SerializeField]
	float obsticleSpawnRate = 2f;
	[SerializeField, Header("For how long a lane will be locked from spawing new object after one was spawned")]
	float occupancyTime = .1f; // this (hopefully) stopes items from being spawned on top of eachother

	[Header("UI stuff suld not be here but im to tired to give a shit")]
	[SerializeField]
	Slider paintProgressBar;
	[SerializeField]
	GameObject guiPanel;
	private GameGUI _gameGui;

	Animator animator;
	int paintCollected;
	float currentSpeed, targetSpeed, lastSpeed, speedTimer;
	bool speedUp;
	float paintSpawnTimer, boostSpawnTimer, obsticleSpawnTimer;
	float[] laneOccupancy;
	List<Item> items = new List<Item>();


	UnityEvent<float> worldSpeedChanged = new UnityEvent<float>();
	bool spawning = true;
	#endregion

	#region Properties
	public int PaintForWin { get => paintForWin; }
	public int PaintCollected { get => paintCollected; }
	public float InitialRunSpeed { get => initialRunSpeed; }
	public float CurrentSpeed { get => currentSpeed; }
	public float MinSpeed { get => minSpeed; }
	public float MaxSpeed { get => maxSpeed; }
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
		currentSpeed = initialRunSpeed;
		laneOccupancy = new float[numOfLanes];
		EventManager.SpeedChange.AddListener(OnSpeedChange);
		EventManager.PickedUpPaint.AddListener(OnPaintPickUp);
		EventManager.WorldSpeedChanged.AddInvoker(worldSpeedChanged);
	}
	private void Start()
	{
		_gameGui = guiPanel.GetComponent<GameGUI>();
		paintProgressBar.value = 0;
		_gameGui = guiPanel.GetComponent<GameGUI>();
		paintProgressBar.value = 0;
		RunnerController _temp = Instantiate(runner);
		_temp.transform.position = runnerSpawnLocation.position;
		_temp.Initialize(numOfLanes, runnerStartPosition);
		animator = _temp.transform.GetChild(0).GetComponent<Animator>();
		animator.SetFloat("speed", currentSpeed / 3);
	}
	private void Update()
	{
		if (spawning)
		{

			float _scaledTime = Time.deltaTime * currentSpeed;
			paintSpawnTimer += _scaledTime;
			boostSpawnTimer += _scaledTime;
			obsticleSpawnTimer += _scaledTime;
			for (int _i = 0; _i < laneOccupancy.Length; _i++)
			{
				if (laneOccupancy[_i] < occupancyTime)
				{
					laneOccupancy[_i] += _scaledTime;
				}
			}
			if (paintSpawnTimer >= 1 / paintSpawnRate)
			{
				if (SpawnPaint())
				{
					paintSpawnTimer = 0;
				}
				else
				{
					paintSpawnTimer -= occupancyTime / 2;
				}
			}
			if (boostSpawnTimer >= 1 / boostSpawnRate)
			{
				if (SpawnBoost())
				{
					boostSpawnTimer = 0;
				}
				else
				{
					boostSpawnTimer -= occupancyTime / 2;
				}
			}
			if (obsticleSpawnTimer >= 1 / obsticleSpawnRate)
			{
				if (SpawnObsticle())
				{
					obsticleSpawnTimer = 0;
				}
				else
				{
					obsticleSpawnTimer -= occupancyTime / 2;
				}
			}
		}
	}
	private void FixedUpdate()
	{
		if (speedTimer > 0)
		{
			speedTimer -= Time.fixedDeltaTime;
			float _progress;
			if (speedUp)
			{
				_progress = 1 - speedTimer / speedUpTime;
				currentSpeed = Mathf.LerpUnclamped(lastSpeed, targetSpeed, speedUpCurve.Evaluate(_progress));

			}
			else
			{
				_progress = 1 - speedTimer / speedDownTime;
				currentSpeed = Mathf.LerpUnclamped(lastSpeed, targetSpeed, speedDownCurve.Evaluate(_progress));

			}
			worldSpeedChanged.Invoke(currentSpeed);
			animator.SetFloat("speed", currentSpeed / 3);
		}
	}
	bool SpawnPaint()
	{
		List<int> _availableLanes = GetFreeLanesIndecies();

		if (_availableLanes.Count > 0)
		{
			int _spawnLaneIndex = _availableLanes[Random.Range(0, _availableLanes.Count)];
			Vector3 _spawnPosition = itemSpawnLocation.position + Vector3.forward * ((numOfLanes) / 2 - _spawnLaneIndex);
			laneOccupancy[_spawnLaneIndex] = 0;
			Item _temp = Instantiate(paint, _spawnPosition, Quaternion.identity, transform);
			items.Add(_temp);
			return true;
		}
		else
		{
			return false;
		}
	}
	bool SpawnBoost()
	{
		List<int> _availableLanes = GetFreeLanesIndecies();
		if (_availableLanes.Count > 0)
		{
			int _spawnLaneIndex = _availableLanes[Random.Range(0, _availableLanes.Count)];
			Vector3 _spawnPosition = itemSpawnLocation.position + Vector3.forward * ((numOfLanes) / 2 - _spawnLaneIndex);
			laneOccupancy[_spawnLaneIndex] = 0;
			Item _temp = Instantiate(boost, _spawnPosition, new Quaternion(0, 270, 270,0), transform);
			items.Add(_temp);
			return true;
		}
		else
		{
			return false;
		}
	}
	bool SpawnObsticle()
	{
		List<int> _availableLanes = GetFreeLanesIndecies();
		if (_availableLanes.Count > 0)
		{
			int _spawnLaneIndex = _availableLanes[Random.Range(0, _availableLanes.Count)];
			Vector3 _spawnPosition = itemSpawnLocation.position + Vector3.forward * ((numOfLanes) / 2 - _spawnLaneIndex);
			laneOccupancy[_spawnLaneIndex] = 0;
			Item _temp = Instantiate(obsticle, _spawnPosition, Quaternion.identity, transform);
			items.Add(_temp);
			return true;
		}
		else
		{
			return false;
		}
	}
	List<int> GetFreeLanesIndecies()
	{
		List<int> _indexes = new List<int>();
		for (int _i = 0; _i < laneOccupancy.Length; _i++)
		{
			if (laneOccupancy[_i] >= occupancyTime)
			{
				_indexes.Add(_i);
			}
		}
		return _indexes;
	}
	void OnSpeedChange(float speedDelta)
	{
		targetSpeed = speedDelta + currentSpeed;
		targetSpeed = Mathf.Clamp(targetSpeed, minSpeed, maxSpeed);

		lastSpeed = currentSpeed;
		if (currentSpeed < targetSpeed)
		{
			speedUp = true;
			speedTimer = speedUpTime;
		}
		else
		{
			speedUp = false;
			speedTimer = speedDownTime;
		}


	}
	void OnPaintPickUp(Vector3 unused)
	{
		paintCollected++;
		paintProgressBar.value = (float)paintCollected / (float)paintForWin;
		if (paintCollected >= PaintForWin)
		{
			_gameGui.WinScreenActive();
			currentSpeed = 0;
			worldSpeedChanged.Invoke(0);
			speedTimer = 0;
			spawning = false;
		}
	}
	#endregion
}
