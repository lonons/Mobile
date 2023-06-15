using System;
using Services.Ads;
using Services.Ads.UnityAds;
using Services.Analytics;
using Services.IAP;
using UnityEngine;

namespace Services
{
    internal class ServiceRoster : MonoBehaviour
    {
        private static ServiceRoster _instance;
        private static ServiceRoster Instance => _instance ??= FindObjectOfType<ServiceRoster>();
        public static IAdsService AdsService => Instance._adsService;
        public static IIAPService IAPService => Instance._iapService;
        public static IAnalyticsManager Analytics => Instance._analyticsManager;

        [SerializeField] private UnityAdsService _adsService;
        [SerializeField] private IAPService _iapService;
        [SerializeField] private AnalyticsManager _analyticsManager;

        private void Awake() => _instance ??= this;
    }
 
}
