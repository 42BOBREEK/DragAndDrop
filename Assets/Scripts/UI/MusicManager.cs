using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Image _musicImage;
    [SerializeField] private Sprite _isOnSprite;
    [SerializeField] private Sprite _isOffSprite;
    [SerializeField] private AudioSource _audioSource;

    public static bool IsOn = true;

    public void ToggleIsOn()
    {
        IsOn = !IsOn;

        if(_audioSource != null)
            _audioSource.mute = !IsOn;

        ChangeSprite();
    }

    private void Start()
    {
        ChangeSprite();

        if(_audioSource != null)
            _audioSource.mute = !IsOn;
    }

    private void ChangeSprite()
    {
        if(IsOn == true)
            _musicImage.sprite = _isOnSprite;
        else
            _musicImage.sprite = _isOffSprite;
    }
}
