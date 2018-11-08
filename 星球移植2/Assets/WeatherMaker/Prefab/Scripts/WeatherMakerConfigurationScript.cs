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

using UnityEngine;

namespace DigitalRuby.WeatherMaker
{
    public class WeatherMakerConfigurationScript : MonoBehaviour
    {
        public WeatherMakerScript WeatherScript;
        public bool ShowFPS = true;
        public bool ShowTimeOfDay = true;
        public float MovementSpeed = 20.0f;
        public bool AllowFlashlight;
        public GameObject ConfigurationPanel;
        public UnityEngine.UI.Text LabelFPS;
        public UnityEngine.UI.Slider TransitionDurationSlider;
        public UnityEngine.UI.Slider IntensitySlider;
        public UnityEngine.UI.Toggle MovementEnabledCheckBox;
        public UnityEngine.UI.Toggle FlashlightToggle;
        public UnityEngine.UI.Toggle TimeOfDayEnabledCheckBox;
        public UnityEngine.UI.Toggle CloudToggle2D;
        public UnityEngine.UI.Slider DawnDuskSlider;
        public UnityEngine.UI.Text TimeOfDayText;
        public UnityEngine.UI.Dropdown CloudDropdown;
        public UnityEngine.EventSystems.EventSystem EventSystem;
        public Light Flashlight;
        public GameObject SidePanel;

        private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        private RotationAxes axes = RotationAxes.MouseXAndY;
        private float sensitivityX = 15F;
        private float sensitivityY = 15F;
        private float minimumX = -360F;
        private float maximumX = 360F;
        private float minimumY = -60F;
        private float maximumY = 60F;
        private float rotationX = 0F;
        private float rotationY = 0F;
        private Quaternion originalRotation;

        private int frameCount = 0;
        private float nextFrameUpdate = 0.0f;
        private float fps = 0.0f;
        private float frameUpdateRate = 4.0f; // 4 per second

        private int frameCounter;

        private void UpdateMovement()
        {
            if (MovementSpeed <= 0.0f || MovementEnabledCheckBox == null || !MovementEnabledCheckBox.isOn)
            {
                return;
            }

            float speed = MovementSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.W))
            {
                WeatherScript.Camera.transform.Translate(0.0f, 0.0f, speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                WeatherScript.Camera.transform.Translate(0.0f, 0.0f, -speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                WeatherScript.Camera.transform.Translate(-speed, 0.0f, 0.0f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                WeatherScript.Camera.transform.Translate(speed, 0.0f, 0.0f);
            }
        }

        private void UpdateMouseLook()
        {
            if (MovementEnabledCheckBox == null || MovementSpeed <= 0.0f)
            {
                return;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                MovementEnabledCheckBox.isOn = !MovementEnabledCheckBox.isOn;
            }

            if (!MovementEnabledCheckBox.isOn)
            {
                return;
            }
            else if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

                WeatherScript.Camera.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            }
            else if (axes == RotationAxes.MouseX)
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                WeatherScript.Camera.transform.localRotation = originalRotation * xQuaternion;
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
                WeatherScript.Camera.transform.localRotation = originalRotation * yQuaternion;
            }
        }

        private void UpdateTimeOfDay()
        {
            DawnDuskSlider.value = WeatherScript.TimeOfDay;
            if (TimeOfDayText.IsActive() && ShowTimeOfDay)
            {
                TimeSpan t = TimeSpan.FromSeconds(WeatherScript.TimeOfDay);
                TimeOfDayText.text = string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
            }
        }

        private void DisplayFPS()
        {
            if (LabelFPS != null && ShowFPS)
            {
                frameCount++;
                if (Time.time > nextFrameUpdate)
                {
                    nextFrameUpdate += (1.0f / frameUpdateRate);
                    fps = (int)Mathf.Floor((float)frameCount * frameUpdateRate);
                    LabelFPS.text = "FPS: " + fps;
                    frameCount = 0;
                }
            }
        }

        private void Start()
        {
            originalRotation = transform.localRotation;
            IntensitySlider.value = WeatherScript.PrecipitationIntensity;
            DawnDuskSlider.value = WeatherScript.TimeOfDay;
            nextFrameUpdate = Time.time;

            if (UnityEngine.EventSystems.EventSystem.current == null && ConfigurationPanel != null && ConfigurationPanel.activeInHierarchy)
            {
                EventSystem.gameObject.SetActive(true);
                UnityEngine.EventSystems.EventSystem.current = EventSystem;
            }
        }

