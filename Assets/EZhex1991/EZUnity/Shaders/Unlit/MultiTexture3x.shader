// Author:			ezhex1991@outlook.com
// CreateTime:		2018-08-24 13:48:29
// Organization:	#ORGANIZATION#
// Description:		

Shader "EZUnity/Unlit/MultiTexture3x" {
	Properties {
		[Header(Base)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[KeywordEnum(UV0, UV1)] _MainUV ("Main UV", Int) = 0
		_Color ("Color", Color) = (1, 1, 1, 1)

		[Header(Additional 1)]
		_Add1Tex ("Texture", 2D) = "black" {}
		[KeywordEnum(UV0, UV1)] _Add1UV ("UV", Int) = 0
		_Add1Color ("Color", Color) = (1, 1, 1, 1)
		[KeywordEnum(AlphaBlend, Additive, Multiply)] _Add1BlendMode ("Blend Mode", Int) = 0
		
		[Header(Additional 2)]
		_Add2Tex ("Texture", 2D) = "black" {}
		[KeywordEnum(UV0, UV1)] _Add2UV ("UV", Int) = 0
		_Add2Color ("Color", Color) = (1, 1, 1, 1)
		[KeywordEnum(AlphaBlend, Additive, Multiply)] _Add2BlendMode ("Blend Mode", Int) = 0
		
		[Header(Rendering Settings)]
		[HideInInspector] _AlphaTex ("Alpha Tex (R)", 2D) = "white" {}
		[HideInInspector][KeywordEnum(None, AlphaTest, AlphaBlend, AlphaPremultiply)] _AlphaMode ("Alpha Mode", Float) = 0
		[HideInInspector] _AlphaClipThreshold ("Alpha Clip Threshold", Range(0, 1)) = 0.5
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendMode ("Src Blend Mode", Float) = 1
		[HideInInspector][Enum(UnityEngine.Rendering.BlendMode)] _DstBlendMode ("Dst Blend Mode", Float) = 0
		[HideInInspector][Toggle] _ZWriteMode ("ZWrite", Float) = 1
		[HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
		[HideInInspector] _OffsetFactor ("Offset Factor", Float) = 0
		[HideInInspector] _OffsetUnit ("Offset Unit", Float) = 0
	}
	CustomEditor "EZRenderingSettingsShaderGUI"
	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			Blend [_SrcBlendMode] [_DstBlendMode]
			ZWrite [_ZWriteMode]
			Cull [_CullMode]
			Offset [_OffsetFactor], [_OffsetUnit]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _MAINUV_UV0 _MAINUV_UV1
			#pragma shader_feature _ADD1UV_UV0 _ADD1UV_UV1
			#pragma shader_feature _ADD1BLENDMODE_ALPHABLEND _ADD1BLENDMODE_ADDITIVE _ADD1BLENDMODE_MULTIPLY
			#pragma shader_feature _ADD2UV_UV0 _ADD2UV_UV1
			#pragma shader_feature _ADD2BLENDMODE_ALPHABLEND _ADD2BLENDMODE_ADDITIVE _ADD2BLENDMODE_MULTIPLY
            #pragma shader_feature _ _ALPHAMODE_ALPHATEST _ALPHAMODE_ALPHABLEND _ALPHAMODE_ALPHAPREMULTIPLY
			#pragma shader_feature _ _ALPHATEX_ON

			#include "UnityCG.cginc"
			
			// Rendering Settings
			sampler2D _AlphaTex;
			fixed _AlphaClipThreshold;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			int _MainUV;
			fixed4 _Color;

			sampler2D _Add1Tex;
			float4 _Add1Tex_ST;
			int _Add1UV;
			fixed4 _Add1Color;
			int _Add1BlendMode;

			sampler2D _Add2Tex;
			float4 _Add2Tex_ST;
			int _Add2UV;
			fixed4 _Add2Color;
			int _Add2BlendMode;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float2 uv_Add1Tex : TEXCOORD1;
				float2 uv_Add2Tex : TEXCOORD2;
			};

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				#if _MAINUV_UV0
					o.uv_MainTex = TRANSFORM_TEX(v.uv0, _MainTex);
				#elif _MAINUV_UV1
					o.uv_MainTex = TRANSFORM_TEX(v.uv1, _MainTex);
				#endif
				#if _ADD1UV_UV0
					o.uv_Add1Tex = TRANSFORM_TEX(v.uv0, _Add1Tex);
				#elif _ADD1UV_UV1
					o.uv_Add1Tex = TRANSFORM_TEX(v.uv1, _Add1Tex);
				#endif
				#if _ADD2UV_UV0
					o.uv_Add2Tex = TRANSFORM_TEX(v.uv0, _Add2Tex);
				#elif _ADD2UV_UV1
					o.uv_Add2Tex = TRANSFORM_TEX(v.uv1, _Add2Tex);
				#endif
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				fixed4 color = tex2D(_MainTex, i.uv_MainTex) * _Color;
				
				// Rendering Settings
				#if !_ALPHAMODE_NONE
					#if _ALPHATEX_ON
						color.a = tex2D(_AlphaTex, i.uv_MainTex).r;
					#endif
					#if _ALPHAMODE_ALPHATEST
						clip(color.a - _AlphaClipThreshold);
					#endif
				#else
					color.a = 1;
				#endif

				fixed4 add1Color = tex2D(_Add1Tex, i.uv_Add1Tex) * _Add1Color;
				#if _ADD1BLENDMODE_ALPHABLEND
					color.rgb = add1Color.rgb * add1Color.a + color.rgb * (1 - add1Color.a);
				#elif _ADD1BLENDMODE_ADDITIVE
					color.rgb += add1Color.rgb;
				#elif _ADD1BLENDMODE_MULTIPLY
					color.rgb *= add1Color.rgb;
				#endif
				
				fixed4 add2Color = tex2D(_Add2Tex, i.uv_Add2Tex) * _Add2Color;
				#if _ADD2BLENDMODE_ALPHABLEND
					color.rgb = add2Color.rgb * add2Color.a + color.rgb * (1 - add2Color.a);
				#elif _ADD2BLENDMODE_ADDITIVE
					color.rgb += add2Color.rgb;
				#elif _ADD2BLENDMODE_MULTIPLY
					color.rgb *= add2Color.rgb;
				#endif

				return color;
			}
			ENDCG
		}
		//UsePass "VertexLit/SHADOWCASTER"
	}
}
