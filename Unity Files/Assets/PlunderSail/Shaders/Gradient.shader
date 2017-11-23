// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33441,y:32630,varname:node_2865,prsc:2|diff-1975-OUT,spec-5732-OUT,gloss-4700-OUT;n:type:ShaderForge.SFN_Lerp,id:3335,x:32580,y:32546,varname:node_3335,prsc:2|A-1268-RGB,B-7672-RGB,T-5424-OUT;n:type:ShaderForge.SFN_Color,id:7672,x:32310,y:32537,ptovrint:False,ptlb:GradientColour,ptin:_GradientColour,varname:node_7672,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:8639,x:31748,y:32706,varname:node_8639,prsc:2|IN-8-OUT,IMIN-4061-OUT,IMAX-3649-OUT,OMIN-7757-OUT,OMAX-3036-OUT;n:type:ShaderForge.SFN_Slider,id:4061,x:31212,y:32688,ptovrint:False,ptlb:iMin,ptin:_iMin,cmnt:1.06 - 3.5,varname:node_4061,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1.06,cur:1.06,max:3.5;n:type:ShaderForge.SFN_Slider,id:3649,x:31212,y:32804,ptovrint:False,ptlb:iMax,ptin:_iMax,cmnt:-2 - .96,varname:_node_4061_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-2,cur:0,max:0.96;n:type:ShaderForge.SFN_Slider,id:8,x:31212,y:32555,ptovrint:False,ptlb:value,ptin:_value,cmnt:min .69 - max .99,varname:node_8,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.69,cur:0.69,max:0.99;n:type:ShaderForge.SFN_Vector1,id:7757,x:31535,y:32804,varname:node_7757,prsc:2,v1:-2;n:type:ShaderForge.SFN_Vector1,id:3036,x:31535,y:32863,varname:node_3036,prsc:2,v1:2;n:type:ShaderForge.SFN_Slider,id:4568,x:31969,y:33015,ptovrint:False,ptlb:HeightControl,ptin:_HeightControl,cmnt:.04 - .1,varname:node_4568,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.04,cur:0.04,max:0.1;n:type:ShaderForge.SFN_FragmentPosition,id:2880,x:31539,y:32947,varname:node_2880,prsc:2;n:type:ShaderForge.SFN_Vector1,id:5843,x:31314,y:31972,varname:node_5843,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:254,x:31534,y:31972,varname:node_254,prsc:2|A-5843-OUT,B-5370-OUT,C-5843-OUT;n:type:ShaderForge.SFN_NormalVector,id:6396,x:31534,y:32108,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:2690,x:31728,y:31972,varname:node_2690,prsc:2,dt:1|A-254-OUT,B-6396-OUT;n:type:ShaderForge.SFN_Slider,id:5370,x:31157,y:32054,ptovrint:False,ptlb:UpNode,ptin:_UpNode,varname:node_3918,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:6592,x:31390,y:32283,ptovrint:False,ptlb:Level,ptin:_Level,varname:_UpNode_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.17,max:10;n:type:ShaderForge.SFN_Multiply,id:4517,x:31925,y:31972,varname:node_4517,prsc:2|A-2690-OUT,B-6592-OUT;n:type:ShaderForge.SFN_Power,id:2737,x:32093,y:31972,varname:node_2737,prsc:2|VAL-4517-OUT,EXP-2624-OUT;n:type:ShaderForge.SFN_Slider,id:2624,x:31390,y:32379,ptovrint:False,ptlb:Contrast,ptin:_Contrast,varname:_Level_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:17.6,max:25;n:type:ShaderForge.SFN_Clamp01,id:8693,x:32253,y:31972,varname:node_8693,prsc:2|IN-2737-OUT;n:type:ShaderForge.SFN_Color,id:6063,x:32691,y:31969,ptovrint:False,ptlb:TopColour,ptin:_TopColour,varname:_BaseColour_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:1268,x:32310,y:32373,ptovrint:False,ptlb:BaseColour,ptin:_BaseColour,varname:_TopColour_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:5424,x:32326,y:32714,varname:node_5424,prsc:2|A-8639-OUT,B-2880-Y,T-4568-OUT;n:type:ShaderForge.SFN_Multiply,id:9128,x:32882,y:32147,varname:node_9128,prsc:2|A-6063-RGB,B-8693-OUT;n:type:ShaderForge.SFN_OneMinus,id:5057,x:32580,y:32398,varname:node_5057,prsc:2|IN-8693-OUT;n:type:ShaderForge.SFN_Multiply,id:3255,x:32882,y:32392,varname:node_3255,prsc:2|A-5057-OUT,B-3335-OUT;n:type:ShaderForge.SFN_Vector1,id:4896,x:28932,y:32871,varname:node_4896,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:6085,x:29152,y:32871,varname:node_6085,prsc:2|A-4896-OUT,B-5721-OUT,C-4896-OUT;n:type:ShaderForge.SFN_NormalVector,id:4600,x:29152,y:33007,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:4245,x:29346,y:32871,varname:node_4245,prsc:2,dt:1|A-6085-OUT,B-4600-OUT;n:type:ShaderForge.SFN_Slider,id:5721,x:28775,y:32953,ptovrint:False,ptlb:UpNode_copy,ptin:_UpNode_copy,varname:_UpNode_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:8722,x:29008,y:33182,ptovrint:False,ptlb:Level_copy,ptin:_Level_copy,varname:_Level_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.17,max:10;n:type:ShaderForge.SFN_Multiply,id:6391,x:29543,y:32871,varname:node_6391,prsc:2|A-4245-OUT,B-8722-OUT;n:type:ShaderForge.SFN_Power,id:121,x:29711,y:32871,varname:node_121,prsc:2|VAL-6391-OUT,EXP-9799-OUT;n:type:ShaderForge.SFN_Slider,id:9799,x:29008,y:33278,ptovrint:False,ptlb:Contrast_copy,ptin:_Contrast_copy,varname:_Contrast_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:17.6,max:25;n:type:ShaderForge.SFN_Clamp01,id:2582,x:29871,y:32871,varname:node_2582,prsc:2|IN-121-OUT;n:type:ShaderForge.SFN_Lerp,id:1746,x:30635,y:32349,varname:node_1746,prsc:2|A-7412-RGB,B-7296-RGB,T-2582-OUT;n:type:ShaderForge.SFN_Color,id:7296,x:30153,y:32435,ptovrint:False,ptlb:GradientColour_copy,ptin:_GradientColour_copy,varname:_GradientColour_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:7412,x:30200,y:32218,ptovrint:False,ptlb:BaseColour_copy,ptin:_BaseColour_copy,varname:_BaseColour_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:1975,x:33169,y:32180,varname:node_1975,prsc:2|A-9128-OUT,B-3255-OUT,T-698-OUT;n:type:ShaderForge.SFN_Slider,id:5732,x:33038,y:32744,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_5732,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:4700,x:33038,y:32850,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Vector1,id:698,x:32882,y:32286,varname:node_698,prsc:2,v1:0.25;n:type:ShaderForge.SFN_Vector1,id:4182,x:31535,y:32738,varname:node_4182,prsc:2,v1:0.03;n:type:ShaderForge.SFN_Vector1,id:701,x:31535,y:32673,varname:node_701,prsc:2,v1:2.73;n:type:ShaderForge.SFN_Vector1,id:2138,x:31535,y:32605,varname:node_2138,prsc:2,v1:0.69;n:type:ShaderForge.SFN_Vector1,id:3499,x:32048,y:32877,varname:node_3499,prsc:2,v1:0.021;proporder:7672-3649-4061-8-4568-6063-5370-6592-2624-1268-4700-5732;pass:END;sub:END;*/

