using System;
using Features.Inventory;
using Features.Shed.Upgrade;
using Profile;
using Tool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.Shed
{
    internal class ShedContext : BaseContext
    {
        private static readonly ResourcePath _viewPath = new("Prefabs/Shed/ShedView");
        private static readonly ResourcePath _dataSourcePath = new("Configs/Shed/UpgradeItemConfigDataSource");
        
        public ShedContext(Transform placeforUi,ProfilePlayer profilePlayer)
        {
            if (placeforUi == null)
                throw new ArgumentException(nameof(placeforUi));
            if (profilePlayer == null)
                throw new ArgumentException(nameof(profilePlayer));
            CreateController(profilePlayer, placeforUi);
        }

        private ShedController CreateController(ProfilePlayer profilePlayer, Transform placeForUi)
        {
            InventoryContext inventoryContext = CreateInventoryContext(placeForUi, profilePlayer.Inventory);
            UpgradeHandlersRepository shedrepository = CreateRepository();
            ShedView shedView = LoadView(placeForUi);
            return new ShedController
            (
                shedView,
                profilePlayer,
                shedrepository
            );
        }
        private ShedView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_viewPath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<ShedView>();
        }

        private UpgradeHandlersRepository CreateRepository()
        {
            UpgradeItemConfig[] upgradeConfigs = LaodConfigs();
            var repository = new UpgradeHandlersRepository(upgradeConfigs);
            AddRepository(repository);

            return repository;
        }

        private InventoryContext CreateInventoryContext(Transform placeForUi, InventoryModel model)
        {
            var context = new InventoryContext(placeForUi, model);
            AddContext(context);
            return context;
        }

        UpgradeItemConfig[] LaodConfigs() =>
            ContentDataSourceLoader.LoadUpgradeItemConfigs(_dataSourcePath);
    }
}