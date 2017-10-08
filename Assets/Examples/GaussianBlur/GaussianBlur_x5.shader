Shader "Custom/GaussianBlur_x5" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}

	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		//XX_TexelSize，XX纹理的像素相关大小width，height对应纹理的分辨率，x = 1/width, y = 1/height, z = width, w = height
		uniform half4 _MainTex_TexelSize;
		uniform float _blurSize;
	
		// weight curves

		static const half curve[2] = {0.238, 0.524};  

		static const half4 coordOffs = half4(1.0h, 1.0h, -1.0h, -1.0h);

		struct v2f_withBlurCoordsSGX
		{
			float4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
			half4 offs : TEXCOORD1;
		};

		v2f_withBlurCoordsSGX vertBlurHorizontalSGX (appdata_img v)
		{
			v2f_withBlurCoordsSGX o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			
			o.uv = v.texcoord.xy;
			half2 netFilterWidth = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _blurSize;
			half4 coords = -netFilterWidth.xyxy;
			o.offs = v.texcoord.xyxy + coords * coordOffs;
			return o; 
		}		
		
		v2f_withBlurCoordsSGX vertBlurVerticalSGX (appdata_img v)
		{
			v2f_withBlurCoordsSGX o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			
			o.uv = v.texcoord.xy;
			half2 netFilterWidth = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _blurSize;
			half4 coords = -netFilterWidth.xyxy;
			o.offs = v.texcoord.xyxy + coords * coordOffs;
			return o; 
		}

		half4 fragBlurSGX ( v2f_withBlurCoordsSGX i ) : SV_Target
		{
			half2 uv = i.uv;
			
			half4 color = tex2D(_MainTex, i.uv) * curve[1];
			

			half4 tapA = tex2D(_MainTex, i.offs.xy);
			half4 tapB = tex2D(_MainTex, i.offs.zw); 
			color += (tapA + tapB) * curve[0];
			color.a = 1;
			return color;
		}

					
	ENDCG
	
	SubShader {
		ZTest Always
		Cull Off
		ZWrite Off
		Blend Off
			
		Pass {
			NAME "GAUSSIAN_BLUR_X5_VERTICAL"
			CGPROGRAM 
			#pragma vertex vertBlurVerticalSGX
			#pragma fragment fragBlurSGX
			ENDCG
		}
		Pass {
			NAME "GAUSSIAN_BLUR_X5_HORIZONTAL"
			CGPROGRAM
			#pragma vertex vertBlurHorizontalSGX
			#pragma fragment fragBlurSGX
			ENDCG
		}	
	}	
	//屏幕后效果 一般关闭FallBack 失败就不显示
	FallBack Off
}
