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
using System;

namespace DigitalRuby.WeatherMaker
{
    public enum WeatherMakerPrecipitationType
    {
        None = 0,
        Rain = 1,
        Snow = 2,
        Sleet = 3,
        Hail = 4,
        Custom = 127
    }

    public enum WeatherMakerCloudType
    {
        None = 0,
        Light = 1,
        Medium = 2,
        Heavy = 3,
        Storm = 4
    }

    public enum WeatherMakerOrbitType
    {
        /// <summary>
        /// Orbit as viewed from Earth
        /// </summary>
        FromEarth = 0
    }

    [System.Serializable]
    public class WeatherMakerCelestialObject
    {
        [Tooltip("The transform that is orbiting")]
        public Transform Transform;

        [Tooltip("The directional light used to emit light from the object")]
        public Light Light;

        [Tooltip("The renderer for the object, must not be null")]
        public Renderer Renderer;

        [Tooltip("The collider for the oject, can be null")]
        public Collider Collider;

        [Tooltip("Hint to have the object render in fast mode. Useful for mobile, but not all shaders support it.")]
        public bool RenderHintFast;

        [Tooltip("Rotation about y axis - changes how the celestial body orbits over the scene")]
        public float RotateYDegrees;

        [Tooltip("The orbit type. Only from Earth orbit is currently supported.")]
        public WeatherMakerOrbitType OrbitType;

        [Range(0.0f, 1.0f)]
        [Tooltip("The scale of the object. For the sun, this is shader specific. For moons, this is a percentage of camera far plane.")]
        public float Scale = 0.03f;

        [Range(0.0f, 128.0f)]
        [Tooltip("Light power, controls how intense the light lights up the clouds, etc. near the object. Lower values reduce the radius and increase the intensity.")]
        public float LightPower = 8.0f;

        [Range(0.0f, 3.0f)]
        [Tooltip("Light multiplier")]
        public float LightMultiplier = 1.0f;

        [Tooltip("Tint color of the object.")]
        public Color TintColor = Color.white;

        [Range(0.0f, 4.0f)]
        [Tooltip("Tint intensity")]
        public float TintIntensity = 1.0f;

        /// <summary>
        /// Whether the object is active
        /// </summary>
        public bool IsActive
        {
            get { return Transform.gameObject.activeInHierarchy && Scale > 0.0f; }
        }

        /// <summary>
        /// Whether the light for this object is active. A light that is not active is not on.
        /// </summary>
        public bool LightIsActive
        {
            get { return Light != null && Light.enabled && IsActive; }
        }

        /// <summary>
        /// Whether the light is on. An active light can have a light that is off.
        /// </summary>
        public bool LightIsOn
        {
            get { return LightIsActive && Light.intensity > 0.0f && LightMultiplier > 0.0001f && Light.color.r > 0.0001f && Light.color.g > 0.0001f && Light.color.b > 0.0001f; }
        }
    }

    [ExecuteInEditMode]
    public class WeatherMakerScript : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Camera the weather should render around. Defaults to main camera.")]
        public Camera Camera;

        [Tooltip("Additional cameras. Do not add the primary camera (i.e. the Camera property) to this array, only add extra cameras. These cameras will get all weather effects including sky sphere, rain, fog ,etc.")]
        public System.Collections.Generic.List<Camera> Cameras;

        /// <summary>
        /// The current camera (used by some scripts in OnWillRenderObject, PreCull, etc.)
        /// </summary>
        internal Camera CurrentCamera { get; set; }

        [Tooltip("Configuration script. Should be deactivated for release builds.")]
        public WeatherMakerConfigurationScript ConfigurationScript;

        [Range(0.0f, 1.0f)]
        [Tooltip("Change the volume of all weather maker sounds.")]
        public float VolumeModifier = 1.0f;

        [Tooltip("Whether per pixel lighting is enabled - currently precipitation mist is the only material that support this.")]
        public bool EnablePerPixelLighting;

        [Range(-100.0f, 150.0f)]
        [Tooltip("Temperature, not used yet")]
        public float Temperature = 70.0f;

        [Range(0.0f, 1.0f)]
        [Tooltip("Humidity, not used yet")]
        public float Humidity = 0.1f;

