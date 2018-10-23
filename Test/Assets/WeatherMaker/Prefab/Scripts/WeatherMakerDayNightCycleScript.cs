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

using System;
using System.Collections.Generic;

using UnityEngine;

namespace DigitalRuby.WeatherMaker
{
    [ExecuteInEditMode]
    public class WeatherMakerDayNightCycleScript : MonoBehaviour
    {
        #region Classes

        public class SunInfo
        {
            /// <summary>
            /// Calculation parameter, the date/time on the observer planet
            /// </summary>
            public DateTime DateTime;

            /// <summary>
            /// Calculation parameter, latitudeof observer planet in degrees
            /// </summary>
            public double Latitude;

            /// <summary>
            /// Calculation parameter, longitude of observer planet in degrees
            /// </summary>
            public double Longitude;

            /// <summary>
            /// Calculation parameter, axis tilt of observer planet in degrees
            /// </summary>
            public double AxisTilt;

            /// <summary>
            /// Position (unit vector) of the sun in the sky from origin
            /// </summary>
            public Vector3 UnitVectorUp;

            /// <summary>
            /// Normal (unit vector) of the sun in the sky pointing to origin (negation of Position)
            /// </summary>
            public Vector3 UnitVectorDown;

            // the rest of these are stored in case needed and are best understood by a Google or Bing search
            public double JulianDays;
            public double Declination;
            public double RightAscension;
            public double Azimuth;
            public double Altitude;
            public double SolarMeanAnomaly;
            public double EclipticLongitude;
            public double SiderealTime;
        }

        public class MoonInfo
        {
            /// <summary>
            /// The sun data used to calculate the moon info
            /// </summary>
            public SunInfo SunData;

            /// <summary>
            /// Position (unit vector) of the moon in the sky from origin
            /// </summary>
            public Vector3 UnitVectorUp;

            /// <summary>
            /// Normal (unit vector) of the moon in the sky pointing to origin (negation of Position)
            /// </summary>
            public Vector3 UnitVectorDown;

            /// <summary>
            /// Distance in kilometers
            /// </summary>
            public double Distance;

            /// <summary>
            /// Moon illumination phase (0.5 is full, 0.0 to 1.0)
            /// </summary>
            public double Phase;

            /// <summary>
            /// Percent (0 to 1) that moon is full
            /// </summary>
            public double PercentFull;

            /// <summary>
            /// Moon illumination angle
            /// </summary>
            public double Angle;

            /// <summary>
            /// Moon illumination fraction
            /// </summary>
            public double Fraction;

            // the rest of these are stored in case needed and are best understood by a Google or Bing search
            public double Azimuth;
            public double Altitude;
            public double RightAscension;
            public double Declination;
            public double LunarMeanAnomaly;
            public double EclipticLongitude;
            public double SiderealTime;
            public double ParallacticAngle;
        }

        #endregion Classes

        #region Public fields

        [Tooltip("Weather Maker Script")]
        public WeatherMakerScript WeatherScript;

        [Header("Day/Night Cycle")]
        [Range(-100000, 100000.0f)]
        [Tooltip("The day speed of the cycle. Set to 0 to freeze the cycle and manually control it. At a speed of 1, the cycle is in real-time. " +
            "A speed of 100 is 100 times faster than normal. Negative numbers run the cycle backwards.")]
        public float Speed = 10.0f;

        [Range(-100000, 100000.0f)]
        [Tooltip("The night speed of the cycle. Set to 0 to freeze the cycle and manually control it. At a speed of 1, the cycle is in real-time. " +
            "A speed of 100 is 100 times faster than normal. Negative numbers run the cycle backwards.")]
        public float NightSpeed = 10.0f;

        [Range(0.0f, SecondsPerDay)]
        [Tooltip("The current time of day in seconds (local time).")]
        public float TimeOfDay = SecondsPerDay * 0.5f; // high noon default time of day

