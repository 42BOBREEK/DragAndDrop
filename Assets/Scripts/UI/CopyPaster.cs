using UnityEngine;
using TMPro;

public class CopyPaster : MonoBehaviour
{
    [SerializeField] private Fruit _copyPastObject;
    [SerializeField] private ObjectsSpawner _spawner;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private GameManager _manager;
    [SerializeField] private int _chargesLeft;
    [SerializeField] private TextMeshProUGUI _chargesLeftText;

    private void Start()
    {
        UpdateChargesText();
    }

    private void UpdateChargesText()
    {
        _chargesLeftText.text = _chargesLeft.ToString();
    }

    public void SpawnCopyPastObject()
    {
        if(_dragNDrop.CanDrag == false || _chargesLeft <= 0)
            return;

        Fruit copyPastObject = _spawner.SpawnFruit(_copyPastObject);
        copyPastObject.OnStartDrag();

        Rigidbody2D copyPastRigidbody = copyPastObject.GetComponent<Rigidbody2D>();

        _manager.SubscribeToCollided(copyPastObject);

        _dragNDrop.DeleteActiveObject();
        _dragNDrop.SetActiveObject(copyPastRigidbody);
        _chargesLeft--;
        UpdateChargesText();
    }

    public void AddCharges(int chargesToPlus) 
    {
        _chargesLeft += chargesToPlus;
        UpdateChargesText();
    }
}
