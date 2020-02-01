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

    [SerializeField] private Image _carDisplay;

    [SerializeField] private RectTransform[] _lapPath;

    private float _pathLength;

    private static UIManager _uiManagerInst;

    public static UIManager UIManagerInstance
    {
        get { return _uiManagerInst; }
    }

    private Vector3 FindPoint(float completionPercent)
    {
        if (_lapPath.Length < 1)
        {
            return Vector3.zero;
        }
        else if (_lapPath.Length < 2)
        {
            return _lapPath[0].localPosition;
        }


        float dist = _pathLength * Mathf.Clamp(completionPercent, 0, 1);

        for (int i = 0; i < _lapPath.Length - 1; i++)
        {
            Vector3 currentPoint = _lapPath[i].localPosition;
            Vector3 nextPoint = _lapPath[i + 1].localPosition;
            float currentDistance = (nextPoint - currentPoint).magnitude;

            if (currentDistance < dist)
            {
                dist -= currentDistance;
                continue;
            }

            return Vector3.Lerp(currentPoint, nextPoint, dist / currentDistance);
        }
        return _lapPath[_lapPath.Length - 1].localPosition;
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

    private void Start()
    {
        RectTransform lastPoint = null;

        foreach (RectTransform point in _lapPath)
        {
            if (lastPoint == null)
            {
                lastPoint = point;
                continue;
            }

            _pathLength += Vector3.Distance(lastPoint.localPosition, point.localPosition);
            lastPoint = point;
        }

    }

    private float test;

    private void Update()
    {
        test += Time.deltaTime;
        _carDisplay.rectTransform.localPosition = FindPoint(test);
        if (test > 1)
        {
            test = test - 1;
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
