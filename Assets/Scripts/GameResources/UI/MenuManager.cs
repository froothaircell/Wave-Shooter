using System.Collections.Generic;
using UnityEngine;
using CoreResources.Utils.Singletons;
using CoreResources.MVCCore;

namespace GameResources.UI
{
    [System.Serializable]
    public class RMenuHandler : MonoBehaviorSingleton<RMenuHandler>
    {
        private GameObject _playerMenus;
        private List<MenuMediator> _mediators;

        protected override void InitSingleton()
        {
            _playerMenus = AppHandler.AssetManager.LoadAsset<GameObject>("PlayerUI");
            _playerMenus = Instantiate(_playerMenus);
            _playerMenus.transform.SetParent(transform);
            _mediators = new List<MenuMediator>(_playerMenus.GetComponentsInChildren<MenuMediator>());

            _mediators?.ForEach(mediator => mediator.InitializeMediator());

            base.InitSingleton();
        }

        protected override void CleanSingleton()
        {
            base.CleanSingleton();

            _mediators?.ForEach(mediator => mediator.DeInitializeMediator());
            _mediators.Clear();
            _mediators = null;
        }
    }
}
