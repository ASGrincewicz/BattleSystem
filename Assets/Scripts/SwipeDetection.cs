using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float _minimumDistance;
    [SerializeField] private float _maximumTime;
    private InputManager _inputManager;
    private Vector2 _startPosition, _endPosition;
    private float _startTime, _endTime;

    private void Awake() => _inputManager = InputManager.Instance;

    private void OnEnable()
    {
        _inputManager.OnStartTouch += SwipeStart;
        _inputManager.OnEndTouch += SwipeEnd;
    }
    private void OnDisable()
    {
        _inputManager.OnStartTouch -= SwipeStart;
        _inputManager.OnEndTouch -= SwipeEnd;
    }
    private void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
    }
    private void SwipeEnd(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;
        DetectSwipe();
    }
    private void DetectSwipe()
    {
        if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistance && (_endTime - _startTime) <= _maximumTime)
        {
            Debug.DrawLine(_startPosition, _endPosition, Color.red, 5.0f);
            Debug.Log("SwipeDetected");
        }
    }
}
