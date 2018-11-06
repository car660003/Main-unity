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
    public class WeatherMakerWindScript : MonoBehaviour
    {
        [Tooltip("Weather maker script")]
        public WeatherMakerScript WeatherScript;

        [Tooltip("Wind intensity (0 - 1). MaximumWindSpeed * WindIntensity = WindSpeed.")]
        [Range(0.0f, 1.0f)]
        public float WindIntensity = 0.0f;

        [Tooltip("The absolute maximum of the wind speed. The wind zone wind main is set to WindIntensity * MaximumWindSpeed * WindMainMultiplier.")]
        [Range(0.0f, 1000.0f)]
        public float MaximumWindSpeed = 100.0f;

        [Tooltip("Multiply the wind zone wind main by this value.")]
        [Range(0.0f, 1.0f)]
        public float WindMainMultiplier = 0.01f;

        [SingleLine("Wind turbulence range - set to a maximum 0 for no random turbulence.")]
        public RangeOfFloats WindTurbulenceRange = new RangeOfFloats { Minimum = 0.0f, Maximum = 100.0f };

        [SingleLine("Wind pulse magnitude range - set to a maximum of 0 for no random pulse magnitude.")]
        public RangeOfFloats WindPulseMagnitudeRange = new RangeOfFloats { Minimum = 2.0f, Maximum = 8.0f };

        [SingleLine("Wind pulse frequency range - set to a maximum of 0 for no random pulse frequency.")]
        public RangeOfFloats WindPulseFrequencyRange = new RangeOfFloats { Minimum = 0.01f, Maximum = 0.1f };

        [Tooltip("Get or set the wind direction. If RandomWindDirection is true, this will periodically change to a random value.")]
        public Vector3 WindDirection;
        private Vector3 lastWindDirection = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        [Tooltip("Whether random wind can blow upwards. Default is false.")]
        public bool AllowBlowUp = false;

        [Tooltip("Whether to randomize the wind direction.")]
        public bool RandomWindDirection = true;

        [Tooltip("Additional sound volume multiplier for the wind")]
        [Range(0.0f, 2.0f)]
        public float WindSoundMultiplier = 0.5f;

        [SingleLine("How often the wind speed and direction changes (minimum and maximum change interval in seconds). Set to 0 for no change.")]
        public RangeOfFloats WindChangeInterval = new RangeOfFloats { Minimum = 0.0f, Maximum = 30.0f };

        [Tooltip("How much the wind affects fog velocity")]
        [Range(0.0f, 1.0f)]
        public float FogVelocityMultiplier = 0.001f;

        /// <summary>
        /// The current wind velocity, not including turbulence and pulsing
        /// </summary>
        public Vector3 CurrentWindVelocity { get; private set; }

        /// <summary>
        /// Wind zone
        /// </summary>
        public WindZone WindZone { get; private set; }

        /// <summary>
        /// Wind audio source
        /// </summary>
        public LoopingAudioSource AudioSourceWind { get; private set; }

        /// <summary>
        /// Allow notification of when the wind velocity changes
        /// </summary>
        public System.Action<Vector3> WindChanged { get; set; }

        private float nextWindTime;

        private void Awake()
        {
            WindZone = GetComponent<WindZone>();
            AudioSourceWind = new LoopingAudioSource(GetComponent<AudioSource>());
        }

        private void UpdateWind()
        {
            if (WindIntensity > 0.0f)
            {
                WindZone.windMain = MaximumWindSpeed * WindIntensity * WindMainMultiplier;
                if (WindTurbulenceRange.Maximum > 0.0f || WindPulseMagnitudeRange.Maximum > 0.0f ||
                    WindPulseFrequencyRange.Maximum > 0.0f || WindDirection != lastWindDirection)
                {
                    lastWindDirection = WindDirection = WindDirection.normalized;
                    if (WeatherScript.Camera != null)
                    {
                        WindZone.transform.position = WeatherScript.Camera.transform.position;
                        if (!WeatherScript.Camera.orthographic)
                        {
                            WindZone.transform.Translate(0.0f, WindZone.radius, 0.0f);
                        }
                    }
                    if (nextWindTime < Time.time)
                    {
                        if (WindTurbulenceRange.Maximum > 0.0f)
                        {
                            WindZone.windTurbulence = WindTurbulenceRange.Random();
                        }
                        if (WindPulseMagnitudeRange.Maximum > 0.0f)
                        {
                            WindZone.windPulseMagnitude = WindPulseMagnitudeRange.Random();
                        }
                        if (WindPulseFrequencyRange.Maximum > 0.0f)
                        {
                            WindZone.windPulseFrequency = WindPulseFrequencyRange.Random();
                        }
                        if (RandomWindDirection)
                        {
                            if (WeatherScript.Camera != null && WeatherScript.Camera.orthographic)
                            {
                                int val = UnityEngine.Random.Range(0, 2);
                                WindZone.transform.rotation = Quaternion.Euler(0.0f, (val == 0 ? 90.0f : -90.0f), 0.0f);
                            }
                            else
                            {
                                float xAxis = (AllowBlowUp ? UnityEngine.Random.Range(-30.0f, 30.0f) : 0.0f);
                                WindZone.transform.rotation = Quaternion.Euler(xAxis, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
                            }
                            WindDirection = WindZone.transform.forward;
                        }
                        else if (WeatherScript.Camera.orthographic)
                        {
                            WindZone.transform.right = new Vector3(WindDirection.x, WindDirection.y, 0.0f);
                        }
                        else
                        {
                            WindZone.transform.forward = WindDirection;
                        }
                        nextWindTime = Time.time + WindChangeInterval.Random();
                    }
                }
                AudioSourceWind.Play(WindIntensity * WindSoundMultiplier);
                Vector3 newVelocity = WindDirection * WindZone.windMain;
                if (newVelocity != CurrentWindVelocity)
                {
                    if (FogVelocityMultiplier != 0.0f && WeatherScript.FogScript != null)
                    {
                        Vector3 fogVelocityOld = WeatherScript.FogScript.FogNoiseVelocity;
                        Vector3 fogVelocity = new Vector3(newVelocity.x * FogVelocityMultiplier, newVelocity.z * FogVelocityMultiplier, 0.0f);
                        TweenFactory.Tween("WeatherMakerWindScriptFogVelocity", fogVelocityOld, fogVelocity, (nextWindTime - Time.time) * 1.1f, TweenScaleFunctions.Linear, (p) =>
                        {
                            WeatherScript.FogScript.FogNoiseVelocity = p.CurrentValue;
                        }, null);
                    }
                    CurrentWindVelocity = newVelocity;
                    if (WindChanged != null)
                    {
                        WindChanged(newVelocity);
                    }
                }
            }
            else
            {
                AudioSourceWind.Stop();
                WindZone.windMain = WindZone.windTurbulence = WindZone.windPulseFrequency = WindZone.windPulseMagnitude = 0.0f;
                CurrentWindVelocity = Vector3.zero;
            }
            AudioSourceWind.Update();
        }

        private void Update()
        {
            UpdateWind();
        }

        /// <summary>
        /// Animate wind intensity change
        /// </summary>
        /// <param name="value">New intensity</param>
        /// <param name="duration">Animation duration</param>
        public void AnimateWindIntensity(float value, float duration)
        {
            TweenFactory.Tween("WeatherMakerWindIntensity", WindIntensity, value, duration, TweenScaleFunctions.Linear, (t) => WindIntensity = t.CurrentValue, null);
        }
    }
}