        [Header("Date")]
        [Tooltip("The year for simulating the sun and moon position - this can change during runtime. " +
            "The calculation is only correct for dates in the range March 1 1900 to February 28 2100.")]
        public int Year = 2000;

        [Tooltip("The month for simulating the sun and moon position - this can change during runtime.")]
        public int Month = 9;

        [Tooltip("The day for simulating the sun and moon position - this can change during runtime.")]
        public int Day = 21;

        [Tooltip("Whether to adjust the date when the day ends. This is important to maintain accurate sun and moon positions as days begin and end, but if your time is static you can turn it off.")]
        public bool AdjustDateWhenDayEnds = true;

        [Tooltip("Offset for the time zone of the lat / lon in seconds. You must calculate this based on the lat/lon you are providing and the year/month/day.")]
        public int TimeZoneOffsetSeconds = 21600;

        [Header("Location")]
        [Range(-90.0f, 90.0f)]
        [Tooltip("The latitude in degrees on the planet that the camera is at - 90 (north pole) to -90 (south pole)")]
        public double Latitude = 40.7608; // salt lake city latitude

        [Range(-180.0f, 180.0f)]
        [Tooltip("The longitude in degrees on the planet that the camera is at. -180 to 180.")]
        public double Longitude = -111.8910; // salt lake city longitude

        [Range(0.0f, 360.0f)]
        [Tooltip("The amount of degrees your planet is tilted - Earth is about 23.439f")]
        public float AxisTilt = 23.439f;

        [Header("Fade from day to dawn/dusk to night")]
        [Range(-1.0f, 1.0f)]
        [Tooltip("Begin fading out the sun when it's dot product vs. the down vector becomes less than or equal to this value.")]
        public float SunDotFadeThreshold = -0.3f;

        [Tooltip("Disable the sun when it's dot product vs. the down vector becomes less than or equal to this value.")]
        [Range(-1.0f, 1.0f)]
        public float SunDotDisableThreshold = -0.4f;

        [Tooltip("The intensity of the sun at default (full) intensity")]
        [Range(0.0f, 3.0f)]
        public float BaseSunIntensity = 1.0f;

        [Tooltip("The base sun shadow strength")]
        [Range(0.0f, 1.0f)]
        public float BaseSunShadowStrength = 0.8f;

        [Tooltip("The intensity of the moon at full moon intensity")]
        [Range(0.0f, 1.0f)]
        public float BaseMoonIntensity = 0.3f;

        [Range(80.0f, 110.0f)]
        [Tooltip("Day full begins or ends when the sun is at this degrees. 90 degrees is the horizon.")]
        public float DayDegrees = 90.0f;

        [Range(0.0f, 30.0f)]
        [Tooltip("The number of degrees that it fades from day to dawn/dusk before starting to fade to night. Set to 0 to fade from day and night directly. " +
            "For equal transitions from day to dusk and night, set this equal to NightFadeDegrees, but this is not required.")]
        public float DawnDuskFadeDegrees = 15.0f;

        [Range(0.0f, 90.0f)]
        [Tooltip("The number of degrees that it fades from day or dawn/dusk to night before becoming fully night")]
        public float NightFadeDegrees = 15.0f;

        /// <summary>
        /// 1 if it is fully day
        /// </summary>
        public float DayMultiplier { get; private set; }

        /// <summary>
        /// 1 if it is fully dawn or dusk
        /// </summary>
        public float DawnDuskMultiplier { get; private set; }

        /// <summary>
        /// 1 if it is fully night
        /// </summary>
        public float NightMultiplier { get; private set; }

        /// <summary>
        /// Directional light intensity multipliers - all are applied to the final directional light intensities
        /// </summary>
        [NonSerialized]
        public readonly Dictionary<string, float> DirectionalLightIntensityMultipliers = new Dictionary<string, float>();

