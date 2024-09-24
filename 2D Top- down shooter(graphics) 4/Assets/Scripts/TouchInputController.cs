using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[DefaultExecutionOrder(-1)]
public class TouchInputController : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsDashing { get; private set; }

    [SerializeField] private float touchMoveSensitivity = 50f;
    [SerializeField] private float touchSwipeThreshold = 50f;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private bool isTouching = false;
    private Vector2 touchStartPosition;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        touchPressAction.performed += ctx => OnTouchPressed(ctx);
        touchPressAction.canceled += ctx => OnTouchReleased(ctx);
    }

    private void OnDisable()
    {
        touchPressAction.performed -= ctx => OnTouchPressed(ctx);
        touchPressAction.canceled -= ctx => OnTouchReleased(ctx);
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        if (isTouching)
        {
            Vector2 touchPosition = touchPositionAction.ReadValue<Vector2>();
            UpdateMoveInput(touchPosition);
        }
    }

    private void OnTouchPressed(InputAction.CallbackContext context)
    {
        isTouching = true;
        touchStartPosition = touchPositionAction.ReadValue<Vector2>();
    }

    private void OnTouchReleased(InputAction.CallbackContext context)
    {
        isTouching = false;
        MoveInput = Vector2.zero;

        Vector2 touchEndPosition = touchPositionAction.ReadValue<Vector2>();
        Vector2 swipeDelta = touchEndPosition - touchStartPosition;

        if (swipeDelta.magnitude > touchSwipeThreshold)
        {
            IsDashing = true;
        }
    }

    private void UpdateMoveInput(Vector2 touchPosition)
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        MoveInput = (touchPosition - screenCenter).normalized / touchMoveSensitivity;
        
        if (MoveInput.magnitude > 1)
        {
            MoveInput.Normalize();
        }
    }

    public void ResetDashFlag()
    {
        IsDashing = false;
    }
}