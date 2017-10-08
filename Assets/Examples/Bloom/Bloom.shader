Shader "Custom/Bloom" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}

	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		//XX_TexelSize，XX纹理的像素相关大小width，height对应纹理的分辨率，x = 1/width, y = 1/height, z = width, w = height
		uniform half4 _MainTex_TexelSize;
		sampler2D _Bloom;
		uniform float _luminanceThreshold;

		struct v2f
		{
			float4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
		};

		v2f vertExtractBright (appdata_img v)
		{
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord;
			return o; 
		}

		fixed luminance(fixed4 color)
		{
			return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
		}

		fixed4 fragExtractBright(v2f i) : SV_Target
		{
			fixed4 c = tex2D(_MainTex, i.uv);
			fixed val = clamp(luminance(c) - _luminanceThreshold, 0.0, 1.0);
			return c * val;
		}

		struct v2f_bloom
		{
			float4 pos : SV_POSITION;
			half4 uv : TEXCOORD0;
		};

		v2f_bloom vertBloom(appdata_img v)
		{
			v2f_bloom o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv.xy = v.texcoord;
			o.uv.zw = v.texcoord;

#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0.0)
				o.uv.w = 1.0 - o.uv.w;
#endif
			return o;
		}

		fixed4 fragBloom(v2f_bloom i) : SV_Target
		{
			return tex2D(_MainTex, i.uv.xy) + tex2D(_Bloom, i.uv.zw);
		}
					
	ENDCG
	
	SubShader {
		ZTest Always
		Cull Off
		ZWrite Off
		Blend Off



		Pass {
			CGPROGRAM
			#pragma vertex vertExtractBright
			#pragma fragment fragExtractBright
			ENDCG
		}

		UsePass "Custom/GaussianBlur_x5/GAUSSIAN_BLUR_X5_VERTICAL"
		UsePass "Custom/GaussianBlur_x5/GAUSSIAN_BLUR_X5_HORIZONTAL"
		
		Pass {
			CGPROGRAM
			#pragma vertex vertBloom
			#pragma fragment fragBloom
			ENDCG
		}	
	}	
	//屏幕后效果 一般关闭FallBack 失败就不显示
	FallBack Off
}