Shader "Shader Forge/Gradient" {
    Properties {
        _GradientColour ("GradientColour", Color) = (0.5,0.5,0.5,1)
        _iMax ("iMax", Range(-2, 0.96)) = 0
        _iMin ("iMin", Range(1.06, 3.5)) = 1.06
        _value ("value", Range(0.69, 0.99)) = 0.69
        _HeightControl ("HeightControl", Range(0.04, 0.1)) = 0.04
        _TopColour ("TopColour", Color) = (0.5,0.5,0.5,1)
        _UpNode ("UpNode", Range(0, 1)) = 1
        _Level ("Level", Range(0, 10)) = 1.17
        _Contrast ("Contrast", Range(0, 25)) = 17.6
        _BaseColour ("BaseColour", Color) = (0.5,0.5,0.5,1)
        _Gloss ("Gloss", Range(0, 1)) = 0
        _Metallic ("Metallic", Range(0, 1)) = 0
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
            uniform float4 _GradientColour;
            uniform float _iMin;
            uniform float _iMax;
            uniform float _value;
            uniform float _HeightControl;
            uniform float _UpNode;
            uniform float _Level;
            uniform float _Contrast;
            uniform float4 _TopColour;
            uniform float4 _BaseColour;
            uniform float _Metallic;
            uniform float _Gloss;
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
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
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
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
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
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float node_5843 = 0.0;
                float node_8693 = saturate(pow((max(0,dot(float3(node_5843,_UpNode,node_5843),i.normalDir))*_Level),_Contrast));
                float node_7757 = (-2.0);
                float3 diffuseColor = lerp((_TopColour.rgb*node_8693),((1.0 - node_8693)*lerp(_BaseColour.rgb,_GradientColour.rgb,lerp((node_7757 + ( (_value - _iMin) * (2.0 - node_7757) ) / (_iMax - _iMin)),i.posWorld.g,_HeightControl))),0.25); // Need this for specular when using metallic
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
            uniform float4 _GradientColour;
            uniform float _iMin;
            uniform float _iMax;
            uniform float _value;
            uniform float _HeightControl;
            uniform float _UpNode;
            uniform float _Level;
            uniform float _Contrast;
            uniform float4 _TopColour;
            uniform float4 _BaseColour;
            uniform float _Metallic;
            uniform float _Gloss;
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
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
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
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float node_5843 = 0.0;
                float node_8693 = saturate(pow((max(0,dot(float3(node_5843,_UpNode,node_5843),i.normalDir))*_Level),_Contrast));
                float node_7757 = (-2.0);
                float3 diffuseColor = lerp((_TopColour.rgb*node_8693),((1.0 - node_8693)*lerp(_BaseColour.rgb,_GradientColour.rgb,lerp((node_7757 + ( (_value - _iMin) * (2.0 - node_7757) ) / (_iMax - _iMin)),i.posWorld.g,_HeightControl))),0.25); // Need this for specular when using metallic
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
            uniform float4 _GradientColour;
            uniform float _iMin;
            uniform float _iMax;
            uniform float _value;
            uniform float _HeightControl;
            uniform float _UpNode;
            uniform float _Level;
            uniform float _Contrast;
            uniform float4 _TopColour;
            uniform float4 _BaseColour;
            uniform float _Metallic;
            uniform float _Gloss;
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
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float node_5843 = 0.0;
                float node_8693 = saturate(pow((max(0,dot(float3(node_5843,_UpNode,node_5843),i.normalDir))*_Level),_Contrast));
                float node_7757 = (-2.0);
                float3 diffColor = lerp((_TopColour.rgb*node_8693),((1.0 - node_8693)*lerp(_BaseColour.rgb,_GradientColour.rgb,lerp((node_7757 + ( (_value - _iMin) * (2.0 - node_7757) ) / (_iMax - _iMin)),i.posWorld.g,_HeightControl))),0.25);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
