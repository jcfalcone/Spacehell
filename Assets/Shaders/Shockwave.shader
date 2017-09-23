// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33035,y:32793,varname:node_4795,prsc:2|normal-4393-OUT,alpha-2365-OUT,refract-5558-OUT;n:type:ShaderForge.SFN_Tex2d,id:3396,x:31289,y:32607,ptovrint:False,ptlb:Refraction Map,ptin:_RefractionMap,varname:node_3396,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b3c39d6e380cc485d8cb54a1a7ec8513,ntxv:0,isnm:False|UVIN-9150-UVOUT;n:type:ShaderForge.SFN_Rotator,id:9150,x:31029,y:32565,varname:node_9150,prsc:2|UVIN-579-UVOUT,SPD-6641-OUT;n:type:ShaderForge.SFN_TexCoord,id:579,x:30760,y:32444,varname:node_579,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Slider,id:6641,x:30648,y:32730,ptovrint:False,ptlb:Rotation Speed,ptin:_RotationSpeed,varname:node_6641,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Slider,id:1647,x:31132,y:32810,ptovrint:False,ptlb:Normal Intensity,ptin:_NormalIntensity,varname:node_1647,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.692308,max:3;n:type:ShaderForge.SFN_Vector3,id:3224,x:31289,y:32443,varname:node_3224,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Lerp,id:4393,x:31543,y:32551,varname:node_4393,prsc:2|A-3224-OUT,B-3396-RGB,T-1647-OUT;n:type:ShaderForge.SFN_Slider,id:8628,x:31041,y:33016,ptovrint:False,ptlb:Refraction Value,ptin:_RefractionValue,varname:node_8628,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-0.5,cur:0.0158201,max:0.5;n:type:ShaderForge.SFN_Multiply,id:1426,x:31543,y:32903,varname:node_1426,prsc:2|A-1647-OUT,B-8628-OUT;n:type:ShaderForge.SFN_ComponentMask,id:790,x:31543,y:32708,varname:node_790,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3396-RGB;n:type:ShaderForge.SFN_Multiply,id:2499,x:31786,y:32779,varname:node_2499,prsc:2|A-790-OUT,B-1426-OUT;n:type:ShaderForge.SFN_Tex2d,id:6112,x:31404,y:33218,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_6112,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f6e5f0349fe0140efa456a2f3aac8bf1,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2365,x:32519,y:33319,varname:node_2365,prsc:2|A-6112-R,B-2686-OUT;n:type:ShaderForge.SFN_Slider,id:2686,x:32004,y:33466,ptovrint:False,ptlb:Opacity Value,ptin:_OpacityValue,varname:node_2686,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ComponentMask,id:9460,x:31786,y:32986,varname:node_9460,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-6112-R;n:type:ShaderForge.SFN_Multiply,id:3545,x:31964,y:32779,varname:node_3545,prsc:2|A-2499-OUT,B-9460-OUT;n:type:ShaderForge.SFN_VertexColor,id:3595,x:31964,y:32942,varname:node_3595,prsc:2;n:type:ShaderForge.SFN_Color,id:3837,x:31964,y:33131,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_3837,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:8318,x:32289,y:33049,varname:node_8318,prsc:2|A-3595-A,B-3837-A;n:type:ShaderForge.SFN_Multiply,id:5558,x:32517,y:32996,varname:node_5558,prsc:2|A-3545-OUT,B-8318-OUT;proporder:3396-6641-1647-6112-2686-8628-3837;pass:END;sub:END;*/

Shader "JCFalcone/Shockwave" {
    Properties {
        _RefractionMap ("Refraction Map", 2D) = "white" {}
        _RotationSpeed ("Rotation Speed", Range(-5, 5)) = 0
        _NormalIntensity ("Normal Intensity", Range(0, 3)) = 2.692308
        _Opacity ("Opacity", 2D) = "white" {}
        _OpacityValue ("Opacity Value", Range(0, 1)) = 0
        _RefractionValue ("Refraction Value", Range(-0.5, 0.5)) = 0.0158201
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _RefractionMap; uniform float4 _RefractionMap_ST;
            uniform float _RotationSpeed;
            uniform float _NormalIntensity;
            uniform float _RefractionValue;
            uniform sampler2D _Opacity; uniform float4 _Opacity_ST;
            uniform float _OpacityValue;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD4;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float4 node_4796 = _Time;
                float node_9150_ang = node_4796.g;
                float node_9150_spd = _RotationSpeed;
                float node_9150_cos = cos(node_9150_spd*node_9150_ang);
                float node_9150_sin = sin(node_9150_spd*node_9150_ang);
                float2 node_9150_piv = float2(0.5,0.5);
                float2 node_9150 = (mul(i.uv0-node_9150_piv,float2x2( node_9150_cos, -node_9150_sin, node_9150_sin, node_9150_cos))+node_9150_piv);
                float4 _RefractionMap_var = tex2D(_RefractionMap,TRANSFORM_TEX(node_9150, _RefractionMap));
                float3 normalLocal = lerp(float3(0,0,1),_RefractionMap_var.rgb,_NormalIntensity);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _Opacity_var = tex2D(_Opacity,TRANSFORM_TEX(i.uv0, _Opacity));
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (((_RefractionMap_var.rgb.rg*(_NormalIntensity*_RefractionValue))*_Opacity_var.r.r)*(i.vertexColor.a*_Color.a));
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,(_Opacity_var.r*_OpacityValue)),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