        /// <summary>
        /// Directional light shadow intensity multipliers - all are applied to the final directional light shadow intensities
        /// </summary>
        [NonSerialized]
        public readonly Dictionary<string, float> DirectionalLightShadowIntensityMultipliers = new Dictionary<string, float>();

        /// <summary>
        /// Current sun info
        /// </summary>
        public readonly SunInfo SunData = new SunInfo();

        /// <summary>
        /// Current moon info
        /// </summary>
        public readonly List<MoonInfo> MoonDatas = new List<MoonInfo>();

        /// <summary>
        /// Number of seconds per day
        /// </summary>
        public const float SecondsPerDay = 86400.0f;

        /// <summary>
        /// Time of day at high noon
        /// </summary>
        public const float HighNoonTimeOfDay = SecondsPerDay * 0.5f;

        /// <summary>
        /// Number of seconds in one degree
        /// </summary>
        public const float SecondsForOneDegree = SecondsPerDay / 360.0f;

        #endregion Public fields

        public static void ConvertAzimuthAtltitudeToUnitVector(double azimuth, double altitude, ref Vector3 v)
        {
            v.y = (float)Math.Sin(altitude);
            float hyp = (float)Math.Cos(altitude);
            v.z = hyp * (float)Math.Cos(azimuth);
            v.x = hyp * (float)Math.Sin(azimuth);
        }

        /// <summary>
        /// Calculate the position of the sun
        /// </summary>
        /// <param name="sunInfo">Calculates and receives sun info, including position, etc. Parameters marked as calculation parameters need to be set first.</param>
        /// <param name="rotateYDegrees">Rotate around the Y axis</param>
        public static void CalculateSunPosition(SunInfo sunInfo, float rotateYDegrees)
        {
            // dateTime should already be UTC format
            double d = (sunInfo.DateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / dayMs) + jDiff;
            double e = degreesToRadians * sunInfo.AxisTilt; // obliquity of the Earth
            double m = SolarMeanAnomaly(d);
            double l = EclipticLongitude(m);
            double dec = Declination(e, l, 0);
            double ra = RightAscension(e, l, 0);
            double lw = -degreesToRadians * sunInfo.Longitude;
            double phi = degreesToRadians * sunInfo.Latitude;
            double h = SiderealTime(d, lw) - ra;
            double azimuth = Azimuth(h, phi, dec);
            double altitude = Altitude(h, phi, dec);
            ConvertAzimuthAtltitudeToUnitVector(azimuth, altitude, ref sunInfo.UnitVectorUp);

            sunInfo.UnitVectorUp = Quaternion.AngleAxis(rotateYDegrees, Vector3.up) * sunInfo.UnitVectorUp;
            sunInfo.UnitVectorDown = -sunInfo.UnitVectorUp;
            sunInfo.JulianDays = d;
            sunInfo.Declination = dec;
            sunInfo.RightAscension = ra;
            sunInfo.Azimuth = azimuth;
            sunInfo.Altitude = altitude;
            sunInfo.SolarMeanAnomaly = m;
            sunInfo.EclipticLongitude = l;
            sunInfo.SiderealTime = h;
        }

