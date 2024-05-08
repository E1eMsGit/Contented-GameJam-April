using UnityEngine;

public class PaintMissile : MonoBehaviour
{
	#region Fields
	float speed;
	float maxSpeed = 20;
	Prop target;
	float timer;
	float lifeTime = 5;
	Rigidbody rgbd;
	#endregion

	#region Properties
	public Prop Target { get => target; }
	#endregion

	#region Methods
	public void Initilize(Vector3 startDirection, float startSpeed, Prop target)
	{
		transform.forward = startDirection;
		speed = startSpeed;
		this.target = target;

		rgbd = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (lifeTime > 0)
		{
			lifeTime -= Time.fixedDeltaTime;
		}
		else
		{
			target.TogglePaint();
			Destroy(gameObject);
		}
		if (target != null)
		{
			Vector3 _direction = target.transform.position - transform.position;
			_direction.Normalize();
			Vector3 _rotateAmount = Vector3.Cross(_direction, transform.forward);
			rgbd.angularVelocity = _rotateAmount * -1 * 200 * Time.fixedDeltaTime;
			rgbd.velocity = transform.forward * speed;
			if (speed < maxSpeed)
			{
				speed += 3 * Time.fixedDeltaTime;
			}

		}
		else
		{
			Destroy(gameObject);
		}
	}

	#endregion
}
