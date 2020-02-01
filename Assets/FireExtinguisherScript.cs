﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FireExtinguisherScript : MonoBehaviour
{
    private bool _isFixing = false;
    [SerializeField]
    private float _extinguishRate = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CarEngineArea" && Simulation.SimulationInst.EngineCanCoolOff())
        {
            _isFixing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CarEngineArea")
        {
            _isFixing = false;
        }
    }

    private void Update()
    {
        if (_isFixing)
        {
            Simulation.SimulationInst.EngineRepair(_extinguishRate*Time.deltaTime);
        }
    }
}
