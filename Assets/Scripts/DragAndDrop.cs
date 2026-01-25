using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private InputAction mouseClick;
    [SerializeField] private float mouseDragPhysicsSpeed;
    [SerializeField] private float mouseDragSpeed;

    private Camera mainCamera;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider != null && hit.collider.gameObject.GetComponent<IDragable>() != null)
            {
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }

        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if(hit2D.collider != null && hit2D.collider.gameObject.GetComponent<IDragable>() != null)
        {
            StartCoroutine(DragUpdate(hit2D.collider.gameObject));
        }
    }
    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        clickedObject.TryGetComponent<Rigidbody>(out var rb);
        clickedObject.TryGetComponent<IDragable>(out var IDragableComponent);

        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);

        IDragableComponent?.OnStartDrag();

        while(mouseClick.ReadValue<float>() != 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if(rb != null)
            {
                Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rb.linearVelocity = direction * mouseDragPhysicsSpeed;

                yield return waitForFixedUpdate;
            }
            else
            {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
                yield return null;
            }
        }

        IDragableComponent?.OnEndDrag();
    }
}
