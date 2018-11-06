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
    /// <summary>
    /// Sky modes
    /// </summary>
    public enum WeatherMakeSkyMode
    {
        /// <summary>
        /// Textured - day, dawn/dusk and night are all done via textures
        /// </summary>
        Textured = 0,

        /// <summary>
        /// Procedural sky - day and dawn/dusk textures are overlaid on top of procedural sky, night texture is used as is
        /// </summary>
        ProceduralTextured,

        /// <summary>
        /// Procedural sky - day, dawn/dusk textures are ignored, night texture is used as is
        /// </summary>
        Procedural
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class WeatherMakerSkySphereScript : WeatherMakerSphereCreatorScript
    {
        [Header("Dependencies")]
        [Tooltip("Weather maker script")]
        public WeatherMakerScript WeatherScript;

        [Tooltip("Cloud shadow projector script")]
        public WeatherMakerCloudShadowProjectorScript CloudShadowProjectorScript;

        [Header("Sky Rendering")]
        [Tooltip("The sky mode. 'Textured' uses a texture for day, dawn/dusk and night. " +
            "'Procedural textured' combines a procedural sky with the day and dawn/dusk textures using alpha, and uses the night texture as is. " +
            "'Procedural' uses the night texture as is and does everything else procedurally.")]
        public WeatherMakeSkyMode SkyMode = WeatherMakeSkyMode.Textured;

        [Range(0.0f, 1.0f)]
        [Tooltip("Dither level")]
        public float DitherLevel = 0.005f;

        [Header("Positioning")]
        [Range(-0.5f, 0.5f)]
        [Tooltip("Offset the sky this amount from the camera y. This value is multiplied by the height of the sky sphere.")]
        public float YOffsetMultiplier = 0.0f;

        [Range(0.1f, 0.99f)]
        [Tooltip("Place the sky sphere at this amount of the far clip plane")]
        public float FarClipPlaneMultiplier = 0.8f;

        [Header("Textures - dawn/dusk not used in procedural sky.")]
        [Tooltip("The daytime texture")]
        public Texture2D DayTexture;

        [Tooltip("The dawn / dusk texture (not used for procedural sky) - this MUST be set if DawnDuskFadeDegrees is not 0, otherwise things will look funny.")]
        public Texture2D DawnDuskTexture;

        [Tooltip("The night time texture")]
        public Texture2D NightTexture;

        [Header("Ambient Colors")]
        [Tooltip("Day ambient color")]
        public Color DayAmbientColor = Color.black;

        [Tooltip("Day ambient intensity")]
        [Range(0.0f, 1.0f)]
        public float DayAmbientIntensity = 0.0f;

        [Tooltip("Dawn/Dusk ambient color")]
        public Color DawnDuskAmbientColor = Color.black;

        [Tooltip("Dawn/Dusk ambient intensity")]
        [Range(0.0f, 1.0f)]
        public float DawnDuskAmbientIntensity = 0.0f;

        [Tooltip("Night ambient color")]
        public Color NightAmbientColor = Color.black;

        [Tooltip("Night ambient intensity")]
        [Range(0.0f, 1.0f)]
        public float NightAmbientIntensity = 0.0f;

        [Header("Night Sky")]
        [Range(0.0f, 1.0f)]
        [Tooltip("Night pixels must have an R, G or B value greater than or equal to this to be visible. Raise this value if you want to hide dimmer elements " +
            "of your night texture or there is a lot of light pollution, i.e. a city.")]
        public float NightVisibilityThreshold = 0.0f;

        [Range(0.0f, 32.0f)]
        [Tooltip("Intensity of night sky. Pixels that don't meet the NightVisibilityThreshold will still be invisible.")]
        public float NightIntensity = 2.0f;

        [Range(0.0f, 100.0f)]
        [Tooltip("How fast the twinkle pulsates")]
        public float NightTwinkleSpeed = 16.0f;

        [Tooltip("The variance of the night twinkle. The higher the value, the more variance.")]
        [Range(0.0f, 10.0f)]
        public float NightTwinkleVariance = 1.0f;

        [Tooltip("The minimum of the max rgb component for the night pixel to twinkle")]
        [Range(0.0f, 1.0f)]
        public float NightTwinkleMinimum = 0.02f;

        [Tooltip("The amount of randomness in the night sky twinkle")]
        [Range(0.0f, 5.0f)]
        public float NightTwinkleRandomness = 0.15f;

        [Header("Clouds - Noise")]
        [Tooltip("Texture for cloud noise")]
        public Texture2D CloudNoise;

        [Tooltip("Cloud noise scale")]
        [Range(0.000001f, 1.0f)]
        public float CloudNoiseScale = 0.02f;

        [Tooltip("Multiplier for cloud noise")]
        [Range(0.0f, 4.0f)]
        public float CloudNoiseMultiplier = 1.0f;

        [Tooltip("Cloud noise velocity (xz)")]
        public Vector2 CloudNoiseVelocity;

        [Tooltip("Texture for masking cloud noise, makes clouds visible in only certain parts of the sky.")]
        public Texture2D CloudNoiseMask;

        [Tooltip("Cloud noise mask scale")]
        [Range(0.000001f, 1.0f)]
        public float CloudNoiseMaskScale = 0.5f;

        [Tooltip("Cloud noise mask rotation in degrees")]
        [Range(0.0f, 360.0f)]
        public float CloudNoiseMaskRotation = 0.0f;

        [Tooltip("Offset for cloud noise mask")]
        public Vector2 CloudNoiseMaskOffset;

        [Tooltip("Cloud noise mask velocity (xz)")]
        public Vector2 CloudNoiseMaskVelocity;

        [Header("Clouds - Appearance")]
        [Tooltip("Cloud color, for lighting")]
        public Color CloudColor = Color.white;

        [Tooltip("Cloud emission color, always emits this color regardless of lighting.")]
        public Color CloudEmissionColor = Color.clear;

        [Tooltip("Cloud height - only affects where the clouds stop at the horizon")]
        public float CloudHeight = 500;

        [Tooltip("Cloud cover, controls how many clouds there are")]
        [Range(0.0f, 1.0f)]
        public float CloudCover = 0.2f;

        [Tooltip("Cloud density, controls how opaque the clouds are")]
        [Range(0.0f, 1.0f)]
        public float CloudDensity = 0.0f;

        [Tooltip("Cloud light absorption. As this approaches 0, all light is absorbed.")]
        [Range(0.0f, 1.0f)]
        public float CloudLightAbsorption = 0.013f;

        [Tooltip("Cloud sharpness, controls how distinct the clouds are")]
        [Range(0.0f, 1.0f)]
        public float CloudSharpness = 0.015f;

        [Tooltip("Cloud whispiness, controls how thin / small particles the clouds get as they change over time.")]
        [Range(0.0f, 3.0f)]
        public float CloudWhispiness = 1.0f;

        [Tooltip("Changes the whispiness of the clouds over time")]
        [Range(0.0f, 1.0f)]
        public float CloudWhispinessChangeFactor = 0.03f;

        [Tooltip("Cloud pixels with alpha greater than this will cast a shadow. Set to 1 to disable cloud shadows.")]
        [Range(0.0f, 1.0f)]
        public float CloudShadowThreshold = 0.1f;

        [Tooltip("Cloud shadow power. 0 is full power, 1 is no power.")]
        [Range(0.0001f, 1.0f)]
        public float CloudShadowPower = 0.5f;

        [Tooltip("Bring clouds down at the horizon at the cost of stretching over the top.")]
        [Range(0.0f, 0.5f)]
        public float CloudRayOffset = 0.1f;

        [Header("Sky Parameters (Advanced)")]
        [Tooltip("Allows eclipses - beware Unity bug that causes raycast to be very expensive. If you see CPU spike, disable.")]
        public bool CheckForEclipse;

        [Range(0.0f, 1.0f)]
        [Tooltip("Sky camera height (km)")]
        public float SkyCameraHeight = 0.0001f;

        [Tooltip("Sky tint color")]
        public Color SkyTintColor = new Color(0.5f, 0.5f, 0.5f);

        [Range(0.0f, 10.0f)]
        [Tooltip("Sky atmosphere mie, controls glow around the sun, etc.")]
        public float SkyAtmosphereMie = 0.99f;

        [Range(0.0f, 5.0f)]
        [Tooltip("Sky atmosphere thickness")]
        public float SkyAtmosphereThickness = 1.0f;

        [Range(1.0f, 2.0f)]
        [Tooltip("Sky outer radius")]
        public float SkyOuterRadius = 1.025f;

        [Range(1.0f, 2.0f)]
        [Tooltip("Sky inner radius")]
        public float SkyInnerRadius = 1.0f;

        [Range(0.0f, 500.0f)]
        [Tooltip("Sky mie multiplier")]
        public float SkyMieMultiplier = 1.0f;

        [Range(0.0f, 100.0f)]
        [Tooltip("Sky rayleigh multiplier")]
        public float SkyRayleighMultiplier = 1.0f;

        /// <summary>
        /// Checks whether clouds are enabled
        /// </summary>
        public bool CloudsEnabled
        {
            get { return (CloudNoise != null && CloudColor.a > 0.0f && CloudNoiseMultiplier > 0.0f && CloudCover > 0.0f); }
        }

        // light wave length constants
        private const float lightWaveLengthRed = 0.65f;
        private const float lightWaveLengthGreen = 0.570f;
        private const float lightWaveLengthBlue = 0.475f;
        private const float lightWaveTintRange = 0.15f;

        private RaycastHit[] eclipseHits = new RaycastHit[16];
        
        private void SetShaderSkyParameters()
        {
            Material.mainTexture = DayTexture;
            Material.SetTexture("_DawnDuskTex", DawnDuskTexture);
            Material.SetTexture("_NightTex", NightTexture);
            if (SkyMode == WeatherMakeSkyMode.Textured)
            {
                Material.DisableKeyword("ENABLE_PROCEDURAL_TEXTURED_SKY");
                Material.DisableKeyword("ENABLE_PROCEDURAL_SKY");
            }
            else if (SkyMode == WeatherMakeSkyMode.Procedural)
            {
                Material.EnableKeyword("ENABLE_PROCEDURAL_SKY");
                Material.DisableKeyword("ENABLE_PROCEDURAL_TEXTURED_SKY");
            }
            else if (SkyMode == WeatherMakeSkyMode.ProceduralTextured)
            {
                Material.EnableKeyword("ENABLE_PROCEDURAL_TEXTURED_SKY");
                Material.DisableKeyword("ENABLE_PROCEDURAL_SKY");
            }

            // global sky parameters

            float mieG = -SkyAtmosphereMie;
            float mieG2 = SkyAtmosphereMie * SkyAtmosphereMie;
            float mieConstant = 0.001f * SkyMieMultiplier;
            float rayleighConstant = 0.0025f * SkyRayleighMultiplier;
            rayleighConstant = Mathf.LerpUnclamped(0.0f, rayleighConstant, Mathf.Pow(SkyAtmosphereThickness, 2.5f));
            float lightWaveLengthRedTint = Mathf.Lerp(lightWaveLengthRed - lightWaveTintRange, lightWaveLengthRed + lightWaveTintRange, 1.0f - SkyTintColor.r);
            float lightWaveLengthGreenTint = Mathf.Lerp(lightWaveLengthGreen - lightWaveTintRange, lightWaveLengthGreen + lightWaveTintRange, 1.0f - SkyTintColor.g);
            float lightWaveLengthBlueTint = Mathf.Lerp(lightWaveLengthBlue - lightWaveTintRange, lightWaveLengthBlue + lightWaveTintRange, 1.0f - SkyTintColor.b);
            float lightWaveLengthRed4 = lightWaveLengthRedTint * lightWaveLengthRedTint * lightWaveLengthRedTint * lightWaveLengthRedTint;
            float lightWaveLengthGreen4 = lightWaveLengthGreenTint * lightWaveLengthGreenTint * lightWaveLengthGreenTint * lightWaveLengthGreenTint;
            float lightWaveLengthBlue4 = lightWaveLengthBlueTint * lightWaveLengthBlueTint * lightWaveLengthBlueTint * lightWaveLengthBlueTint;
            float lightInverseWaveLengthRed4 = 1.0f / lightWaveLengthRed4;
            float lightInverseWaveLengthGreen4 = 1.0f / lightWaveLengthGreen4;
            float lightInverseWaveLengthBlue4 = 1.0f / lightWaveLengthBlue4;
            const float sunBrightnessFactor = 40.0f;
            float sunRed = rayleighConstant * sunBrightnessFactor * lightInverseWaveLengthRed4;
            float sunGreen = rayleighConstant * sunBrightnessFactor * lightInverseWaveLengthGreen4;
            float sunBlue = rayleighConstant * sunBrightnessFactor * lightInverseWaveLengthBlue4;
            float sunIntensity = mieConstant * sunBrightnessFactor;
            float pi4Red = rayleighConstant * 4.0f * Mathf.PI * lightInverseWaveLengthRed4;
            float pi4Green = rayleighConstant * 4.0f * Mathf.PI * lightInverseWaveLengthGreen4;
            float pi4Blue = rayleighConstant * 4.0f * Mathf.PI * lightInverseWaveLengthBlue4;
            float pi4Intensity = mieConstant * 4.0f * Mathf.PI;
            float scaleFactor = 1.0f / (SkyOuterRadius - 1.0f);
            const float scaleDepth = 0.25f;
            float scaleOverScaleDepth = scaleFactor / scaleDepth;
            Shader.SetGlobalFloat("_WeatherMakerSkySamples", 3.0f);
            Shader.SetGlobalFloat("_WeatherMakerSkyMieG", mieG);
            Shader.SetGlobalFloat("_WeatherMakerSkyMieG2", mieG2);
            Shader.SetGlobalFloat("_WeatherMakerSkyAtmosphereThickness", SkyAtmosphereThickness);
            Shader.SetGlobalVector("_WeatherMakerSkyRadius", new Vector4(SkyOuterRadius, SkyOuterRadius * SkyOuterRadius, SkyInnerRadius, SkyInnerRadius * SkyInnerRadius));
            Shader.SetGlobalVector("_WeatherMakerSkyMie", new Vector4(1.5f * ((1.0f - mieG2) / (2.0f + mieG2)), 1.0f + mieG2, 2.0f + mieG, 0.0f));
            Shader.SetGlobalVector("_WeatherMakerSkyLightScattering", new Vector4(sunRed, sunGreen, sunBlue, sunIntensity));
            Shader.SetGlobalVector("_WeatherMakerSkyLightPIScattering", new Vector4(pi4Red, pi4Green, pi4Blue, pi4Intensity));
            Shader.SetGlobalVector("_WeatherMakerSkyScale", new Vector4(scaleFactor, scaleDepth, scaleOverScaleDepth, SkyCameraHeight));


#if DEBUG

            if (WeatherScript.CurrentCamera == null)
            {
                Debug.LogWarning("Sky sphere requires a camera be set on WeatherScript");
            }
            else

#endif

            {
                float radius = (WeatherScript.CurrentCamera.farClipPlane * WeatherScript.SkySphereScript.FarClipPlaneMultiplier) * 0.95f;
                Shader.SetGlobalFloat("_WeatherMakerSkySphereRadius", radius);
                Shader.SetGlobalFloat("_WeatherMakerSkySphereRadiusSquared", radius * radius);
                Shader.SetGlobalFloat("_WeatherMakerSkyDitherLevel", DitherLevel);
                Shader.SetGlobalFloat("_WeatherMakerCloudRayOffset", CloudRayOffset);
            }
        }

        internal void SetShaderCloudParameters(Material m)
        {
            m.SetTexture("_FogNoise", CloudNoise);
            m.SetColor("_FogColor", CloudColor);
            m.SetColor("_FogEmissionColor", CloudEmissionColor);
            m.SetVector("_FogNoiseVelocity", CloudNoiseVelocity);
            if (CloudNoiseMask == null)
            {
                m.EnableKeyword("ENABLE_CLOUDS");
            }
            else
            {
                m.EnableKeyword("ENABLE_CLOUDS_MASK");
                m.SetTexture("_FogNoiseMask", CloudNoiseMask);
                m.SetVector("_FogNoiseMaskOffset", CloudNoiseMaskOffset);
                m.SetVector("_FogNoiseMaskVelocity", CloudNoiseMaskVelocity);
                m.SetFloat("_FogNoiseMaskScale", CloudNoiseMaskScale * 0.01f);
                float rotRadians = CloudNoiseMaskRotation * Mathf.Deg2Rad;
                m.SetFloat("_FogNoiseMaskRotationSin", Mathf.Sin(rotRadians));
                m.SetFloat("_FogNoiseMaskRotationCos", Mathf.Cos(rotRadians));
            }
            m.SetFloat("_FogNoiseScale", CloudNoiseScale);
            m.SetFloat("_FogNoiseMultiplier", CloudNoiseMultiplier);
            m.SetFloat("_FogHeight", CloudHeight);
            m.SetFloat("_FogCover", CloudCover);
            m.SetFloat("_FogDensity", CloudDensity);
            m.SetFloat("_FogLightAbsorption", CloudLightAbsorption);
            m.SetFloat("_FogSharpness", CloudSharpness);
            m.SetFloat("_FogWhispiness", CloudWhispiness);
            m.SetFloat("_FogWhispinessChangeFactor", CloudWhispinessChangeFactor);
            m.SetFloat("_FogShadowThreshold", CloudShadowThreshold);
            m.SetFloat("_FogShadowPower", CloudShadowPower);
        }

        private void SetShaderLightParameters()
        {
            Color ambientLight = (DayAmbientColor * DayAmbientIntensity * WeatherScript.DayNightScript.DayMultiplier) +
                (DawnDuskAmbientColor * DawnDuskAmbientIntensity * WeatherScript.DayNightScript.DawnDuskMultiplier) +
                (NightAmbientColor * NightAmbientIntensity * WeatherScript.DayNightScript.NightMultiplier);

            RenderSettings.ambientLight = ambientLight;

            Material.SetFloat("_DayMultiplier", WeatherScript.DayNightScript.DayMultiplier);
            Material.SetFloat("_DawnDuskMultiplier", WeatherScript.DayNightScript.DawnDuskMultiplier);
            Material.SetFloat("_NightMultiplier", WeatherScript.DayNightScript.NightMultiplier);
            Material.SetFloat("_NightSkyMultiplier", Mathf.Max(1.0f - Mathf.Min(1.0f, SkyAtmosphereThickness), WeatherScript.DayNightScript.NightMultiplier));
            Material.SetFloat("_NightVisibilityThreshold", NightVisibilityThreshold);
            Material.SetFloat("_NightIntensity", NightIntensity);
            Material.DisableKeyword("ENABLE_CLOUDS");
            Material.DisableKeyword("ENABLE_CLOUDS_MASK");
            Material.DisableKeyword("ENABLE_NIGHT_TWINKLE");

            if (NightTwinkleRandomness > 0.0f || (NightTwinkleVariance > 0.0f && NightTwinkleSpeed > 0.0f))
            {
                Material.SetFloat("_NightTwinkleSpeed", NightTwinkleSpeed);
                Material.SetFloat("_NightTwinkleVariance", NightTwinkleVariance);
                Material.SetFloat("_NightTwinkleMinimum", NightTwinkleMinimum);
                Material.SetFloat("_NightTwinkleRandomness", NightTwinkleRandomness);
                Material.EnableKeyword("ENABLE_NIGHT_TWINKLE");
            }

            if (CloudsEnabled)
            {
                SetShaderCloudParameters(Material);

#if UNITY_EDITOR

                if (Application.isPlaying)
                {

#endif

                    float cover = CloudCover * (1.5f - CloudLightAbsorption);
                    float sunIntensityMultiplier = Mathf.Clamp(1.0f - (CloudDensity * 0.5f), 0.0f, 1.0f);
                    WeatherScript.DayNightScript.DirectionalLightIntensityMultipliers["WeatherMakerSkySphereScript"] = sunIntensityMultiplier;
                    float sunShadowMultiplier = Mathf.Lerp(1.0f, 0.0f, Mathf.Clamp(((CloudDensity + cover) * 0.85f), 0.0f, 1.0f));
                    WeatherScript.DayNightScript.DirectionalLightShadowIntensityMultipliers["WeatherMakerSkySphereScript"] = sunShadowMultiplier;

#if UNITY_EDITOR

                }

#endif

            }
            else
            {

#if UNITY_EDITOR

                if (Application.isPlaying)
                {

#endif

                    WeatherScript.DayNightScript.DirectionalLightIntensityMultipliers["WeatherMakerSkySphereScript"] = 1.0f;
                    WeatherScript.DayNightScript.DirectionalLightShadowIntensityMultipliers["WeatherMakerSkySphereScript"] = 1.0f;

#if UNITY_EDITOR

                }

#endif

            }
        }

        private void RaycastForEclipse()
        {

#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                return;
            }

            if (WeatherScript.CurrentCamera == null)
            {
                Debug.LogWarning("Sky sphere requires a camera be set on WeatherScript");
                return;
            }

#endif

            // disable allow eclipses everywhere by default
            float eclipsePower = 0.0f;
            foreach (WeatherMakerCelestialObject moon in WeatherScript.Moons)
            {
                moon.Renderer.sharedMaterial.DisableKeyword("ENABLE_SUN_ECLIPSE");
                if (moon.Collider != null)
                {
                    moon.Collider.enabled = CheckForEclipse;
                }
            }

            if (CheckForEclipse)
            {
                float sunRadius = Mathf.Lerp(0.0f, 1000.0f, Mathf.Pow(WeatherScript.Sun.Scale, 0.5f));
                Vector3 origin = WeatherScript.CurrentCamera.transform.position - (WeatherScript.Sun.Transform.forward * WeatherScript.CurrentCamera.farClipPlane * 1.7f);
                int hitCount = Physics.SphereCastNonAlloc(origin, sunRadius, WeatherScript.Sun.Transform.forward, eclipseHits, WeatherScript.CurrentCamera.farClipPlane);
                for (int i = 0; i < hitCount; i++)
                {
                    foreach (WeatherMakerCelestialObject moon in WeatherScript.Moons)
                    {
                        if (moon.Transform.gameObject == eclipseHits[i].collider.gameObject)
                        {
                            float dot = Mathf.Abs(Vector3.Dot(eclipseHits[i].normal, WeatherScript.Sun.Transform.forward));
                            eclipsePower += Mathf.Pow(dot, 256.0f);
                            moon.Renderer.sharedMaterial.EnableKeyword("ENABLE_SUN_ECLIPSE");
                            //Debug.LogFormat("Eclipse raycast normal: {0}, dot: {1}, power: {2}", eclipseHits[i].normal, dot, eclipsePower);
                            break;
                        }
                    }
                }
            }

            if (eclipsePower == 0.0f)
            {
                WeatherScript.DayNightScript.DirectionalLightIntensityMultipliers["WeatherMakerSkySphereScriptEclipse"] = 1.0f;
                Material.DisableKeyword("ENABLE_SUN_ECLIPSE");
            }
            else
            {
                float eclipseLightReducer = 1.0f - Mathf.Clamp(eclipsePower, 0.0f, 1.0f);
                WeatherScript.DayNightScript.DirectionalLightIntensityMultipliers["WeatherMakerSkySphereScriptEclipse"] = eclipseLightReducer;
                Material.EnableKeyword("ENABLE_SUN_ECLIPSE");
                Material.SetFloat("_NightSkyMultiplier", Mathf.Max(1.0f - Mathf.Min(1.0f, SkyAtmosphereThickness), eclipsePower));
            }
        }

        private void SetSkySphereScalesAndPositions()
        {

#if DEBUG

            if (WeatherScript.CurrentCamera == null)
            {
                return;
            }

#endif

            // adjust sky sphere position and scale
            float farPlane = WeatherScript.CurrentCamera.farClipPlane;
            Vector3 anchor = WeatherScript.CurrentCamera.transform.position;
            float yOffset = farPlane * YOffsetMultiplier;
            gameObject.transform.position = anchor + new Vector3(0.0f, yOffset, 0.0f);
            float scale = farPlane * FarClipPlaneMultiplier * ((WeatherScript.CurrentCamera.farClipPlane - Mathf.Abs(yOffset)) / WeatherScript.CurrentCamera.farClipPlane);
            float finalScale;
            gameObject.transform.localScale = new Vector3(scale, scale, scale);

            // move sun back near the far plane and scale appropriately
            scale = farPlane * Mathf.Min(0.425f, 9.0f * WeatherScript.Sun.Scale);
            Vector3 sunOffset = (WeatherScript.Sun.Transform.forward * ((farPlane * 0.9f) - scale));
            WeatherScript.Sun.Transform.position = anchor - sunOffset;
            WeatherScript.Sun.Transform.localScale = new Vector3(scale, scale, scale);

            // move moons back near the far plane and scale appropriately
            foreach (WeatherMakerCelestialObject moon in WeatherScript.Moons)
            {
                scale = farPlane * moon.Scale;
                finalScale = Mathf.Clamp(Mathf.Abs(moon.Transform.forward.y) * 3.0f, 0.8f, 1.0f);
                finalScale = scale / finalScale;
                Vector3 moonOffset = (moon.Transform.forward * ((farPlane * 0.9f) - finalScale));
                moon.Transform.position = anchor - moonOffset;
                moon.Transform.localScale = new Vector3(finalScale, finalScale, finalScale);
            }
        }

        private void UpdateSkySphere()
        {
            if (UnityEngine.RenderSettings.skybox != null)
            {
                Debug.LogError("Weather maker sky sphere requires that you set the Unity skybox material to null. Window -> Lighting -> Settings -> Skybox Material & Sun Source -> Set to None");
            }

            WeatherScript.CurrentCamera = (Camera.current == null ? WeatherScript.Camera : Camera.current);
            SetSkySphereScalesAndPositions();
            SetShaderSkyParameters();
            SetShaderLightParameters();
            RaycastForEclipse();
        }

        internal void PreCullCamera(Camera c)
        {
            UpdateSkySphere();
        }

        protected override void Start()
        {
            base.Start();
            UpdateSkySphere();
        }

        protected override void OnWillRenderObject()
        {
            base.OnWillRenderObject();

#if UNITY_EDITOR

            // during play mode, the precull event is used from WeatherScript
            if (!Application.isPlaying)
            {
                UpdateSkySphere();
            }

#endif

            // for now only render cloud shadows on the main camera set on the weather script
            // TODO: multiple camera with cloud shadow support
            if (Camera.current == WeatherScript.Camera)
            {
                CloudShadowProjectorScript.RenderCloudShadows();
            }
        }

        /// <summary>
        /// Enable / disable lens flare
        /// </summary>
        /// <param name="enable">Enable or disable</param>
        public void SetFlareEnabled(bool enable)
        {
            if (WeatherScript.Sun == null)
            {
                return;
            }
            LensFlare flare = WeatherScript.Sun.Transform.GetComponent<LensFlare>();
            if (flare == null)
            {
                return;
            }
            Color startColor = flare.color;
            Color endColor = (enable ? Color.white : Color.black);
            TweenFactory.Tween("WeatherMakerLensFlare", startColor, endColor, 3.0f, TweenScaleFunctions.Linear, (ITween<Color> c) =>
            {
                flare.color = c.CurrentValue;
            }, null);
        }

        /// <summary>
        /// Show cloud animated
        /// </summary>
        /// <param name="duration">How long until clouds fully transition to the parameters</param>
        /// <param name="cover">Cloud cover, 0 to 1</param>
        /// <param name="density">Cloud density, 0 to 1</param>
        /// <param name="whispiness">Cloud whispiness, 0 to 3</param>
        /// <param name="color">Cloud color, if null defaults to white</param>
        public void ShowCloudsAnimated(float duration, float cover, float density = 0.0f, float whispiness = 0.3f, float sharpness = 0.015f, Color? color = null)
        {
            SetFlareEnabled(cover < 0.25f);
            if (!color.HasValue)
            {
                color = CloudColor;
            }
            float startCover = CloudCover;
            float startDensity = CloudDensity;
            float startWhispiness = CloudWhispiness;
            float startSharpness = CloudSharpness;
            Color startColor = CloudColor;
            TweenFactory.Tween("WeatherMakerClouds", 0.0f, 1.0f, duration, TweenScaleFunctions.Linear, (ITween<float> c) =>
            {
                CloudCover = Mathf.Lerp(startCover, cover, c.CurrentValue);
                CloudDensity = Mathf.Lerp(startDensity, density, c.CurrentValue);
                CloudWhispiness = Mathf.Lerp(startWhispiness, whispiness, c.CurrentValue);
                CloudColor = Color.Lerp(startColor, color.Value, c.CurrentValue);
                CloudSharpness = Mathf.Lerp(startSharpness, sharpness, c.CurrentValue);
            }, null);
        }

        public void HideCloudsAnimated(float duration)
        {
            SetFlareEnabled(true);
            float cover = CloudCover;
            float density = CloudDensity;
            TweenFactory.Tween("WeatherMakerClouds", 1.0f, 0.0f, duration, TweenScaleFunctions.Linear, (ITween<float> c) =>
            {
                CloudCover = c.CurrentValue * cover;
                CloudDensity = c.CurrentValue * density;
            }, null);
        }
    }
}