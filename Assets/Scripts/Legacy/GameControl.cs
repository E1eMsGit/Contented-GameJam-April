using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl Instance;
    public PlayerController player;
    public RoadController road;

    void Awake()
    {
        Instance = this;
    }
    public void PickUpSpeedBoost()
    {
        road.ApplySpeedBoost(20f);
    }
}
