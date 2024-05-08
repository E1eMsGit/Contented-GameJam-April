using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : Item
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			speedChange.Invoke(PublicStuff.speedUpAmount);
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "AnihilatorPlane")
		{
			Destroy(gameObject);
		}
	}
}
