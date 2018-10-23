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

Shader "WeatherMaker/WeatherMakerParticleShader"
{
    Properties
	{
		_MainTex ("Color (RGB) Alpha (A)", 2D) = "gray" {}
		_TintColor ("Tint Color (RGB)", Color) = (1, 1, 1, 1)
		_PointSpotLightMultiplier ("Point/Spot Light Multiplier", Range (0, 10)) = 1
		_DirectionalLightMultiplier ("Directional Light Multiplier", Range (0, 10)) = 1
		_InvFade ("Soft Particles Factor", Range(0.01, 3.0)) = 1.0
		_AmbientLightMultiplier ("Ambient light multiplier", Range(0, 4)) = 1
		_Intensity ("Increase the alpha value by this multiplier", Range(0, 10)) = 1
		_SrcBlendMode ("SrcBlendMode (Source Blend Mode)", Int) = 5 // SrcAlpha
		_DstBlendMode ("DstBlendMode (Destination Blend Mode)", Int) = 10 // OneMinusSrcAlpha
		_ParticleDitherLevel("Dither Level", Range(0, 1)) = 0.002
    }

    SubShader
	{
        Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		LOD 100

        Pass
		{
			ZWrite Off
			Cull Back
            Lighting On     
			Blend [_SrcBlendMode] [_DstBlendMode]
						 
            CGPROGRAM

			#pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma glsl_no_auto_normalization
			#pragma multi_compile_particles
			#pragma multi_compile __ ORTHOGRAPHIC_MODE
			#pragma multi_compile __ WEATHER_MAKER_PER_PIXEL_LIGHTING
			#pragma multi_compile __ UNITY_HDR_ON

            #include "UnityCG.cginc"
			#include "WeatherMakerShader.cginc"

			fixed _ParticleDitherLevel;

			struct appdata_t
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

		    struct v2f
            {
                half2 uv_MainTex : TEXCOORD0;
                fixed4 color : COLOR0;
                float4 pos : SV_POSITION;

#if defined(WEATHER_MAKER_PER_PIXEL_LIGHTING)

				float3 viewPos : TEXCOORD1;

#endif

#if defined(SOFTPARTICLES_ON)

                float4 projPos : TEXCOORD2;

#endif

            };
 
            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);


#if defined(WEATHER_MAKER_PER_PIXEL_LIGHTING)

				o.viewPos = UnityObjectToViewPos(v.vertex);
				o.color = v.color * _TintColor;

#else

				o.color = CalculateVertexColor(UnityObjectToViewPos(v.vertex).xyz, 1.0) * v.color * _TintColor;

#endif

				// o.color = v.color * _TintColor; // temp if you want to disable lighting

#if defined(SOFTPARTICLES_ON)

                o.projPos = ComputeScreenPos(o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);

#endif

                return o; 
            }
			
            fixed4 frag (v2f i) : COLOR
			{       

#if defined(WEATHER_MAKER_PER_PIXEL_LIGHTING)

				fixed4 color = tex2D(_MainTex, i.uv_MainTex) * i.color * CalculateVertexColor(i.viewPos, 1.0);

#else

				fixed4 color = tex2D(_MainTex, i.uv_MainTex) * i.color;

#endif

				color.a = saturate(color.a * _Intensity);

#if defined(SOFTPARTICLES_ON)

				float sceneZ = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
				float partZ = i.projPos.z;
				float diff = (sceneZ - partZ);
				color.a *= saturate(_InvFade * diff);

				// dither
				ApplyDither(color.rgb, i.projPos.xy, fixed3(0.06711056, 0.00583715, 52.9829189), _ParticleDitherLevel);

#endif

				return color;
            }

            ENDCG
        }
    }
 
    Fallback "Particles/Alpha Blended"
}