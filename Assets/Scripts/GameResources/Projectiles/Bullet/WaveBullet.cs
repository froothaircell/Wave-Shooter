using System;
using UnityEngine;

namespace GameResources.Projectiles.Bullet
{
    public enum ModulationType
    {
        None = 0,
        AmplitudeMod = 1,
        FrequencyMod = 2
    }
    
    public class WaveBullet : RBullet
    {
        private ModulationType _currentModType = ModulationType.None;
        private float _modulationFrequencyFactor = 2f; // Defines how much larger will the modulation wave be
        private float _amplitudeFactor = 1f; // For base amplitude modifications
        private float _frequencyFactor = 15f; // For base frequency modifications

        // Defines the wave functions to use for the respective task
        private Func<float, float> _waveFunction;
        private Func<float, float> _ampModulator;
        private Func<float, float> _freqModulator;

        private Transform _bulletBody;

        public void SetSpawnedWaveBulletSpecs(int damage, float bulletSpeed, int spawnIndex, ModulationType modType, 
            Func<float, float> waveFunc, Func<float, float> modFunction = null)
        {
            SetSpawnedBulletSpecs(damage, bulletSpeed, spawnIndex);
            _currentModType = modType;
            _waveFunction = waveFunc;
            switch (modType)
            {
                case ModulationType.AmplitudeMod:
                    _ampModulator = modFunction;
                    break;
                case ModulationType.FrequencyMod:
                    _freqModulator = modFunction;
                    break;
            }
        }

        public void ResetWaveSpecs()
        {
            _currentModType = ModulationType.None;
            _waveFunction = null;
            _ampModulator = null;
            _freqModulator = null;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            
            if (_bulletBody == null)
                _bulletBody = transform.GetChild(0);
        }

        public override void OnSpawnedUpdate()
        {
            base.OnSpawnedUpdate();
            BulletTrajectory();
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            GetComponentInChildren<TrailRenderer>().Clear();
        }

        private void BulletTrajectory()
        {
            if (_waveFunction == null)
                return;
            float lateralPos = 0f;
            switch (_currentModType)
            {
                case ModulationType.None:
                    lateralPos = _amplitudeFactor * _waveFunction(_frequencyFactor * LocalTime);
                    break;
                case ModulationType.AmplitudeMod:
                    lateralPos = AmplitudeModulation(LocalTime);
                    break;
                case ModulationType.FrequencyMod:
                    lateralPos = FrequencyModulation(LocalTime);
                    break;
            }
            
            _bulletBody.localPosition =
                new Vector3(lateralPos, _bulletBody.localPosition.y, _bulletBody.localPosition.z);
        }

        private float AmplitudeModulation(float time)
        {
            return 2f * _ampModulator(_modulationFrequencyFactor * time) * _amplitudeFactor *
                   _waveFunction(_frequencyFactor * time);
        }

        private float FrequencyModulation(float time)
        {
            return 2f * _amplitudeFactor * _waveFunction(_freqModulator(_modulationFrequencyFactor * time) *
                                                _frequencyFactor * time);
        }
    }
}