//
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

using UnityEngine;
using System.Collections;

namespace DigitalRuby.WeatherMaker
{
    public abstract class WeatherMakerFogScript : MonoBehaviour
    {
        #region Public fields

        [Header("Fog Appearance")]
        [Tooltip("Fog mode")]
        public WeatherMakerFogMode FogMode = WeatherMakerFogMode.Exponential;

        [Tooltip("Fog density")]
        [Range(0.0f, 1.0f)]
        public float FogDensity = 0.05f;

        [Tooltip("Fog color")]
        public Color FogColor = Color.white;

        [Tooltip("Whether to enable volumetric fog point/spot lights. Fog always uses directional lights. Disable to improve performance.")]
        public bool EnableFogLights = false;

        [Tooltip("Maximum fog factor, where 1 is the maximum allowed fog.")]
        [Range(0.0f, 1.0f)]
        public float MaxFogFactor = 1.0f;

        [Header("Fog Noise")]
        [Tooltip("Fog noise scale. Lower values get less tiling. 0 to disable noise.")]
        [Range(0.0f, 1.0f)]
        public float FogNoiseScale = 0.0001f;

        [Tooltip("Controls how the noise value is calculated. Negative values allow areas of no noise, higher values increase the intensity of the noise.")]
        [Range(-1.0f, 1.0f)]
        public float FogNoiseAdder = 0.0f;

        [Tooltip("How much the noise effects the fog.")]
        [Range(0.0f, 10.0f)]
        public float FogNoiseMultiplier = 0.15f;

        [Tooltip("Fog noise velocity, determines how fast the fog moves. Not all fog scripts support 3d velocity, some only support 2d velocity (x and y).")]
        public Vector3 FogNoiseVelocity = new Vector3(0.01f, 0.01f, 0.0f);

        [Tooltip("Number of samples to take for 3D fog. If the player will never enter the fog, this can be a lower value. If the player can move through the fog, 40 or higher is better, but will cost some performance.")]
        [Range(1, 100)]
        public int FogNoiseSampleCount = 40;

        [Header("Fog Rendering")]
        [Tooltip("Fog material")]
        public Material FogMaterial;

        [Tooltip("Material to sample depth buffer")]
        public Material FogDepthSampleMaterial;

        [Tooltip("Blur material for down-sampled fog to fix artifacts.")]
        public Material FogBlurMaterial;

        [Tooltip("Blur shader type. Determines the strength of the blur if FogBlurMaterial is set and down sample scaling is set. " +
            "GaussianBlur7 does 7 pixels, GaussianBlur17 does 17.")]
        public BlurShaderType BlurShader = BlurShaderType.GaussianBlur17;
        protected BlurShaderType lastBlurShader = (BlurShaderType)0x7FFFFFFF;

        [Range(0.25f, 1.0f)]
        [Tooltip("Down-sample from screen size by this percent. 1 for no scaling.")]
        public float DownSampleScale = 1.0f;
        protected float lastDownSampleScale;

        [Tooltip("Dithering level. 0 to disable dithering.")]
        [Range(0.0f, 1.0f)]
        public float DitherLevel = 0.005f;

        #endregion Public fields

        #region Public methods

        /// <summary>
        /// Set a new fog density over a period of time - if set to 0, game object will be disabled at end of transition
        /// </summary>
        /// <param name="fromDensity">Start of new fog density</param>
        /// <param name="toDensity">End of new fog density</param>
        /// <param name="transitionDuration">How long to transition to the new fog density in seconds</param>
        public void TransitionFogDensity(float fromDensity, float toDensity, float transitionDuration)
        {
            FogDensity = fromDensity;
            UpdateMaterial();
            TweenFactory.Tween("WeatherMakerFog_" + gameObject.name, fromDensity, toDensity, transitionDuration, TweenScaleFunctions.Linear, (v) =>
            {
                FogDensity = v.CurrentValue;
            }, null);
        }

