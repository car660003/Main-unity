﻿//
// Weather Maker for Unity
// (c) 2016 Digital Ruby, LLC
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 
// *** A NOTE ABOUT PIRACY ***
// 
// If you got this asset off of leak forums or any other horrible evil pirate site, please consider buying it from the Unity asset store at https ://www.assetstore.unity3d.com/en/#!/content/60955?aid=1011lGnL. This asset is only legally available from the Unity Asset Store.
// 
// I'm a single indie dev supporting my family by spending hundreds and thousands of hours on this and other assets. It's very offensive, rude and just plain evil to steal when I(and many others) put so much hard work into the software.
// 
// Thank you.
//
//*** END NOTE ABOUT PIRACY ***
//

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace DigitalRuby.WeatherMaker
{
    public class WeatherMakerFallingParticleScript : MonoBehaviour
    {
        [Tooltip("Weather Maker Script")]
        public WeatherMakerScript WeatherScript;

        [Tooltip("Light particle looping audio source")]
        public AudioSource LoopSourceLight;

        [Tooltip("Medium particle looping audio source")]
        public AudioSource LoopSourceMedium;

        [Tooltip("Heavy particle looping audio source")]
        public AudioSource LoopSourceHeavy;

        [Tooltip("Intensity threshold for medium looping sound")]
        public float SoundMediumIntensityThreshold = 0.33f;

        [Tooltip("Intensity threshold for heavy loop sound")]
        public float SoundHeavyIntensityThreshold = 0.67f;

        [Tooltip("Overall intensity of the system (0-1)")]
        [Range(0.0f, 1.0f)]
        public float Intensity;

        [Tooltip("Intensity multiplier for fewer or extra particles")]
        [Range(0.1f, 10.0f)]
        public float IntensityMultiplier = 1.0f;

        [Tooltip("Intensity multiplier for fewer or extra secondary particles")]
        [Range(0.1f, 10.0f)]
        public float SecondaryIntensityMultiplier = 1.0f;

        [Tooltip("Intensity multiplier for fewer or extra mist particles")]
        [Range(0.1f, 10.0f)]
        public float MistIntensityMultiplier = 1.0f;

        [Tooltip("Base number of particles to emit per second. This is multiplied by intensity and intensity multiplier.")]
        [Range(100, 10000)]
        public int BaseEmissionRate = 1000;

        [Tooltip("Base number of secondary particles to emit per second. This is multiplied by intensity and intensity multiplier.")]
        [Range(100, 10000)]
        public int BaseEmissionRateSecondary = 100;

        [Tooltip("Base number of mist paticles to emit per second. This is multiplied by intensity and intensity multiplier.")]
        [Range(5, 500)]
        public int BaseMistEmissionRate = 50;

        [Tooltip("Particle system")]
        public ParticleSystem ParticleSystem;

        [Tooltip("Particle system that is secondary and optional")]
        public ParticleSystem ParticleSystemSecondary;

        [Tooltip("Particle system to use for mist")]
        public ParticleSystem MistParticleSystem;

        [Tooltip("Particles system for when particles hit something")]
        public ParticleSystem ExplosionParticleSystem;

        [Tooltip("The threshold that Intensity must pass for secondary particles to appear (0 - 1). Set to 1 for no secondary particles. Set this before changing Intensity.")]
        [Range(0.0f, 1.0f)]
        public float SecondaryThreshold = 0.75f;

        [Tooltip("The threshold that Intensity must pass for mist particles to appear (0 - 1). Set to 1 for no mist. Set this before changing Intensity.")]
        [Range(0.0f, 1.0f)]
        public float MistThreshold = 0.5f;

        [Tooltip("Particle dithering factor")]
        [Range(0.0f, 1.0f)]
        public float DitherLevel = 0.002f;

        public LoopingAudioSource AudioSourceLight { get; private set; }
        public LoopingAudioSource AudioSourceMedium { get; private set; }
        public LoopingAudioSource AudioSourceHeavy { get; private set; }
        public LoopingAudioSource AudioSourceCurrent { get; private set; }

        public Material Material { get; private set; }
        public Material MaterialSecondary { get; private set; }
        public Material ExplosionMaterial { get; private set; }
        public Material MistMaterial { get; private set; }

        [NonSerialized]
        private float lastIntensityValue = -1.0f;

        [NonSerialized]
        private float lastIntensityMultiplierValue = -1.0f;

        [NonSerialized]
        private float lastSecondaryIntensityMultiplierValue = -1.0f;

        [NonSerialized]
        private float lastMistIntensityMultiplierValue = -1.0f;

        private void PlayParticleSystem(ParticleSystem p, int baseEmissionRate, float intensityMultiplier)
        {
            var e = p.emission;
            ParticleSystem.MinMaxCurve rate = p.emission.rateOverTime;
            rate.mode = ParticleSystemCurveMode.Constant;
            rate.constantMin = rate.constantMax = baseEmissionRate * Intensity * intensityMultiplier;
            e.rateOverTime = rate;
            var m = p.main;
            m.maxParticles = (int)Mathf.Max(m.maxParticles, rate.constantMax * m.startLifetime.constantMax);
            if (!p.isEmitting)
            {
                p.Play();
            }
        }

        private void CheckForIntensityChange()
        {
            if (lastIntensityValue == Intensity && lastIntensityMultiplierValue == IntensityMultiplier &&
                lastSecondaryIntensityMultiplierValue == SecondaryIntensityMultiplier && lastMistIntensityMultiplierValue == MistIntensityMultiplier)
            {
                return;
            }

            lastIntensityValue = Intensity;
            lastIntensityMultiplierValue = IntensityMultiplier;
            lastSecondaryIntensityMultiplierValue = SecondaryIntensityMultiplier;
            lastMistIntensityMultiplierValue = MistIntensityMultiplier;

            if (Intensity < 0.01f)
            {
                if (AudioSourceCurrent != null)
                {
                    AudioSourceCurrent.Stop();
                    AudioSourceCurrent = null;
                }
                if (ParticleSystem != null && ParticleSystem.isEmitting)
                {
                    ParticleSystem.Stop();
                }
                if (ParticleSystemSecondary != null && ParticleSystemSecondary.isEmitting)
                {
                    ParticleSystemSecondary.Stop();
                }
                if (MistParticleSystem != null && MistParticleSystem.isEmitting)
                {
                    MistParticleSystem.Stop();
                }
            }
            else
            {
                LoopingAudioSource newSource;
                if (Intensity >= SoundHeavyIntensityThreshold)
                {
                    newSource = AudioSourceHeavy;
                }
                else if (Intensity >= SoundMediumIntensityThreshold)
                {
                    newSource = AudioSourceMedium;
                }
                else
                {
                    newSource = AudioSourceLight;
                }
                if (AudioSourceCurrent != newSource)
                {
                    if (AudioSourceCurrent != null)
                    {
                        AudioSourceCurrent.Stop();
                    }
                    AudioSourceCurrent = newSource;
                    AudioSourceCurrent.Play(1.0f);
                }
                AudioSourceCurrent.SecondaryVolumeModifier = Mathf.Pow(Intensity, 0.3f);
                if (ParticleSystem != null)
                {
                    PlayParticleSystem(ParticleSystem, BaseEmissionRate, IntensityMultiplier);
                }
                if (ParticleSystemSecondary != null)
                {
                    if (SecondaryThreshold >= Intensity)
                    {
                        ParticleSystemSecondary.Stop();
                    }
                    else
                    {
                        PlayParticleSystem(ParticleSystemSecondary, BaseEmissionRateSecondary, SecondaryIntensityMultiplier);
                    }
                }
                if (MistParticleSystem != null)
                {
                    if (MistThreshold >= Intensity)
                    {
                        MistParticleSystem.Stop();
                    }
                    else
                    {
                        PlayParticleSystem(MistParticleSystem, BaseMistEmissionRate, MistIntensityMultiplier);
                    }
                }
            }
        }

        private Material InitParticleSystem(ParticleSystem p)
        {
            if (p == null)
            {
                return null;
            }

            Renderer renderer = p.GetComponent<Renderer>();
            Material m = new Material(renderer.material);
            renderer.material = m;

            return m;
        }

        private void CheckForParticleSystem()
        {

#if DEBUG

            if (ParticleSystem == null)
            {
                Debug.LogError("Particle system is null");
                return;
            }

#endif

        }

        private void UpdateCollisionForParticleSystem(ParticleSystem p)
        {
            if (p != null)
            {
                var c = p.collision;
                var s = p.subEmitters;
                c.enabled = collisionEnabled;
                s.enabled = collisionEnabled;
            }
        }

        private void UpdateInitialParticleSystemValues(ParticleSystem p, System.Action<float> startSpeed, System.Action<float> startSize)
        {
            if (p != null)
            {
                var m = p.main;
                startSpeed(((float)m.startSpeed.constantMax + (float)m.startSpeed.constantMin) * 0.5f);
                startSize(((float)m.startSize.constantMax + (float)m.startSize.constantMin) * 0.5f);
            }
        }

        protected virtual void OnCollisionEnabledChanged() { }

        protected virtual void Awake()
        {
            CheckForParticleSystem();
            AudioSourceLight = new LoopingAudioSource(LoopSourceLight);
            AudioSourceMedium = new LoopingAudioSource(LoopSourceMedium);
            AudioSourceHeavy = new LoopingAudioSource(LoopSourceHeavy);
            UpdateInitialParticleSystemValues(ParticleSystem, (f) => InitialStartSpeed = f, (f) => InitialStartSize = f);
            UpdateInitialParticleSystemValues(ParticleSystemSecondary, (f) => InitialStartSpeedSecondary = f, (f) => InitialStartSizeSecondary = f);
            UpdateInitialParticleSystemValues(MistParticleSystem, (f) => InitialStartSpeedMist = f, (f) => InitialStartSizeMist = f);
            Material = InitParticleSystem(ParticleSystem);
            MaterialSecondary = InitParticleSystem(ParticleSystemSecondary);
            MistMaterial = InitParticleSystem(MistParticleSystem);
            ExplosionMaterial = InitParticleSystem(ExplosionParticleSystem);
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            CheckForIntensityChange();
            AudioSourceLight.Update();
            AudioSourceMedium.Update();
            AudioSourceHeavy.Update();
            if (MistMaterial != null)
            {
                MistMaterial.SetFloat("_ParticleDitherLevel", DitherLevel);
            }
        }

        protected virtual void FixedUpdate()
        {
        }

        protected float InitialStartSpeed { get; private set; }
        protected float InitialStartSize { get; private set; }
        protected float InitialStartSpeedSecondary { get; private set; }
        protected float InitialStartSizeSecondary { get; private set; }
        protected float InitialStartSpeedMist { get; private set; }
        protected float InitialStartSizeMist { get; private set; }

        private bool collisionEnabled;
        public bool CollisionEnabled
        {
            get { return collisionEnabled; }
            set
            {
                if (value != collisionEnabled)
                {
                    collisionEnabled = value;
                    UpdateCollisionForParticleSystem(ParticleSystem);
                    UpdateCollisionForParticleSystem(ParticleSystemSecondary);
                    UpdateCollisionForParticleSystem(MistParticleSystem);
                    UpdateCollisionForParticleSystem(ExplosionParticleSystem);
                    OnCollisionEnabledChanged();
                }
            }
        }

        public virtual void PreCullCamera(Camera c) { }
    }
}