﻿using System;
using CoreResources.Utils;
using UnityEngine;

namespace GameResources.Bullet
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
        private float _modulationFrequencyFactor = 1f / 3f; // Defines how much larger will the modulation wave be
        private float _amplitudeFactor = 1f; // For base amplitude modifications
        private float _frequencyFactor = 6f; // For base frequency modifications
        private float _localTime = 0f; // Every bullet should have a unique timer;

        // Defines the wave functions to use for the respective task
        private Func<float, float> _waveFunction;
        private Func<float, float> _ampModulator;
        private Func<float, float> _freqModulator;

        private Transform _bulletBody;

        public void SetWaveSpecs(ModulationType modType, Func<float, float> waveFunc, Func<float, float> modFunction = null)
        {
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

        public override void OnInit()
        {
            _localTime = 0f;
            _currentModType = ModulationType.None;
            
            if (_bulletBody == null)
                _bulletBody = transform.GetChild(0);
        }

        public override void OnUpdate()
        {
            BulletTrajectory();
        }

        private void BulletTrajectory()
        {
            _localTime += Time.deltaTime;
            float lateralPos = 0f;
            switch (_currentModType)
            {
                case ModulationType.None:
                    lateralPos = _amplitudeFactor * _waveFunction(_frequencyFactor * _localTime);
                    break;
                case ModulationType.AmplitudeMod:
                    lateralPos = AmplitudeModulation(_localTime);
                    break;
                case ModulationType.FrequencyMod:
                    lateralPos = FrequencyModulation(_localTime);
                    break;
            }
            
            _bulletBody.localPosition =
                new Vector3(lateralPos, _bulletBody.localPosition.y, _bulletBody.localPosition.z);
        }

        private float AmplitudeModulation(float time)
        {
            return _ampModulator(_modulationFrequencyFactor * time) * _amplitudeFactor *
                   _waveFunction(_frequencyFactor * time);
        }

        private float FrequencyModulation(float time)
        {
            return _amplitudeFactor * _waveFunction(_freqModulator(_modulationFrequencyFactor * time) *
                                                _frequencyFactor * time);
        }
    }
}