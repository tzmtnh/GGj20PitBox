﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private int _initialValue = 100;
    private int _initialValueEngine = 0;
    [SerializeField]  private float _wheelsDecayRate = 2;
	[SerializeField] private float _fuelDecayRate = 3;

    [SerializeField] private float _engineLowHeatingRate = 7;
    [SerializeField] private float _engineMedHeatingRate = 11;
    [SerializeField] private float _engineHighHeatingRate = 15;

    [SerializeField] private float _engineMedThreshold = 50;
    [SerializeField] private float _engineHighThreshold = 75;

    [SerializeField] private float _enginePitCoolingRate = 3;
    [SerializeField] private float _engineSlowPitCoolingRate = 2;

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

    public float _wheelsFix = 25;

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

    [SerializeField] private NewMouseDrag _mouseDrag;

    [SerializeField] private float _stopDistance=700;

    [SerializeField]
    private int _startTime = 5;
    private int _timerTime;

    private bool _isStopping = false;

    private bool _canRepairEngine = false;

    public bool EngineCanCoolOff()
    {
        return _canRepairEngine;
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

	int _numRemovedWheels;
	public void WheelRemoved() {
		if (!_pitStop) return;

		_numRemovedWheels++;
		if (_numRemovedWheels > 4) {
			_numRemovedWheels = 4;
		}

		_wheelDurability = Mathf.Min(_wheelDurability, _initialValue * (4f - _numRemovedWheels) / 4f);

		UIManager.UIManagerInstance.UpdateWheels(_wheelDurability);
	}

    public void WheelRepair()
    {
        if (!_pitStop)
        {
            return;
        }

		_numRemovedWheels--;
		if (_numRemovedWheels < 0)
			_numRemovedWheels = 0;

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
            UIManager.UIManagerInstance.UpdateInformationText(ETextState.Running);
            _pitStop = false;
            _wantsStop = false;
            _mouseDrag.Reset(); 
            Car.inst.ExitPit();
            return;
        }
        UIManager.UIManagerInstance.UpdateInformationText(ETextState.Parking);
    }

    void Awake()
    {
        if (SimulationInstance != null && SimulationInstance != this)
            Destroy(this.gameObject);
        else
            SimulationInstance = this;
    }

    void Start() {

        float increment;

        _timerTime = _startTime;

        _wheelDurability = _initialValue;
        _fuelAmount = _initialValue;
        _engineHeat = _initialValueEngine;

        increment = (_topSpeed - _baseSpeed) / (_fuelWeight + _wheelsWeight);
        _wheelsIncrement = increment * _wheelsWeight;
        _fuelIncrement = increment * _fuelWeight;

        _speed = _topSpeed;

        UIManager.UIManagerInstance.InitializeUI(_initialValue,_initialValue,_initialValue);

        UIManager.UIManagerInstance.UpdateLaps(_lapsElapsed + 1, _totalLaps);
        StartCoroutine(StartEnum());
    }

    
    private IEnumerator StartEnum()
    {
        while (true)
        {
            //Debug.Log("pois");
            _timerTime--;
            UIManager.UIManagerInstance.UpdateStartTimer(_timerTime.ToString());
            if (_timerTime == 0)
            {
                UIManager.UIManagerInstance.UpdateStartTimer("GO!");
                _gameStarted = true;
                PitStopCall(!_pitStop);
				AudioManager.AuidoManagerInstance.PlayBeep(2);
            } else if (_timerTime > 0) {
				AudioManager.AuidoManagerInstance.PlayBeep(1);
			}

			if (_timerTime < 0)
            {
                UIManager.UIManagerInstance.UpdateStartTimer("");
                    StopAllCoroutines();
            }

			yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        if (!IsGameRunning())
        {
            return;
        }
        if (!_gameStarted)
        {
            return;
        }
        Car.inst.fireStrength = ((_engineHeat * 2) - 100) / 100;
        if (Car.inst.fireStrength > 100 && !AudioManager.AuidoManagerInstance.IsPlayingFire())
        {
            AudioManager.AuidoManagerInstance.PlayingEngineFireAudio(true,Mathf.Clamp(Car.inst.fireStrength+0.5f,0,1));
        }
        else if (Car.inst.fireStrength<=100 && AudioManager.AuidoManagerInstance.IsPlayingFire())
        {
            AudioManager.AuidoManagerInstance.PlayingEngineFireAudio(false,1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PitStopCall(!_pitStop);
        }

        _timeElapsed += _timeDilation * Time.deltaTime;
        UIManager.UIManagerInstance.UpdateTimer(_timeElapsed);

        if (_isStopping)
        {
            _distanceTraveled += _speed * Time.deltaTime;
            if (_distanceTraveled >= _raceDistance)
            {
                UIManager.UIManagerInstance.UpdateInformationText(ETextState.Stopped);
                _pitStop = true;
                _isStopping = false;
                _distanceTraveled = 0;
            }
            UIManager.UIManagerInstance.FindPoint(_distanceTraveled / _raceDistance);
            return;
        }

        if (_pitStop)
        {
            if (_engineHeat >= _engineHighThreshold)
            {
                _canRepairEngine = true;
                return;
            }
            else if(_engineHeat >=_engineMedThreshold)
            {
                _canRepairEngine = true;
                EngineRepair(_engineSlowPitCoolingRate*Time.deltaTime);
            }
            else
            {
                _canRepairEngine = false;
                EngineRepair(_enginePitCoolingRate*Time.deltaTime);
            }

            return;
        }

		_numRemovedWheels = 0;

        if (_fuelAmount <= 0 || _engineHeat >= _initialValue)
        {
            _speed -= _decreasingTime * Time.deltaTime;
            _speed = Mathf.Clamp(_speed - _decreasingTime * Time.deltaTime, _baseSpeed, _topSpeed);

            _distanceTraveled += _speed * Time.deltaTime;

            if (_fuelAmount <= 0 && _speed <= _baseSpeed + 10)
            {
                SceneManager.LoadScene(3);
            }

            if (_wantsStop && _distanceTraveled >= _stopDistance)
            {
                float wantedDuration = (_raceDistance - _distanceTraveled) / _speed;
                /*if (wantedDuration > Car.inst.animationDuration || wantedDuration < Car.inst.animationDuration/4)
                {
                    UIManager.UIManagerInstance.FindPoint(_distanceTraveled / _raceDistance);
                    return;
                }*/

                float startParam = 1 - Mathf.Clamp(wantedDuration / Car.inst.animationDuration, 0, 1);
				Car.inst.EnterPit(startParam);
                _isStopping = true;
                //_wheelsFix = (_initialValue - _wheelDurability) / 4;
                _wantsStop = false;
            }

            if (_distanceTraveled > _raceDistance)
            {
                _distanceTraveled -= _raceDistance;
                
            }

            UIManager.UIManagerInstance.FindPoint(_distanceTraveled / _raceDistance);
            if (_wheelDurability <= 0)
            {
                _wheelDurability = 0;
            }
            else
            {
                _wheelDurability -= _wheelsDecayRate * Time.deltaTime;

                UIManager.UIManagerInstance.UpdateWheels(_wheelDurability);
            }

            if (_fuelAmount <= 0)
            {
                _fuelAmount = 0;
            }
            else
            {
                _fuelAmount -= _fuelDecayRate * Time.deltaTime;

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
            return;
        }
        if (_wheelDurability <= 0)
        {
            _wheelDurability = 0;
        }
        else
        {
            _wheelDurability -= _wheelsDecayRate * Time.deltaTime;

            UIManager.UIManagerInstance.UpdateWheels(_wheelDurability);
        }
       
        if (_fuelAmount <= 0)
        {
            _fuelAmount = 0;
        }
        else
        {
            _fuelAmount -= _fuelDecayRate * Time.deltaTime;

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
        if (_wantsStop && _distanceTraveled >= _stopDistance)
        {
            float wantedDuration = (_raceDistance - _distanceTraveled) / _speed;
            /*if (wantedDuration > Car.inst.animationDuration||wantedDuration < Car.inst.animationDuration/10)
            {
                UIManager.UIManagerInstance.FindPoint(_distanceTraveled / _raceDistance);
                return;
            }*/

            float startParam = 1 - Mathf.Clamp(wantedDuration / Car.inst.animationDuration, 0, 1);
			Car.inst.EnterPit(startParam);
            _isStopping = true;
            //_wheelsFix = (_initialValue - _wheelDurability) / 4;
            _wantsStop = false;
        }
        if (_distanceTraveled >= _raceDistance)
        {
            _lapsElapsed++;
            if (_lapsElapsed == _totalLaps)
            {
                _gameRunning = false;
                SceneManager.LoadScene(2);
                return;
            }

            UIManager.UIManagerInstance.UpdateLaps(_lapsElapsed+1,_totalLaps);
            _distanceTraveled -= _raceDistance;
            
        }

        UIManager.UIManagerInstance.FindPoint(_distanceTraveled / _raceDistance);
    }
   
}
