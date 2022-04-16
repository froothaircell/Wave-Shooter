using System;
using UnityEngine;

namespace GameResources.Bullet
{
    public enum ModulationType
    {
        None = 0,
        AmplitudeMod = 1,
        FrequencyMod = 2
    }
    
    public abstract class RSpecBullet : MonoBehaviour, IBullet
    {
        protected ModulationType currentModType = ModulationType.None;
        protected float ModulationFrequencyFactor = 1f / 3f; // Defines how much larger will the modulation wave be
        protected float AmplitudeFactor = 1f; // For base amplitude modifications
        protected float FrequencyFactor = 6f; // For base frequency modifications
        protected float LocalTime = 0f; // Every bullet should have a unique timer;

        // Defines the wave functions to use for the respective task
        protected Func<float, float> WaveFunction;
        protected Func<float, float> AmpModulator;
        protected Func<float, float> FreqModulator;

        private Transform _bulletBody;

        public virtual void OnEnable()
        {
            LocalTime = 0f;
            currentModType = ModulationType.None;

            if (_bulletBody == null)
                _bulletBody = transform.GetChild(0);
        }

        public virtual void Update()
        {
            BulletTrajectory();
        }

        public void BulletTrajectory()
        {
            LocalTime += Time.deltaTime;
            float lateralPos;
            switch (currentModType)
            {
                case ModulationType.None:
                    lateralPos = AmplitudeFactor * WaveFunction(FrequencyFactor * LocalTime);
                    break;
                case ModulationType.AmplitudeMod:
                    lateralPos = AmplitudeModulation(LocalTime);
                    break;
                case ModulationType.FrequencyMod:
                    lateralPos = FrequencyModulation(LocalTime);
                    break;
                default:
                    lateralPos = AmplitudeFactor * WaveFunction(FrequencyFactor * LocalTime);
                    break;
            }
            
            _bulletBody.localPosition =
                new Vector3(lateralPos, _bulletBody.localPosition.y, _bulletBody.localPosition.z);
        }

        public virtual float AmplitudeModulation(float time)
        {
            return AmpModulator(ModulationFrequencyFactor * time) * AmplitudeFactor *
                   WaveFunction(FrequencyFactor * time);
        }

        public virtual float FrequencyModulation(float time)
        {
            return AmplitudeFactor * WaveFunction(FreqModulator(ModulationFrequencyFactor * time) *
                                                FrequencyFactor * time);
        }
    }
}