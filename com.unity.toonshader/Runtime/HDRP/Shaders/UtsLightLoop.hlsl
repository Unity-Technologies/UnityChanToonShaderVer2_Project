//Unity Toon Shader/HDRP
//nobuyuki@unity3d.com
//toshiyuki@unity3d.com (Universal RP/HDRP) 

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Macros.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/PhysicalCamera.hlsl"
#include "HDRPToonHead.hlsl"

// Channel mask enum.
// this must be same to UI cs code
// HDRPToonGUI._ChannelEnum
int eBaseColor = 0;
int eFirstShade = 1;
int eSecondShade = 2;
int eHighlight = 3;
int eAngelRing = 4;
int eRimLight = 5;
int eOutline = 6;


uniform sampler2D _Set_HighColorMask; uniform float4 _Set_HighColorMask_ST;
uniform float _Tweak_HighColorMaskLevel;
uniform fixed _RimLight;
uniform float4 _RimLightColor;
uniform fixed _Is_LightColor_RimLight;
uniform fixed _Is_NormalMapToRimLight;
uniform float _RimLight_Power;
uniform float _RimLight_InsideMask;
uniform fixed _RimLight_FeatherOff;
uniform fixed _LightDirection_MaskOn;
uniform float _Tweak_LightDirection_MaskLevel;
uniform fixed _Add_Antipodean_RimLight;
uniform float4 _Ap_RimLightColor;
uniform fixed _Is_LightColor_Ap_RimLight;
uniform float _Ap_RimLight_Power;
uniform fixed _Ap_RimLight_FeatherOff;
uniform sampler2D _Set_RimLightMask; uniform float4 _Set_RimLightMask_ST;
uniform float _Tweak_RimLightMaskLevel;
uniform fixed _MatCap;

uniform sampler2D _MatCap_Sampler; uniform float4 _MatCap_Sampler_ST;

uniform float4 _MatCapColor;
uniform fixed _Is_LightColor_MatCap;
uniform fixed _Is_BlendAddToMatCap;
uniform float _Tweak_MatCapUV;
uniform float _Rotate_MatCapUV;
uniform fixed _Is_NormalMapForMatCap;
uniform sampler2D _NormalMapForMatCap; uniform float4 _NormalMapForMatCap_ST;
uniform float _Rotate_NormalMapForMatCapUV;
uniform fixed _Is_UseTweakMatCapOnShadow;
uniform float _TweakMatCapOnShadow;
//MatcapMask
uniform sampler2D _Set_MatcapMask; uniform float4 _Set_MatcapMask_ST;
uniform float _Tweak_MatcapMaskLevel;
//v.2.0.5
uniform fixed _Is_Ortho;
//v.2.0.6
uniform float _CameraRolling_Stabilizer;
uniform fixed _BlurLevelMatcap;
uniform fixed _Inverse_MatcapMask;
#if UCTS_LWRP
#else
uniform float _BumpScale;
#endif
uniform float _BumpScaleMatcap;
//Emissive
uniform sampler2D _Emissive_Tex; uniform float4 _Emissive_Tex_ST;
uniform float4 _Emissive_Color;
//v.2.0.7
uniform fixed _Is_ViewCoord_Scroll;
uniform float _Rotate_EmissiveUV;
uniform float _Base_Speed;
uniform float _Scroll_EmissiveU;
uniform float _Scroll_EmissiveV;
uniform fixed _Is_PingPong_Base;
uniform float4 _ColorShift;
uniform float4 _ViewShift;
uniform float _ColorShift_Speed;
uniform fixed _Is_ColorShift;
uniform fixed _Is_ViewShift;
uniform float3 emissive;
// 
uniform float _Unlit_Intensity;
//v.2.0.5
uniform fixed _Is_Filter_HiCutPointLightColor;
uniform fixed _Is_Filter_LightColor;
//v.2.0.4.4
uniform float _StepOffset;
uniform fixed _Is_BLD;
uniform float _Offset_X_Axis_BLD;
uniform float _Offset_Y_Axis_BLD;
uniform fixed _Inverse_Z_Axis_BLD;
//v.2.0.4

//DoubleShadeWithFeather_TransClipping
uniform sampler2D _ClippingMask; uniform float4 _ClippingMask_ST;
uniform fixed _IsBaseMapAlphaAsClippingMask;
uniform float _Clipping_Level;
uniform fixed _Inverse_Clipping;
uniform float _Tweak_transparency;




