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

// Resources:
// http://library.nd.edu/documents/arch-lib/Unity/SB/Assets/SampleScenes/Shaders/Skybox-Procedural.shader
//

Shader "WeatherMaker/WeatherMakerSkySphereShader"
{
	Properties
	{
		_MainTex("Day Texture", 2D) = "blue" {}
		_DawnDuskTex("Dawn/Dusk Texture", 2D) = "orange" {}
		_NightTex("Night Texture", 2D) = "black" {}
		_DayMultiplier("Day Multiplier", Range(0, 3)) = 1
		_DawnDuskMultiplier("Dawn/Dusk Multiplier", Range(0, 1)) = 0
		_NightMultiplier("Night Multiplier", Range(0, 1)) = 0
		_NightSkyMultiplier("Night Sky Multiplier", Range(0, 1)) = 0
		_NightVisibilityThreshold("Night Visibility Threshold", Range(0, 1)) = 0
		_NightIntensity("Night Intensity", Range(0, 32)) = 2
		_NightTwinkleSpeed("Night Twinkle Speed", Range(0, 100)) = 16
		_NightTwinkleVariance("Night Twinkle Variance", Range(0, 10)) = 1
		_NightTwinkleMinimum("Night Twinkle Minimum Color", Range(0, 1)) = 0.02
		_NightTwinkleRandomness("Night Twinkle Randomness", Range(0, 5)) = 0.15
		_FogDensity("Fog Density", Range(0.0, 1.0)) = 0.0
		_FogColor("Fog Color", Color) = (1,1,1,1)
		_FogEmissionColor("Fog Emission Color", Color) = (0,0,0,0)
		_FogNoise("Fog Noise", 2D) = "white" {}
		_FogNoise2("Fog Noise2", 2D) = "clear" {}
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
		_FogLightAbsorption("Fog Light Absorption", Range(0.0, 1.0)) = 0.013
		_FogSharpness("Fog Sharpness", Range(0.0, 1.0)) = 0.015
		_FogWhispiness("Fog Whispiness", Range(0.0, 3.0)) = 1.0
		_FogWhispinessChangeFactor("Fog Whispiness Change Factor", Range(0.0, 1.0)) = 0.03
		_PointSpotLightMultiplier("Point/Spot Light Multiplier", Range(0, 10)) = 1
		_DirectionalLightMultiplier("Directional Light Multiplier", Range(0, 10)) = 1
		_AmbientLightMultiplier("Ambient light multiplier", Range(0, 4)) = 1
		_WeatherMakerSkyDitherLevel("Sky dither factor", Range(0, 1)) = 0.005
	}
	SubShader
	{
		Tags { "Queue" = "Background+10" "RenderType" = "Background" "IgnoreProjector" = "True" "PerformanceChecks" = "False" }
		Cull Front Lighting Off ZWrite Off ZTest LEqual Blend Off

		CGINCLUDE

		#include "WeatherMakerSkyShader.cginc"

		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma glsl_no_auto_normalization
		#pragma multi_compile __ UNITY_HDR_ON
		#pragma multi_compile __ ENABLE_SUN_ECLIPSE
		#pragma multi_compile __ ENABLE_CLOUDS ENABLE_CLOUDS_MASK
		#pragma multi_compile __ ENABLE_PROCEDURAL_TEXTURED_SKY ENABLE_PROCEDURAL_SKY
		#pragma multi_compile __ ENABLE_NIGHT_TWINKLE

		fixed4 GetNightColor(v2fSky i, fixed skyAlpha)
		{
			fixed4 nightColor = tex2D(_NightTex, i.uv);
			fixed maxValue = max(nightColor.r, max(nightColor.g, nightColor.b));

#if defined(ENABLE_NIGHT_TWINKLE)

			fixed twinkleRandom = _NightTwinkleRandomness * RandomFloat(i.ray * _Time.y);
			fixed twinkle = 1.0 + twinkleRandom + (_NightTwinkleVariance * sin(_NightTwinkleSpeed * _Time.w * maxValue));
			fixed noTwinkle = saturate(ceil(maxValue - _NightTwinkleMinimum));
			twinkle *= noTwinkle;
			twinkle += (1.0 - noTwinkle);
			nightColor *= twinkle;

#endif

			nightColor *= ceil(maxValue - _NightVisibilityThreshold) * _NightIntensity * _NightSkyMultiplier;

			return nightColor;
		}

		v2fSky vert(appdata v)
		{
			v2fSky o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv.xy = v.uv; // TRANSFORM_TEX not supported
			o.ray = WorldSpaceVertexPos(v.vertex) - _WorldSpaceCameraPos;
			procedural_sky_info i = CalculateScatteringCoefficients(_WeatherMakerSunDirectionUp, _WeatherMakerSunColor.rgb * pow(_WeatherMakerSunColor.a, 0.5), 1.0, normalize(o.ray));
			o.inScatter = i.inScatter;
			o.outScatter = i.outScatter;
			return o;
		}

		fixed4 fragBase(v2fSky i)
		{
			fixed4 result;
			i.ray = normalize(i.ray);
			procedural_sky_info p = CalculateScatteringColor(_WeatherMakerSunDirectionUp, _WeatherMakerSunColor.rgb, _WeatherMakerSunVar1.x, i.ray, i.inScatter, i.outScatter);
			fixed sunMoon;
			fixed4 skyColor = p.skyColor;
			fixed4 nightColor = GetNightColor(i, skyColor.a);

#if defined(ENABLE_CLOUDS) || defined(ENABLE_CLOUDS_MASK)

			fixed4 cloudColor;
			float3 worldPos;
			float3 cloudRay = float3(i.ray.x, i.ray.y + _WeatherMakerCloudRayOffset, i.ray.z);

#if defined(ENABLE_PROCEDURAL_TEXTURED_SKY) || defined(ENABLE_PROCEDURAL_SKY)

			fixed sunAngleAmount = (1.0 - pow(_WeatherMakerSunColor.a * 0.7, 8.0)) * pow(1.0 - abs(max(0.0, min(1.0, _WeatherMakerSunDirectionUp.y + 0.04))), 8.0);
			fixed4 sunLightColor =  lerp(_WeatherMakerSunColor, fixed4(p.inScatter, _WeatherMakerSunColor.a), sunAngleAmount);
			cloudColor = ComputeCloudColor(cloudRay, sunLightColor, worldPos);

#else

			cloudColor = ComputeCloudColor(cloudRay, _WeatherMakerSunColor, worldPos);

#endif

#endif

#if defined(ENABLE_PROCEDURAL_TEXTURED_SKY)

			fixed4 dayColor = tex2D(_MainTex, i.uv) * _DayMultiplier;
			fixed4 dawnDuskColor = tex2D(_DawnDuskTex, i.uv);
			fixed4 dawnDuskColor2 = dawnDuskColor * _DawnDuskMultiplier;
			dayColor += dawnDuskColor2;

			// hide night texture wherever dawn/dusk is opaque, reduce if clouds
			nightColor.rgb *= (1.0 - dawnDuskColor.a);

#if defined(ENABLE_CLOUDS) || defined(ENABLE_CLOUDS_MASK)

			nightColor.rgb *= saturate(1.0 - (cloudColor.a * CLOUD_ALPHA_NIGHT_COLOR_REDUCER_MULTIPLIER));

#endif

			// blend texture on top of sky
			result = ((dayColor * dayColor.a) + (skyColor * (1.0 - dayColor.a)));

			// blend previous result on top of night
			result = ((result * result.a) + (nightColor * (1.0 - result.a)));

#elif defined(ENABLE_PROCEDURAL_SKY)

#if defined(ENABLE_CLOUDS) || defined(ENABLE_CLOUDS_MASK)

			// reduce night color behind clouds
			nightColor.rgb *= saturate(1.0 - (cloudColor.a * CLOUD_ALPHA_NIGHT_COLOR_REDUCER_MULTIPLIER));

#endif

			result = skyColor + nightColor;

#else

			fixed4 dayColor = tex2D(_MainTex, i.uv) * _DayMultiplier;
			fixed4 dawnDuskColor = (tex2D(_DawnDuskTex, i.uv) * _DawnDuskMultiplier);
			result = (dayColor + dawnDuskColor + nightColor);

#endif

#if defined(ENABLE_CLOUDS) || defined(ENABLE_CLOUDS_MASK)

			// blend clouds on top of result
			result = ((cloudColor * cloudColor.a) + (result * (1.0 - cloudColor.a)));
			result.a = cloudColor.a;

#else

			result.a = 0.0;

#endif

			ApplyDither(result.rgb, i.uv, fixed3(0.06711056, 100.00583715, 452.9829189), _WeatherMakerSkyDitherLevel);
			return result;
		}

		fixed4 frag(v2fSky i) : SV_Target
		{
			return fragBase(i);
		}

		ENDCG

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			ENDCG
		}
	}

	FallBack Off
}
