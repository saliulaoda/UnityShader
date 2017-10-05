Shader "Custom/Color Adjustments/Color Correction Effect" 
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
		_Saturation("Saturation", Float) = 1.0//饱和度
		_Brightness("Brightness", Float) = 1.0//亮度
		_Contrast("Contrast", Float) = 1.0//对比度
	}

	SubShader 
	{
		Pass 
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform sampler2D _RampTex;
			uniform fixed _Saturation;
			uniform fixed _Brightness;
			uniform fixed _Contrast;
			
			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 orig = tex2D(_MainTex, i.uv);
				
				fixed rr = tex2D(_RampTex, orig.rr).r;
				fixed gg = tex2D(_RampTex, orig.gg).g;
				fixed bb = tex2D(_RampTex, orig.bb).b;
				
				fixed4 color = fixed4(rr, gg, bb, orig.a);
			
				fixed lum = Luminance(color.rgb);
				color.rgb = lerp(fixed3(lum,lum,lum), color.rgb, _Saturation);
				

				//brigtness亮度直接乘以一个系数，也就是RGB整体缩放，调整亮度  
				fixed3 finalColor = color * _Brightness;

				//saturation饱和度：首先根据公式计算同等亮度情况下饱和度最低的值：  
				fixed gray = 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
				fixed3 grayColor = fixed3(gray, gray, gray);
				//根据Saturation在饱和度最低的图像和原图之间差值  
				finalColor = lerp(grayColor, finalColor, _Saturation);

				//contrast对比度：首先计算对比度最低的值  
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				//根据Contrast在对比度最低的图像和原图之间差值  
				finalColor = lerp(avgColor, finalColor, _Contrast);

				//返回结果，alpha通道不变  
				return fixed4(finalColor, color.a);
			}
			ENDCG
		}
	}
	Fallback off
}
