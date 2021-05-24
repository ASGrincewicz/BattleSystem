using UnityEngine;
using UnityEngine.InputSystem;
using Veganimus.BattleSystem;
[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    private Camera mainCamera;
    private Controls _controls;

    protected override void Awake()
    {
        _instance = this;
        _controls = new Controls();
        mainCamera = Camera.main;
    }
    private void OnEnable() => _controls.Enable();

    private void OnDisable() => _controls.Disable();

    private void Start()
    {
        _controls.UI.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        _controls.UI.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

    }
    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
            OnStartTouch(Utilities.ScreenToWorld(mainCamera, _controls.UI.PrimaryPosition.ReadValue<Vector2>()),
                         (float)context.startTime);
    }
    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
            OnEndTouch(Utilities.ScreenToWorld(mainCamera, _controls.UI.PrimaryPosition.ReadValue<Vector2>()),
                         (float)context.time);
    }
}
