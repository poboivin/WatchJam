Shader "Unlit/PaletteSwap"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_In0("in0", Color) = (1,1,1,1)
		_Out0("Out0", Color) = (1,1,1,1)
		_In1("in1", Color) = (1,1,1,1)
		_Out1("Out1", Color) = (1,1,1,1)
		_In2("in2", Color) = (1,1,1,1)
		_Out2("Out2", Color) = (1,1,1,1)
		_In3("in3", Color) = (1,1,1,1)
		_Out3("Out3", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			fixed4 _In0;
			fixed4 _Out0;
			fixed4 _In1;
			fixed4 _Out1;
			fixed4 _In2;
			fixed4 _Out2;
			fixed4 _In3;
			fixed4 _Out3;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				// apply fog
			if (abs(length(col.rgb - _In0.rgb)) < 0.18)
				col = _Out0;
			if (abs(length(col.rgb - _In1.rgb)) < 0.18)
				col = _Out1;
			if (abs(length(col.rgb - _In2.rgb)) < 0.18)
				col = _Out2;
			if (abs(length(col.rgb - _In3.rgb)) < 0.18)
				col = _Out3;

		
				/*if (all(col.rgb == _In0.rgb))
					col = _Out0;
				if (all(col.rgb == _In1.rgb))
					col = _Out1;
				if (all(col.rgb == _In2.rgb))
					col = _Out2;
				if (all(col.rgb == _In3.rgb))
					col = _Out3;*/

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;

				//return col;
			}
			ENDCG
		}
	}
}
