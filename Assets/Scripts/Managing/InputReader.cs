using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class InputReader : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference _pressAction;
    [SerializeField] private InputActionReference _positionAction;
    [SerializeField] private Camera _mainCamera;

    public bool IsPressed => _pressAction != null && _pressAction.action.ReadValue<float>() != 0f;

    public event Action Pressed;

    public Vector2 PointerScreenPosition =>
        _positionAction != null
            ? _positionAction.action.ReadValue<Vector2>()
            : (Vector2)Input.mousePosition;

    private void OnEnable()
    {
        _pressAction?.action.Enable();
        _pressAction.action.performed += OnPressPerformed;
        _positionAction?.action.Enable();
    }

    private void OnDisable()
    {
        _pressAction?.action.Disable();
        _pressAction.action.performed -= OnPressPerformed;
        _positionAction?.action.Disable();
    }

    private void OnPressPerformed(InputAction.CallbackContext context)
    {
        Pressed?.Invoke();
    }

    public bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = PointerScreenPosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    public Fruit GetFruitUnderPointer()
    {
        Vector2 worldPos = _mainCamera.ScreenToWorldPoint(PointerScreenPosition);

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<Fruit>(out Fruit fruit))
                return fruit;

            hit.GetComponentInParent<Fruit>();

            if(hit != null)
                return null;
        }

        return null;
    }

    public bool IsPointerOverChest(out TreasureChest chest)
    {
        chest = null;

        Vector2 worldPos = _mainCamera.ScreenToWorldPoint(PointerScreenPosition);

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        foreach (Collider2D hit in hits)
        {
            TreasureChest foundChest = hit.GetComponentInParent<TreasureChest>();

            if (foundChest != null)
               chest = foundChest; 
        }

        return chest != null;
    }
}