        [Header("Celestial Objects")]
        [Tooltip("All suns in the scene - must have at least one. *Only one sun is currently supported*.")]
        public WeatherMakerCelestialObject[] Suns;

        [Tooltip("All moons in the scene - must have at least one. Only the first moon will cast it's directional light currently. All items must not be null.")]
        public WeatherMakerCelestialObject[] Moons;

        private WeatherMakerPrecipitationType precipitation = WeatherMakerPrecipitationType.None;
        [Header("Precipitation")]
        [Tooltip("Current precipitation")]
        public WeatherMakerPrecipitationType Precipitation = WeatherMakerPrecipitationType.None;

        [Tooltip("Intensity of precipitation (0-1)")]
        [Range(0.0f, 1.0f)]
        public float PrecipitationIntensity;

        [Tooltip("How long in seconds to fully change from one precipitation type to another")]
        [Range(0.0f, 300.0f)]
        public float PrecipitationChangeDuration = 4.0f;

        [Tooltip("The threshold change in intensity that will cause a cross-fade between precipitation changes. Intensity changes smaller than this value happen quickly.")]
        [Range(0.0f, 0.2f)]
        public float PrecipitationChangeThreshold = 0.1f;

        [Tooltip("Whether the precipitation collides with the world. This can be a performance problem on lower end devices. Please be careful.")]
        public bool PrecipitationCollisionEnabled;

        private WeatherMakerCloudType clouds = WeatherMakerCloudType.None;
        [Header("Clouds")]
        [Tooltip("Cloud type. In 2D, clouds are either none or any other option will be storm.")]
        public WeatherMakerCloudType Clouds = WeatherMakerCloudType.None;

        [Tooltip("How long in seconds to fully change from one cloud type to another, does not apply in 2D")]
        [Range(0.0f, 300.0f)]
        public float CloudChangeDuration = 4.0f;

        [Header("Dependencies")]
        [Tooltip("Rain script")]
        public WeatherMakerFallingParticleScript RainScript;

        [Tooltip("Snow script")]
        public WeatherMakerFallingParticleScript SnowScript;

        [Tooltip("Hail script")]
        public WeatherMakerFallingParticleScript HailScript;

        [Tooltip("Sleet script")]
        public WeatherMakerFallingParticleScript SleetScript;

        [Tooltip("Set a custom precipitation script for use with Precipitation = WeatherMakerPrecipitationType.Custom ")]
        public WeatherMakerFallingParticleScript CustomPrecipitationScript;

        [Tooltip("Wind script")]
        public WeatherMakerWindScript WindScript;

        [Tooltip("Day night script")]
        public WeatherMakerDayNightCycleScript DayNightScript;

        [Tooltip("Sky sphere script, null if none")]
        public WeatherMakerSkySphereScript SkySphereScript;

        [Tooltip("Fog script, null if none")]
        public WeatherMakerFullScreenFogScript FogScript;

        [Tooltip("Lightning script (random bolts)")]
        public WeatherMakerThunderAndLightningScript LightningScript;

        [Tooltip("Lightning bolt script")]
        public WeatherMakerLightningBoltScript LightningBoltScript;

        [Tooltip("Custom cloud script, null if none. Currently only used in 2D.")]
        public WeatherMakerCloudScript CloudScript;

        [Tooltip("Light manager, 3D only.")]
        public WeatherMakerLightManagerScript LightManagerScript;

        [Tooltip("A list of all the weather managers. Only one object should be active at a time. Use the ActivateWeatherManager method to switch managers.")]
        public System.Collections.Generic.List<WeatherMakerWeatherManagerScript> WeatherManagers;

        /// <summary>
        /// The current precipitation script - use Precipitation to change precipitation
        /// </summary>
        public WeatherMakerFallingParticleScript PrecipitationScript { get; private set; }

        /// <summary>
        /// Get / set the intensity of the wind. Simply a shortcut to WindScript.WindIntensity.
        /// </summary>
        public float WindIntensity { get { return WindScript.WindIntensity; } set { WindScript.WindIntensity = value; } }

        /// <summary>
        /// Gets the current time of day in seconds. 86400 seconds per day.
        /// </summary>
        public float TimeOfDay { get { return DayNightScript.TimeOfDay; } set { DayNightScript.TimeOfDay = value; } }

