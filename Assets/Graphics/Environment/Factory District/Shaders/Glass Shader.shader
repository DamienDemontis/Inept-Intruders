// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PolyPixel/GlassShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_GlassColor("Glass Color", Color) = (0.8046605,0.8650721,0.8897059,0)
		_GlassGrunge("Glass Grunge", 2D) = "white" {}
		_GrungeAmount("Grunge Amount", Range( 0 , 3)) = 0.5
		_GlassReflectionAmount("Glass Reflection Amount", Range( 0 , 1)) = 0.98
		_BlankNormal("Blank Normal", 2D) = "white" {}
		_GlassWobbleNormal("Glass Wobble Normal", 2D) = "white" {}
		_GlassWrapingScale("Glass Wraping Scale", Float) = 0
		_GlassWrapingStrength("Glass Wraping Strength", Float) = 0.1339
		_Opacity("Opacity", Range( 0 , 1)) = 0.94
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 5.0
		#pragma multi_compile _ LOD_FADE_CROSSFADE
		struct Input
		{
			float2 uv_texcoord;
			UNITY_DITHER_CROSSFADE_COORDS
		};

		uniform sampler2D _BlankNormal;
		uniform float4 _BlankNormal_ST;
		uniform sampler2D _GlassWobbleNormal;
		uniform float _GlassWrapingScale;
		uniform float _GlassWrapingStrength;
		uniform float4 _GlassColor;
		uniform sampler2D _GlassGrunge;
		uniform float4 _GlassGrunge_ST;
		uniform float _GrungeAmount;
		uniform float _GlassReflectionAmount;
		uniform float _Opacity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			UNITY_TRANSFER_DITHER_CROSSFADE( o, v.vertex );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			UNITY_APPLY_DITHER_CROSSFADE(i);
			float2 uv_BlankNormal = i.uv_texcoord * _BlankNormal_ST.xy + _BlankNormal_ST.zw;
			float3 lerpResult23 = lerp( UnpackNormal( tex2D( _BlankNormal, uv_BlankNormal ) ) , UnpackNormal( tex2D( _GlassWobbleNormal, ( uv_BlankNormal * _GlassWrapingScale ) ) ) , _GlassWrapingStrength);
			o.Normal = lerpResult23;
			float2 uv_GlassGrunge = i.uv_texcoord * _GlassGrunge_ST.xy + _GlassGrunge_ST.zw;
			o.Albedo = ( ( ( _GlassColor + tex2D( _GlassGrunge, uv_GlassGrunge ) ) * _GrungeAmount ) + _GlassColor ).rgb;
			float3 temp_cast_1 = (0.0).xxx;
			o.Emission = temp_cast_1;
			o.Metallic = 1.0;
			o.Smoothness = _GlassReflectionAmount;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13110
1773;112;1266;659;767.2;272.5818;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;75;-1157.4,-688.882;Float;False;899.3701;620.52;Grunge Setup;6;66;64;69;73;81;82;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;76;-1508.9,-31.98202;Float;False;1256.76;559.8;Glass Normal Setup;7;23;22;21;18;20;17;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1417.201,36.19866;Float;False;0;20;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;66;-1065.9,-584.282;Float;False;Property;_GlassColor;Glass Color;0;0;0.8046605,0.8650721,0.8897059,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;64;-1136.9,-393.282;Float;True;Property;_GlassGrunge;Glass Grunge;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;-1423.301,246.3986;Float;False;Property;_GlassWrapingScale;Glass Wraping Scale;6;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-776.3999,-417.0818;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1149.601,125.5986;Float;False;2;2;0;FLOAT2;0.0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;69;-860.2001,-253.482;Float;False;Property;_GrungeAmount;Grunge Amount;2;0;0.5;0;3;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-572.1,-453.382;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;22;-603.0356,251.9506;Float;False;Property;_GlassWrapingStrength;Glass Wraping Strength;7;0;0.1339;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;21;-966.0015,199.3986;Float;True;Property;_GlassWobbleNormal;Glass Wobble Normal;5;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;20;-979.001,7.19865;Float;True;Property;_BlankNormal;Blank Normal;4;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;74;-498.7,562.318;Float;False;390.6801;138.8699;Fade Mask Settings;1;70;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;23;-437.7357,42.15062;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;2;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-408.3999,-260.0818;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;71;-304.8,160.818;Float;False;Property;_GlassReflectionAmount;Glass Reflection Amount;3;0;0.98;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;78;-171.3999,-22.08179;Float;False;Constant;_Float0;Float 0;8;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;70;-444.3001,613.4181;Float;False;Property;_Opacity;Opacity;8;0;0.94;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;84;-162.3999,77.91821;Float;False;Constant;_Float1;Float 1;9;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;71,2;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;PolyPixel/GlassShader;False;False;False;False;False;False;False;False;False;False;False;False;True;False;True;False;False;Back;0;3;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0.0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;81;0;66;0
WireConnection;81;1;64;0
WireConnection;18;0;17;0
WireConnection;18;1;16;0
WireConnection;73;0;81;0
WireConnection;73;1;69;0
WireConnection;21;1;18;0
WireConnection;23;0;20;0
WireConnection;23;1;21;0
WireConnection;23;2;22;0
WireConnection;82;0;73;0
WireConnection;82;1;66;0
WireConnection;0;0;82;0
WireConnection;0;1;23;0
WireConnection;0;2;78;0
WireConnection;0;3;84;0
WireConnection;0;4;71;0
WireConnection;0;9;70;0
ASEEND*/
//CHKSM=450A3EC35A086D09D94DB26200669EE811A977E6