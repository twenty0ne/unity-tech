Shader "Tutorial/03_SimpleTransparency"
{
	Properties{
		_Color("Tint", Color) = (0.5, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { 
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				// 顶点对应屏幕位置
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= _Color;
				col *= i.color;
				return col;
			}
			ENDCG
		}
	}
}
