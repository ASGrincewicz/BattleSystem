using UnityEngine;

public struct DieRoll
{
    private int _firstDieRoll;
    private int _secondDieRoll;
    private float _result;
    private float _modifiedResult;
    private float _finalResult;

    public float GetResult() => _finalResult;

    public bool Roll()
    {
        _firstDieRoll = Random.Range(1, 6);
        _secondDieRoll = Random.Range(1, 6);
        _result = (_firstDieRoll + _secondDieRoll);
        _finalResult = _result;
        if (_finalResult > 3)
            return true;
        else
            return false;
    }

    public bool Roll(float moveAccuracy)
    {
        _firstDieRoll = Random.Range(1, 6);
        _secondDieRoll = Random.Range(1, 6);
        _result = (_firstDieRoll + _secondDieRoll) * moveAccuracy;
        _finalResult = _result / 100f;

        if (_finalResult > 3)
            return true;
        else
            return false;
    }

    public bool Roll(float moveAccuracy, int accuracyModifier)
    {
        _firstDieRoll = Random.Range(1, 6);
        _secondDieRoll = Random.Range(1, 6);
        _result = (_firstDieRoll + _secondDieRoll) * moveAccuracy;
        _modifiedResult = Mathf.Round(_result + accuracyModifier);
        _finalResult = _modifiedResult / 100f;

        if (_finalResult > 3)
            return true;
        else
            return false;
    }


}