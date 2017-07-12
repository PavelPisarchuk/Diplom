Shader "Custom/Standart" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
			_BumpMap ("Normalmap", 2D) = "bump" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		
			_StencilVal ("stencilVal", Int ) = 0

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
		LOD 700
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
				float2 uv_BumpMap;

		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;



		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}
	FallBack "Default"
}
