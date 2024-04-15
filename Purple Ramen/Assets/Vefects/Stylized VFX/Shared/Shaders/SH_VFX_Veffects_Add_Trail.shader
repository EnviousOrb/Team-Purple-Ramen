// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_VFX_Veffects_AddTrail"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Emissive_Intensity("Emissive_Intensity", Float) = 1
		_TrailScale("TrailScale", Float) = 1
		_TrailSpeed("TrailSpeed", Float) = 1
		[Space(33)][Header(AR)][Space(13)]_Cull("Cull", Float) = 0
		_Src("Src", Float) = 1
		_Dst("Dst", Float) = 1
		_ZWrite("ZWrite", Float) = 0
		_ZTest("ZTest", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_Cull]
		ZWrite [_ZWrite]
		ZTest [_ZTest]
		Blend [_Src] [_Dst]
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _Dst;
		uniform float _ZWrite;
		uniform float _ZTest;
		uniform float _Src;
		uniform float _Cull;
		uniform sampler2D _Texture;
		uniform float _TrailScale;
		uniform float _TrailSpeed;
		uniform float _Emissive_Intensity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 appendResult16 = (float2(_TrailScale , 1.0));
			float2 appendResult19 = (float2(_TrailSpeed , 0.0));
			float2 uv_TexCoord13 = i.uv_texcoord * appendResult16 + ( _Time.y * appendResult19 );
			float4 tex2DNode1 = tex2D( _Texture, uv_TexCoord13 );
			o.Emission = ( ( ( tex2DNode1.r + tex2DNode1.g ) * _Emissive_Intensity ) * (i.vertexColor).rgb * i.vertexColor.a );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
0;0;2560;1371;3142.451;943.5172;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;48;-2667.451,-247.5172;Inherit;False;Property;_TrailSpeed;TrailSpeed;4;0;Create;True;0;0;0;False;0;False;1;-5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2482.161,-721.9373;Inherit;False;Property;_TrailScale;TrailScale;3;0;Create;True;0;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;12;-2549.461,-469.7377;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-2485.461,-240.7375;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2290.461,-372.7379;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-2325.161,-670.9374;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-2036.159,-516.9374;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1692.365,-555.6248;Inherit;True;Property;_Texture;Texture;1;0;Create;True;0;0;0;False;0;False;-1;6c7e2fe3b9a387644ace302722a670c2;6c7e2fe3b9a387644ace302722a670c2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-1262.622,-559.2845;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1302.347,-371.0855;Inherit;False;Property;_Emissive_Intensity;Emissive_Intensity;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;7;-1259,89.5;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;42;336,-560;Inherit;False;1238;166;Lush was here! <3;5;47;46;45;44;43;Lush was here! <3;0.3880934,0.240566,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1085.905,-523.9055;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;8;-930,-382.5;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;46;896,-512;Inherit;False;Property;_Dst;Dst;7;0;Create;True;0;0;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-385.1932,-402.7726;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;43;1152,-512;Inherit;False;Property;_ZWrite;ZWrite;8;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;1408,-512;Inherit;False;Property;_ZTest;ZTest;9;0;Create;True;0;0;0;True;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;640,-512;Inherit;False;Property;_Src;Src;6;0;Create;True;0;0;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;384,-512;Inherit;False;Property;_Cull;Cull;5;0;Create;True;0;0;0;True;3;Space(33);Header(AR);Space(13);False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;-110.5602,-444.3488;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SH_VFX_Veffects_AddTrail;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;True;43;0;True;44;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;True;45;10;True;46;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;True;47;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;48;0
WireConnection;20;0;12;0
WireConnection;20;1;19;0
WireConnection;16;0;17;0
WireConnection;13;0;16;0
WireConnection;13;1;20;0
WireConnection;1;1;13;0
WireConnection;35;0;1;1
WireConnection;35;1;1;2
WireConnection;41;0;35;0
WireConnection;41;1;4;0
WireConnection;8;0;7;0
WireConnection;2;0;41;0
WireConnection;2;1;8;0
WireConnection;2;2;7;4
WireConnection;6;2;2;0
ASEEND*/
//CHKSM=71573CB774174D2243AC16849EA6FCF51FC95F5E