using UnityEngine;
using YG;

public static class YGAdsProvider
{
    public static void TryShowFullscreenAdWithChance(int chance, int maxChance)
    {
        int randomChance = Random.Range(0,maxChance);

        if(chance < maxChance)
            return;
        
        YandexGame.FullscreenShow();
    }

    public static void ShowRewardedAd(int id)
    {
        YandexGame.RewVideoShow(id);
    }
}
