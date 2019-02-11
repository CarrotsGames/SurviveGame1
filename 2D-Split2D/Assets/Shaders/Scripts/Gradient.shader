// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Gradient" 
{

	Properties
	{

		[PerRenderData] _MainTex("Sprite Texture" , 2D) = "White"{}
		_TopColour("Top Colour" , Color) = (1,1,1,1)
		_MidColour("Middle Colour" , Color) = (1,1,1,1)
		_BotColour("Bottom Colour" , Color) = (1,1,1,1)
		_Middle("Middle", Range(0.001, 0.999)) = 1
	}

		SubShader
		{
			Tags{ "Queue" = "Background"  "IgnoreProjector" = "True" }
			LOD 100
			ZWrite On
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				fixed4 _TopColour;
				fixed4 _MidColour;
				fixed4 _BotColour;
				float _Middle;
				struct v2f {
					float4 pos : SV_POSITION;
					float4 texcoord : TEXCOORD0;
				};
				v2f vert(appdata_full v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.texcoord = v.texcoord;
					return o;
				}
				fixed4 frag(v2f i) : COLOR{
					fixed4 c = lerp(_BotColour, _MidColour, i.texcoord.y / _Middle) * step(i.texcoord.y, _Middle);
				c += lerp(_MidColour, _TopColour, (i.texcoord.y - _Middle) / (1 - _Middle)) * step(_Middle, i.texcoord.y);
				c.a = 1;
				return c;
			}
			ENDCG
		}
	}
}