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

// https://alastaira.wordpress.com/2014/12/30/adding-shadows-to-a-unity-vertexfragment-shader-in-7-easy-steps/

Shader "WeatherMaker/WeatherMakerMoonShader"
{
	Properties
	{
		_MainTex("Moon Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue" = "Background+20" "RenderType" = "Background" "IgnoreProjector" = "True" "PerformanceChecks" = "False" }
		Cull Back Lighting Off ZWrite Off ZTest LEqual Blend Off

		CGINCLUDE

		#include "WeatherMakerSkyShader.cginc"

		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma glsl_no_auto_normalization
		#pragma multi_compile __ UNITY_HDR_ON
		#pragma multi_compile __ ENABLE_SUN_ECLIPSE

		sampler2D _WeatherMakerSkySphereTexture;
		int _MoonIndex;

		struct vertexInput
		{
			float4 vertex: POSITION;
			float4 normal: NORMAL;
			float4 texcoord: TEXCOORD0;
		};

		struct vertexOutput
		{
			float4 pos: SV_POSITION;
			float3 normalWorld: NORMAL;
			float2 tex: TEXCOORD0;
			float4 grabPos: TEXCOORD1;
			float3 ray : TEXCOORD2;
		};

		vertexOutput vert(vertexInput v)
		{
			vertexOutput o;
			o.normalWorld = normalize(WorldSpaceVertexPosNear(v.normal));
			o.pos = UnityObjectToClipPos(v.vertex);
			o.tex = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.grabPos = ComputeGrabScreenPos(o.pos);
			o.ray = WorldSpaceVertexPos(v.vertex) - _WorldSpaceCameraPos;

			return o;
		}

		fixed4 fragBase(vertexOutput i, fixed4 bgColor)
		{
			i.ray = normalize(i.ray);
			fixed nightMultiplier = max(0.0, 1.0 - _WeatherMakerSunColor.a);

#if defined(ENABLE_SUN_ECLIPSE)

			fixed lerpSun = CalcSunSpot(_WeatherMakerSunVar1.x * 1.6, _WeatherMakerSunDirectionUp, i.ray);
			fixed4 moonColor = bgColor * (1.0 - nightMultiplier);
			fixed lerper = max(pow(bgColor.a, 0.5), 1.0 - lerpSun);
			fixed feather = max((1.0 - pow(nightMultiplier, 8)), pow(abs(dot(i.ray, i.normalWorld)), 0.1));
			bgColor.a = moonColor.a = max(bgColor.a, lerpSun * feather);

#else

			fixed4 moonColor = tex2D(_MainTex, i.tex.xy);
			fixed lightNormal = max(0.0, dot(i.normalWorld, _WeatherMakerSunDirectionUp));
			fixed3 lightFinal = _WeatherMakerSunColor.rgb * lightNormal * _WeatherMakerMoonTintColor[_MoonIndex].rgb * _WeatherMakerMoonTintColor[_MoonIndex].a;
			fixed lightMax = max(lightFinal.r, max(lightFinal.g, lightFinal.b));
			fixed feather = pow(abs(dot(i.ray, i.normalWorld)), 0.5);

			// alpha ramps up as night approaches or is the maximum light value
			moonColor.a = max(pow(nightMultiplier, 8), lightMax);

			// reduce alpha during the day, at full night no alpha reduction
			moonColor.a *= (pow(nightMultiplier, 4) + 0.5) * 0.6667;

			// TODO: There is a tiny window where stars appear and show through the moon, fix this...

			moonColor.rgb *= lightFinal;
			moonColor.a *= feather;
			fixed lerper = max(pow(bgColor.a, 0.02), 1.0 - moonColor.a);
			bgColor.a = 1.0;

#endif

			return lerp(moonColor, bgColor, lerper);
		}

		fixed4 frag(vertexOutput i) : SV_TARGET
		{
			return fragBase(i, tex2Dproj(_WeatherMakerSkySphereTexture, i.grabPos));
		}

		ENDCG

		GrabPass { "_WeatherMakerSkySphereTexture" }

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