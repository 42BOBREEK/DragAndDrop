using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(Image))]
public class ActionButton : MonoBehaviour
{
    [SerializeField] protected int _chargesLeft;
    [SerializeField] private TextMeshProUGUI _chargesLeftText;
    [SerializeField] private UpgradeButton _upgradeButton;

    [SerializeField] private Image _buttonImage;

    public event Action ChargesChanged;
    public int ChargesLeft => _chargesLeft;

    private void Start()
    {
        UpdateChargesText();
    }

    protected void UpdateChargesText()
    {
        _chargesLeftText.text = _chargesLeft.ToString();
    }

    protected void InvokeChargesChanged()
    {
       ChargesChanged?.Invoke(); 

       CheckCharges();
    } 

    protected void CheckCharges()
    {
        if(_chargesLeft <= 0)
        {
            _upgradeButton.gameObject.SetActive(true);
            _buttonImage.raycastTarget = false;
        }
        else
        {
            _upgradeButton.gameObject.SetActive(false);
            _buttonImage.raycastTarget = true;
        }
    }

    public void SetCharges(int charges)
    {
        _chargesLeft = charges;
        InvokeChargesChanged();
        UpdateChargesText();
    }

    public void AddCharges(int chargesToPlus) 
    {
        _chargesLeft += chargesToPlus;
        InvokeChargesChanged();
        UpdateChargesText();
    }
}