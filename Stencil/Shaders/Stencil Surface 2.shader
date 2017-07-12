Shader "Custom/Stencil Surface 2(Mull)" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Metallic ("Metallic", Range(0,1)) = 0.0

	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_SecondaryTex ("Combine Textures", 2D) = "white" {}
	_BumpMap2 ("Combine Textures Normalmap2", 2D) = "bump" {}

	
	_EdgeLength ("Edge length", Range(3,50)) = 10
	_Smoothness ("Smoothness", Range(0,1)) = 0.5
	
	
	_StencilVal ("stencilVal", Int ) = 0
	_Mull ("Mull", Range(0,1)) = 0.5

	_EmissionLM ("0-nonEmmision  1 - Emmision", Float) = 0
	_EmColor ("Main Color", Color) = (1,1,1,1)
	_EmissionTex ("Emission Texutres", 2D) = "white" {}

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
	
		Tags
		{
			//"Queue"="Geometry"
			//"IgnoreProjector"="False"	
			"RenderType"="Opaque"
		}


	LOD 200
	//Cull Off


CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

	float _EdgeLength;
	float _Smoothness;

	sampler2D _MainTex;
	sampler2D _EmissionTex;
	sampler2D _BumpMap;
	sampler2D _BumpMap2;
	fixed4 _Color;
	fixed4 _EmColor;
	fixed _EmissionLM;
	half _Shininess;
	half _Mull;
	half _Metallic;

	sampler2D _SecondaryTex;

	struct Input {
		float2 uv_MainTex;
		float2 uv_SecondaryTex;
		float2 uv_BumpMap;
		float2 uv_BumpMap2;
		float2 uv_EmissionTex;
	};

	struct SurfaceOut
	{
		fixed3 Albedo;
		fixed3 Normal;
		fixed3 Emission;
		half Specular;
		fixed3 Gloss;
		fixed Alpha;
	};

	void surf (Input IN, inout SurfaceOutputStandard o) 
	{
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 sectex = tex2D(_SecondaryTex, IN.uv_MainTex);
		
		
		o.Albedo = lerp(tex,sectex,_Mull)*_Color.rgb;//(tex.rgb*_Color.rgb)+(sectex.rgb*_Mull);//tex.rgb * _Color.rgb*(sectex.rgb*_Mull);
		//o.Gloss = tex.a;
		o.Alpha = tex.a ;
		o.Metallic = _Metallic;

		//o.Specular = _Shininess;
		o.Smoothness = _Shininess;
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
