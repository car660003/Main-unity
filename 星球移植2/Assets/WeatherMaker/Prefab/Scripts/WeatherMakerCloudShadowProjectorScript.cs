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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.WeatherMaker
{
    [ExecuteInEditMode]
    public class WeatherMakerCloudShadowProjectorScript : MonoBehaviour
    {
        [Header("Dependencies")]
        public WeatherMakerScript WeatherScript;

        [Header("Shadow properties")]
        [Tooltip("Cloud shadow material or null for no cloud shadows")]
        public Material CloudShadowMaterial;

        [Tooltip("Cloud shadow projector")]
        public Projector CloudShadowProjector;

        private RenderTexture cloudShadowTexture;

        private Vector3? SunGroundIntersect()
        {
            Vector3 cameraPos = WeatherScript.Camera.transform.position;
            Vector3 normal = Vector3.up;
            Vector3 ray = -WeatherScript.Sun.Transform.forward;
            ray.y += WeatherScript.SkySphereScript.CloudRayOffset;
            float denom = Vector3.Dot(normal, ray);
            if (denom < Mathf.Epsilon)
            {
                // sun too low
                return null;
            }

            Vector3 plane = new Vector3(cameraPos.x, WeatherScript.SkySphereScript.CloudHeight, cameraPos.z);
            float t = Vector3.Dot(plane, normal) / denom;
            float multiplier = WeatherScript.Camera.farClipPlane / t;
            multiplier = Mathf.Min(1.0f, Mathf.Pow(multiplier, 3.0f));

            if (t > WeatherScript.Camera.farClipPlane * 2.0f)
            {
                // remove shadows as sun gets close to horizon
                return null;
            }

            Vector3 intersect = cameraPos + (ray * t);
            CloudShadowMaterial.SetVector("_FogShadowCenterPoint", intersect);

            // reduce shadow when sun is low in the sky
            CloudShadowMaterial.SetFloat("_FogShadowMultiplier", multiplier);
            return intersect;
        }

        private Vector3? ProcessCloudShadows()
        {
            if (CloudShadowMaterial == null || WeatherScript.SkySphereScript.CloudShadowThreshold >= 0.999f || WeatherScript.SkySphereScript.CloudShadowPower >= 0.999f ||
                !WeatherScript.SkySphereScript.CloudsEnabled || WeatherScript.SkySphereScript.CloudCover > 0.95f || !WeatherScript.Sun.LightIsOn || WeatherScript.Sun.Light.shadows == LightShadows.None)
            {
                if (cloudShadowTexture != null)
                {
                    cloudShadowTexture.Release();
                    cloudShadowTexture = null;
                }
                CloudShadowProjector.enabled = false;
                return null;
            }
            else
            {
                WeatherScript.SkySphereScript.SetShaderCloudParameters(CloudShadowMaterial);
                if (cloudShadowTexture == null)
                {
                    cloudShadowTexture = new RenderTexture(1024, 1024, 0);
                    cloudShadowTexture.name = "WeatherMakerSkySphereCloudShadowsTexture";
                    cloudShadowTexture.wrapMode = TextureWrapMode.Clamp;
                    cloudShadowTexture.filterMode = FilterMode.Bilinear;
                    cloudShadowTexture.anisoLevel = 1;
                    cloudShadowTexture.useMipMap = false;
                    CloudShadowProjector.material.SetTexture("_ShadowTex", cloudShadowTexture);
                    CloudShadowProjector.orthographicSize = WeatherScript.Camera.farClipPlane * WeatherScript.SkySphereScript.FarClipPlaneMultiplier;
                    //{ GameObject debugObj = GameObject.Find("DebugQuad"); if (debugObj != null) { Renderer debugRenderer = debugObj.GetComponent<Renderer>(); if (debugRenderer != null) { debugRenderer.sharedMaterial.mainTexture = cloudShadowTexture; } } }
                }
                Vector3 pos = WeatherScript.Camera.transform.position;
                pos.y = WeatherScript.SkySphereScript.CloudHeight;
                CloudShadowProjector.transform.position = pos;
                return SunGroundIntersect();
            }
        }

        private void OnDestroy()
        {
            if (cloudShadowTexture != null)
            {
                cloudShadowTexture.Release();
                cloudShadowTexture = null;
            }
        }

        public void RenderCloudShadows()
        {
            Vector3? sunGroundIntersect = ProcessCloudShadows();
            if (sunGroundIntersect == null)
            {
                CloudShadowProjector.enabled = false;
            }
            else
            {
                CloudShadowProjector.enabled = true;
                Graphics.SetRenderTarget(cloudShadowTexture);
                GL.Clear(true, true, Color.white);
                if (sunGroundIntersect != null && CloudShadowMaterial.SetPass(0))
                {
                    float scale = WeatherScript.Camera.farClipPlane * WeatherScript.SkySphereScript.FarClipPlaneMultiplier;
                    float viewportBounds = 1024.0f;
                    Vector3 pos = sunGroundIntersect.Value;
                    GL.PushMatrix();
                    GL.Viewport(new Rect(0.0f, 0.0f, viewportBounds, viewportBounds));
                    Vector2 bottomLeft = new Vector2(pos.x - scale, pos.z - scale);
                    Vector2 topRight = new Vector2(pos.x + scale, pos.z + scale);
                    Matrix4x4 proj = Matrix4x4.Ortho(bottomLeft.x, topRight.x, bottomLeft.y, topRight.y, -1.0f, 1.0f);
                    GL.LoadProjectionMatrix(proj);
                    GL.Begin(GL.QUADS);
                    GL.Vertex3(bottomLeft.x, bottomLeft.y, 0.0f);
                    GL.Vertex3(bottomLeft.x, topRight.y, 0.0f);
                    GL.Vertex3(topRight.x, topRight.y, 0.0f);
                    GL.Vertex3(topRight.x, bottomLeft.y, 0.0f);
                    GL.End();
                    GL.PopMatrix();
                }
                Graphics.SetRenderTarget(null);
            }
        }
    }
}
