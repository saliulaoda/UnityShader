Shader "Custom/Color Adjustments/Color Correction Mask" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MaskTex("Base (RGB)", 2D) = "grayscaleRamp" {}
	}

		SubShader{
		Pass{
		ZTest Always Cull Off ZWrite Off
		//Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform sampler2D _MaskTex;

		fixed4 frag(v2f_img i) : SV_Target
		{
			fixed4 orig = tex2D(_MainTex, i.uv);
			fixed4 mask = tex2D(_MaskTex, i.uv);
			return fixed4(lerp(orig.rgb, mask.rgb, mask.a), 1);
		}
		ENDCG

		}
	}

	Fallback off
}
