using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.WeatherMaker
{
    public class WeatherMakerDemoScriptCloudMask : MonoBehaviour
    {
        public WeatherMakerScript WeatherScript;

        private void Start()
        {
            WeatherScript.ConfigurationScript.CloudDropdown.value = 3;
        }

        private void Update()
        {

        }
    }
}
