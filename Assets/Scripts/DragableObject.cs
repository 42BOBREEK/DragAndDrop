using UnityEngine;

public class DragableObject : MonoBehaviour, IDragable
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnStartDrag()
    {
        rb.gravityScale = 0f;
    }

    public void OnEndDrag()
    {
        rb.gravityScale = 1f;
    }
}
