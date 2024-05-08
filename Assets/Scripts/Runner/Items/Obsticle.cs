using UnityEngine;

public class Obsticle : Item
{
	protected override void Start()
	{
		base.Start();
		transform.GetChild(0).rotation *= Quaternion.Euler(-90f, Random.Range(0f, 360f), 0);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			speedChange.Invoke(PublicStuff.slowDownAmount);
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "AnihilatorPlane")
		{
			Destroy(gameObject);
		}
	}
}
