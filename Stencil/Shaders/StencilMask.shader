Shader "Custom/StencilMask" {
	Properties {
		_StencilVal ("stencilVal", Int) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)]		_StencilComp("Stencil Comparison", Float)	= 8
		[Enum(UnityEngine.Rendering.StencilOp)]				_StencilOp("Stencil Operation", Float)		= 2
		
		_StencilWriteMask("Stencil Write Mask", Float)				= 255
		_StencilReadMask("Stencil Read Mask", Float)				= 255
	}


	SubShader 
	{
		Tags
		{
			"RenderType"		= "StencilMaskOpaque" 
			"Queue"				= "Geometry-100" 
			"IgnoreProjector"	= "True"
		}
		ColorMask 0
		ZWrite off
		Stencil 
		{
			Ref [_StencilVal]			
			Comp[_StencilComp]	// always
			Pass[_StencilOp]	// replace


		}
		
		Pass
		{
		
		}
	}
}
