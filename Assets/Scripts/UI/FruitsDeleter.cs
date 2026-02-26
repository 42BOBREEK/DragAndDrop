using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class FruitsDeleter : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private DragAndDrop _dragNDrop;
    [SerializeField] private bool _canDelete;
    [SerializeField] private float _canDragDelay;
    [SerializeField] private DeleterAnimation _animation;
        
    [SerializeField] private int _chargesLeft;
    [SerializeField] private TextMeshProUGUI _chargesLeftText;

    public event Action<Fruit> FruitDeleted;

    private void OnEnable()
    {
        _input.Pressed += OnPressed;
        _animation.AnimationEnded += DeleteFruitObject;
        UpdateChargesText();
    }

    private void OnDisable()
    {
        _input.Pressed -= OnPressed;
        _animation.AnimationEnded -= DeleteFruitObject;
    }

    private void OnPressed()
    {
        if(_canDelete == false || _chargesLeft <= 0)
            return;

        Fruit fruitToDelete = _input.GetFruitUnderPointer();

        if(fruitToDelete == null)
            return;

        DeleteFruit(fruitToDelete);

        _canDelete = false;
        
        StartCoroutine(SetCanDragWithDelay());
    }

    private void DeleteFruitObject(Fruit fruit)
    {
        Destroy(fruit.gameObject);
        _chargesLeft--;
        UpdateChargesText();
    }

    private void DeleteFruit(Fruit fruit)
    {
        FruitDeleted?.Invoke(fruit);
    }

    private IEnumerator SetCanDragWithDelay()
    {
        yield return new WaitForSeconds(_canDragDelay);

        _dragNDrop.SetCanDrag(true);
    }

    private void UpdateChargesText()
    {
        _chargesLeftText.text = _chargesLeft.ToString();
    }

    public void AddCharges(int chargesToPlus) 
    {
        _chargesLeft += chargesToPlus;
        UpdateChargesText();
    }

    public void ChangeCanDelete(bool canDelete) 
    {
        _canDelete = canDelete;
        _dragNDrop.SetCanDrag(!canDelete);
    }
}
