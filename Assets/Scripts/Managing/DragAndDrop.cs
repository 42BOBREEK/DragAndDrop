using System.Collections;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private InputReader _input;

    [Header("Basket")]
    [SerializeField] private BoxCollider2D _basketArea;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _gravityScale = 1f;
    [SerializeField] private bool _canDrag = true;
    [SerializeField] private Rigidbody2D _activeRb;

    private Camera _mainCamera;
    private WaitForFixedUpdate _waitForFixedUpdate = new();

    private float _fixedY;
    private float _minX;
    private float _maxX;

    public bool HasActiveObject => _activeRb != null;
    public bool CanDrag => _canDrag;

    private void Awake()
    {
        _mainCamera = Camera.main;
        CacheBasketBounds();
    }

    private void OnEnable()
    {
        _input.Pressed += OnPressPerformed;
    }

    private void OnDisable()
    {
        _input.Pressed -= OnPressPerformed;
    }

    private void CacheBasketBounds()
    {
        if (_basketArea == null) return;

        Bounds bounds = _basketArea.bounds;
        _minX = bounds.min.x;
        _maxX = bounds.max.x;
    }

    private void OnPressPerformed()
    {
        if (_input.IsPointerOverUI())
            return;

        if(_canDrag == false)
            return;

        Vector2 screenPos = _input.PointerScreenPosition;

        Vector2 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);

        if (!_basketArea.OverlapPoint(worldPos))
            return;

        StartCoroutine(MoveUpdate());
    }

    private IEnumerator MoveUpdate()
    {
        if (_activeRb == null || !_canDrag)
            yield break;

        _fixedY = _activeRb.position.y;

        DragableObject dragableObject = _activeRb.GetComponent<DragableObject>();

        dragableObject.OnStartDrag();

        while (_input.IsPressed)
        {
            Vector2 screenPos = _input.PointerScreenPosition;

            Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(screenPos);

            float clampedX = Mathf.Clamp(mouseWorldPos.x, _minX, _maxX);
            Vector2 targetPosition = new Vector2(clampedX, _fixedY);

            _activeRb.linearVelocity = (targetPosition - _activeRb.position) * _moveSpeed;

            yield return _waitForFixedUpdate;
        }

        _activeRb.gravityScale = _gravityScale;
        _activeRb.linearVelocity = Vector2.zero;

        dragableObject.OnEndDrag();
        dragableObject.gameObject.GetComponent<RotatingObject>().StopRotation();
        SetCanDrag(false);
    }

    public void SetCanDrag(bool canDrag) => _canDrag = canDrag;

    public void SetActiveObject(Rigidbody2D rb) => _activeRb = rb;

    public void SetActiveObjectNull() => _activeRb = null;

    public void DeleteActiveObject() => Destroy(_activeRb.gameObject);
}
