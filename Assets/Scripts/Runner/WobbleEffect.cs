using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
	#region Fields
	[SerializeField]
	float horizontalDelta = 1f;
	[SerializeField]
	float horizontalSpeed = 1f;
	[SerializeField]
	AnimationCurve horizonatlMoveCurve;
	[SerializeField]
	float horizontalSpeedRandomMultiplier = .1f;
	[SerializeField]
	float spinSpeed = 15f;
	[SerializeField]
	float spinSpeedRandomMultiplier = .1f;

	float timer;
	bool goUp = true;
	Transform grfxTransform;
	Vector3 lowestPoint, highestPoint;

	#endregion

	#region Properties

	#endregion

	#region Methods
	private void Awake()
	{
		timer = .5f;
		grfxTransform = transform.GetChild(0);
		lowestPoint = (grfxTransform.localPosition.y + horizontalDelta * -.5f) * Vector3.up;
		highestPoint = (grfxTransform.localPosition.y - horizontalDelta * -.5f) * Vector3.up;
		horizontalSpeed += Random.Range(-horizontalSpeedRandomMultiplier, horizontalSpeedRandomMultiplier) * horizontalSpeed;
		spinSpeed += Random.Range(-spinSpeedRandomMultiplier, spinSpeedRandomMultiplier) * horizontalSpeed;
	}
	private void Update()
	{
		if (goUp)
		{
			timer += Time.deltaTime * horizontalSpeed;
			if (timer >= 1)
			{
				goUp = false;
			}
		}
		else
		{
			timer -= Time.deltaTime * horizontalSpeed;
			if (timer <= 0)
			{
				goUp = true;
			}
		}
		grfxTransform.localPosition = Vector3.Lerp(lowestPoint, highestPoint, horizonatlMoveCurve.Evaluate(timer));
		grfxTransform.rotation *= Quaternion.Euler(0, spinSpeed * Time.deltaTime, 0);
	}
	#endregion
}
