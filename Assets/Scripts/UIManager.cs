using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum ETextState
{
    Running,Parking,Stopped,Empty
};

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _wheelsSlider;
    [SerializeField] private Slider _fuelSlider;
    [SerializeField] private Slider _engineSlider;

    [SerializeField] private Text _lapsText;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _informationText;
    [SerializeField] private Text _startTimer;

    [SerializeField] private Image _carDisplay;

    [SerializeField] private RectTransform[] _lapPath;

    private float _pathLength;

    private static UIManager _uiManagerInst;

    public static UIManager UIManagerInstance
    {
        get { return _uiManagerInst; }
    }

    public void UpdateInformationText(ETextState state)
    {
        switch (state)
        {
            case ETextState.Parking:
                _informationText.text = "The Car is Coming";
                break;
            case ETextState.Running:
                _informationText.text = "Press \"Space\" to Bring in the Car";
                break;
            case ETextState.Stopped:
                _informationText.text = "Press \"Space\" to Send out the Car";
                break;
            case ETextState.Empty:
                _informationText.text = "";
                break;
        }
    }

    public void FindPoint(float completionPercent)
    {
        if (_lapPath.Length < 1)
        {
            _carDisplay.rectTransform.localPosition = Vector3.zero;
            return;
        }
        else if (_lapPath.Length < 2)
        {
            _carDisplay.rectTransform.localPosition = _lapPath[0].localPosition;
            return;
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

             _carDisplay.rectTransform.localPosition = Vector3.Lerp(currentPoint, nextPoint, dist / currentDistance);
            return;
        }
        _carDisplay.rectTransform.localPosition = _lapPath[_lapPath.Length - 1].localPosition;
        return;
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
        UpdateInformationText(ETextState.Empty);
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

	Dictionary<Slider, Image> _imageBySlider = new Dictionary<Slider, Image>();

	void SetSliderColor(Slider slider, float value) {
		Image image;
		if (_imageBySlider.ContainsKey(slider)) {
			image = _imageBySlider[slider];
		} else {
			Transform area = slider.transform.Find("Fill Area");
			Transform fill = area.Find("Fill");
			image = fill.GetComponent<Image>();
			_imageBySlider.Add(slider, image);
		}

		float t = 1f - (1f - value) * (1f - value);
		Color color = Color.Lerp(Color.red, Color.white, t);
		image.color = color;
	}

	public void UpdateWheels(float value)
    {
        _wheelsSlider.value = value;
		SetSliderColor(_wheelsSlider, value / 100f);
	}

    public void UpdateFuel(float value)
    {
        _fuelSlider.value = value;
		SetSliderColor(_fuelSlider, value / 100f);
	}

    public void UpdateEngine(float value)
    {
        _engineSlider.value = value;
		SetSliderColor(_engineSlider, 1f - value / 100f);
	}

    public void UpdateTimer(float elapsedTime)
    {
        string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
        string seconds = Mathf.Floor(elapsedTime % 60).ToString("00");

        _timerText.text = minutes + ":" + seconds;
        PersistScript.PersistScriptInstance.SetTime(_timerText.text);
    }


    public void UpdateLaps(int currentLap,int lapTotal)
    {
        _lapsText.text = "Lap/n" + currentLap + "/" + lapTotal;
    }

    public void UpdateStartTimer(string text)
    {
        _startTimer.text = text;
    }
}
