Shader "Custom/Stencil_Main" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_BackColor("Color (Chameleon Paint)", Color) = (0,0,0,0)
		_Paint("Paint: RED - Gloss, GREEN - Metallic", Color) = (0,1,0,0)

		_Flakes("Flakes",2D) = "black" {}
		_SecondaryTex ("Combine Textures", 2D) = "white" {}
		_Mul ("Multipli textures", Range (0, 1)) = 0.0


		_BumpMap ("Normalmap", 2D) = "bump" { }
		_BumpAmount ("Bump Amount", Range (0, 2)) = 1.0
		_MetallicFalloff("Metallic Falloff", Range (0, 10) ) = 2.5
		_ChameleonFalloff("Chameleon Falloff", Range (-10, 5)) = 5.0
		_FresnelPower("Fresnel Power", Range (0, 5) ) = 5.0
		_FresnelBias("Fresnel Bias", Range (0, 5) ) = 0.0
		_BlurReflIntens("Blur Reflection Intensity", Range (0, 5) ) = 0.0
		
		_StencilVal ("stencilVal", Int ) = 0
	}
	
	SubShader 
	{
		Stencil 
		{
			Ref [_StencilVal]
			Comp equal
			Pass keep
			Fail keep
		}	
	
	
		Tags{"Queue"="Geometry"
			"IgnoreProjector"="False"			"RenderType"="StencilOpaque"		}	
			
			
		Cull Off
		LOD 600
		//ZWrite On
		//UsePass "Transparent/Diffuse/FORWARD"
		
	CGPROGRAM
	#pragma surface surf CarPaintGsKit
	#include "GraphicsKit.cginc"
	#pragma target 3.0
	
	fixed4 _Color;
	fixed4 _BackColor;
	fixed4 _Paint;
	fixed _MetallicFalloff;
	fixed _ChameleonFalloff;
	fixed _FresnelPower;
	fixed _FresnelBias;
	fixed _BlurReflIntens;

	sampler2D _Flakes; 
	sampler2D _SecondaryTex;
	fixed _Mul;


	sampler2D _BumpMap;
	half _BumpAmount;	

	struct Input 
	{
		fixed3 worldRefl;
		fixed3 viewDir;
		fixed3 worldNormal;
		fixed2 uv_Flakes;
		float2 uv_BumpMap;
		INTERNAL_DATA
	};
				
		void surf (Input IN, inout SurfaceOutputCarPaintGsKit o) 
		{				
			fixed3 FrontColorTex = _Color.rgb;
			fixed3 BackColorTex = _BackColor.rgb;
			
			fixed3 MixTex = _Paint.rgb;
			
			fixed GlossyA = max(0, MixTex.r - MixTex.g);
			fixed MetallicA = max(0, MixTex.g - MixTex.r - MixTex.b);
			fixed MatteA = MixTex.b;
			fixed ChameleonA = MixTex.r*MixTex.g;
			
			half3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));		
			o.Normal = lerp(half3(0.0f, 0.0f, 1.0f), normal, _BumpAmount);
			
			float3 worldRefl = WorldReflectionVector (IN, o.Normal);
			
			half Fresnel = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
				
			half FresnelRefl = pow(Fresnel, _FresnelPower);
			FresnelRefl = min(1, FresnelRefl + _FresnelBias );
			
			fixed FresnelColors = saturate(pow(Fresnel, _MetallicFalloff)*(MetallicA) +
											pow(Fresnel, _ChameleonFalloff)*(ChameleonA));				
						
			fixed3 MixColors = lerp(FrontColorTex,  BackColorTex*ChameleonA, FresnelColors);



			Flakes = tex2D(_Flakes, IN.uv_Flakes);//*_Color.rgb;;		
			float4 overlayTex = tex2D (_SecondaryTex, IN.uv_Flakes);

			half3 mainTexVisible = Flakes.rgb ;
         	half3 overlayTexVisible = overlayTex.rgb * (overlayTex.a);

			
			o.Albedo = (mainTexVisible + overlayTexVisible*_Mul) * _Color; //Flakes.rgb;//MixColors;
			o.PaintMasks = half3(MetallicA, MatteA, ChameleonA);

			o.ReflectMasks = half2(FresnelRefl, _BlurReflIntens);
			o.Specular = 1.0f;
		}
		
	ENDCG
		}
	
	Fallback "Default"
}