        /// <summary>
        /// Returns the first object in Suns
        /// </summary>
        public WeatherMakerCelestialObject Sun { get { return Suns[0]; } }

        /// <summary>
        /// Returns the first object in Moons
        /// </summary>
        public WeatherMakerCelestialObject Moon { get { return (Moons.Length == 0 ? null : Moons[0]); } }

        /// <summary>
        /// Returns whether the camera is orthographic
        /// </summary>
        public bool CameraIsOrthographic { get { return Camera != null && Camera.orthographic; } }

        /// <summary>
        /// The planes of the current camera view frustum
        /// </summary>
        public Plane[] CurrentCameraFrustumPlanes { get; private set; }

        /// <summary>
        /// Singleton
        /// </summary>
        public static WeatherMakerScript Instance { get; private set; }

        [NonSerialized]
        private float lastPrecipitationIntensityChange = -1.0f;

        [NonSerialized]
        private float lastVolumeModifier = -1.0f;

        /// <summary>
        /// Max number of moons supported. This should match the constant in WeatherMakerShader.cginc.
        /// </summary>
        public const int MaxMoonCount = 8;

        private readonly Vector4[] moonDirectionUpShaderBuffer = new Vector4[MaxMoonCount];
        private readonly Vector4[] moonDirectionDownShaderBuffer = new Vector4[MaxMoonCount];
        private readonly Vector4[] moonLightColorShaderBuffer = new Vector4[MaxMoonCount];
        private readonly Vector4[] moonLightPowerShaderBuffer = new Vector4[MaxMoonCount];
        private readonly Vector4[] moonTintColorShaderBuffer = new Vector4[MaxMoonCount];
        private readonly Vector4[] moonVar1ShaderBuffer = new Vector4[MaxMoonCount];

