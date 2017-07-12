Shader "Custom/qqqqq" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0


		_BumpMap ("Normalmap", 2D) = "bump" {}
		_SecondaryTex ("Combine Textures", 2D) = "white" {}

		_StencilVal ("stencilVal", Int ) = 0
		_Mull ("Mull", Range(0,1)) = 0

		_EmissionLM ("0-nonEmmision  1 - Emmision", Float) = 0
		_EmColor ("Main Color", Color) = (1,1,1,1)
		_EmissionTex ("Emission Texutres", 2D) = "white" {}


	}
	SubShader {
		Stencil 
		{
			Ref [_StencilVal]
			Comp equal
			Pass keep
			Fail keep
		}	
	


		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SecondaryTex;
		sampler2D _EmissionTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SecondaryTex;
			float2 uv_BumpMap;
			float2 uv_EmissionTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;


		fixed4 _EmColor;
		fixed _EmissionLM;
		half _Mull;



		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 sectex = tex2D(_SecondaryTex, IN.uv_MainTex) * _Color;

			o.Albedo =lerp(tex.rgb,sectex.rgb,_Mull);// tex.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = tex.a;


			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap) );
			if(_EmissionLM==1){
				fixed4 emisionTex = tex2D(_EmissionTex, IN.uv_EmissionTex);
				o.Emission = tex.rgb*emisionTex.a*_EmColor.rgb;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
