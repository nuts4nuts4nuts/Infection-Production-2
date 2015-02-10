// Shader created with Shader Forge v1.01 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.01;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1612,x:33301,y:32763,varname:node_1612,prsc:2|diff-717-OUT,spec-1478-OUT,gloss-1924-OUT,normal-9130-RGB,emission-5195-OUT,alpha-7313-OUT;n:type:ShaderForge.SFN_Color,id:2382,x:31963,y:32209,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_2382,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:717,x:32420,y:32313,varname:node_717,prsc:2|A-2382-RGB,B-7197-RGB;n:type:ShaderForge.SFN_Tex2d,id:7197,x:31950,y:32438,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_7197,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9130,x:31729,y:32830,ptovrint:False,ptlb:BumpMap,ptin:_BumpMap,varname:_MainTex_copy,prsc:2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Multiply,id:1478,x:32621,y:32518,varname:node_1478,prsc:2|A-4859-RGB,B-4884-RGB;n:type:ShaderForge.SFN_Color,id:4884,x:32338,y:32628,ptovrint:False,ptlb:SpecularColor,ptin:_SpecularColor,varname:node_4884,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Slider,id:2444,x:32508,y:32893,ptovrint:False,ptlb:Shininess,ptin:_Shininess,varname:node_2444,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Fresnel,id:5642,x:32234,y:33142,varname:node_5642,prsc:2|NRM-5008-OUT,EXP-2695-OUT;n:type:ShaderForge.SFN_Multiply,id:5195,x:32452,y:33058,varname:node_5195,prsc:2|A-9184-OUT,B-5642-OUT;n:type:ShaderForge.SFN_Color,id:8396,x:32078,y:32952,ptovrint:False,ptlb:RimColor,ptin:_RimColor,varname:_SpecularColor_copy,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1924,x:32890,y:32789,varname:node_1924,prsc:2|A-4884-A,B-2444-OUT;n:type:ShaderForge.SFN_Tex2d,id:4859,x:32255,y:32457,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:_BumpMap_copy,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:2695,x:31771,y:33246,ptovrint:False,ptlb:RimExponent,ptin:_RimExponent,varname:node_2695,prsc:2,min:0,cur:0,max:5;n:type:ShaderForge.SFN_NormalVector,id:5008,x:32007,y:33026,prsc:2,pt:True;n:type:ShaderForge.SFN_Multiply,id:7313,x:32774,y:33100,varname:node_7313,prsc:2|A-7197-A,B-2382-A;n:type:ShaderForge.SFN_Multiply,id:9184,x:32312,y:32952,varname:node_9184,prsc:2|A-9185-OUT,B-8396-RGB;n:type:ShaderForge.SFN_ValueProperty,id:9185,x:32134,y:32880,ptovrint:False,ptlb:RimBoost,ptin:_RimBoost,varname:node_9185,prsc:2,glob:False,v1:1;proporder:2382-7197-9130-4884-2444-4859-8396-2695-9185;pass:END;sub:END;*/

Shader "Infection/RimLighter" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _BumpMap ("BumpMap", 2D) = "bump" {}
        _SpecularColor ("SpecularColor", Color) = (0.5,0.5,0.5,1)
        _Shininess ("Shininess", Range(0, 1)) = 0
        _Specular ("Specular", 2D) = "white" {}
        _RimColor ("RimColor", Color) = (0.5,0.5,0.5,1)
        _RimExponent ("RimExponent", Range(0, 5)) = 0
        _RimBoost ("RimBoost", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
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
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float4 _SpecularColor;
            uniform float _Shininess;
            uniform float4 _RimColor;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform float _RimExponent;
            uniform float _RimBoost;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = (_SpecularColor.a*_Shininess);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float3 specularColor = (_Specular_var.rgb*_SpecularColor.rgb);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuse = (directDiffuse + indirectDiffuse) * (_Color.rgb*_MainTex_var.rgb);
////// Emissive:
                float3 emissive = ((_RimBoost*_RimColor.rgb)*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimExponent));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                float node_7313 = (_MainTex_var.a*_Color.a);
                return fixed4(finalColor,node_7313);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float4 _SpecularColor;
            uniform float _Shininess;
            uniform float4 _RimColor;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform float _RimExponent;
            uniform float _RimBoost;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = (_SpecularColor.a*_Shininess);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float3 specularColor = (_Specular_var.rgb*_SpecularColor.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuse = directDiffuse * (_Color.rgb*_MainTex_var.rgb);
/// Final Color:
                float3 finalColor = diffuse + specular;
                float node_7313 = (_MainTex_var.a*_Color.a);
                return fixed4(finalColor * node_7313,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