        /// <summary>
        /// Calculate moon position
        /// </summary>
        /// <param name="sunInfo">Sun info, already calculated</param>
        /// <param name="moonInfo">Receives moon info</param>
        /// <param name="rotateYDegrees">Rotate the moon in the sky around the y axis by this degrees</param>
        public static void CalculateMoonPosition(SunInfo sunInfo, MoonInfo moonInfo, float rotateYDegrees)
        {
            double d = sunInfo.JulianDays;
            double e = degreesToRadians * sunInfo.AxisTilt; // obliquity of the Earth
            double L = degreesToRadians * (218.316 + 13.176396 * d); // ecliptic longitude
            double M = degreesToRadians * (134.963 + 13.064993 * d); // mean anomaly
            double F = degreesToRadians * (93.272 + 13.229350 * d); // mean distance
            double l = L + degreesToRadians * 6.289 * Math.Sin(M); // longitude
            double b = degreesToRadians * 5.128 * Math.Sin(F); // latitude
            double dist = 385001.0 - (20905.0 * Math.Cos(M)); // distance to the moon in km
            double ra = RightAscension(e, l, b);
            double dec = Declination(e, l, b);
            const double sunDistance = 149598000.0; // avg sun distance to Earth
            double phi = Math.Acos(Math.Sin(sunInfo.Declination) * Math.Sin(dec) + Math.Cos(sunInfo.Declination) * Math.Cos(dec) * Math.Cos(sunInfo.RightAscension - ra));
            double inc = Math.Atan2(sunDistance * Math.Sin(phi), dist - sunDistance * Math.Cos(phi));
            double angle = Math.Atan2(Math.Cos(sunInfo.Declination) * Math.Sin(sunInfo.RightAscension - ra), Math.Sin(sunInfo.Declination) * Math.Cos(dec) - Math.Cos(sunInfo.Declination) * Math.Sin(dec) * Math.Cos(sunInfo.RightAscension - ra));
            double fraction = (1.0 + Math.Cos(inc)) * 0.5;
            double phase = 0.5 + (0.5 * inc * Math.Sign(angle) * (1.0 / Math.PI));
            double lw = -degreesToRadians * sunInfo.Longitude;
            phi = degreesToRadians * sunInfo.Latitude;
            double H = SiderealTime(d, lw) - ra;
            double h = Altitude(H, phi, dec);

            // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            double pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(dec) - Math.Sin(dec) * Math.Cos(H));
            h = h + AstroRefraction(h); // altitude correction for refraction
            double azimuth = Azimuth(H, phi, dec);
            double altitude = h;
            ConvertAzimuthAtltitudeToUnitVector(azimuth, altitude, ref moonInfo.UnitVectorUp);

            // set moon position and look at the origin
            moonInfo.UnitVectorUp = Quaternion.AngleAxis(rotateYDegrees, Vector3.up) * moonInfo.UnitVectorUp;
            moonInfo.UnitVectorDown = -moonInfo.UnitVectorUp;
            moonInfo.Distance = dist;
            moonInfo.Phase = phase;
            moonInfo.PercentFull = 1.0 - Math.Abs((0.5 - phase) * 2.0);
            moonInfo.Angle = angle;
            moonInfo.Fraction = fraction;
            moonInfo.Azimuth = azimuth;
            moonInfo.Altitude = altitude;
            moonInfo.RightAscension = ra;
            moonInfo.Declination = dec;
            moonInfo.LunarMeanAnomaly = M;
            moonInfo.EclipticLongitude = L;
            moonInfo.SiderealTime = H;
            moonInfo.ParallacticAngle = pa;
        }

        private const double degreesToRadians = Math.PI / 180.0;
        private const double dayMs = 1000.0 * 60.0 * 60.0 * 24.0;
        private const double j1970 = 2440587.5;
        private const double j2000 = 2451545.0;
        private const double jDiff = (j1970 - j2000);

