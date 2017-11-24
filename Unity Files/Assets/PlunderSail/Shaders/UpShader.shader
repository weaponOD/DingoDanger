// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33317,y:32871,varname:node_2865,prsc:2|diff-5856-OUT,spec-3780-OUT,gloss-8074-OUT;n:type:ShaderForge.SFN_Slider,id:3780,x:32540,y:33505,ptovrint:False,ptlb:M,ptin:_M,varname:node_3780,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:8074,x:32540,y:33598,ptovrint:False,ptlb:G,ptin:_G,varname:_node_3780_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_FragmentPosition,id:2273,x:31317,y:32926,varname:node_2273,prsc:2;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:7202,x:31970,y:33112,varname:node_7202,prsc:2|IN-9943-OUT,IMIN-2438-OUT,IMAX-2994-OUT,OMIN-1270-OUT,OMAX-2070-OUT;n:type:ShaderForge.SFN_Vector1,id:1270,x:31497,y:33573,varname:node_1270,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:2070,x:31497,y:33637,varname:node_2070,prsc:2,v1:1;n:type:ShaderForge.SFN_Clamp01,id:5428,x:32138,y:33112,varname:node_5428,prsc:2|IN-7202-OUT;n:type:ShaderForge.SFN_Multiply,id:3159,x:32358,y:33064,varname:node_3159,prsc:2|A-7850-RGB,B-9704-OUT,C-5428-OUT;n:type:ShaderForge.SFN_Color,id:7850,x:32138,y:32961,ptovrint:False,ptlb:GradientColour,ptin:_GradientColour,varname:node_7850,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:1833,x:32755,y:33045,varname:node_1833,prsc:2|A-5536-RGB,B-3159-OUT,T-4120-OUT;n:type:ShaderForge.SFN_Color,id:5536,x:32335,y:32857,ptovrint:False,ptlb:BaseColour,ptin:_BaseColour,varname:node_5536,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_NormalVector,id:2008,x:31183,y:33033,prsc:2,pt:False;n:type:ShaderForge.SFN_Subtract,id:7005,x:31502,y:32990,varname:node_7005,prsc:2|A-2273-Y,B-877-Y;n:type:ShaderForge.SFN_ObjectPosition,id:877,x:31317,y:33068,varname:node_877,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9943,x:31683,y:33042,varname:node_9943,prsc:2|A-7005-OUT,B-7869-OUT;n:type:ShaderForge.SFN_Slider,id:7869,x:31328,y:33240,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7869,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Vector1,id:7591,x:31333,y:32388,varname:node_7591,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:3253,x:31553,y:32388,varname:node_3253,prsc:2|A-7591-OUT,B-4034-OUT,C-7591-OUT;n:type:ShaderForge.SFN_NormalVector,id:6799,x:31553,y:32524,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:5176,x:31747,y:32388,varname:node_5176,prsc:2,dt:1|A-3253-OUT,B-6799-OUT;n:type:ShaderForge.SFN_Slider,id:4034,x:31176,y:32470,ptovrint:False,ptlb:UpNode,ptin:_UpNode,varname:node_3918,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:4025,x:31409,y:32699,ptovrint:False,ptlb:Level,ptin:_Level,varname:_UpNode_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.17,max:10;n:type:ShaderForge.SFN_Multiply,id:8704,x:31944,y:32388,varname:node_8704,prsc:2|A-5176-OUT,B-4025-OUT;n:type:ShaderForge.SFN_Power,id:5305,x:32112,y:32388,varname:node_5305,prsc:2|VAL-8704-OUT,EXP-1093-OUT;n:type:ShaderForge.SFN_Slider,id:1093,x:31409,y:32795,ptovrint:False,ptlb:Contrast,ptin:_Contrast,varname:_Level_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:17.6,max:25;n:type:ShaderForge.SFN_Clamp01,id:811,x:32272,y:32388,varname:node_811,prsc:2|IN-5305-OUT;n:type:ShaderForge.SFN_Color,id:3143,x:32442,y:32581,ptovrint:False,ptlb:TopColour,ptin:_TopColour,varname:_BaseColour_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1821,x:32686,y:32581,varname:node_1821,prsc:2|A-3143-RGB,B-811-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:6392,x:33089,y:32734,varname:node_6392,prsc:2,chbt:0|M-1821-OUT,R-1833-OUT,G-1833-OUT,B-1833-OUT;n:type:ShaderForge.SFN_ChannelBlend,id:5856,x:33089,y:32904,varname:node_5856,prsc:2,chbt:1|M-811-OUT,R-1821-OUT,BTM-1833-OUT;n:type:ShaderForge.SFN_Vector1,id:4120,x:32535,y:33151,varname:node_4120,prsc:2,v1:0.5;n:type:ShaderForge.SFN_RemapRange,id:2994,x:31736,y:33430,varname:node_2994,prsc:2,frmn:0,frmx:1,tomn:-100,tomx:0|IN-462-OUT;n:type:ShaderForge.SFN_Slider,id:462,x:31339,y:33506,ptovrint:False,ptlb:High,ptin:_High,varname:node_462,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:9264,x:31337,y:33393,ptovrint:False,ptlb:Low,ptin:_Low,varname:node_9264,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRange,id:2438,x:31736,y:33272,varname:node_2438,prsc:2,frmn:0,frmx:1,tomn:-30,tomx:30|IN-9264-OUT;n:type:ShaderForge.SFN_RemapRange,id:9704,x:32241,y:33306,varname:node_9704,prsc:2,frmn:0,frmx:1,tomn:0,tomx:3|IN-1536-OUT;n:type:ShaderForge.SFN_Slider,id:1536,x:31935,y:33306,ptovrint:False,ptlb:GradientColourIntensity,ptin:_GradientColourIntensity,varname:node_1536,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:3780-8074-7850-5536-7869-4034-4025-1093-3143-462-9264-1536;pass:END;sub:END;*/