uniform float _GI_Intensity;
#if defined(_SHADINGGRADEMAP)

#ifdef _IS_ANGELRING_OFF
//
#elif _IS_ANGELRING_ON
uniform fixed _AngelRing;

uniform sampler2D _AngelRing_Sampler; uniform float4 _AngelRing_Sampler_ST;
uniform float4 _AngelRing_Color;
uniform fixed _Is_LightColor_AR;
uniform float _AR_OffsetU;
uniform float _AR_OffsetV;
uniform fixed _ARSampler_AlphaOn;

#endif
#endif     //#if defined(_SHADINGGRADEMP)


uniform float  _BaseColorVisible;
uniform float  _BaseColorOverridden;
uniform float4 _BaseColorMaskColor;

uniform float  _FirstShadeVisible;
uniform float  _FirstShadeOverridden;
uniform float4 _FirstShadeMaskColor;

uniform float  _SecondShadeVisible;
uniform float  _SecondShadeOverridden;
uniform float4 _SecondShadeMaskColor;

uniform float  _HighlightVisible;
uniform float  _HighlightOverridden;
uniform float4 _HighlightMaskColor;

uniform float  _AngelRingVisible;
uniform float  _AngelRingOverridden;
uniform float4 _AngelRingMaskColor;

uniform float  _RimLightVisible;
uniform float  _RimLightOverridden;
uniform float4 _RimLightMaskColor;

uniform float _OutlineVisible;
uniform float _OutlineOverridden;
uniform float4 _OutlineMaskColor;

uniform float _ComposerMaskMode;
uniform int _ClippingMatteMode;

#ifdef _SYNTHESIZED_TEXTURE
uniform sampler2D _MainTexSynthesized; uniform float4 _MainTexSynthesized_ST;
uniform sampler2D _ShadowControlSynthesized; uniform float4 _ShadowControlSynthesized_ST;
uniform sampler2D _HighColor_TexSynthesized; uniform float4 _HighColor_TexSynthesized_ST;
uniform sampler2D _MatCap_SamplerSynthesized; uniform float4 _MatCap_SamplerSynthesized_ST;
#endif



uniform int _ToonEvAdjustmentCurve;
uniform float _ToonEvAdjustmentValueArray[128];
uniform float _ToonEvAdjustmentValueMin;
uniform float _ToonEvAdjustmentValueMax;

// just grafted from UTS/Universal RP
struct UtsLight
{
    float4   direction;
    float3   color;
    float    distanceAttenuation;
    float    shadowAttenuation;
    int     type;
};

// function to rotate the UV: RotateUV()
//float2 rotatedUV = RotateUV(i.uv0, (_angular_Verocity*3.141592654), float2(0.5, 0.5), _Time.g);
float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
{
    float RotateUV_ang = _radian;
    float RotateUV_cos = cos(_time*RotateUV_ang);
    float RotateUV_sin = sin(_time*RotateUV_ang);
    return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
}

float3 ConvertFromEV100(float3 EV100)
{
#if 1
    float3 value = pow(2, EV100) * 2.5f;
    return value;
#else
    float3 maxLuminance = 1.2f * pow(2.0f, EV100);
    return 1.0f / maxLuminance;
#endif
}

float3 ConvertToEV100(float3 value)
{
#if 1
    return log2(value*0.4f);
#else
    return log2(1.0f / (1.2f * value));
#endif
}



float WeightSample(PositionInputs positionInput)
{
    // Center-weighted
    const float2 kCenter = _ScreenParams.xy * 0.5;
    const float weight = pow(length((kCenter.xy - positionInput.positionSS.xy) / _ScreenParams.xy),1.0) ;
    return 1.0 - saturate(weight);
}


