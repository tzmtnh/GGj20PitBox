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
    private int InitialValueEngine = 0;
    [SerializeField] 
    private float DecayRate = 5;

    private float Speed;
    [SerializeField] 
    private float BaseSpeed = 15;
    [SerializeField] 
    private float TopSpeed = 300;

    public float RaceDistance = 900f;
    private float DistanceTraveled;

    [SerializeField] 
    private float FuelWeight = 1;
    [SerializeField] 
    private float WheelsWeight = 4;

    public float WheelsFix;
    public float EngineFix;

    private float FuelIncrement;
    private float WheelsIncrement;

    [SerializeField] 
    private float DecreasingTime = 100;

    [SerializeField]
    private bool PitStop = true;
    [SerializeField]
    private bool WantsStop = false;

    private bool _gameStarted = false;

    [SerializeField]
    private int _totalLaps = 30;
    private int _lapsElapsed = 0;

    private bool _gameRunning = true;

    private float _timeElapsed = 0;
    [SerializeField]
    private float _timeDilation = 1;

    [SerializeField] private Car _car;

    public bool IsGameRunning()
    {
        return _gameRunning;
    }

    public void WheelRepair()
    {
        if (!PitStop)
        {
            return;
        }
        WheelDurability += WheelsFix;
        if (WheelDurability >= InitialValue)
            WheelDurability = InitialValue;

        UIManager.UIManagerInstance.UpdateWheels(WheelDurability);
    }

    public void FuelRefil(float _amount)
    {
        if (!PitStop)
        {
            return;
        }
        FuelAmount += _amount;
        if (FuelAmount >= InitialValue)
            FuelAmount = InitialValue;

        UIManager.UIManagerInstance.UpdateFuel(FuelAmount);
    }

    public void EngineRepair()
    {
        if (!PitStop)
        {
            return;
        }
        EngineHeat += EngineFix;
        if (EngineHeat <= InitialValueEngine)
            EngineHeat = InitialValueEngine;

        UIManager.UIManagerInstance.UpdateEngine(EngineHeat);
    }

    public void PitStopCall(bool state)
    {
        WantsStop = state;
        if (PitStop)
        {
            PitStop = false;
            WantsStop = false;
            _car.ExitPit();
        }

    }

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
        EngineHeat = InitialValueEngine;

        increment = (TopSpeed - BaseSpeed) / (FuelWeight + WheelsWeight);
        WheelsIncrement = increment * WheelsWeight;
        FuelIncrement = increment * FuelWeight;

        Speed = TopSpeed;

        UIManager.UIManagerInstance.InitializeUI(InitialValue,InitialValue,InitialValue);

        UIManager.UIManagerInstance.UpdateLaps(_lapsElapsed + 1, _totalLaps);
    }

    void Update()
    {
        if (!IsGameRunning())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PitStopCall(!PitStop);
            _gameStarted = true;
        }

        if (!_gameStarted)
        {
            return;
        }

        _timeElapsed += _timeDilation * Time.deltaTime;
        UIManager.UIManagerInstance.UpdateTimer(_timeElapsed);

    if (PitStop) {

            return;
                 }

        if (FuelAmount <= 0 || EngineHeat >= InitialValue)
        {
            Speed -= DecreasingTime * Time.deltaTime;
            Speed = Mathf.Clamp(Speed - DecreasingTime * Time.deltaTime, BaseSpeed, TopSpeed);

            DistanceTraveled += Speed * Time.deltaTime;

            if (DistanceTraveled > RaceDistance)
            {
                DistanceTraveled -= RaceDistance;
                if (WantsStop)
                {
                    PitStop = true;
                    WheelsFix = (InitialValue - WheelDurability) / 4;
                    EngineFix = -8;
                    WantsStop = false;
                    _car.EnterPit();
                }
            }

            UIManager.UIManagerInstance.FindPoint(DistanceTraveled / RaceDistance);

            return;
        }
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

        if (EngineHeat >= InitialValue)
        {
            EngineHeat = InitialValue;
        }
        else
        {
            EngineHeat += DecayRate * Time.deltaTime;

            UIManager.UIManagerInstance.UpdateEngine(EngineHeat);
        }

        Speed = BaseSpeed + ((1 / (FuelAmount + 5)) * FuelIncrement) + ((WheelDurability / InitialValue) * WheelsIncrement);
        
        DistanceTraveled += Speed * Time.deltaTime;
        if (DistanceTraveled >= RaceDistance)
        {
            _lapsElapsed++;
            if (_lapsElapsed == _totalLaps)
            {
                _gameRunning = false;
                return;
            }

            UIManager.UIManagerInstance.UpdateLaps(_lapsElapsed+1,_totalLaps);
            DistanceTraveled -= RaceDistance;
            if (WantsStop)
            { 
                PitStop = true;
                WheelsFix = (InitialValue - WheelDurability) / 4;
                EngineFix = -8;
                WantsStop = false;
                _car.EnterPit();
            }
        }

        UIManager.UIManagerInstance.FindPoint(DistanceTraveled / RaceDistance);
    }
   
}
