using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    [Header("Input (assign InputActionReferences)")]
    [SerializeField] private InputActionReference pressAction;    // Button: <Pointer>/press  (or mouse/touch bindings)
    [SerializeField] private InputActionReference positionAction; // Vector2: <Pointer>/position

    [Header("Basket")]
    [SerializeField] private BoxCollider2D _basketArea;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _gravityScale = 1f;

    private Camera _mainCamera;
    private WaitForFixedUpdate _waitForFixedUpdate = new();

    [SerializeField] private Rigidbody2D _activeRb;

    private float _fixedY;
    private float _minX;
    private float _maxX;
    [SerializeField] private bool _canDrag = true;

    public bool HasActiveObject => _activeRb != null;

    private void Awake()
    {
        _mainCamera = Camera.main;
        CacheBasketBounds();
    }

    private void CacheBasketBounds()
    {
        if (_basketArea == null) return;
        Bounds bounds = _basketArea.bounds;
        _minX = bounds.min.x;
        _maxX = bounds.max.x;
    }

    private void OnEnable()
    {
        if (pressAction != null) pressAction.action.Enable();
        if (positionAction != null) positionAction.action.Enable();

        if (pressAction != null)
            pressAction.action.performed += OnPressPerformed;
    }

    private void OnDisable()
    {
        if (pressAction != null)
            pressAction.action.performed -= OnPressPerformed;

        if (pressAction != null) pressAction.action.Disable();
        if (positionAction != null) positionAction.action.Disable();
    }

    private void OnPressPerformed(InputAction.CallbackContext context)
    {
        if (positionAction == null || _basketArea == null || _mainCamera == null || _canDrag == false)
            return;

        Vector2 screenPos = positionAction.action.ReadValue<Vector2>();

        if (IsPointerOverUI(screenPos))
            return;

        Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);

        if (!_basketArea.OverlapPoint(worldPos))
            return;

        StartCoroutine(MoveUpdate());
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    private IEnumerator MoveUpdate()
    {
        if (_activeRb == null || !_canDrag)
            yield break;

        _fixedY = _activeRb.position.y;
        _activeRb.gravityScale = 0f;

        while (pressAction != null && pressAction.action.ReadValue<float>() != 0)
        {
            Vector2 screenPos = positionAction != null
                ? positionAction.action.ReadValue<Vector2>()
                : (Vector2)Input.mousePosition; // fallback, если вдруг

            Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(screenPos);

            float clampedX = Mathf.Clamp(mouseWorldPos.x, _minX, _maxX);
            Vector2 targetPosition = new Vector2(clampedX, _fixedY);

            _activeRb.linearVelocity = (targetPosition - _activeRb.position) * _moveSpeed;

            yield return _waitForFixedUpdate;
        }

        _activeRb.gravityScale = _gravityScale;
        _activeRb.linearVelocity = Vector2.zero;

        SetCanDrag(false);
    }

    public void SetCanDrag(bool canDrag) => _canDrag = canDrag;

    public void SetActiveObject(Rigidbody2D rb) => _activeRb = rb;

    public void SetActiveObjectNull() => _activeRb = null;

    public void DeleteActiveObject() => Destroy(_activeRb.gameObject);
}
