Shader "MyShaders/Ghost" {
	 Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _RimColor ("Rim Color", Color) = (0.46,0.0,1.0,0.0)
      _RimPower ("Rim Power", Range(-20,45)) = 20
    }
    SubShader {
      Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
      
    // extra pass that renders to depth buffer only
     Pass {
        ZWrite On
        ColorMask 0
       }
          
      CGPROGRAM
      #pragma surface surf Lambert alpha noambient nolightmap nodirlightmap  novertexlights
      struct Input {
          float2 uv_MainTex;
          float3 viewDir;//будет содержать направление взгляда, для расчёта параллакс эффектов, 
          //подсветки краёв модели (задней подсветки) и т.д.
      };
      sampler2D _MainTex;
      float4 _RimColor;
      float _RimPower;

      void surf (Input IN, inout SurfaceOutput o) {      	
          half rim =1.0-(dot (IN.viewDir, o.Normal));
          o.Emission = _RimColor.rgb *pow (rim, _RimPower);        
      }
      ENDCG
    } 
    Fallback off
  }