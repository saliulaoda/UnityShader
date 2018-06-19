Shader "Custom/Distort" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NoiseTex ("Base (RGB)", 2D) = "black" {}
		_MaskTex ("Mask", 2D) = "black" {}
	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		uniform sampler2D _MaskTex;
		uniform float _DistortTimeFactor;
		uniform float _DistortStrength;

		fixed4 frag(v2f_img i) : SV_Target
		{
			//根据时间改变采样噪声图获得随机的输出  
			float4 noise = tex2D(_NoiseTex, i.uv - _Time.xy * _DistortTimeFactor);
			//以随机的输出*控制系数得到偏移值  
			float2 offset = noise.xy * _DistortStrength;
			//像素采样时偏移offset  

			//采样Mask图获得权重信息  
			fixed4 factor = tex2D(_MaskTex, i.uv);
			//像素采样时偏移offset，用Mask权重进行修改  
			float2 uv = offset * factor.r + i.uv;

			//float2 uv = offset + i.uv;
			return tex2D(_MainTex, uv);
		}
					
	ENDCG
	
	SubShader {
		Pass
		{
			ZTest Always
			Cull Off
			ZWrite Off
			Fog{ Mode off }

			CGPROGRAM
			#pragma vertex vert_img  
			#pragma fragment frag  
			//#pragma fragmentoption ARB_precision_hint_fastest   
			#pragma fragmentoption ARB_precision_hint_nicest
			ENDCG
		}
	}

	FallBack Off
}
