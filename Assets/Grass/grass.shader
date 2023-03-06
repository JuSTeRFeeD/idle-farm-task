Shader "Custom/grass" {

	Properties{
	
		[NoScaleOffset] _MainTex("Grass A", 2D) = "white" {}
		[NoScaleOffset] _GrassTwo("Grass B", 2D) = "white" {}
		_Splat("Splat", 2D) = "white" {}
		
		_Cutoff("Cutoff", Range(0,1)) = 0.25
		
		_GrassHeight("Grass height", Float) = 0.25
		
		_WindSpeed("Wind speed", Float) = 100
		_WindStength("Wind strength", Float) = 0.05
		
	}
	
	SubShader{
	
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 200

		Pass{

		CULL OFF

		CGPROGRAM
		
		#include "UnityCG.cginc" 
		#pragma vertex vert
		#pragma fragment frag
		#pragma geometry geom
		//#pragma alphatest : _Cutoff

		// Use shader model 4.0 target, we need geometry shader support
		#pragma target 4.0

		sampler2D _MainTex, _GrassTwo, _Splat;

		struct v2g{
		
			float4 pos : SV_POSITION;
			float3 norm : NORMAL;
			float2 uv : TEXCOORD0;
			float3 color : TEXCOORD1;
			
		};

		struct g2f{
		
			float4 pos : SV_POSITION;
			float3 norm : NORMAL;
			float2 uv : TEXCOORD0;
			float3 diffuseColor : TEXCOORD1;
			
			//float3 specularColor : TEXCOORD2;
			
		};
		
		half _GrassHeight;
		half _Cutoff;
		half _WindStength;
		half _WindSpeed;

		v2g vert(appdata_full v){
		
			float3 v0 = v.vertex.xyz;

			v2g OUT;
			
			OUT.pos = v.vertex;
			OUT.norm = v.normal;
			OUT.uv = v.texcoord;
			OUT.color = tex2Dlod(_MainTex, v.texcoord).rgb;
			
			return OUT;
			
		}
	
		[maxvertexcount(24)]
		void geom(point v2g IN[1], inout TriangleStream<g2f> triStream){
		
			float3 lightPosition = _WorldSpaceLightPos0;

			float3 perpendicularAngle = float3(0, 0, 1);
			float3 faceNormal = cross(perpendicularAngle, IN[0].norm);

			float3 v0 = IN[0].pos.xyz;
			float3 v1 = IN[0].pos.xyz + IN[0].norm * _GrassHeight;

			float3 wind =
			
				float3(sin(_Time.x * _WindSpeed + v0.x) + sin(_Time.x * _WindSpeed + v0.z * 2) +
				sin(_Time.x * _WindSpeed * 0.1 + v0.x), 0,
				cos(_Time.x * _WindSpeed + v0.x * 2) + cos(_Time.x * _WindSpeed + v0.z));
			
			v1 += wind * _WindStength;

			float3 color = (IN[0].color);

			float sin30 = 0.5;
			float sin60 = 0.866f;
			float cos30 = sin60;
			float cos60 = sin30;

			g2f OUT;

			// Quad 1

			OUT.pos = UnityObjectToClipPos(v0 + perpendicularAngle * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(1, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1 + perpendicularAngle * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(1, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1 - perpendicularAngle * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0 - perpendicularAngle * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 1);
			triStream.Append(OUT);

			// Quad 2

			OUT.pos = UnityObjectToClipPos(v0 + float3(sin60, 0, -cos60) * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(1, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1 + float3(sin60, 0, -cos60)* 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(1, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0 - float3(sin60, 0, -cos60) * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1 - float3(sin60, 0, -cos60) * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 1);
			triStream.Append(OUT);

			// Quad 3 - Positive

			OUT.pos = UnityObjectToClipPos(v0 + float3(sin60, 0, cos60) * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(1, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1 + float3(sin60, 0, cos60)* 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(1, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 1);
			triStream.Append(OUT);

			// Quad 3 - Negative
			
			OUT.pos = UnityObjectToClipPos(v0 - float3(sin60, 0, cos60) * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1 - float3(sin60, 0, cos60) * 0.5 * _GrassHeight);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0, 1);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v0);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 0);
			triStream.Append(OUT);

			OUT.pos = UnityObjectToClipPos(v1);
			OUT.norm = faceNormal;
			OUT.diffuseColor = color;
			OUT.uv = float2(0.5, 1);
			triStream.Append(OUT);
			
		}

		half4 frag(g2f IN) : COLOR{
		
			fixed4 c = tex2D(_MainTex, IN.uv);

			clip(c.a - _Cutoff);
		
			return c;
		
		}
		
		ENDCG

		}
		
	}
	
}