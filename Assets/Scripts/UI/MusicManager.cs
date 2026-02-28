using UnityEngine;
using UnityEngine.UI;
using System;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Image _musicImage;
    [SerializeField] private Sprite _isOnSprite;
    [SerializeField] private Sprite _isOffSprite;
    [SerializeField] private AudioSource _audioSource;

    private bool _isOn;

    public bool IsOn => _isOn;
    public event Action MusicToggled;

    private void Start()
    {
        ChangeSprite();

        ChangeMutedIfNeeded();
    }

    private void ChangeSprite()
    {
        if(IsOn == true)
            _musicImage.sprite = _isOnSprite;
        else
            _musicImage.sprite = _isOffSprite;
    }

    private void ChangeMutedIfNeeded()
    {
        if(_audioSource != null)
            _audioSource.mute = !_isOn;
    }

    public void ToggleIsOn()
    {
        _isOn = !_isOn;

        MusicToggled?.Invoke();

        ChangeMutedIfNeeded();

        ChangeSprite();
    }

    public void SetMusicState(bool isOn)
    {
        _isOn = isOn;

        ChangeMutedIfNeeded();

        ChangeSprite();
    }
}