        private void Update()
        {
            UpdateMovement();
            UpdateMouseLook();
            DisplayFPS();
            if (AllowFlashlight && Flashlight != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    FlashlightToggle.isOn = !FlashlightToggle.isOn;

                    // hack: Bug in Unity, doesn't recognize that the light was enabled unless we rotate the camera
                    WeatherScript.Camera.transform.Rotate(0.0f, 0.01f, 0.0f);
                    WeatherScript.Camera.transform.Rotate(0.0f, -0.01f, 0.0f);
                }
                WeatherMakerLightManagerScript.Instance.AddLight(Flashlight);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                LightningStrikeButtonClicked();
            }
            if (Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                // Unity bug, disabling or setting transform scale to 0 will break any projectors in the scene
                SidePanel.transform.localScale = (SidePanel.transform.localScale.x < 0.9f ? Vector3.one : new Vector3(0.001f, 0.001f, 0.001f));
            }
            UpdateTimeOfDay();
            frameCounter++;
        }

        // Weather configuration...

        public void RainToggleChanged(bool isOn)
        {
            WeatherScript.Precipitation = (isOn ? WeatherMakerPrecipitationType.Rain : WeatherMakerPrecipitationType.None);
		}

        public void SnowToggleChanged(bool isOn)
        {
            WeatherScript.Precipitation = (isOn ? WeatherMakerPrecipitationType.Snow : WeatherMakerPrecipitationType.None);
        }

        public void HailToggleChanged(bool isOn)
        {
            WeatherScript.Precipitation = (isOn ? WeatherMakerPrecipitationType.Hail : WeatherMakerPrecipitationType.None);
        }

        public void SleetToggleChanged(bool isOn)
        {
            WeatherScript.Precipitation = (isOn ? WeatherMakerPrecipitationType.Sleet : WeatherMakerPrecipitationType.None);
        }

        public void CloudToggleChanged()
        {
            WeatherScript.CloudChangeDuration = TransitionDurationSlider.value;
            if (CloudDropdown == null)
            {
                // 2D only supports storm clouds
                WeatherScript.Clouds = (CloudToggle2D.isOn ? WeatherMakerCloudType.Storm : WeatherMakerCloudType.None);
            }
            else if (CloudDropdown.value == 0)
            {
                // no clouds
                WeatherScript.Clouds = WeatherMakerCloudType.None;
            }
            else if (CloudDropdown.value == 1)
            {
                // light clouds - all done in sky sphere shader
                WeatherScript.Clouds = WeatherMakerCloudType.Light;
            }
            else if (CloudDropdown.value == 2)
            {
                // medium clouds - all done in sky sphere shader
                WeatherScript.Clouds = WeatherMakerCloudType.Medium;
            }
            else if (CloudDropdown.value == 3)
            {
                // heavy clouds - all done in sky sphere shader
                WeatherScript.Clouds = WeatherMakerCloudType.Heavy;
            }
            else
            {
                // storm clouds - all done in sky sphere shader
                WeatherScript.Clouds = WeatherMakerCloudType.Storm;
            }
        }

        public void LightningToggleChanged(bool isOn)
        {
            WeatherScript.LightningScript.EnableLightning = isOn;
        }

        public void CollisionToggleChanged(bool isOn)
        {
            WeatherScript.PrecipitationCollisionEnabled = isOn;
        }

        public void WindToggleChanged(bool isOn)
        {
            WeatherScript.WindScript.AnimateWindIntensity(isOn ? 0.3f : 0.0f, TransitionDurationSlider.value);
        }

        public void TransitionDurationSliderChanged(float val)
        {
            WeatherScript.PrecipitationChangeDuration = val;
        }

        public void IntensitySliderChanged(float val)
        {
            WeatherScript.PrecipitationIntensity = val;
        }

        public void MovementEnabledChanged(bool val)
        {
            MovementEnabledCheckBox.isOn = val;
        }

        public void FlashlightChanged(bool val)
        {
            if (AllowFlashlight && FlashlightToggle != null && Flashlight != null)
            {
                FlashlightToggle.isOn = val;
                Flashlight.enabled = val;
            }
        }

        public void FogChanged(bool val)
        {
            // if fog is not active, set the start fog density to 0, otherwise start at whatever density it is at
            float startFogDensity = WeatherScript.FogScript.FogDensity;
            float endFogDensity = (startFogDensity == 0.0f ? 0.02f : 0.0f);
            WeatherScript.FogScript.TransitionFogDensity(startFogDensity, endFogDensity, TransitionDurationSlider.value);
        }

        public void ManagerChanged(bool val)
        {
            if (WeatherScript.WeatherManagers != null && WeatherScript.WeatherManagers.Count > 0)
            {
                WeatherScript.WeatherManagers[0].gameObject.SetActive(val);
            }
        }

        public void TimeOfDayEnabledChanged(bool val)
        {
            WeatherScript.DayNightScript.Speed = WeatherScript.DayNightScript.NightSpeed = (val ? 10.0f : 0.0f);
        }

        public void LightningStrikeButtonClicked()
        {
            WeatherScript.LightningScript.CallIntenseLightning();
        }

        public void DawnDuskSliderChanged(float val)
        {
            WeatherScript.DayNightScript.TimeOfDay = val;
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}