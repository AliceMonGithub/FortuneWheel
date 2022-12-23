using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour
{
    [SerializeField] private Vector2Int _turnsRange;
    [SerializeField] private float _rotatingTime;
    [SerializeField] private float _counterSmooth;

    [Space]

    [SerializeField] private Transform _wheelTransform;
    [SerializeField] private Button _rotateButton;
    [SerializeField] private TMP_Text _counter;

    private int _counterValue;
    private bool _rotating;

    private void Awake()
    {
        _rotateButton.onClick.AddListener(TryRotate);
    }

    private void TryRotate()
    {
        if (_rotating == false)
        {
            RotateWheel();
        }
    }

    private void RotateWheel()
    {
        _rotating = true;

        int turnsCount = UnityEngine.Random.Range(_turnsRange.x, _turnsRange.y);

        float rawAngle = 45 * UnityEngine.Random.Range(1, 9);
        float result = rawAngle;

        PrintWinByAngle(result);

        rawAngle -= UnityEngine.Random.Range(0.1f, 44.9f) + _wheelTransform.eulerAngles.z; // offset;

        float rotatingAngle = rawAngle + 360 * turnsCount;

        _wheelTransform.DORotate(Vector3.forward * rotatingAngle, _rotatingTime, RotateMode.WorldAxisAdd).onComplete += () =>
        {
            _rotating = false;

            AddResult(GetWinValue(result));
        };
    }

    private void AddResult(int value)
    {
        _counterValue += value;

        StartCoroutine(SmoothCounter(_counterValue, _counterValue - value));
    }

    private IEnumerator SmoothCounter(int endValue, int startValue)
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime / _counterSmooth;

            _counter.text = Convert.ToInt32(Mathf.Lerp(startValue, endValue, time)).ToString();

            yield return new WaitForEndOfFrame();
        }

        _counter.text = endValue.ToString();
    }

    private void PrintWinByAngle(float angle)
    {
        print(GetWinValue(angle));
    }

    private int GetWinValue(float angle)
    {
        switch (angle)
        {
            case 45:
                return 1000;

            case 90:
                return 400;

            case 135:
                return 800;

            case 180:
                return 7000;

            case 225:
                return 5000;

            case 270:
                return 300;

            case 315:
                return 2000;

            case 360:
                return 100;
            default:
                throw new Exception("Value not found");
        }
    }
}