        public void SetFogShaderProperties(Material m)
        {
            m.SetColor("_FogColor", FogColor);
            m.SetFloat("_FogNoiseScale", FogNoiseScale);
            m.SetFloat("_FogNoiseAdder", FogNoiseAdder);
            m.SetFloat("_FogNoiseMultiplier", FogNoiseMultiplier);
            m.SetVector("_FogNoiseVelocity", FogNoiseVelocity);
            m.SetFloat("_FogNoiseSampleCount", (float)FogNoiseSampleCount);
            m.SetFloat("_FogNoiseSampleCountInverse", 1.0f / (float)FogNoiseSampleCount);
            m.SetFloat("_MaxFogFactor", MaxFogFactor);
            m.DisableKeyword("FOG_NONE");
            m.DisableKeyword("FOG_CONSTANT");
            m.DisableKeyword("FOG_EXPONENTIAL");
            m.DisableKeyword("FOG_LINEAR");
            m.DisableKeyword("FOG_EXPONENTIAL_SQUARED");
            if (FogMode == WeatherMakerFogMode.None || FogDensity <= 0.0f || MaxFogFactor <= 0.001f)
            {
                m.EnableKeyword("FOG_NONE");
            }
            else if (FogMode == WeatherMakerFogMode.Exponential)
            {
                m.EnableKeyword("FOG_EXPONENTIAL");
            }
            else if (FogMode == WeatherMakerFogMode.Linear)
            {
                m.EnableKeyword("FOG_LINEAR");
            }
            else if (FogMode == WeatherMakerFogMode.ExponentialSquared)
            {
                m.EnableKeyword("FOG_EXPONENTIAL_SQUARED");
            }
            else
            {
                m.EnableKeyword("FOG_CONSTANT");
            }
            if (FogNoiseScale > 0.0f && FogNoiseMultiplier > 0.0f && WeatherMakerLightManagerScript.NoiseTexture3DInstance != null)
            {
                m.EnableKeyword("ENABLE_FOG_NOISE");
            }
            else
            {
                m.DisableKeyword("ENABLE_FOG_NOISE");
            }
            if (EnableFogLights)
            {
                m.EnableKeyword("ENABLE_FOG_LIGHTS");
            }
            else
            {
                m.DisableKeyword("ENABLE_FOG_LIGHTS");
            }
            m.SetFloat("_FogDitherLevel", DitherLevel);
            m.SetFloat("_FogDensity", FogDensity);
        }

        #endregion Public methods

        #region Protected methods

        protected virtual void Awake()
        {

#if UNITY_EDITOR

            if (Application.isPlaying)
            {

#endif

                // clone fog material
                FogMaterial = new Material(FogMaterial);

                if (FogBlurMaterial != null)
                {
                    // clone blur material
                    FogBlurMaterial = new Material(FogBlurMaterial);
                }

#if UNITY_EDITOR

            }

#endif

        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            UpdateMaterial();
        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void OnEnable()
        {
            // https://issuetracker.unity3d.com/issues/screen-dot-width-and-screen-dot-height-values-in-onenable-function-are-incorrect
            // bug in Unity, screen width and height are wrong in OnEnable... sigh...
            lastDownSampleScale = -1.0f; // forces refresh of command buffer or anything else relying on scaling factor
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnWillRenderObject()
        {
        }

        protected virtual void OnBecameVisible()
        {
        }

        protected virtual void OnBecameInvisible()
        {
        }

        protected virtual void UpdateMaterial()
        {

#if UNITY_EDITOR

            if (FogMaterial == null)
            {
                Debug.LogError("Must set fog material and fog blur material");
            }

#endif

            if (FogBlurMaterial != null)
            {
                if (BlurShader == BlurShaderType.GaussianBlur17)
                {
                    FogBlurMaterial.DisableKeyword("BLUR7");
                }
                else
                {
                    FogBlurMaterial.EnableKeyword("BLUR7");
                }
            }
            SetFogShaderProperties(FogMaterial);
        }

        #endregion Protected methods
    }
}
