using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class StretchToCamera2D : MonoBehaviour
{
    public enum StretchMode
    {
        WidthOnly,
        HeightOnly,
        WidthAndHeight
    }

    [Header("Behaviour")]
    public bool executeInUpdate = false;
    public StretchMode stretchMode = StretchMode.WidthOnly;

    [Header("Scale Settings")]
    public Vector2 baseSize = Vector2.one;     // Базовый размер объекта (в world units)
    public Vector2 scaleMultiplier = Vector2.one; // Дополнительный множитель

    private IEnumerator _updateRoutine;
    private Vector2 _lastScreenSize;

    void Start()
    {
        _updateRoutine = UpdateScaleAsync();
        StartCoroutine(_updateRoutine);
    }

    IEnumerator UpdateScaleAsync()
    {
        uint waitFrames = 0;

        while (Camera.main == null)
        {
            waitFrames++;
            yield return new WaitForEndOfFrame();
        }

        if (waitFrames > 0)
        {
            Debug.Log($"StretchToCamera2D waited {waitFrames} frame(s) for Camera.main.");
        }

        UpdateScale();
        _updateRoutine = null;
    }

    void UpdateScale()
    {
        if (Camera.main == null)
            return;

        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Vector3 newScale = transform.localScale;

        switch (stretchMode)
        {
            case StretchMode.WidthOnly:
                newScale.x = (cameraWidth / baseSize.x) * scaleMultiplier.x;
                break;

            case StretchMode.HeightOnly:
                newScale.y = (cameraHeight / baseSize.y) * scaleMultiplier.y;
                break;

            case StretchMode.WidthAndHeight:
                newScale.x = (cameraWidth / baseSize.x) * scaleMultiplier.x;
                newScale.y = (cameraHeight / baseSize.y) * scaleMultiplier.y;
                break;
        }

        transform.localScale = newScale;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!executeInUpdate)
            return;

        Vector2 currentSize = new Vector2(Screen.width, Screen.height);

        if (_lastScreenSize != currentSize && _updateRoutine == null)
        {
            _lastScreenSize = currentSize;
            UpdateScale();
        }
    }
#else
    void Update()
    {
        if (!executeInUpdate)
            return;

        Vector2 currentSize = new Vector2(Screen.width, Screen.height);

        if (_lastScreenSize != currentSize)
        {
            _lastScreenSize = currentSize;
            UpdateScale();
        }
    }
#endif
}