        private static double RightAscension(double e, double l, double b)
        {
            return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l));
        }

        private static double Declination(double e, double l, double b)
        {
            return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l));
        }

        private static double Azimuth(double h, double phi, double dec)
        {
            return Math.Atan2(Math.Sin(h), Math.Cos(h) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi));
        }

        private static double Altitude(double h, double phi, double dec)
        {
            return Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(h));
        }

        private static double SiderealTime(double d, double lw)
        {
            return degreesToRadians * (280.16 + 360.9856235 * d) - lw;
        }

        private static double SolarMeanAnomaly(double d)
        {
            return degreesToRadians * (357.5291 + 0.98560028 * d);
        }

        private static double EclipticLongitude(double m)
        {
            double c = degreesToRadians * (1.9148 * Math.Sin(m) + 0.02 * Math.Sin(2.0 * m) + 0.0003 * Math.Sin(3.0 * m)); // equation of center
            double p = degreesToRadians * 102.9372; // perihelion of the Earth
            return m + c + p + Math.PI;
        }

        private static double AstroRefraction(double h)
        {
            // the following formula works for positive altitudes only.
            // if h = -0.08901179 a div/0 would occur.
            h = (h < 0.0 ? 0.0 : h);

            // formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            // 1.02 / tan(h + 10.26 / (h + 5.10)) h in degrees, result in arc minutes -> converted to rad:
            return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
        }

        private static double CorrectAngle(double angleInRadians)
        {
            if (angleInRadians < 0)
            {
                return (2 * Math.PI) + angleInRadians;
            }
            else if (angleInRadians > 2 * Math.PI)
            {
                return angleInRadians - (2 * Math.PI);
            }
            else
            {
                return angleInRadians;
            }
        }

        private void UpdateTimeOfDay()
        {

#if UNITY_EDITOR

            if (!Application.isPlaying)
            {
                return;
            }

#endif

            if (NightMultiplier != 1.0f && Speed != 0.0f)
            {
                TimeOfDay += (Speed * Time.deltaTime);
            }
            else if (NightMultiplier == 1.0f && NightSpeed != 0.0f)
            {
                TimeOfDay += (NightSpeed * Time.deltaTime);
            }
            if (AdjustDateWhenDayEnds)
            {
                // handle wrapping of time of day
                if (TimeOfDay < 0.0f)
                {
                    TimeOfDay += SecondsPerDay;
                    DateTime dt = new DateTime(Year, Month, Day) - TimeSpan.FromDays(1.0) + TimeSpan.FromSeconds(TimeOfDay);
                    Year = dt.Year;
                    Month = dt.Month;
                    Day = dt.Day;
                }
                else if (TimeOfDay >= SecondsPerDay)
                {
                    TimeOfDay -= SecondsPerDay;
                    DateTime dt = new DateTime(Year, Month, Day) + TimeSpan.FromDays(1.0) + TimeSpan.FromSeconds(TimeOfDay);
                    Year = dt.Year;
                    Month = dt.Month;
                    Day = dt.Day;
                }
            }
            else if (TimeOfDay < 0.0f)
            {
                TimeOfDay += SecondsPerDay;
            }
            else if (TimeOfDay >= SecondsPerDay)
            {
                TimeOfDay -= SecondsPerDay;
            }
        }

        private void UpdateSun()
        {
            if (WeatherScript == null)
            {
                return;
            }
            else if (WeatherScript.CameraIsOrthographic)
            {
                WeatherScript.Sun.Transform.rotation = Quaternion.AngleAxis(180.0f + ((TimeOfDay / SecondsPerDay) * 360.0f), Vector3.right);
            }
            else
            {
                // convert local time of day to UTC time of day - quick and dirty calculation
                double offsetSeconds = TimeZoneOffsetSeconds;// 3600.0 * (Math.Sign(Longitude) * Longitude * 24.0 / 360.0);
                TimeSpan t = TimeSpan.FromSeconds(TimeOfDay + offsetSeconds);
                SunData.DateTime = new DateTime(Year, Month, Day, 0, 0, 0, DateTimeKind.Utc) + t; ;
                SunData.Latitude = Latitude;
                SunData.Longitude = Longitude;
                SunData.AxisTilt = AxisTilt;

                CalculateSunPosition(SunData, WeatherScript.Sun.RotateYDegrees);

                // set rotation of sun
                WeatherScript.Sun.Transform.forward = SunData.UnitVectorDown;

                // calculate sun intensity and shadow strengths
                float dot = Vector3.Dot(WeatherScript.Sun.Transform.forward, Vector3.down);
                if (dot <= SunDotDisableThreshold)
                {
                    WeatherScript.Sun.Light.intensity = WeatherScript.Sun.Light.shadowStrength = 0.0f;
                }
                else
                {
                    if (dot <= SunDotFadeThreshold)
                    {
                        Debug.Assert(SunDotDisableThreshold <= SunDotFadeThreshold, "SunDotDisableThreshold should be less than or equal to SunDotFadeThreshold");
                        float range = Mathf.Abs(SunDotFadeThreshold - SunDotDisableThreshold);
                        float distanceThroughRange = Mathf.Abs(SunDotFadeThreshold - dot) / range;
                        float lerp = Mathf.Lerp(1.0f, 0.0f, distanceThroughRange);
                        WeatherScript.Sun.Light.intensity = BaseSunIntensity * lerp;
                        WeatherScript.Sun.Light.shadowStrength = BaseSunShadowStrength;
                    }
                    else
                    {
                        WeatherScript.Sun.Light.intensity = BaseSunIntensity;
                        WeatherScript.Sun.Light.shadowStrength = BaseSunShadowStrength;
                    }

                    // only one sun for now, so no need to loop
                    foreach (float multiplier in DirectionalLightIntensityMultipliers.Values)
                    {
                        WeatherScript.Sun.Light.intensity *= multiplier;
                    }
                    foreach (float multiplier in DirectionalLightShadowIntensityMultipliers.Values)
                    {
                        WeatherScript.Sun.Light.shadowStrength *= multiplier;
                    }

                    // reduce shadow strength as sun gets near the horizon
                    float sunYShadowReducer = Mathf.Pow(Mathf.Clamp(0.7f - SunData.UnitVectorDown.y, 0.0f, 1.0f), 0.5f);
                    WeatherScript.Sun.Light.shadowStrength *= sunYShadowReducer;
                }
            }
        }

        private void UpdateMoons()
        {
            float dot, yPower;
            while (MoonDatas.Count > WeatherScript.Moons.Length)
            {
                MoonDatas.RemoveAt(MoonDatas.Count - 1);
            }
            while (MoonDatas.Count < WeatherScript.Moons.Length)
            {
                MoonDatas.Add(new MoonInfo());
            }

            for (int i = 0; i < WeatherScript.Moons.Length; i++)
            {
                WeatherMakerCelestialObject moon = WeatherScript.Moons[i];
                CalculateMoonPosition(SunData, MoonDatas[i], moon.RotateYDegrees);
                Vector3 moonPos = MoonDatas[i].UnitVectorDown;
                moon.Transform.forward = moonPos;

                // intensity raises squared compare to moon fullness - this means less full is squared amount of less light
                // moon light intensity reduces as sun light intensity approaches 1
                // reduce moon light as it drops below horizon
                dot = Mathf.Clamp(Vector3.Dot(moonPos, Vector3.down) + 0.1f, 0.0f, 1.0f);
                dot = Mathf.Pow(dot, 0.25f);
                yPower = Mathf.Pow(Mathf.Max(0.0f, MoonDatas[i].UnitVectorUp.y), 0.5f);
                moon.Light.intensity = yPower * (1.0f - Mathf.Min(1.0f, WeatherScript.Sun.Light.intensity)) * (float)MoonDatas[i].PercentFull * (float)MoonDatas[i].PercentFull * dot * moon.LightMultiplier;

                foreach (float multiplier in DirectionalLightIntensityMultipliers.Values)
                {
                    moon.Light.intensity *= multiplier;
                }

                // set moon light shadow strength
                moon.Light.shadowStrength = 1.0f;
                foreach (float multiplier in DirectionalLightShadowIntensityMultipliers.Values)
                {
                    moon.Light.shadowStrength *= multiplier;
                }

                // reduce shadow strength as moon gets near the horizon
                float moonYShadowReducer = Mathf.Pow(Mathf.Clamp(0.7f - moonPos.y, 0.0f, 1.0f), 0.5f);
                moon.Light.shadowStrength *= moonYShadowReducer;
                moon.Light.shadows = LightShadows.None;
            }
        }

        private void UpdateDayMultipliers()
        {
            if (WeatherScript.CameraIsOrthographic)
            {
                return;
            }

            float sunRotation = (WeatherScript.Sun == null ? 60.0f : WeatherScript.Sun.Transform.eulerAngles.x);
            if (sunRotation > 180.0f)
            {
                sunRotation -= 270.0f;
            }
            else
            {
                sunRotation += 90.0f;
            }

            if (sunRotation >= DayDegrees)
            {
                // fully day
                DayMultiplier = 1.0f;
                DawnDuskMultiplier = NightMultiplier = 0.0f;
            }
            else if (sunRotation < DayDegrees - NightFadeDegrees - DawnDuskFadeDegrees)
            {
                // fully night
                NightMultiplier = 1.0f;
                DayMultiplier = DawnDuskMultiplier = 0.0f;
            }
            else if (DawnDuskFadeDegrees == 0.0f && sunRotation < DayDegrees)
            {
                // fade from day/night
                float degreeDiff = DayDegrees - sunRotation;
                DawnDuskMultiplier = 0.0f;
                NightMultiplier = Mathf.Lerp(0.0f, 1.0f, degreeDiff / NightFadeDegrees);
                DayMultiplier = 1.0f - NightMultiplier;
            }
            else if (sunRotation < DayDegrees - DawnDuskFadeDegrees)
            {
                // fade from night/dawn/dusk
                float degreeDiff = DayDegrees - DawnDuskFadeDegrees - sunRotation;
                DayMultiplier = 0.0f;
                NightMultiplier = Mathf.Lerp(0.0f, 1.0f, degreeDiff / NightFadeDegrees);
                DawnDuskMultiplier = 1.0f - NightMultiplier;
            }
            else
            {
                // fade from day/dawn/dusk
                float degreeDiff = DayDegrees - sunRotation;
                NightMultiplier = 0.0f;
                DawnDuskMultiplier = Mathf.Lerp(0.0f, 1.0f, degreeDiff / DawnDuskFadeDegrees);
                DayMultiplier = 1.0f - DawnDuskMultiplier;
            }
            // Debug.LogFormat("Sun Rotation: {0}, Day Multiplier: {1}, Dawn/Dusk Multiplier: {2}, Night Multiplier: {3}", sunRotation, DayMultiplier, DawnDuskMultiplier, NightMultiplier);
        }

        private void DoDayNightCycle()
        {
            UpdateTimeOfDay();
            UpdateSun();
            UpdateDayMultipliers();
            UpdateMoons();
            // Debug.LogFormat("Moon angle: {0}, phase: {1}, fraction: {2}", MoonData.Angle, MoonData.Phase, MoonData.Fraction);
        }

        private void Start()
        {
            DoDayNightCycle();
        }

        private void Update()
        {
            DoDayNightCycle();
        }
    }
}

// resources:
// https://en.wikipedia.org/wiki/Position_of_the_Sun
// http://stackoverflow.com/questions/8708048/position-of-the-sun-given-time-of-day-latitude-and-longitude
// http://www.grasshopper3d.com/forum/topics/solar-calculation-plugin
// http://guideving.blogspot.nl/2010/08/sun-position-in-c.html
// https://github.com/mourner/suncalc
// http://stackoverflow.com/questions/1058342/rough-estimate-of-the-time-offset-from-gmt-from-latitude-longitude
// http://www.stjarnhimlen.se/comp/tutorial.html
// http://www.suncalc.net/#/40.7608,-111.891,12/2000.09.21/12:46
// http://www.suncalc.net/scripts/suncalc.js

// total eclipse:
// 43.7678
// -111.8323
// Maximum eclipse : 	2017/08/21	17:34:18.6	49.5°	133.1°