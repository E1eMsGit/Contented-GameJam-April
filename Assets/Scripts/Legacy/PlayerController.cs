using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RoadController roadController;
    public float laneOffset = 2f;
    private int currentLane = 1;
    private float speed = 5f;
    public GameObject[] boost;

    private void Start()
    {
        roadController = FindObjectOfType<RoadController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print(speed);
        if (other.gameObject.tag == "Obstacle")
        {
            roadController.ReduceSpeed();
            Destroy(other.gameObject);
        }
        if(other.gameObject.tag == "Boost")
        {
            roadController.ApplySpeed(5);
            Destroy(other.gameObject);
        }
    }

    void Move(int direction)
    {
        int newLane = currentLane + direction;
        if (newLane >= 0 && newLane <= 2)
        {
            transform.position += new Vector3(direction * laneOffset, 0, 0);
            currentLane = newLane;
        }
    }
}