float3 GetExposureAdjustedColor(float3 originalColor, PositionInputs posInput)
{
    if (_ToonEvAdjustmentCurve != 0)
    {

        float3 ev100_Color = ConvertToEV100(originalColor);
        ev100_Color = clamp(ev100_Color, _ToonEvAdjustmentValueMin, _ToonEvAdjustmentValueMax);
        float3 ev100_remap = (ev100_Color - _ToonEvAdjustmentValueMin) * (128-1) / (_ToonEvAdjustmentValueMax - _ToonEvAdjustmentValueMin);
        ev100_remap = clamp(ev100_remap, 0.0, 127.0);
        int3  ev100_idx = (int3)ev100_remap;
        float3 ev100_lerp = ev100_remap - ev100_idx;
        float3  ev100_remapped;

        ev100_remapped.r = _ToonEvAdjustmentValueArray[ev100_idx.r] +(_ToonEvAdjustmentValueArray[ev100_idx.r + 1] - _ToonEvAdjustmentValueArray[ev100_idx.r]) * ev100_lerp.r;
        ev100_remapped.g = _ToonEvAdjustmentValueArray[ev100_idx.g] +(_ToonEvAdjustmentValueArray[ev100_idx.g + 1] - _ToonEvAdjustmentValueArray[ev100_idx.g]) * ev100_lerp.g;
        ev100_remapped.b = _ToonEvAdjustmentValueArray[ev100_idx.b] +(_ToonEvAdjustmentValueArray[ev100_idx.b + 1] - _ToonEvAdjustmentValueArray[ev100_idx.b]) * ev100_lerp.b;


        float3 resultColor = ConvertFromEV100(ev100_remapped);


        return resultColor;
    }
    else  // else is neccessary to avoid warrnings.
    {
        return originalColor;
    }
}

float3 GetAdjustedLightColor(float3 originalLightColor )
{


#if 0
    if (_UTS_LightAdjustment == 0)
    {
#endif
        return originalLightColor * GetCurrentExposureMultiplier();
#if 0
    }
    else // else is neccessary to avoid warrnings.
    {

        float minBrightness = 0.0001;
        float maxBrightness = 100000;
        float logOffset = 5.0;
        originalLightColor = max(float3(minBrightness, minBrightness, minBrightness), originalLightColor);
        originalLightColor = min(float3(maxBrightness, maxBrightness, maxBrightness), originalLightColor);
        float3 log10color = log10(originalLightColor);
        return clamp((log10color + float3(logOffset, logOffset, logOffset)) / 10.0, 0, 1);
    }
#endif
}

float  GetLightAttenuation(float3 lightColor)
{
    float lightAttenuation = rateR *lightColor.r + rateG *lightColor.g + rateB *lightColor.b;
    return lightAttenuation;
}


int GetNextDirectionalLightIndex(BuiltinData builtinData, int currentIndex, int mainLightIndex)
{
    int i = 0; // Declare once to avoid the D3D11 compiler warning.
    for (i = 0; i < (int)_DirectionalLightCount; ++i)
    {
        if (IsMatchingLightLayer(_DirectionalLightDatas[i].lightLayers, builtinData.renderingLayers))
        {
            if (mainLightIndex != i)
            {
                if (currentIndex < i)
                {
                    return i;
                }
            }
        }
    }
    return -1; // not found
}
int GetUtsMainLightIndex(BuiltinData builtinData)
{
    int mainLightIndex = -1;
    float3 lightColor = float3(0.0f, 0.0f, 0.0f);
    float  lightAttenuation = 0.0f;
    uint i = 0; // Declare once to avoid the D3D11 compiler warning.
    for (i = 0; i < _DirectionalLightCount; ++i)
    {
        if (IsMatchingLightLayer(_DirectionalLightDatas[i].lightLayers, builtinData.renderingLayers))
        {
            float3 currentLightColor = _DirectionalLightDatas[i].color;
            float  currentLightAttenuation = GetLightAttenuation(currentLightColor);

            if (mainLightIndex == -1 || (currentLightAttenuation > lightAttenuation))
            {
                mainLightIndex = i;
                lightAttenuation = currentLightAttenuation;
                lightColor = currentLightColor;
            } 
        }
    }

    return mainLightIndex;
}
#if defined(_SHADINGGRADEMAP)
# include "ShadingGrademapOtherLight.hlsl"
#else //#if defined(_SHADINGGRADEMAP)
# include "DoubleShadeWithFeatherOtherLight.hlsl"
#endif //#if defined(_SHADINGGRADEMAP)


#if defined(_SHADINGGRADEMAP)
# include "ShadingGrademapMainLight.hlsl"
#else
# include "DoubleShadeWithFeatherMainLight.hlsl"
#endif

