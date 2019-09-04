Shader "Tutorial/05_TriplanarMapping"
{
	Properties{
		// _Color("Color", Color) = (0, 0, 0, 1)
		// _SecondaryColor ("Secondary Color", Color) = (1, 1, 1, 1)
		_Blend ("Blend Value", Range(0, 1)) = 0
		_MainTex("Texture", 2D) = "white" {}
		_SecondaryTex("Secondary Texture", 2D) = "white" {}
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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				// 顶点对应屏幕位置
				float4 position : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float3 normal : NORMAL;
			};

			float _Blend;
			// fixed4 _Color;
			// fixed4 _SecondaryColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _SecondaryTex;
			float4 _SecondaryTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos = worldPos.xyz;
				float3 worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
				o.normal = normalize(worldNormal);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(i.normal.xyz, 1);
			}
			ENDCG
		}
	}
}
