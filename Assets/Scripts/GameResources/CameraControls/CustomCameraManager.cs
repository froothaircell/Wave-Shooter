using CoreResources.Utils.Singletons;
using Cinemachine;
using GameResources.Events;
using UnityEngine;

namespace GameResources.CameraControls
{
    public class CustomCameraManager : MonoBehaviorSingleton<CustomCameraManager>
    {
        private CinemachineBrain _mainBrain;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private CinemachineFramingTransposer _cinemachineFramingTransposer;
        private CinemachineBasicMultiChannelPerlin _cinemachineLookNoise;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out _mainBrain);
            if (_mainBrain == null)
            {
                _mainBrain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
            }

            _cinemachineVirtualCamera = gameObject.AddComponent<CinemachineVirtualCamera>();
            _cinemachineFramingTransposer = _cinemachineVirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
            _cinemachineLookNoise = _cinemachineVirtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _cinemachineVirtualCamera.m_Lens.FarClipPlane = 150f;
            _cinemachineVirtualCamera.m_Lens.FieldOfView = 95f;
            _cinemachineFramingTransposer.m_CameraDistance = 20f;
            _cinemachineLookNoise.m_AmplitudeGain = 1.2f;
            _cinemachineLookNoise.m_NoiseProfile =
                AppHandler.AssetManager.LoadAsset<NoiseSettings>("Handheld_normal_mild");

            AppHandler.EventManager.Subscribe<REvent_PlayerSpawned>(OnShipSpawned, _disposables);
        }

        private void OnShipSpawned(REvent_PlayerSpawned evt)
        {
            _cinemachineVirtualCamera.Follow = evt.ShipTransform;
        }
    }
}