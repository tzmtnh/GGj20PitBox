using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    private static Simulation SimulationInstance;

    public static Simulation SimulationInst
    {
        get { return SimulationInstance;
        }
    }

    private float WheelDurability;
    private float FuelAmount;
    private float EngineHeat;
    [SerializeField] 
    private int InitialValue = 100;
    private float DecayRate = 5;

    private float Speed;
    private float BaseSpeed = 15;
    private float TopSpeed = 300;

    private float FuelWeight = 1;
    private float WheelsWeight = 4;

    private float FuelIncrement;
    private float WheelsIncrement;

    void Awake()
    {
        if (SimulationInstance != null && SimulationInstance != this)
            Destroy(this.gameObject);
        else
            SimulationInstance = this;
            DontDestroyOnLoad(this);
    }

    void Start() {

        float increment;

        WheelDurability = InitialValue;
        FuelAmount = InitialValue;
        EngineHeat = InitialValue;

        increment = (TopSpeed - BaseSpeed) / (FuelWeight + WheelsWeight);
        WheelsIncrement = increment * WheelsWeight;
        FuelIncrement = increment * FuelWeight;

    }

    // Update is called once per frame
    void Update()
    {
        if (WheelDurability <= 0)
        {
            WheelDurability = 0;
        }
        else
        {
            WheelDurability -= DecayRate * Time.deltaTime;

            UIManager.UIManagerInstance.UpdateWheels(WheelDurability);
        }

        if (FuelAmount <= 0)
        {
            FuelAmount = 0;
        }
        else
        {
            FuelAmount -= DecayRate * Time.deltaTime;

            UIManager.UIManagerInstance.UpdateFuel(FuelAmount);
        }

        if (EngineHeat <= 0)
        {
            EngineHeat = 0;
        }
        else
        {
            EngineHeat -= DecayRate * Time.deltaTime;

            UIManager.UIManagerInstance.UpdateEngine(EngineHeat);
        }

        Speed = BaseSpeed + ((1 / (FuelAmount + 5)) * FuelIncrement) + ((WheelDurability / InitialValue) * WheelsIncrement);
        Debug.Log(Speed);

    }
   
}