        /// <summary>
        /// Swap two items in a list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">List</param>
        /// <param name="indexA">First index</param>
        /// <param name="indexB">Second index</param>
        public static void Swap<T>(System.Collections.Generic.IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        /// <summary>
        /// Turn off all weather managers
        /// </summary>
        public void DeactivateWeatherManagers()
        {
            if (WeatherManagers != null)
            {
                foreach (WeatherMakerWeatherManagerScript manager in WeatherManagers)
                {
                    manager.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Activate the nth weather manager in the WeatherManagers list, deactivating the current weather manager and activating the selected weather manager
        /// </summary>
        /// <param name="index">Index of the new weather manager to activate</param>
        /// <returns>True if success, false if index out of bounds</returns>
        public bool ActivateWeatherManager(int index)
        {
            if (WeatherManagers != null && index < WeatherManagers.Count)
            {
                DeactivateWeatherManagers();
                WeatherManagers[index].gameObject.SetActive(true);
                return true;
            }
            return false;
        }

        private void TweenScript(WeatherMakerFallingParticleScript script, float end)
        {
            if (PrecipitationChangeDuration < 0.1f)
            {
                script.Intensity = end;
                return;
            }

            float duration = (Mathf.Abs(script.Intensity - end) < PrecipitationChangeThreshold ? 0.0f : PrecipitationChangeDuration);
            TweenFactory.Tween("WeatherMakerPrecipitationChange_" + script.gameObject.GetInstanceID(), script.Intensity, end, duration, TweenScaleFunctions.Linear, (t) =>
            {
                // Debug.LogFormat("Tween key: {0}, value: {1}, prog: {2}", t.Key, t.CurrentValue, t.CurrentProgress);
                script.Intensity = t.CurrentValue;
            }, null);
        }

        private void ChangePrecipitation(WeatherMakerFallingParticleScript newPrecipitation)
        {
            if (newPrecipitation != PrecipitationScript && PrecipitationScript != null)
            {
                TweenScript(PrecipitationScript, 0.0f);
                lastPrecipitationIntensityChange = -1.0f;
            }
            PrecipitationScript = newPrecipitation;
        }

        private void UpdateCollision()
        {
            RainScript.CollisionEnabled = PrecipitationCollisionEnabled;
            SnowScript.CollisionEnabled = PrecipitationCollisionEnabled;
            HailScript.CollisionEnabled = PrecipitationCollisionEnabled;
            SleetScript.CollisionEnabled = PrecipitationCollisionEnabled;
        }

        private void UpdateSoundsVolumes()
        {
            LightningScript.VolumeModifier = VolumeModifier;
            RainScript.AudioSourceLight.VolumeModifier = RainScript.AudioSourceMedium.VolumeModifier = RainScript.AudioSourceHeavy.VolumeModifier = VolumeModifier;
            SnowScript.AudioSourceLight.VolumeModifier = SnowScript.AudioSourceMedium.VolumeModifier = SnowScript.AudioSourceHeavy.VolumeModifier = VolumeModifier;
            HailScript.AudioSourceLight.VolumeModifier = HailScript.AudioSourceMedium.VolumeModifier = HailScript.AudioSourceHeavy.VolumeModifier = VolumeModifier;
            SleetScript.AudioSourceLight.VolumeModifier = SleetScript.AudioSourceMedium.VolumeModifier = SleetScript.AudioSourceHeavy.VolumeModifier = VolumeModifier;
            WindScript.AudioSourceWind.VolumeModifier = VolumeModifier;
        }

        private void SetEnablePerPixelLighting()
        {

#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                return;
            }

#endif

            if (EnablePerPixelLighting && SystemInfo.graphicsShaderLevel >= 30)
            {
                Shader.EnableKeyword("WEATHER_MAKER_PER_PIXEL_LIGHTING");
            }
            else
            {
                Shader.DisableKeyword("WEATHER_MAKER_PER_PIXEL_LIGHTING");
            }
        }

        private void SetGlobalShaders()
        {
            // Sun
            Shader.SetGlobalVector("_WeatherMakerSunDirectionUp", -Sun.Transform.forward);
            Shader.SetGlobalVector("_WeatherMakerSunDirectionDown", Sun.Transform.forward);
            Shader.SetGlobalVector("_WeatherMakerSunPositionNormalized", Sun.Transform.position.normalized);
            Shader.SetGlobalVector("_WeatherMakerSunPositionWorldSpace", Sun.Transform.position);
            Vector4 sunColor = new Vector4(Sun.Light.color.r, Sun.Light.color.g, Sun.Light.color.b, Sun.Light.intensity);
            Shader.SetGlobalVector("_WeatherMakerSunColor", sunColor);
            sunColor = new Vector4(Sun.TintColor.r, Sun.TintColor.g, Sun.TintColor.b, Sun.TintColor.a * Sun.TintIntensity * Sun.Light.intensity);
            Shader.SetGlobalVector("_WeatherMakerSunTintColor", sunColor);
            float sunHorizonScaleMultiplier = Mathf.Clamp(Mathf.Abs(Sun.Transform.forward.y) * 3.0f, 0.5f, 1.0f);
            sunHorizonScaleMultiplier = Mathf.Min(1.0f, Sun.Scale / sunHorizonScaleMultiplier);
            Shader.SetGlobalVector("_WeatherMakerSunLightPower", new Vector4(Sun.LightPower, Sun.LightMultiplier, Sun.Light.shadowStrength, 0.0f));
            Shader.SetGlobalVector("_WeatherMakerSunVar1", new Vector4(sunHorizonScaleMultiplier, 0.0f, 0.0f, 0.0f));

            if (Sun.Renderer != null)
            {
                if (Sun.RenderHintFast)
                {
                    Sun.Renderer.sharedMaterial.EnableKeyword("RENDER_HINT_FAST");
                }
                else
                {
                    Sun.Renderer.sharedMaterial.DisableKeyword("RENDER_HINT_FAST");
                }
            }

            // Moons
            Shader.SetGlobalInt("_WeatherMakerMoonCount", Moons.Length);
            for (int i = 0; i < Moons.Length; i++)
            {
                WeatherMakerCelestialObject moon = Moons[i];
                moon.Renderer.sharedMaterial.SetFloat("_MoonIndex", i);
                moonDirectionUpShaderBuffer[i] = -moon.Transform.forward;
                moonDirectionDownShaderBuffer[i] = moon.Transform.forward;
                moonLightColorShaderBuffer[i] = (moon.LightIsOn ? new Vector4(moon.Light.color.r, moon.Light.color.g, moon.Light.color.b, moon.Light.intensity) : Vector4.zero);
                moonLightPowerShaderBuffer[i] = new Vector4(moon.LightPower, moon.LightMultiplier, moon.Light.shadowStrength, 0.0f);
                moonTintColorShaderBuffer[i] = new Vector4(moon.TintColor.r, moon.TintColor.g, moon.TintColor.b, moon.TintColor.a * moon.TintIntensity);
                moonVar1ShaderBuffer[i] = new Vector4(moon.Scale, 0.0f, 0.0f, 0.0f);
            }

            Shader.SetGlobalVectorArray(WeatherMakerShaderIds.ArrayWeatherMakerMoonDirectionUp, moonDirectionUpShaderBuffer);
            Shader.SetGlobalVectorArray(WeatherMakerShaderIds.ArrayWeatherMakerMoonDirectionDown, moonDirectionDownShaderBuffer);
            Shader.SetGlobalVectorArray(WeatherMakerShaderIds.ArrayWeatherMakerMoonLightColor, moonLightColorShaderBuffer);
            Shader.SetGlobalVectorArray(WeatherMakerShaderIds.ArrayWeatherMakerMoonLightPower, moonLightPowerShaderBuffer);
            Shader.SetGlobalVectorArray(WeatherMakerShaderIds.ArrayWeatherMakerMoonTintColor, moonTintColorShaderBuffer);
            Shader.SetGlobalVectorArray(WeatherMakerShaderIds.ArrayWeatherMakerMoonVar1, moonVar1ShaderBuffer);
        }

        private void CameraPreCull(Camera c)
        {
            if ((c.depthTextureMode & DepthTextureMode.Depth) == DepthTextureMode.None)
            {
                c.depthTextureMode |= DepthTextureMode.Depth;
            }

            CurrentCamera = c;
            CurrentCameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(c);
            RainScript.PreCullCamera(c);
            SnowScript.PreCullCamera(c);
            HailScript.PreCullCamera(c);
            SleetScript.PreCullCamera(c);
            if (SkySphereScript != null)
            {
                SkySphereScript.PreCullCamera(c);
            }
            if (LightManagerScript != null)
            {
                LightManagerScript.UpdateLights();
            }
        }

        private void UpdateShaders()
        {
            SetEnablePerPixelLighting();
            SetGlobalShaders();
        }

        private void UpdateCameras()
        {
            Camera = (Camera == null ? (Camera.main == null ? Camera.current : Camera.main) : Camera);

#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                return;
            }

#endif

            if (!CameraIsOrthographic)
            {
                foreach (Camera c in Camera.allCameras)
                {
                    if (c.GetComponent<WeatherMakerCameraPreCullScript>() == null && (c == Camera || Cameras.Contains(c)))
                    {
                        WeatherMakerCameraPreCullScript script = c.gameObject.AddComponent<WeatherMakerCameraPreCullScript>();
                        script.hideFlags = HideFlags.HideAndDontSave;
                        script.PreCull += CameraPreCull;
                    }
                }
            }
        }

        private void UpdateClouds()
        {
            if (clouds == Clouds)
            {
                return;
            }
            clouds = Clouds;
            if (Camera.orthographic)
            {
                if (clouds == WeatherMakerCloudType.None)
                {
                    CloudScript.RemoveClouds();
                }
                else
                {
                    CloudScript.CreateClouds();
                }
            }
            else if (clouds == WeatherMakerCloudType.None)
            {
                // no clouds
                SkySphereScript.HideCloudsAnimated(CloudChangeDuration);
            }
            else if (clouds == WeatherMakerCloudType.Light)
            {
                // light clouds - all done in sky sphere shader
                SkySphereScript.ShowCloudsAnimated(CloudChangeDuration, 0.17f);
            }
            else if (clouds == WeatherMakerCloudType.Medium)
            {
                // medium clouds - all done in sky sphere shader
                SkySphereScript.ShowCloudsAnimated(CloudChangeDuration, 0.32f);
            }
            else if (clouds == WeatherMakerCloudType.Heavy)
            {
                // heavy clouds - all done in sky sphere shader
                SkySphereScript.ShowCloudsAnimated(CloudChangeDuration, 0.65f);
            }
            else
            {
                // storm clouds - all done in sky sphere shader
                SkySphereScript.ShowCloudsAnimated(CloudChangeDuration, 1.0f, 0.8f);
            }
        }

        private void CheckParticleVariables(WeatherMakerFallingParticleScript script)
        {
            if (script == null || script.AudioSourceLight == null || script.AudioSourceMedium == null || script.AudioSourceHeavy == null)
            {
                Debug.LogErrorFormat("{0} script and/or audio are null", script.gameObject);
            }
        }

        private void CheckVariables()
        {

#if UNITY_EDITOR

            if (Camera == null)
            {
                Debug.LogWarning("Must assign a camera for weather maker to work properly. Tag a camera as main camera, or manually assign the camera property.");
            }
            CheckParticleVariables(RainScript);
            CheckParticleVariables(HailScript);
            CheckParticleVariables(SnowScript);
            CheckParticleVariables(SleetScript);

#endif

            if (Precipitation != precipitation)
            {
                precipitation = Precipitation;
                switch (precipitation)
                {
                    default:
                        ChangePrecipitation(null);
						//VegetableMainMessange.isRain = false;
                        break;

					case WeatherMakerPrecipitationType.Rain:
						ChangePrecipitation (RainScript);
						//VegetableMainMessange.isRain = true;
                        break;

                    case WeatherMakerPrecipitationType.Snow:
                        ChangePrecipitation(SnowScript);
                        break;

                    case WeatherMakerPrecipitationType.Hail:
                        ChangePrecipitation(HailScript);
                        break;

                    case WeatherMakerPrecipitationType.Sleet:
                        ChangePrecipitation(SleetScript);
                        break;

                    case WeatherMakerPrecipitationType.Custom:
                        ChangePrecipitation(CustomPrecipitationScript);
                        break;
                }
            }

        }

        private void SetupReferences()
        {
            Instance = this;
            UpdateCollision();
            WeatherMakerShaderIds.Initialize();
        }

        private void Awake()
        {
            SetupReferences();
            Camera = (Camera == null ? (Camera.main == null ? Camera.current : Camera.main) : Camera);
        }

        private void Start()
        {

#if UNITY_EDITOR

            if (Application.isPlaying)
            {

#endif

                if (WeatherMakerLightManagerScript.Instance != null)
                {
                    // wire up lightning bolt lights to the light manager
                    LightningBoltScript.LightAddedCallback = LightAdded;
                    LightningBoltScript.LightRemovedCallback = LightRemoved;
                }

#if UNITY_EDITOR

            }

#endif

            UpdateCameras();
        }

        private void Update()
        {

#if UNITY_EDITOR

            if (transform.position != Vector3.zero || transform.localScale != Vector3.one || transform.rotation != Quaternion.identity)
            {
                Debug.LogError("For correct rendering, weather maker prefab should have position and rotation of 0, and scale of 1.");
            }

            if (Application.isPlaying)
            {

#endif

                CheckVariables();
                if (PrecipitationScript != null && PrecipitationIntensity != lastPrecipitationIntensityChange)
                {
                    lastPrecipitationIntensityChange = PrecipitationIntensity;
                    TweenScript(PrecipitationScript, PrecipitationIntensity);
                }
                if (VolumeModifier != lastVolumeModifier)
                {
                    lastVolumeModifier = VolumeModifier;
                    UpdateSoundsVolumes();
                }
                UpdateCollision();
                UpdateClouds();

#if UNITY_EDITOR

            }

#endif

            UpdateShaders();
            UpdateCameras();
        }

        private void OnDestroy()
        {
            if (Camera == null || !Camera.orthographic)
            {
                foreach (Camera c in Camera.allCameras)
                {
                    WeatherMakerCameraPreCullScript script = c.GetComponent<WeatherMakerCameraPreCullScript>();
                    if (script != null)
                    {
                        script.PreCull -= CameraPreCull;
                    }
                }
            }
        }

        private void LightAdded(Light l)
        {
            WeatherMakerLightManagerScript.Instance.AddLight(l);
        }

        private void LightRemoved(Light l)
        {
            WeatherMakerLightManagerScript.Instance.RemoveLight(l);
        }
    }

    public class WeatherMakerCameraPreCullScript : MonoBehaviour
    {
        private Camera Camera;

        private void Start()
        {
            Camera = GetComponent<Camera>();
        }

        private void OnPreCull()
        {
            if (PreCull != null)
            {
                PreCull.Invoke(Camera);
            }
        }

        public event System.Action<Camera> PreCull;
    }
}
