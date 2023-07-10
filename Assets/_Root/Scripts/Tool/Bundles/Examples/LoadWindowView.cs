using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

namespace Tool.Bundles.Examples
{
    internal class LoadWindowView : AssetBundleViewBase
    {
        [Header("Asset Bundles")]
        [SerializeField] private Button _loadAssetsButton;

        [SerializeField] private Button _changeBackGroundButton;

        [Header("Addressables Spawning")]
        [SerializeField] private AssetReference _spawningButtonPrefab;
        [SerializeField] private RectTransform _spawnedButtonsContainer;
        [SerializeField] private Button _spawnAssetButton;

        [Header("Addressables BackGround")]
        [SerializeField] private AssetReference _backGround;
        [SerializeField] private Image _backGroundComponent;
        [SerializeField] private Button _addBackGroundButton;
        [SerializeField] private Button _removeBackGroundButton;

        private readonly List<AsyncOperationHandle<GameObject>> _addressablePrefabs =
            new List<AsyncOperationHandle<GameObject>>();

        private AsyncOperationHandle<Sprite>? _loadedBackGround;

        private void Start()
        {
            _loadAssetsButton.onClick.AddListener(LoadAssets);
            _changeBackGroundButton.onClick.AddListener(ChangeBackground);
            _spawnAssetButton.onClick.AddListener(SpawnPrefab);
            
            _addBackGroundButton.onClick.AddListener(AddBackGround);
            _removeBackGroundButton.onClick.AddListener(RemoveBackGround);
        }

        private void OnDestroy()
        {
            _loadAssetsButton.onClick.RemoveAllListeners();
            _changeBackGroundButton.onClick.RemoveAllListeners();
            _spawnAssetButton.onClick.RemoveAllListeners();

            _addBackGroundButton.onClick.RemoveAllListeners();
            _removeBackGroundButton.onClick.RemoveAllListeners();
            
            DespawnPrefabs();
            RemoveBackGround();
        }

        private void LoadAssets()
        {
            _loadAssetsButton.interactable = false;
            StartCoroutine(DownloadAndSetSpritesAssetBundles());
            StartCoroutine(DownloadAndSetAudioAssetBundles());
        }

        private void ChangeBackground()
        {
            _changeBackGroundButton.interactable = false;
            StartCoroutine(DownloadAndSetBackGroundsAssetBundle());
        }
        
        private void AddBackGround()
        {
            if (!_loadedBackGround.HasValue)
            {
                _loadedBackGround = Addressables.LoadAssetAsync<Sprite>(_backGround);
                _loadedBackGround.Value.Completed += OnBackGroundLoaded;
            }
        }
        
        private void RemoveBackGround()
        {
            if (!_loadedBackGround.HasValue && _loadedBackGround.Value.IsValid())
            {
                _loadedBackGround.Value.Completed -= OnBackGroundLoaded;
                Addressables.Release(_loadedBackGround);
                _loadedBackGround = null;
                
                SetBackGround(null);
            }
        }
        
        private void OnBackGroundLoaded(AsyncOperationHandle<Sprite> asyncOperationHandle)
        {
            asyncOperationHandle.Completed -= OnBackGroundLoaded;
            SetBackGround(asyncOperationHandle.Result);
        }

        private void SetBackGround(Sprite backGround) => _backGroundComponent.sprite = backGround;

        private void SpawnPrefab()
        {
            AsyncOperationHandle<GameObject> addressablePrefab =
                Addressables.InstantiateAsync(_spawningButtonPrefab, _spawnedButtonsContainer);

            _addressablePrefabs.Add(addressablePrefab);
        }

        private void DespawnPrefabs()
        {
            foreach (AsyncOperationHandle<GameObject> addressablePrefab in _addressablePrefabs)
                Addressables.ReleaseInstance(addressablePrefab);

            _addressablePrefabs.Clear();
        }
    }
}
