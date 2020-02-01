﻿using System.Collections;
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

    private float _wheelDurability;
    private float _fuelAmount;
    private float _engineHeat;
    [SerializeField] 
    private int _initialValue = 100;
    private int _initialValueEngine = 0;
    [SerializeField] 
    private float _decayRate = 5;

    [SerializeField] private float _engineLowHeatingRate = 7;
    [SerializeField] private float _engineMedHeatingRate = 11;
    [SerializeField] private float _engineHighHeatingRate = 15;

    [SerializeField] private float _engineMedThreshold = 50;
    [SerializeField] private float _engineHighThreshold = 75;

    [SerializeField] private float __enginePitCoolingRate = 3;
    [SerializeField] private float __engineSlowPitCoolingRate = 2;

    private float _speed;
    [SerializeField] 
    private float _baseSpeed = 15;
    [SerializeField] 
    private float _topSpeed = 300;

    public float _raceDistance = 900f;
    private float _distanceTraveled;

    [SerializeField] 
    private float _fuelWeight = 1;
    [SerializeField] 
    private float _wheelsWeight = 4;

    public float _wheelsFix;
    public float _engineFix;

    private float _fuelIncrement;
    private float _wheelsIncrement;

    [SerializeField] 
    private float _decreasingTime = 100;

    private bool _pitStop = true;
    private bool _wantsStop = false;

    private bool _gameStarted = false;

    [SerializeField]
    private int _totalLaps = 30;
    private int _lapsElapsed = 0;

    private bool _gameRunning = true;

    private float _timeElapsed = 0;
    [SerializeField]
    private float _timeDilation = 1;

    [SerializeField] private Car _car;
    [SerializeField] private NewMouseDrag _mouseDrag;

    public bool EngineCanCoolOff()
    {
        return _engineHeat > _engineMedThreshold;
    }

    public bool HasGameStarted()
    {
        return _gameStarted;
    }

    public bool IsGameRunning()
    {
        return _gameRunning;
    }

    public void Stop(bool State)
    {
        _gameRunning = State;
    }

    public void WheelRepair()
    {
        if (!_pitStop)
        {
            return;
        }
        _wheelDurability += _wheelsFix;
        if (_wheelDurability >= _initialValue)
            _wheelDurability = _initialValue;

        UIManager.UIManagerInstance.UpdateWheels(_wheelDurability);
    }

    public void FuelRefil(float _amount)
    {
        if (!_pitStop)
        {
            return;
        }
        _fuelAmount += _amount;
        if (_fuelAmount >= _initialValue)
            _fuelAmount = _initialValue;

        UIManager.UIManagerInstance.UpdateFuel(_fuelAmount);
    }

    public void EngineRepair(float _amount)
    {
        if (!_pitStop)
        {
            return;
        }
        _engineHeat -= _amount;
        if (_engineHeat <= _initialValueEngine)
            _engineHeat = _initialValueEngine;

        UIManager.UIManagerInstance.UpdateEngine(_engineHeat);
    }

    public void PitStopCall(bool state)
    {
        _wantsStop = state;
        if (_pitStop)
        {
            _pitStop = false;
            _wantsStop = false;
            _mouseDrag.Reset(); 
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

        _wheelDurability = _initialValue;
        _fuelAmount = _initialValue;
        _engineHeat = _initialValueEngine;

        increment = (_topSpeed - _baseSpeed) / (_fuelWeight + _wheelsWeight);
        _wheelsIncrement = increment * _wheelsWeight;
        _fuelIncrement = increment * _fuelWeight;

        _speed = _topSpeed;

        UIManager.UIManagerInstance.InitializeUI(_initialValue,_initialValue,_initialValue);

        UIManager.UIManagerInstance.UpdateLaps(_lapsElapsed + 1, _totalLaps);
    }

    void Update()
    {
        if (!IsGameRunning())
        {
            return;
        }

        Car.inst.fireStrength = ((_engineHeat * 2) - 100) / 100;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PitStopCall(!_pitStop);
            _gameStarted = true;
        }

        if (!_gameStarted)
        {
            return;
        }

        _timeElapsed += _timeDilation * Time.deltaTime;
        UIManager.UIManagerInstance.UpdateTimer(_timeElapsed);

        if (_pitStop)
        {
            if (_engineHeat >= _engineHighThreshold)
            {
                return;
            }
            else if(_engineHeat >=_engineMedThreshold)
            {
                EngineRepair(__engineSlowPitCoolingRate);
            }
            else
            {
                EngineRepair(__engineSlowPitCoolingRate);
            }

            return;
        }

        if (_fuelAmount <= 0 || _engineHeat >= _initialValue)
        {
            _speed -= _decreasingTime * Time.deltaTime;
            _speed = Mathf.Clamp(_speed - _decreasingTime * Time.deltaTime, _baseSpeed, _topSpeed);

            _distanceTraveled += _speed * Time.deltaTime;

            if (_distanceTraveled > _raceDistance)
            {
                _distanceTraveled -= _raceDistance;
                if (_wantsStop)
                {
                    _pitStop = true;
                    _wheelsFix = (_initialValue - _wheelDurability) / 4;
                    _engineFix = -8;
                    _wantsStop = false;
                    _car.EnterPit();
                }
            }

            UIManager.UIManagerInstance.FindPoint(_distanceTraveled / _raceDistance);

            return;
        }
        if (_wheelDurability <= 0)
        {
            _wheelDurability = 0;
        }
        else
        {
            _wheelDurability -= _decayRate * Time.deltaTime;

            UIManager.UIManagerInstance.UpdateWheels(_wheelDurability);
        }
       
        if (_fuelAmount <= 0)
        {
            _fuelAmount = 0;
        }
        else
        {
            _fuelAmount -= _decayRate * Time.deltaTime;

            UIManager.UIManagerInstance.UpdateFuel(_fuelAmount);
        }

        if (_engineHeat >= _initialValue)
        {
            _engineHeat = _initialValue;
        }
        else
        {
            if (_engineHeat >= _engineHighThreshold)
            {
                _engineHeat += _engineHighHeatingRate * Time.deltaTime;
            }
            else if (_engineHeat >= _engineMedThreshold)
            {
                _engineHeat += _engineMedHeatingRate * Time.deltaTime;
            }
            else
            {
                _engineHeat += _engineLowHeatingRate * Time.deltaTime;
            }
            UIManager.UIManagerInstance.UpdateEngine(_engineHeat);
        }

        _speed = _baseSpeed + ((1 / (_fuelAmount + 5)) * _fuelIncrement) + ((_wheelDurability / _initialValue) * _wheelsIncrement);
        
        _distanceTraveled += _speed * Time.deltaTime;
        if (_distanceTraveled >= _raceDistance)
        {
            _lapsElapsed++;
            if (_lapsElapsed == _totalLaps)
            {
                _gameRunning = false;
                return;
            }

            UIManager.UIManagerInstance.UpdateLaps(_lapsElapsed+1,_totalLaps);
            _distanceTraveled -= _raceDistance;
            if (_wantsStop)
            { 
                _pitStop = true;
                _wheelsFix = (_initialValue - _wheelDurability) / 4;
                _engineFix = -8;
                _wantsStop = false;
                _car.EnterPit();
            }
        }

        UIManager.UIManagerInstance.FindPoint(_distanceTraveled / _raceDistance);
    }
   
}
