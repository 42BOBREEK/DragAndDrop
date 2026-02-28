using UnityEngine;
using YG;

public class UpgradeButton : MonoBehaviour
{
    private const int GetChargesAdId = 1;

    [SerializeField] private ActionButton _actionButton;
    [SerializeField] private int _chargesToPlus;
    
    public void PlusChargesWithAd()
    {
        YGAdsProvider.ShowRewardedAd(GetChargesAdId);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += OnAdWatched;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= OnAdWatched;
    }

    private void OnAdWatched(int adId)
    {
        if(adId == GetChargesAdId)
        {
            _actionButton.AddCharges(_chargesToPlus);
            gameObject.SetActive(false);
        }
    }
}
