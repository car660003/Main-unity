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

Shader "WeatherMaker/WeatherMakerCloudShadowShader"
{
	Properties
	{
		_FogDensity("Fog Density", Range(0.0, 1.0)) = 0.0
		_FogColor("Fog Color", Color) = (1,1,1,1)
		_FogEmissionColor("Fog Emission Color", Color) = (0,0,0,0)
		_FogNoise("Fog Noise", 2D) = "white" {}
		_FogNoiseScale("Fog Noise Scale", Range(0.0, 1.0)) = 0.02
		_FogNoiseMultiplier("Fog Noise Multiplier", Range(0.01, 4.0)) = 1
		_FogNoiseVelocity("Fog Noise Velocity", Vector) = (0.1, 0.2, 0, 0)
		_FogNoiseMask("Fog Noise Mask", 2D) = "white" {}
		_FogNoiseMaskScale("Fog Noise Mask Scale", Range(0.000001, 1.0)) = 0.02
		_FogNoiseMaskOffset("Fog Noise Mask Offset", Vector) = (0.0, 0.0, 0.0)
		_FogNoiseMaskVelocity("Fog Noise Mask Velocity", Vector) = (0.1, 0.2, 0, 0)
		_FogNoiseMaskRotationSin("Fog Noise Mask Rotation Sin", Float) = 0.0
		_FogNoiseMaskRotationCos("Fog Noise Mask Rotation Cos", Float) = 0.0
		_FogCover("Fog Cover", Range(0.0, 1.0)) = 0.25
		_FogSharpness("Fog Sharpness", Range(0.0, 1.0)) = 0.015
		_FogShadowThreshold("Fog Shadow Threshold", Range(0.0, 1.0)) = 0.02
		_FogShadowPower("Fog Shadow Power (0 = full, 1 = none)", Range(0.0001, 1.0)) = 0.5
		_FogShadowMultiplier("Fog Shadow Multiplier", Range(0.0, 1.0)) = 1.0
		_FogShadowCenterPoint("Fog Shadow Center", Vector) = (0, 0, 0)
	}

	SubShader
	{
		CGINCLUDE

		#pragma target 3.0
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma glsl_no_auto_normalization
		#pragma multi_compile __ ENABLE_CLOUDS ENABLE_CLOUDS_MASK

		#include "WeatherMakerSkyShader.cginc"

		ENDCG
		
		Pass
		{
			Name "SkySphereCloudShadowPass"
			// Tags { "Queue" = "Transparent+100" "Type" = "Transparent" "IgnoreProjector" = "True" "LightMode" = "Always" }
			Cull Off Lighting Off ZWrite Off ZTest Off Blend One Zero

			CGPROGRAM

			#pragma vertex shadowVert
			#pragma fragment shadowFrag

			float3 _FogShadowCenterPoint;

			v2fShadow shadowVert(appdata v)
			{
				v2fShadow o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = WorldSpaceVertexPosNear(v.vertex);
				return o;
			}

			fixed4 shadowFrag(v2fShadow i) : SV_TARGET
			{

#if defined(ENABLE_CLOUDS) || defined(ENABLE_CLOUDS_MASK)

				// shadow strength * sun alpha * shadow multiplier * fbm
				fixed f = (_WeatherMakerSunLightPower.z * _WeatherMakerSunColor.a * pow(max(0.0, (_FogShadowMultiplier * ComputeCloudShadowFBMFromXZ(i.worldPos.xy)) - _FogShadowThreshold), _FogShadowPower));
				f = min(1.0, f / max(0.01, (_FogLightAbsorption * 64.0)));

				// restrict to circle
				// (x - center_x)^2 + (y - center_y)^2 < radius^2.
				float dx = (i.worldPos.x - _FogShadowCenterPoint.x);
				dx *= dx;
				float dy = (i.worldPos.y - _FogShadowCenterPoint.z);
				dy *= dy;
				f *= ((dx + dy) <= _WeatherMakerSkySphereRadiusSquared);
				return 1.0 - f;

#else

				return 1;

#endif

			}

			ENDCG
		}
		/*
		Pass
        { 
			Name "SkySphereCloudShadowCasterPass"
			Tags { "LightMode" = "ShadowCaster" }
			ZWrite On ZTest LEqual

			CGPROGRAM

			#pragma vertex shadowVert2
			#pragma fragment shadowFrag2
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster

			struct v2fShadow2
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
			};
 
			v2fShadow2 shadowVert2(appdata_full v)
			{
				v2fShadow2 o;
				o.worldPos = WorldSpaceVertexPosNear(v.vertex);
				TRANSFER_SHADOW_CASTER(o); 
				return o;
			}
 
			fixed4 shadowFrag2(v2fShadow2 i) : SV_Target
			{

#if defined(ENABLE_CLOUDS) || defined(ENABLE_CLOUDS_MASK)

				fixed f = ComputeCloudFBMFromWorldPos(i.worldPos);
				clip(f - _FogShadowThreshold);
				SHADOW_CASTER_FRAGMENT(i);

#else

				return 1;

#endif

			}

			ENDCG
        }
		*/
	}

	Fallback Off
}