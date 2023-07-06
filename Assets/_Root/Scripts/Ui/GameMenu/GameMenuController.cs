using Profile;
using Tool;
using UnityEngine;

namespace Ui
{
    internal class GameMenuController : BaseController
    {
        private readonly ResourcePath _resourcePath = new ResourcePath("Prefabs/Game/GameMenu");
        private readonly ProfilePlayer _profilePlayer;
        private readonly Pause _pause;

        public GameMenuController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;

            GameMenuView view = LoadView(placeForUi);
            view.Init(Back,Pause);

            _pause = new Pause();
            CreatePauseMenuController(placeForUi, profilePlayer, _pause);
        }

        private GameMenuView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_resourcePath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<GameMenuView>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (_pause.IsEnabled)
                _pause.Disable();
        }

        private void Back() => _profilePlayer.CurrentState.Value = GameState.Start;

        private void Pause() => _pause.Enable();

        private PauseMenuController CreatePauseMenuController(
            Transform placeForUi, ProfilePlayer profilePlayer, Pause pause)
        {
            var pauseMenuController = new PauseMenuController(placeForUi, profilePlayer, pause);
            AddController(pauseMenuController);

            return pauseMenuController;
        }

    }
}