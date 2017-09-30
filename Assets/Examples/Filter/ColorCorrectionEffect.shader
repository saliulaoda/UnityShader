Shader "Custom/Color Adjustments/Color Correction Effect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
	_Saturation("Saturation", Float) = 1.0
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _RampTex;
uniform fixed _Saturation;
	
fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 orig = tex2D(_MainTex, i.uv);
	
	fixed rr = tex2D(_RampTex, orig.rr).r;
	fixed gg = tex2D(_RampTex, orig.gg).g;
	fixed bb = tex2D(_RampTex, orig.bb).b;
	
	fixed4 color = fixed4(rr, gg, bb, orig.a);

	fixed lum = Luminance(color.rgb);
	color.rgb = lerp(fixed3(lum,lum,lum), color.rgb, _Saturation);
	return color;		

}
ENDCG

	}
}

Fallback off

}