Shader "Shader Forge/UpShader" {
    Properties {
        _M ("M", Range(0, 1)) = 0
        _G ("G", Range(0, 1)) = 0
        _GradientColour ("GradientColour", Color) = (0.5,0.5,0.5,1)
        _BaseColour ("BaseColour", Color) = (0.5,0.5,0.5,1)
        _Opacity ("Opacity", Range(0, 1)) = 0
        _UpNode ("UpNode", Range(0, 1)) = 1
        _Level ("Level", Range(0, 10)) = 1.17
        _Contrast ("Contrast", Range(0, 25)) = 17.6
        _TopColour ("TopColour", Color) = (0.5,0.5,0.5,1)
        _High ("High", Range(0, 1)) = 0
        _Low ("Low", Range(0, 1)) = 0
        _GradientColourIntensity ("GradientColourIntensity", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _M;
            uniform float _G;
            uniform float4 _GradientColour;
            uniform float4 _BaseColour;
            uniform float _Opacity;
            uniform float _UpNode;
            uniform float _Level;
            uniform float _Contrast;
            uniform float4 _TopColour;
            uniform float _High;
            uniform float _Low;
            uniform float _GradientColourIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD9;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _G;
                float perceptualRoughness = 1.0 - _G;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _M;
                float specularMonochrome;
                float node_7591 = 0.0;
                float node_811 = saturate(pow((max(0,dot(float3(node_7591,_UpNode,node_7591),i.normalDir))*_Level),_Contrast));
                float node_2438 = (_Low*60.0+-30.0);
                float node_1270 = 0.0;
                float3 node_1833 = lerp(_BaseColour.rgb,(_GradientColour.rgb*(_GradientColourIntensity*3.0+0.0)*saturate((node_1270 + ( (((i.posWorld.g-objPos.g)*_Opacity) - node_2438) * (1.0 - node_1270) ) / ((_High*100.0+-100.0) - node_2438)))),0.5);
                float3 node_1821 = (_TopColour.rgb*node_811);
                float3 diffuseColor = (lerp( node_1833, node_1821, node_811.r )); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _M;
            uniform float _G;
            uniform float4 _GradientColour;
            uniform float4 _BaseColour;
            uniform float _Opacity;
            uniform float _UpNode;
            uniform float _Level;
            uniform float _Contrast;
            uniform float4 _TopColour;
            uniform float _High;
            uniform float _Low;
            uniform float _GradientColourIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _G;
                float perceptualRoughness = 1.0 - _G;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _M;
                float specularMonochrome;
                float node_7591 = 0.0;
                float node_811 = saturate(pow((max(0,dot(float3(node_7591,_UpNode,node_7591),i.normalDir))*_Level),_Contrast));
                float node_2438 = (_Low*60.0+-30.0);
                float node_1270 = 0.0;
                float3 node_1833 = lerp(_BaseColour.rgb,(_GradientColour.rgb*(_GradientColourIntensity*3.0+0.0)*saturate((node_1270 + ( (((i.posWorld.g-objPos.g)*_Opacity) - node_2438) * (1.0 - node_1270) ) / ((_High*100.0+-100.0) - node_2438)))),0.5);
                float3 node_1821 = (_TopColour.rgb*node_811);
                float3 diffuseColor = (lerp( node_1833, node_1821, node_811.r )); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _M;
            uniform float _G;
            uniform float4 _GradientColour;
            uniform float4 _BaseColour;
            uniform float _Opacity;
            uniform float _UpNode;
            uniform float _Level;
            uniform float _Contrast;
            uniform float4 _TopColour;
            uniform float _High;
            uniform float _Low;
            uniform float _GradientColourIntensity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float node_7591 = 0.0;
                float node_811 = saturate(pow((max(0,dot(float3(node_7591,_UpNode,node_7591),i.normalDir))*_Level),_Contrast));
                float node_2438 = (_Low*60.0+-30.0);
                float node_1270 = 0.0;
                float3 node_1833 = lerp(_BaseColour.rgb,(_GradientColour.rgb*(_GradientColourIntensity*3.0+0.0)*saturate((node_1270 + ( (((i.posWorld.g-objPos.g)*_Opacity) - node_2438) * (1.0 - node_1270) ) / ((_High*100.0+-100.0) - node_2438)))),0.5);
                float3 node_1821 = (_TopColour.rgb*node_811);
                float3 diffColor = (lerp( node_1833, node_1821, node_811.r ));
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _M, specColor, specularMonochrome );
                float roughness = 1.0 - _G;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
