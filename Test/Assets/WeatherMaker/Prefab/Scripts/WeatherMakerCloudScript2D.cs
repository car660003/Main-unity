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
    public class WeatherMakerCloudScript2D : WeatherMakerCloudScript
    {
        private ParticleSystem cloudParticleSystem;
        private Renderer cloudParticleSystemRenderer;

        private void UpdateParticleSystem()
        {
            var m = cloudParticleSystem.main;
            m.maxParticles = NumberOfClouds;
            var anim = cloudParticleSystem.textureSheetAnimation;
            anim.numTilesX = MaterialColumns;
            anim.numTilesY = MaterialRows;
            cloudParticleSystemRenderer.sharedMaterial.mainTexture = MaterialTexture;
            cloudParticleSystemRenderer.sharedMaterial.SetColor("_TintColor", TintColor);
            cloudParticleSystemRenderer.sharedMaterial.EnableKeyword("ORTHOGRAPHIC_MODE");
        }

        protected override void OnUpdateMaterial(Material m)
        {
            base.OnUpdateMaterial(m);
        }

        protected override void Start()
        {
            base.Start();

            cloudParticleSystem = GetComponentInChildren<ParticleSystem>();
            cloudParticleSystemRenderer = cloudParticleSystem.GetComponent<Renderer>();
            UpdateParticleSystem();
        }

        protected override void Update()
        {
            base.Update();

            UpdateParticleSystem();
        }

        public override void CreateClouds()
        {
            cloudParticleSystem.Play();
        }

        public override void RemoveClouds()
        {
            cloudParticleSystem.Stop();
        }

        public override void Reset()
        {
            cloudParticleSystem.Stop();
            cloudParticleSystem.Clear();
        }
    }
}