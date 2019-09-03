Shader "Tutorial/04_ColorInterpolation"
{
	Properties{
		_Color("Color", Color) = (0, 0, 0, 1)
		_SecondaryColor ("Secondary Color", Color) = (1, 1, 1, 1)
		_Blend ("Blend Value", Range(0, 1)) = 0
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { 
			"RenderType"="Opaque"
			"Queue"="Geometry"
		}

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
			};

			struct v2f
			{
				// 顶点对应屏幕位置
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float _Blend;
			fixed4 _Color;
			fixed4 _SecondaryColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col = _Color + _SecondaryColor * _Blend;
				return col;
			}
			ENDCG
		}
	}
}
