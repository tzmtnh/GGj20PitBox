using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _wheelsSlider;
    [SerializeField] private Slider _fuelSlider;
    [SerializeField] private Slider _engineSlider;

    [SerializeField] private Text _lapsText;

    private static UIManager _uiManagerInst;

    public static UIManager UIManagerInstance
    {
        get { return _uiManagerInst; }
    }
    
    private void Awake()
    {
        if (_uiManagerInst != null && _uiManagerInst != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _uiManagerInst = this;
            DontDestroyOnLoad(this);
        }
    }

    public void InitializeUI(float wheelsMaxValue, float fuelMaxValue, float engineMaxValue)
    {
        _wheelsSlider.maxValue = wheelsMaxValue;
        _fuelSlider.maxValue = fuelMaxValue;
        _engineSlider.maxValue = engineMaxValue;

        _wheelsSlider.value = _wheelsSlider.maxValue;
        _fuelSlider.value = _fuelSlider.maxValue;
        _engineSlider.value = 0;
    }



    public void UpdateWheels(float value)
    {
        _wheelsSlider.value = value;
    }

    public void UpdateFuel(float value)
    {
        _fuelSlider.value = value;
    }

    public void UpdateEngine(float value)
    {
        _engineSlider.value = value;
    }




    public void UpdateLaps(int currentLap,int lapTotal)
    {
        _lapsText.text = currentLap + "/" + lapTotal;
    }
}
