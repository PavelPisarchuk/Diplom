﻿Shader "Custom/Stencil_Grass" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		_Glossiness ("Glossiness", Range (0.2, 3) ) = 3
		_FresnelPower ("Fresnel Power", Range (0.0, 3) ) = 3
		_FresnelBias ("Fresnel Bias", Range(0.00, 1)) = 0.0
		_CubeHDR ("Reflection Cubemap (RGBM)", Cube) = "" { TexGen CubeReflect }
		_CubeBlurHDR ("Blurred Reflection Cubemap (RGBM)", Cube) = "" { TexGen CubeReflect }
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 300

		CGPROGRAM
		#pragma surface surf BlinnPhongGsKit alpha
		#include "GraphicsKit.cginc"
		#pragma target 3.0

		
		fixed4 _Color;
		sampler2D _MainTex;
		fixed _Glossiness;
		half _FresnelPower;
		half _FresnelBias;	
		samplerCUBE _CubeHDR;
		samplerCUBE _CubeBlurHDR;

		struct Input {
			float2 uv_MainTex;
			float3 worldRefl;
			float3 viewDir;
			INTERNAL_DATA
		};
      
		void surf (Input IN, inout SurfaceOutputGsKit o) {
			fixed4 diffuse = tex2D (_MainTex, IN.uv_MainTex);			
			o.Albedo = diffuse.rgb * _Color.rgb;
			
			_Glossiness = _Glossiness * _Glossiness * 0.111111;
			o.Gloss = _Glossiness;
			
			o.Normal = half3(0.0f, 0.0f, 1.0f);
			
			float3 worldRefl = WorldReflectionVector (IN, o.Normal);
			half3 refl = DecodeHDR(texCUBE (_CubeHDR, worldRefl));
			half3 reflBlur = DecodeHDR(texCUBE (_CubeBlurHDR, worldRefl));			
			half3 resultRefl = lerp(reflBlur, refl, _Glossiness);			
			o.Reflect = resultRefl;
			
			half fresnel = 1.0f - saturate(dot (normalize(IN.viewDir), o.Normal));
			fresnel = pow(fresnel, _FresnelPower);
  			fresnel = min(fresnel + _FresnelBias, 1.0f);
			o.Fresnel = fresnel;
			
			o.Gloss = _Glossiness;
			o.Specular = _Glossiness;

			o.Alpha = (_Color.a + Luminance(o.Fresnel*resultRefl*_ReflectColor.rgb)) * diffuse.a;
		}
		ENDCG
	} 
	Fallback "Default"
}
