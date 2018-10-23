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

using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace DigitalRuby.WeatherMaker
{
    public enum WeatherMakerFogMode
    {
        None,
        Constant,
        Linear,
        Exponential,
        ExponentialSquared
    }

    public enum BlurShaderType
    {
        None,
        GaussianBlur7,
        GaussianBlur17
    }

    public class WeatherMakerFullScreenFogScript : WeatherMakerFogScript
    {
        private class CameraAndCommandBuffer
        {
            public Camera Camera;
            public CommandBuffer CommandBuffer;
            public Material FogMaterial;
        }

        [Header("Full Screen Fog")]
        [Tooltip("Weather Maker Script")]
        public WeatherMakerScript WeatherScript;

        [Tooltip("Material to render the fog full screen after it has been calculated")]
        public Material FogFullScreenMaterial;

        [Tooltip("Fog height. Set to 0 for unlimited height.")]
        [Range(0.0f, 1000.0f)]
        public float FogHeight = 0.0f;

        [Tooltip("Depth buffer less than far plane multiplied by this value will occlude the sun light through the fog.")]
        [Range(0.0f, 1.0f)]
        public float FarPlaneSunThreshold = 0.75f;

        [Tooltip("Render fog in this render queue for the command buffer.")]
        public CameraEvent FogRenderQueue = CameraEvent.AfterForwardAlpha;

        private readonly List<CameraAndCommandBuffer> cameras = new List<CameraAndCommandBuffer>();

        protected override void LateUpdate()
        {
            base.LateUpdate();

            UpdateFogProperties();
            CreateCommandBuffers();
            UpdateShaderProperties();
        }

        private void UpdateFogProperties()
        {
            float multiplier;
            if (FogMode == WeatherMakerFogMode.Constant || FogMode == WeatherMakerFogMode.Linear)
            {
                float h = (FogHeight <= 0.0f ? 1000.0f : FogHeight) * 0.01f;
                multiplier = 1.0f - (FogDensity * 4.0f * h);
            }
            else if (FogMode == WeatherMakerFogMode.Exponential)
            {
                float h = (FogHeight <= 0.0f ? 1000.0f : FogHeight) * 0.02f;
                multiplier = 1.0f - Mathf.Min(1.0f, Mathf.Pow(FogDensity * 32.0f * h, 0.5f));
            }
            else
            {
                float h = (FogHeight <= 0.0f ? 1000.0f : FogHeight) * 0.04f;
                multiplier = 1.0f - Mathf.Min(1.0f, Mathf.Pow(FogDensity * 64.0f * h, 0.5f));
            }
            WeatherScript.DayNightScript.DirectionalLightShadowIntensityMultipliers["WeatherMakerFullScreenFogScript"] = Mathf.Clamp(multiplier, 0.0f, 1.0f);
        }

        private void UpdateShaderProperties()
        {
            foreach (CameraAndCommandBuffer c in cameras)
            {
                c.FogMaterial.SetMatrix("_WeatherMakerCameraInverseMVP", c.Camera.cameraToWorldMatrix * c.Camera.projectionMatrix.inverse);
                c.FogMaterial.SetMatrix("_WeatherMakerCameraInverseMV", c.Camera.cameraToWorldMatrix);
                c.FogMaterial.SetMatrix("_WeatherMakerCameraWorldToCamera", c.Camera.worldToCameraMatrix);
                SetFogShaderProperties(c.FogMaterial);
                if (FogHeight > 0.0f)
                {
                    c.FogMaterial.SetFloat("_FogHeight", FogHeight);
                    c.FogMaterial.EnableKeyword("ENABLE_FOG_HEIGHT");
                }
                else
                {
                    c.FogMaterial.DisableKeyword("ENABLE_FOG_HEIGHT");
                }
            }
        }

        private void CreateCommandBufferForCamera(Camera camera)
        {
            if (camera == null)
            {
                return;
            }

            WeatherScript.DayNightScript.DirectionalLightShadowIntensityMultipliers.Remove("WeatherMakerFullScreenFogScript");

            if (FogDensity <= 0.0f || FogMode == WeatherMakerFogMode.None)
            {
                // no fog, nuke command buffer
                RemoveCommandBuffer(camera);
                return;
            }

            // if no changes and we already have a command buffer for this camera, early exit
            if (DownSampleScale == lastDownSampleScale && BlurShader == lastBlurShader && cameras.Find((CameraAndCommandBuffer c) =>
            {
                return c.Camera == camera;
            }) != null)
            {
                // no change in fog render settings, no need to change command buffer
                return;
            }

            RemoveCommandBuffer(camera);
            Material fogMaterial = new Material(FogMaterial);
            CommandBuffer commandBuffer = new CommandBuffer { name = "WeatherMakerFullScreenFogScript" };
            camera.AddCommandBuffer(FogRenderQueue, commandBuffer);
            float scale = Mathf.Clamp(DownSampleScale, 0.25f, 1.0f);
            if (scale < 0.99f)
            {
                // scale is less than 1, create scaled down textures for depth and fog
                int width = (int)(Screen.width * scale);
                int height = (int)(Screen.height * scale);

                // render fog to low res texture, disable alpha blend
                int fogRenderTargetId = Shader.PropertyToID("_MainTex");
                RenderTargetIdentifier downScaledFog = new RenderTargetIdentifier(fogRenderTargetId);
                commandBuffer.GetTemporaryRT(fogRenderTargetId, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
                fogMaterial.SetInt("_SrcBlendMode", (int)BlendMode.One);
                fogMaterial.SetInt("_DstBlendMode", (int)BlendMode.Zero);
                commandBuffer.Blit((Texture2D)null, downScaledFog, fogMaterial);

                // blur fog texture if scaled down
                if (FogBlurMaterial != null && BlurShader != BlurShaderType.None)
                {
                    // render fog on top of camera using alpha blend + blur
                    FogBlurMaterial.SetInt("_SrcBlendMode", (int)BlendMode.One);
                    FogBlurMaterial.SetInt("_DstBlendMode", (int)BlendMode.OneMinusSrcAlpha);
                    commandBuffer.Blit(downScaledFog, BuiltinRenderTextureType.CameraTarget, FogBlurMaterial);
                }
                else
                {
                    // render fog on top of camera using alpha blend
                    commandBuffer.Blit(downScaledFog, BuiltinRenderTextureType.CameraTarget, FogFullScreenMaterial);
                    FogFullScreenMaterial.SetInt("_SrcBlendMode", (int)BlendMode.One);
                    FogFullScreenMaterial.SetInt("_DstBlendMode", (int)BlendMode.OneMinusSrcAlpha);
                }

                // cleanup
                commandBuffer.ReleaseTemporaryRT(fogRenderTargetId);
            }
            else if (FogBlurMaterial != null && BlurShader != BlurShaderType.None)
            {
                // render fog into render texture, then blur that to final result

                // create fog render target, draw fog to that
                int fogRenderTargetId = Shader.PropertyToID("_MainTex");
                RenderTargetIdentifier fogRenderTarget = new RenderTargetIdentifier(fogRenderTargetId);
                commandBuffer.GetTemporaryRT(fogRenderTargetId, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
                fogMaterial.SetInt("_SrcBlendMode", (int)BlendMode.One);
                fogMaterial.SetInt("_DstBlendMode", (int)BlendMode.Zero);

                // draw fog into fog render texture at full scale
                commandBuffer.Blit(new RenderTargetIdentifier(BuiltinRenderTextureType.None), fogRenderTarget, fogMaterial);

                // render final result with alpha blend + blur on top of camera texture
                FogBlurMaterial.SetInt("_SrcBlendMode", (int)BlendMode.One);
                FogBlurMaterial.SetInt("_DstBlendMode", (int)BlendMode.OneMinusSrcAlpha);
                commandBuffer.Blit(fogRenderTarget, BuiltinRenderTextureType.CameraTarget, FogBlurMaterial);

                // cleanup
                commandBuffer.ReleaseTemporaryRT(fogRenderTargetId);
            }
            else
            {
                // render final image to camera target using transparent overlay
                fogMaterial.SetInt("_SrcBlendMode", (int)BlendMode.One);
                fogMaterial.SetInt("_DstBlendMode", (int)BlendMode.OneMinusSrcAlpha);
                commandBuffer.Blit((Texture2D)null, BuiltinRenderTextureType.CameraTarget, fogMaterial);
            }

            cameras.Add(new CameraAndCommandBuffer { Camera = camera, CommandBuffer = commandBuffer, FogMaterial = fogMaterial });
        }

        private void CreateCommandBuffers()
        {
            CreateCommandBufferForCamera(WeatherScript.Camera);
            foreach (Camera c in WeatherScript.Cameras)
            {
                CreateCommandBufferForCamera(c);
            }

            // remove destroyed camera command buffers
            for (int i = cameras.Count - 1; i >= 0; i--)
            {
                if (cameras[i].Camera == null)
                {
                    cameras.RemoveAt(i);
                }
            }

            lastDownSampleScale = DownSampleScale;
            lastBlurShader = BlurShader;
        }

        private void RemoveAllCommandBuffers()
        {
            for (int i = cameras.Count - 1; i >= 0; i--)
            {
                RemoveCommandBuffer(i);
            }
            lastDownSampleScale = 0.0f;
        }

        private void RemoveCommandBuffer(int index)
        {
            CameraAndCommandBuffer camera = cameras[index];
            if (camera.Camera != null && camera.CommandBuffer != null)
            {
                camera.Camera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, camera.CommandBuffer);
                camera.CommandBuffer.Release();
            }
            if (camera.FogMaterial != null)
            {
                GameObject.Destroy(camera.FogMaterial);
            }
            cameras.RemoveAt(index);
            lastDownSampleScale = 0.0f;
        }

        private void RemoveCommandBuffer(Camera camera)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i].Camera == camera)
                {
                    RemoveCommandBuffer(i);
                    break;
                }
            }
        }

        protected override void OnBecameInvisible()
        {
            base.OnBecameInvisible();
            RemoveAllCommandBuffers();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveAllCommandBuffers();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RemoveAllCommandBuffers();
        }
    }
}