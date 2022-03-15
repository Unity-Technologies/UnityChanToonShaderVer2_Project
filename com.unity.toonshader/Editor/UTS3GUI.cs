﻿//#define USE_GAME_RECOMMENDATION 
//#define USE_SIMPLE_UI
//#define USE_MANUAL_LINK_BUTTONS
//#define USE_TOGGLE_BUTTONS
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;


namespace UnityEditor.Rendering.Toon
{
#if USE_GAME_RECOMMENDATION
    internal struct GameRecommendation
    {
        public float ShaderPropIsLightColor_Base;
        public float ShaderPropIs_LightColor_1st_Shade;
        public float ShaderPropIs_LightColor_2nd_Shade;
        public float ShaderPropIs_LightColor_HighColor;
        public float ShaderPropIs_LightColor_RimLight;
        public float ShaderPropIs_LightColor_Ap_RimLight;
        public float ShaderPropIs_LightColor_MatCap;
        public float ShaderPropIs_LightColor_AR;
        public float ShaderPropIs_LightColor_Outline;
        public float ShaderPropSetSystemShadowsToBase;
        public float ShaderPropIsFilterHiCutPointLightColor;
        public float ShaderPropCameraRolling_Stabilizer;
        public float ShaderPropIs_Ortho;
        public float ShaderPropGI_Intensity;
        public float ShaderPropUnlit_Intensity;
        public float ShaderPropIs_Filter_LightColor;
    };
#endif //USE_GAME_RECOMMENDATION
    internal partial class UTS3GUI : UnityEditor.ShaderGUI
    {


        protected const float kVersionX = 0.0f;
        protected const float kVersionY = 7.0f;
        protected const float kVersionZ = 0.0f;

        // Render Pipelines UTS supports are the followings 
        enum RenderPipeline
        {
            Unknown,
            Legacy,
            Universal,
            HDRP,
        }

        RenderPipeline currentRenderPipeline
        {
            get
            {
                const string kHdrpShaderPrefix = "HDRP/";
                const string kUrpShaderPrefix = "Universal Render Pipeline/";
                var currentRenderPipeline = GraphicsSettings.currentRenderPipeline;
                if (currentRenderPipeline == null)
                {
                    return RenderPipeline.Legacy; 
                }
                if (currentRenderPipeline.defaultMaterial.shader.name.StartsWith(kHdrpShaderPrefix))
                {
                    return RenderPipeline.HDRP;
                }
                if (currentRenderPipeline.defaultMaterial.shader.name.StartsWith(kUrpShaderPrefix))
                {
                    return RenderPipeline.Universal;
                }
                return RenderPipeline.Unknown;
            }
        }
        internal string srpDefaultLightModeName 
        {
             get
             {
                const string legacyDefaultLightModeName = "Always";
                const string srpDefaultLightModeName = "SRPDefaultUnlit";

                if (currentRenderPipeline == RenderPipeline.Legacy)
                {
                    return legacyDefaultLightModeName; // default.
                }

                return srpDefaultLightModeName;
             }
        }


        internal  void TessellationSetting(Material material) 
        {
            if (currentRenderPipeline == RenderPipeline.HDRP)
            {

                TessellationSettingHDRP(material);
            }

        }
        internal  void RenderingPerChennelsSetting(Material material) 
        {
            if (currentRenderPipeline == RenderPipeline.HDRP)
            {

                RenderingPerChennelsSettingHDRP(material);
            }
        }
        internal  void ApplyTessellation(Material materal) 
        {
            if (currentRenderPipeline == RenderPipeline.HDRP)
            {
                ApplyTessellationHDRP(materal);
            }
        }
        internal  void ApplyRenderingPerChennelsSetting(Material material) 
        {

        }
        internal  void FindTessellationProperties(MaterialProperty[] props) 
        {
            if (currentRenderPipeline == RenderPipeline.HDRP)
            {

                FindTessellationPropertiesHDRP(props);
            }
        }

        protected const string ShaderDefineSHADINGGRADEMAP = "_SHADINGGRADEMAP";
        protected const string ShaderDefineANGELRING_ON = "_IS_ANGELRING_ON";
        protected const string ShaderDefineANGELRING_OFF = "_IS_ANGELRING_OFF";
        protected const string ShaderDefineUTS_USE_RAYTRACING_SHADOW = "UTS_USE_RAYTRACING_SHADOW";
        protected const string ShaderPropAngelRing = "_AngelRing";
        protected const string ShaderPropRTHS = "_RTHS";
        protected const string ShaderPropMatCap = "_MatCap";
        protected const string ShaderPropMainTex = "_MainTex";
        protected const string ShaderPropClippingMode = "_ClippingMode";
        protected const string ShaderPropClippingMask = "_ClippingMask";
        protected const string ShaderProp_Set_1st_ShadePosition = "_Set_1st_ShadePosition";
        protected const string ShaderProp_Set_2nd_ShadePosition = "_Set_2nd_ShadePosition";
        protected const string ShaderProp_ShadingGradeMap = "_ShadingGradeMap";
        protected const string ShaderProp_Set_RimLightMask = "_Set_RimLightMask";
        protected const string ShaderProp_HighColor_Tex = "_HighColor_Tex";
        protected const string ShaderProp_Set_HighColorMask = "_Set_HighColorMask";
        protected const string ShaderProp_MatCap_Sampler = "_MatCap_Sampler";
        protected const string ShaderProp_Set_MatcapMask = "_Set_MatcapMask";
        protected const string ShaderProp_OutlineTex = "_OutlineTex";
        protected const string ShaderProp_Outline_Sampler = "_Outline_Sampler";

        protected const string ShaderPropSimpleUI = "_simpleUI";
        protected const string ShaderPropUtsTechniqe = "_utsTechnique";
        protected const string ShaderPropAutoRenderQueue = "_AutoRenderQueue";
        protected const string ShaderPropStencilMode = "_StencilMode";
        protected const string ShaderPropStencilNo = "_StencilNo";
        protected const string ShaderPropTransparentEnabled = "_TransparentEnabled";
        protected const string ShaderPropStencilComp = "_StencilComp";
        protected const string ShaderPropStencilOpPass = "_StencilOpPass";
        protected const string ShaderPropStencilOpFail = "_StencilOpFail";
        protected const string ShaderPropStencilWriteMask = "_StencilWriteMask";
        protected const string ShaderPropStencilReadMask = "_StencilReadMask";
        protected const string ShaderPropUtsVersionX = "_utsVersionX";
        protected const string ShaderPropUtsVersionY = "_utsVersionY";
        protected const string ShaderPropUtsVersionZ = "_utsVersionZ";
        protected const string ShaderPropIsUnityToonShader = "_isUnityToonshader";
        protected const string ShaderPropOutline = "_OUTLINE";
        protected const string ShaderPropNormalMapToHighColor = "_Is_NormalMapToHighColor";
        protected const string ShaderPropIsNormalMapToRimLight = "_Is_NormalMapToRimLight";
        protected const string ShaderPropSetSystemShadowsToBase = "_Set_SystemShadowsToBase";
        protected const string ShaderPropIsFilterHiCutPointLightColor = "_Is_Filter_HiCutPointLightColor";
        protected const string ShaderPropInverseClipping = "_Inverse_Clipping";
        protected const string ShaderPropIsBaseMapAlphaAsClippingMask = "_IsBaseMapAlphaAsClippingMask";
        protected const string ShaderPropIsLightColor_Base = "_Is_LightColor_Base";
        protected const string ShaderPropCameraRolling_Stabilizer = "_CameraRolling_Stabilizer";
        protected const string ShaderPropIs_Ortho = "_Is_Ortho";
        protected const string ShaderPropGI_Intensity = "_GI_Intensity";
        protected const string ShaderPropUnlit_Intensity = "_Unlit_Intensity";
        protected const string ShaderPropIs_Filter_LightColor = "_Is_Filter_LightColor";
        protected const string ShaderPropIs_LightColor_1st_Shade = "_Is_LightColor_1st_Shade";
        protected const string ShaderPropIs_LightColor_2nd_Shade = "_Is_LightColor_2nd_Shade";
        protected const string ShaderPropIs_LightColor_HighColor = "_Is_LightColor_HighColor";
        protected const string ShaderPropIs_LightColor_RimLight = "_Is_LightColor_RimLight";
        protected const string ShaderPropIs_LightColor_Ap_RimLight = "_Is_LightColor_Ap_RimLight";
        protected const string ShaderPropIs_LightColor_MatCap = "_Is_LightColor_MatCap";
        protected const string ShaderPropIs_LightColor_AR = "_Is_LightColor_AR";
        protected const string ShaderPropIs_LightColor_Outline = "_Is_LightColor_Outline";
        protected const string ShaderPropInverse_MatcapMask = "_Inverse_MatcapMask";
        protected const string ShaderPropUse_BaseAs1st = "_Use_BaseAs1st";
        protected const string ShaderPropUse_1stAs2nd = "_Use_1stAs2nd";
        protected const string ShaderPropIs_NormalMapToBase = "_Is_NormalMapToBase";
        protected const string ShaderPropIs_ColorShift = "_Is_ColorShift";
        protected const string ShaderPropRimLight = "_RimLight";
        protected const string ShaderPropRimLight_FeatherOff = "_RimLight_FeatherOff";
        protected const string ShaderPropAp_RimLight_FeatherOff = "_Ap_RimLight_FeatherOff";
        protected const string ShaderPropIs_BlendAddToMatCap = "_Is_BlendAddToMatCap";
        protected const string ShaderPropARSampler_AlphaOn = "_ARSampler_AlphaOn";
        protected const string ShaderPropIs_UseTweakHighColorOnShadow = "_Is_UseTweakHighColorOnShadow";

        protected const string ShaderPropIs_SpecularToHighColor = "_Is_SpecularToHighColor";
        protected const string ShaderPropIs_BlendAddToHiColor = "_Is_BlendAddToHiColor";

        protected const string ShaderPropAdd_Antipodean_RimLight = "_Add_Antipodean_RimLight";
        protected const string ShaderPropLightDirection_MaskOn = "_LightDirection_MaskOn";

        protected const string ShaderProp1st_ShadeColor_Step = "_1st_ShadeColor_Step";
        protected const string ShaderPropBaseColor_Step = "_BaseColor_Step";
        protected const string ShaderProp1st_ShadeColor_Feather = "_1st_ShadeColor_Feather";
        protected const string ShaderPropBaseShade_Feather = "_BaseShade_Feather";
        protected const string ShaderProp2nd_ShadeColor_Step = "_2nd_ShadeColor_Step";
        protected const string ShaderPropShadeColor_Step = "_ShadeColor_Step";
        protected const string ShaderProp2nd_ShadeColor_Feather = "_2nd_ShadeColor_Feather";
        protected const string ShaderProp1st2nd_Shades_Feather = "_1st2nd_Shades_Feather";
        protected const string ShaderPropIs_NormalMapForMatCap = "_Is_NormalMapForMatCap";
        protected const string ShaderPropIs_UseTweakMatCapOnShadow = "_Is_UseTweakMatCapOnShadow";
        protected const string ShaderPropIs_ViewCoord_Scroll = "_Is_ViewCoord_Scroll";
        protected const string ShaderPropIs_PingPong_Base = "_Is_PingPong_Base";

        protected const string ShaderPropIs_ViewShift = "_Is_ViewShift";
        protected const string ShaderPropIs_BlendBaseColor = "_Is_BlendBaseColor";
        protected const string ShaderPropIs_OutlineTex = "_Is_OutlineTex";
        protected const string ShaderPropIs_BakedNormal = "_Is_BakedNormal";
        protected const string ShaderPropIs_BLD = "_Is_BLD";
        protected const string ShaderPropInverse_Z_Axis_BLD = "_Inverse_Z_Axis_BLD";



        

        protected const string ShaderDefineIS_OUTLINE_CLIPPING_NO = "_IS_OUTLINE_CLIPPING_NO";
        protected const string ShaderDefineIS_OUTLINE_CLIPPING_YES = "_IS_OUTLINE_CLIPPING_YES";

        protected const string ShaderDefineIS_CLIPPING_OFF = "_IS_CLIPPING_OFF";
        protected const string ShaderDefineIS_CLIPPING_MODE = "_IS_CLIPPING_MODE";
        protected const string ShaderDefineIS_CLIPPING_TRANSMODE = "_IS_CLIPPING_TRANSMODE";

        protected const string ShaderDefineIS_TRANSCLIPPING_OFF = "_IS_TRANSCLIPPING_OFF";
        protected const string ShaderDefineIS_TRANSCLIPPING_ON = "_IS_TRANSCLIPPING_ON";

        protected const string ShaderDefineIS_CLIPPING_MATTE = "_IS_CLIPPING_MATTE";
#if  USE_TOGGLE_BUTTONS
        protected const string STR_ONSTATE = "Active";
        protected const string STR_OFFSTATE = "Off";
#endif
        protected readonly string[] MainTexHash128 = { "_MainTexHash128_0", "_MainTexHash128_1", "_MainTexHash128_2", "_MainTexHash128_3" };
        protected readonly string[] MainTexGUID =  { "_MainTexGUID_0", "_MainTexGUID_1", "_MainTexGUID_2", "_MainTexGUID_3" };
       //
       protected readonly string[] ClippingMaskHash128 = { "_ClippingMaskHash128_0", "_ClippingMaskHash128_1", "_ClippingMaskHash128_2", "_ClippingMaskHash128_3" };
        protected readonly string[] ClippingMaskGUID = { "_ClippingMaskGUID_0", "_ClippingMaskGUID_1", "_ClippingMaskGUID_2", "_ClippingMaskGUID_3" };
        //
        protected readonly string[] Set_1st_ShadePositionHash128 = { "_Set_1st_ShadePositionHash128_0", "_Set_1st_ShadePositionHash128_1", "_Set_1st_ShadePositionHash128_2", "_Set_1st_ShadePositionHash128_3" };
        protected readonly string[] Set_1st_ShadePositionGUID = { "_Set_1st_ShadePositionGUID_0", "_Set_1st_ShadePositionGUID_1", "_Set_1st_ShadePositionGUID_2", "_Set_1st_ShadePositionGUID_3" };
        //
        protected readonly string[] Set_2nd_ShadePositionHash128 = { "_Set_2nd_ShadePositionHash128_0", "_Set_2nd_ShadePositionHash128_1", "_Set_2nd_ShadePositionHash128_2", "_Set_2nd_ShadePositionHash128_3" };
        protected readonly string[] Set_2nd_ShadePositionGUID = { "_Set_2nd_ShadePositionGUID_0", "_Set_2nd_ShadePositionGUID_1", "_Set_2nd_ShadePositionGUID_2", "_Set_2nd_ShadePositionGUID_3" };
        //
        protected readonly string[] ShadingGradeMapHash128 = { "_ShadingGradeMapHash128_0", "_ShadingGradeMapHash128_1", "_ShadingGradeMapHash128_2", "_ShadingGradeMapHash128_3" };
        protected readonly string[] ShadingGradeMapGUID = { "_ShadingGradeMapGUID_0", "_ShadingGradeMapGUID_1", "_ShadingGradeMapGUID_2", "_ShadingGradeMapGUID_3" };
        //
        protected readonly string[] Set_RimLightMaskHash128 = { "_Set_RimLightMaskHash128_0", "_Set_RimLightMaskHash128_1", "_Set_RimLightMaskHash128_2", "_Set_RimLightMaskHash128_3" };
        protected readonly string[] Set_RimLightMaskGUID = { "_Set_RimLightMaskGUID_0", "_Set_RimLightMaskGUID_1", "_Set_RimLightMaskGUID_2", "_Set_RimLightMaskGUID_3" };
        //
        protected readonly string[] HighColor_TexHash128 = { "_HighColor_TexHash128_0", "_HighColor_TexHash128_1", "_HighColor_TexHash128_2", "_HighColor_TexHash128_3" };
        protected readonly string[] HighColor_TexGUID = { "_Set_RimLightMaskGUID_0", "_Set_RimLightMaskGUID_1", "_Set_RimLightMaskGUID_2", "_Set_RimLightMaskGUID_3" };
        //
        protected readonly string[] Set_HighColorMaskHash128 = { "_Set_HighColorMaskHash128_0", "_Set_HighColorMaskHash128_1", "_Set_HighColorMaskHash128_2", "_Set_HighColorMaskHash128_3" };
        protected readonly string[] Set_HighColorMaskGUID = { "_Set_HighColorMaskGUID_0", "_Set_HighColorMaskGUID_1", "_Set_HighColorMaskGUID_2", "_Set_HighColorMaskGUID_3" };
        //
        protected readonly string[] MatCap_SamplerHash128 = { "_MatCap_SamplerHash128_0", "_MatCap_SamplerHash128_1", "_MatCap_SamplerHash128_2", "_MatCap_SamplerHash128_3" };
        protected readonly string[] MatCap_SamplerGUID = { "_MatCap_SamplerGUID_0", "_MatCap_SamplerGUID_1", "_MatCap_SamplerGUID_2", "_MatCap_SamplerGUID_3" };
        //
        protected readonly string[] Set_MatcapMaskHash128 = { "_Set_MatcapMaskHash128_0", "_Set_MatcapMaskHash128_1", "_Set_MatcapMaskHash128_2", "_Set_MatcapMaskHash128_3" };
        protected readonly string[] Set_MatcapMaskGUID = { "_Set_MatcapMaskGUID_0", "_Set_MatcapMaskGUID_1", "_Set_MatcapMaskGUID_2", "_Set_MatcapMaskGUID_3" };
        //
        protected readonly string[] Outline_SamplerHash128 = { "_Outline_SamplerHash128_0", "_Outline_SamplerHash128_1", "_Outline_SamplerHash128_2", "_Outline_SamplerHash128_3" };
        protected readonly string[] Outline_SamplerGUID = { "_Outline_SamplerGUID_0", "_Outline_SamplerGUID_1", "_Outline_SamplerGUID_2", "_Outline_SamplerGUID_3" };
        //
        protected readonly string[] OutlineTexHash128 = { "_OutlineTexHash128_0", "_OutlineTexHash128_1", "_OutlineTexHash128_2", "_OutlineTexHash128_3" };
        protected readonly string[] OutlineTexGUID = { "_OutlineTexGUID_0", "_OutlineTexGUID_1", "_OutlineTexGUID_2", "_OutlineTexGUID_3" };

        protected readonly string[] UtsTechniqueNames = { "Double Shade With Feather", "Shading Grade Map" };
        protected readonly string[] EmissiveScrollMode = { "UV Coordinate Scroll", "View Coordinate Scroll" };

        public enum _UTS_Technique
        {
            DoubleShadeWithFeather, ShadingGradeMap
        }

        public enum _UTS_ClippingMode
        {
            Off, On, TransClippingMode
        }

        public enum _UTS_TransClippingMode
        {
            Off, On,
        }
        public enum _UTS_Transparent
        {
            Off, On,
        }
        public enum _UTS_StencilMode
        {
            Off, StencilOut, StencilMask
        }
        public enum _UTS_SpeculerMode
        {
            Solid, Natural
        }
        public enum _UTS_SpeculerColorBlendMode
        {
            Multiply, Additive
        }
        public enum _UTS_MatcapColorBlendMode
        {
            Multiply, Additive
        }

        public enum _StencilOperation
        {
            //https://docs.unity3d.com/Manual/SL-Stencil.html
            Keep, //    Keep the current contents of the buffer.
            Zero, //    Write 0 into the buffer.
            Replace, // Write the reference value into the buffer.
            IncrSat, // Increment the current value in the buffer. If the value is 255 already, it stays at 255.
            DecrSat, // Decrement the current value in the buffer. If the value is 0 already, it stays at 0.
            Invert, //  Negate all the bits.
            IncrWrap, //    Increment the current value in the buffer. If the value is 255 already, it becomes 0.
            DecrWrap, //    Decrement the current value in the buffer. If the value is 0 already, it becomes 255.
        }

        public enum _StencilCompFunction
        {

            Disabled,//    Depth or stencil test is disabled.
            Never,   //   Never pass depth or stencil test.
            Less,   //   Pass depth or stencil test when new value is less than old one.
            Equal,  //  Pass depth or stencil test when values are equal.
            LessEqual, // Pass depth or stencil test when new value is less or equal than old one.
            Greater, // Pass depth or stencil test when new value is greater than old one.
            NotEqual, //    Pass depth or stencil test when values are different.
            GreaterEqual, // Pass depth or stencil test when new value is greater or equal than old one.
            Always,//  Always pass depth or stencil test.
        }



        public enum _OutlineMode
        {
            NormalDirection, PositionScaling
        }

        public enum _CullingMode
        {
            CullingOff, FrontCulling, BackCulling
        }

        public enum _EmissiveMode
        {
            SimpleEmissive, EmissiveAnimation
        }

        public enum _CameraProjectionType
        {
            Perspective,
            Orthographic
        }
        protected enum Expandable
        {
            Inputs = 1 << 0,
            Advanced = 1 << 1,
        }
        // variables which must be gotten from shader at the beggning of GUI
        internal int _autoRenderQueue = 1;
        internal int _renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        // variables which just to be held.
        internal _OutlineMode outlineMode;
        internal _CullingMode cullingMode;
        internal _EmissiveMode emissiveMode;

#if SRPCORE_IS_INSTALLED_FOR_UTS
        readonly MaterialHeaderScopeList m_MaterialScopeList = new MaterialHeaderScopeList(uint.MaxValue & ~((uint)Expandable.Advanced));

#endif // SRPCORE_IS_INSTALLED_FOR_UTS


        //Button sizes
        static internal GUILayoutOption[] longButtonStyle = new GUILayoutOption[] { GUILayout.Width(180) };
        static internal GUILayoutOption[] shortButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };
        static internal GUILayoutOption[] middleButtonStyle = new GUILayoutOption[] { GUILayout.Width(130) };
        static internal GUILayoutOption[] toggleStyle = new GUILayoutOption[] { GUILayout.Width(130) };

        //
        protected static _UTS_Transparent _Transparent_Setting;
        protected static int _StencilNo_Setting;
        protected static bool _OriginalInspector = false;
        protected static bool _SimpleUI = false;
#if USE_GAME_RECOMMENDATION
        //For display messages
        protected bool _Use_GameRecommend = false;

        internal GameRecommendation _GameRecommendationStore;
#endif //#if USE_GAME_RECOMMENDATION
        //Initial values of Foldout.
        protected static bool _BasicShaderSettings_Foldout = false;
        protected static bool _BasicThreeColors_Foldout = true;
        protected static bool _NormalMap_Foldout = false;
        protected static bool _ShadowControlMaps_Foldout = false;
        protected static bool _StepAndFeather_Foldout = true;
        protected static bool _AdditionalLookdevs_Foldout = false;
        protected static bool _HighColor_Foldout = true;

        protected static bool _RimLight_Foldout = true;
        protected static bool _MatCap_Foldout = true;
        protected static bool _AngelRing_Foldout = true;
        protected static bool _Emissive_Foldout = true;
        protected static bool _Outline_Foldout = true;
        protected static bool _AdvancedOutline_Foldout = false;
        protected static bool _Tessellation_Foldout = false;
        protected static bool _LightColorContribution_Foldout = false;
        protected static bool _AdditionalLightingSettings_Foldout = false;

        // -----------------------------------------------------
        //Specify only those that use the m_MaterialEditor method as their UI.
        // UTS3 materal properties -------------------------
        protected MaterialProperty utsTechnique = null;
        protected MaterialProperty specularMode = null;
        protected MaterialProperty specularBlendMode = null;
        protected MaterialProperty matcapCameraMode = null;
        protected MaterialProperty matcapBlendMode = null;
        protected MaterialProperty matcapOrtho = null;
        protected MaterialProperty transparentMode = null;
        protected MaterialProperty clippingMode = null;
        protected MaterialProperty clippingMask = null;
        protected MaterialProperty clipping_Level = null;
        protected MaterialProperty tweak_transparency = null;
        protected MaterialProperty stencilMode = null;
        protected MaterialProperty mainTex = null;
        protected MaterialProperty baseColor = null;
        protected MaterialProperty firstShadeMap = null;
        protected MaterialProperty firstShadeColor = null;
        protected MaterialProperty secondShadeMap = null;
        protected MaterialProperty secondShadeColor = null;
        protected MaterialProperty normalMap = null;
        protected MaterialProperty bumpScale = null;
        protected MaterialProperty set_1st_ShadePosition = null;
        protected MaterialProperty set_2nd_ShadePosition = null;
        protected MaterialProperty shadingGradeMap = null;
        protected MaterialProperty tweak_ShadingGradeMapLevel = null;
        protected MaterialProperty blurLevelSGM = null;
        protected MaterialProperty tweak_SystemShadowsLevel = null;
        protected MaterialProperty baseColor_Step = null;
        protected MaterialProperty baseShade_Feather = null;
        protected MaterialProperty shadeColor_Step = null;
        protected MaterialProperty first2nd_Shades_Feather = null;
        protected MaterialProperty first_ShadeColor_Step = null;
        protected MaterialProperty first_ShadeColor_Feather = null;
        protected MaterialProperty second_ShadeColor_Step = null;
        protected MaterialProperty second_ShadeColor_Feather = null;
        protected MaterialProperty stepOffset = null;
        protected MaterialProperty highColor_Tex = null;
        protected MaterialProperty highColor = null;
        protected MaterialProperty highColor_Power = null;
        protected MaterialProperty tweakHighColorOnShadow = null;
        protected MaterialProperty set_HighColorMask = null;
        protected MaterialProperty tweak_HighColorMaskLevel = null;
        protected MaterialProperty rimLightColor = null;
        protected MaterialProperty rimLight_Power = null;
        protected MaterialProperty rimLight_InsideMask = null;
        protected MaterialProperty tweak_LightDirection_MaskLevel = null;
        protected MaterialProperty ap_RimLightColor = null;
        protected MaterialProperty ap_RimLight_Power = null;
        protected MaterialProperty set_RimLightMask = null;
        protected MaterialProperty tweak_RimLightMaskLevel = null;
        protected MaterialProperty matCap_Sampler = null;
        protected MaterialProperty matCapColor = null;
        protected MaterialProperty blurLevelMatcap = null;
        protected MaterialProperty tweak_MatCapUV = null;
        protected MaterialProperty rotate_MatCapUV = null;
        protected MaterialProperty normalMapForMatCap = null;
        protected MaterialProperty bumpScaleMatcap = null;
        protected MaterialProperty rotate_NormalMapForMatCapUV = null;
        protected MaterialProperty tweakMatCapOnShadow = null;
        protected MaterialProperty set_MatcapMask = null;
        protected MaterialProperty tweak_MatcapMaskLevel = null;
        protected MaterialProperty angelRing_Sampler = null;
        protected MaterialProperty angelRing_Color = null;
        protected MaterialProperty ar_OffsetU = null;
        protected MaterialProperty ar_OffsetV = null;
        protected MaterialProperty emissive_Tex = null;
        protected MaterialProperty emissive_Color = null;
        protected MaterialProperty base_Speed = null;
        protected MaterialProperty scroll_EmissiveU = null;
        protected MaterialProperty scroll_EmissiveV = null;
        protected MaterialProperty rotate_EmissiveUV = null;
        protected MaterialProperty colorShift = null;
        protected MaterialProperty colorShift_Speed = null;
        protected MaterialProperty viewShift = null;
        protected MaterialProperty outline_Width = null;
        protected MaterialProperty outline_Color = null;
        protected MaterialProperty outline_Sampler = null;
        protected MaterialProperty offset_Z = null;
        protected MaterialProperty farthest_Distance = null;
        protected MaterialProperty nearest_Distance = null;
        protected MaterialProperty outlineTex = null;
        protected MaterialProperty bakedNormal = null;
        protected MaterialProperty tessEdgeLength = null;
        protected MaterialProperty tessPhongStrength = null;
        protected MaterialProperty tessExtrusionAmount = null;
        protected MaterialProperty gi_Intensity = null;
        protected MaterialProperty unlit_Intensity = null;
        protected MaterialProperty offset_X_Axis_BLD = null;
        protected MaterialProperty offset_Y_Axis_BLD = null;
        //------------------------------------------------------

        protected MaterialEditor m_MaterialEditor;

        protected Hash128 MainTexHash128Store = new Hash128();
        protected string MainTexGUIDStore;

        protected Hash128 ClippingMaskHash128Store = new Hash128();
        protected string ClippingMaskGUIDStore;

        protected Hash128 Set_1st_ShadePositionHash128Store = new Hash128();
        protected string Set_1st_ShadePositionGUIDStore;

        protected Hash128 Set_2nd_ShadePositionHash128Store = new Hash128();
        protected string Set_2nd_ShadePositionGUIDStore;

        protected Hash128 ShadingGradeMapHash128Store = new Hash128();
        protected string ShadingGradeMapGUIDStore;

        protected Hash128 Set_RimLightMaskHash128Store = new Hash128();
        protected string Set_RimLightMaskGUIDStore;

        protected Hash128 HighColor_TexHash128Store = new Hash128();
        protected string HighColor_TexGUIDStore;

        protected Hash128 Set_HighColorMaskHash128Store = new Hash128();
        protected string Set_HighColorMaskGUIDStore;

        protected Hash128 MatCap_SamplerHash128Store = new Hash128();
        protected string MatCap_SamplerGUIDStore;

        protected Hash128 Set_MatcapMaskHash128Store = new Hash128();
        protected string Set_MatcapMaskGUIDStore;

        protected Hash128 Outline_SamplerHash128Store = new Hash128();
        protected string Outline_SamplerGUIDStore;

        protected Hash128 OutlineTexHash128Store = new Hash128();
        protected string OutlineTexGUIDStore;


        protected Texture mainTexStore = null;
        protected Texture clippingMaskStore = null;
        protected Texture set1stShadePositionStore = null;
        protected Texture set2ndShadePositionStore = null;
        protected Texture shadingGradeMapStore = null;
        protected Texture rimLightMaskStore = null;
        protected Texture highColorTexStore = null;
        protected Texture highColorMaskStore = null;
        protected Texture matcapSamplerStore = null;
        protected Texture setMatcapMaskStore = null;
        protected Texture outlineTexStore = null;
        protected Texture outlineSamplerStore = null;

        const int HDRPGeometryMin = 2650; // UnityEngine.Rendering.RenderQueue.Geometry;
        private void UpdateVersionInMaterial(Material material)
        {
            material.SetInt(ShaderPropIsUnityToonShader, 1);
            material.SetFloat(ShaderPropUtsVersionX,  kVersionX);
            material.SetFloat(ShaderPropUtsVersionY,  kVersionY);
            material.SetFloat(ShaderPropUtsVersionZ,  kVersionZ);

        }
        private void StoreHashAndGUID(Material material)
        {
            mainTexStore = material.GetTexture(ShaderPropMainTex);
            if (mainTexStore != null )
            {
                MainTexHash128Store = mainTexStore.imageContentsHash;
                var path  = AssetDatabase.GetAssetPath(mainTexStore);
                MainTexGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                MainTexHash128Store = new Hash128();
                MainTexGUIDStore = null;
            }

            clippingMaskStore =  material.GetTexture(ShaderPropClippingMask);
            if (clippingMaskStore != null)
            {

                ClippingMaskHash128Store = clippingMaskStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(clippingMaskStore);
                ClippingMaskGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                ClippingMaskHash128Store = new Hash128();
                ClippingMaskGUIDStore = null;
            }

            set1stShadePositionStore = material.GetTexture(ShaderProp_Set_1st_ShadePosition);
            if (set1stShadePositionStore != null)
            {
                Set_1st_ShadePositionHash128Store = set1stShadePositionStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(set1stShadePositionStore);
                Set_1st_ShadePositionGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                Set_1st_ShadePositionHash128Store = new Hash128();
                Set_1st_ShadePositionGUIDStore = null;
            }

            set2ndShadePositionStore = material.GetTexture(ShaderProp_Set_2nd_ShadePosition);
            if (set2ndShadePositionStore != null)
            {
                Set_2nd_ShadePositionHash128Store = set2ndShadePositionStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(set2ndShadePositionStore);
                Set_2nd_ShadePositionGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                Set_2nd_ShadePositionHash128Store = new Hash128();
                Set_2nd_ShadePositionGUIDStore = null;
            }

            shadingGradeMapStore = material.GetTexture(ShaderProp_ShadingGradeMap);
            if (shadingGradeMapStore != null)
            {
                ShadingGradeMapHash128Store = shadingGradeMapStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(shadingGradeMapStore);
                ShadingGradeMapGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                ShadingGradeMapHash128Store = new Hash128();
                ShadingGradeMapGUIDStore = null;
            }

            rimLightMaskStore = material.GetTexture(ShaderProp_Set_RimLightMask);
            if (rimLightMaskStore != null)
            {
                Set_RimLightMaskHash128Store = rimLightMaskStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(rimLightMaskStore);
                Set_RimLightMaskGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                Set_RimLightMaskHash128Store = new Hash128();
                Set_RimLightMaskGUIDStore = null;
            }


            highColorTexStore = material.GetTexture(ShaderProp_HighColor_Tex);
            if (highColorTexStore != null)
            {
                HighColor_TexHash128Store = highColorTexStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(highColorTexStore);
                HighColor_TexGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                HighColor_TexHash128Store = new Hash128();
                HighColor_TexGUIDStore = null;
            }


            highColorMaskStore = material.GetTexture(ShaderProp_Set_HighColorMask);
            if (highColorMaskStore != null)
            {
                Set_HighColorMaskHash128Store = highColorMaskStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(highColorMaskStore);
                Set_HighColorMaskGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                Set_HighColorMaskHash128Store = new Hash128();
                Set_HighColorMaskGUIDStore = null;
            }


            matcapSamplerStore = material.GetTexture(ShaderProp_MatCap_Sampler);
            if (matcapSamplerStore != null)
            {
                MatCap_SamplerHash128Store = matcapSamplerStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(matcapSamplerStore);
                MatCap_SamplerGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                MatCap_SamplerHash128Store = new Hash128();
                MatCap_SamplerGUIDStore = null;
            }

            setMatcapMaskStore = material.GetTexture(ShaderProp_Set_MatcapMask);
            if (setMatcapMaskStore != null)
            {
                Set_MatcapMaskHash128Store = setMatcapMaskStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(setMatcapMaskStore);
                Set_MatcapMaskGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                Set_MatcapMaskHash128Store = new Hash128();
                Set_MatcapMaskGUIDStore = null;
            }

            outlineTexStore = material.GetTexture(ShaderProp_OutlineTex);
            if (outlineTexStore != null)
            {
                OutlineTexHash128Store = outlineTexStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(outlineTexStore);
                OutlineTexGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                OutlineTexHash128Store = new Hash128();
                OutlineTexGUIDStore = null;
            }

            outlineSamplerStore = material.GetTexture(ShaderProp_Outline_Sampler);
            if (outlineSamplerStore != null)
            {
                Outline_SamplerHash128Store = outlineSamplerStore.imageContentsHash;
                var path = AssetDatabase.GetAssetPath(outlineSamplerStore);
                Outline_SamplerGUIDStore = AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                Outline_SamplerHash128Store = new Hash128();
                Outline_SamplerGUIDStore = null;
            }

        }



        private bool IsClippingMaskPropertyAvailable(_UTS_Technique technique)
        {

            Material material = m_MaterialEditor.target as Material;
            bool bRet = false;
            switch (technique)
            {
                case _UTS_Technique.DoubleShadeWithFeather:
                    bRet = ((_UTS_ClippingMode)material.GetInt(ShaderPropClippingMode) != _UTS_ClippingMode.Off);
                    break;
                case _UTS_Technique.ShadingGradeMap:
                    bRet = (_UTS_TransClippingMode)material.GetInt(ShaderPropClippingMode) != _UTS_TransClippingMode.Off;
                    break;


            }

            return bRet;

        }

        private bool ClippingModePropertyAvailable
        {
            get
            {
                Material material = m_MaterialEditor.target as Material;
                return material.GetInt(ShaderPropClippingMode) != 0;
            }
        }

        private bool StencilShaderPropertyAvailable
        {
            get
            {
                //     Material material = m_MaterialEditor.target as Material;
                //     return (_UTS_StencilMode)material.GetInt(ShaderPropStencilMode) != _UTS_StencilMode.Off;
                return true;
            }
        }
        private bool IsShadingGrademap
        {
            get
            {

                Material material = m_MaterialEditor.target as Material;
                return material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.ShadingGradeMap;

            }
        }

        public static GUIContent specularModeText = new GUIContent("Specular Mode", "Specular light mode. Solid Color or Natural.");
        public static GUIContent specularBlendModeText = new GUIContent("Color Blend Mode", "Specular color blending mode. Multiply or Additive.");
        public static GUIContent matcapBlendModeText = new GUIContent("Color Blend Mode", "MatCap color blending mode. Multiply or Additive.");
        public static GUIContent matcapOrthoText = new GUIContent("MatCap Camera Mode", "MatCap camera mode. Perspective or Orthographic.");
        public static GUIContent transparentModeText = new GUIContent("Transparent Mode",
            "Transparent  mode that fits you. ");
        public static GUIContent workflowModeText = new GUIContent("Mode",
            "Select a workflow that fits your textures. Choose between DoubleShadeWithFeather or ShadingGradeMap.");
        // -----------------------------------------------------
        public static GUIContent clippingmodeModeText0 = new GUIContent("Clipping Mode",
            "Select clipping mode that fits you. ");
        public static GUIContent clippingmodeModeText1 = new GUIContent("Trans Clipping",
            "Select clipping mode that fits you. ");
        public static GUIContent stencilmodeModeText = new GUIContent("Stencil Mode",
    "Select stencil mode that fits you. ");
        //Specify only those that use the m_MaterialEditor method as their UI.
        public void FindProperties(MaterialProperty[] props)
        {
            // false is added if theare are possiblities the properties are not aveialable
            utsTechnique = FindProperty(ShaderPropUtsTechniqe, props);
            specularMode = FindProperty(ShaderPropIs_SpecularToHighColor, props);
            specularBlendMode = FindProperty(ShaderPropIs_BlendAddToHiColor, props);
            matcapBlendMode = FindProperty(ShaderPropIs_BlendAddToMatCap, props);
            matcapCameraMode = FindProperty(ShaderPropIs_Ortho, props);
            transparentMode = FindProperty(ShaderPropTransparentEnabled, props);
            clippingMask = FindProperty(ShaderPropClippingMask, props);
            clippingMode = FindProperty(ShaderPropClippingMode, props);
            clipping_Level = FindProperty("_Clipping_Level", props, false);
            tweak_transparency = FindProperty("_Tweak_transparency", props, false);
            stencilMode = FindProperty(ShaderPropStencilMode, props);
            mainTex = FindProperty(ShaderPropMainTex, props);
            baseColor = FindProperty("_BaseColor", props);
            firstShadeMap = FindProperty("_1st_ShadeMap", props);
            firstShadeColor = FindProperty("_1st_ShadeColor", props);
            secondShadeMap = FindProperty("_2nd_ShadeMap", props);
            secondShadeColor = FindProperty("_2nd_ShadeColor", props);
            normalMap = FindProperty("_NormalMap", props);
            bumpScale = FindProperty("_BumpScale", props);
            set_1st_ShadePosition = FindProperty(ShaderProp_Set_1st_ShadePosition, props, false);
            set_2nd_ShadePosition = FindProperty(ShaderProp_Set_2nd_ShadePosition, props, false);
            shadingGradeMap = FindProperty(ShaderProp_ShadingGradeMap, props, false);
            tweak_ShadingGradeMapLevel = FindProperty("_Tweak_ShadingGradeMapLevel", props, false);
            blurLevelSGM = FindProperty("_BlurLevelSGM", props, false);
            tweak_SystemShadowsLevel = FindProperty("_Tweak_SystemShadowsLevel", props);
            baseColor_Step = FindProperty(ShaderPropBaseColor_Step, props);
            baseShade_Feather = FindProperty(ShaderPropBaseShade_Feather, props);
            shadeColor_Step = FindProperty(ShaderPropShadeColor_Step, props);
            first2nd_Shades_Feather = FindProperty(ShaderProp1st2nd_Shades_Feather, props);
            first_ShadeColor_Step = FindProperty(ShaderProp1st_ShadeColor_Step, props);
            first_ShadeColor_Feather = FindProperty(ShaderProp1st_ShadeColor_Feather, props);
            second_ShadeColor_Step = FindProperty(ShaderProp2nd_ShadeColor_Step, props);
            second_ShadeColor_Feather = FindProperty(ShaderProp2nd_ShadeColor_Feather, props);
            stepOffset = FindProperty("_StepOffset", props, false);
            highColor_Tex = FindProperty(ShaderProp_HighColor_Tex, props);
            highColor = FindProperty("_HighColor", props);
            highColor_Power = FindProperty("_HighColor_Power", props);
            tweakHighColorOnShadow = FindProperty("_TweakHighColorOnShadow", props);
            set_HighColorMask = FindProperty(ShaderProp_Set_HighColorMask, props);
            tweak_HighColorMaskLevel = FindProperty("_Tweak_HighColorMaskLevel", props);
            rimLightColor = FindProperty("_RimLightColor", props);
            rimLight_Power = FindProperty("_RimLight_Power", props);
            rimLight_InsideMask = FindProperty("_RimLight_InsideMask", props);
            tweak_LightDirection_MaskLevel = FindProperty("_Tweak_LightDirection_MaskLevel", props);
            ap_RimLightColor = FindProperty("_Ap_RimLightColor", props);
            ap_RimLight_Power = FindProperty("_Ap_RimLight_Power", props);
            set_RimLightMask = FindProperty(ShaderProp_Set_RimLightMask, props);
            tweak_RimLightMaskLevel = FindProperty("_Tweak_RimLightMaskLevel", props);
            matCap_Sampler = FindProperty(ShaderProp_MatCap_Sampler, props);
            matCapColor = FindProperty("_MatCapColor", props);
            blurLevelMatcap = FindProperty("_BlurLevelMatcap", props);
            tweak_MatCapUV = FindProperty("_Tweak_MatCapUV", props);
            rotate_MatCapUV = FindProperty("_Rotate_MatCapUV", props);
            normalMapForMatCap = FindProperty("_NormalMapForMatCap", props);
            bumpScaleMatcap = FindProperty("_BumpScaleMatcap", props);
            rotate_NormalMapForMatCapUV = FindProperty("_Rotate_NormalMapForMatCapUV", props);
            tweakMatCapOnShadow = FindProperty("_TweakMatCapOnShadow", props);
            set_MatcapMask = FindProperty(ShaderProp_Set_MatcapMask, props);
            tweak_MatcapMaskLevel = FindProperty("_Tweak_MatcapMaskLevel", props);
            angelRing_Sampler = FindProperty("_AngelRing_Sampler", props, false);
            angelRing_Color = FindProperty("_AngelRing_Color", props, false);
            ar_OffsetU = FindProperty("_AR_OffsetU", props, false);
            ar_OffsetV = FindProperty("_AR_OffsetV", props, false);
            emissive_Tex = FindProperty("_Emissive_Tex", props);
            emissive_Color = FindProperty("_Emissive_Color", props);
            base_Speed = FindProperty("_Base_Speed", props);
            scroll_EmissiveU = FindProperty("_Scroll_EmissiveU", props);
            scroll_EmissiveV = FindProperty("_Scroll_EmissiveV", props);
            rotate_EmissiveUV = FindProperty("_Rotate_EmissiveUV", props);
            colorShift = FindProperty("_ColorShift", props);
            colorShift_Speed = FindProperty("_ColorShift_Speed", props);
            viewShift = FindProperty("_ViewShift", props);
            outline_Width = FindProperty("_Outline_Width", props, false);
            outline_Color = FindProperty("_Outline_Color", props, false);
            outline_Sampler = FindProperty(ShaderProp_Outline_Sampler, props, false);
            offset_Z = FindProperty("_Offset_Z", props, false);
            farthest_Distance = FindProperty("_Farthest_Distance", props, false);
            nearest_Distance = FindProperty("_Nearest_Distance", props, false);
            outlineTex = FindProperty(ShaderProp_OutlineTex, props, false);
            bakedNormal = FindProperty("_BakedNormal", props, false);
            tessEdgeLength = FindProperty("_TessEdgeLength", props, false);
            tessPhongStrength = FindProperty("_TessPhongStrength", props, false);
            tessExtrusionAmount = FindProperty("_TessExtrusionAmount", props, false);
            gi_Intensity = FindProperty(ShaderPropGI_Intensity, props);
            unlit_Intensity = FindProperty(ShaderPropUnlit_Intensity, props);
            offset_X_Axis_BLD = FindProperty("_Offset_X_Axis_BLD", props);
            offset_Y_Axis_BLD = FindProperty("_Offset_Y_Axis_BLD", props);

            FindTessellationProperties(props);
        }
        // --------------------------------

        // --------------------------------
        static void Line()
        {
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
        }

        protected static bool Foldout(bool display, string title)
        {
#if USE_TOGGLE_BUTTONS
            var style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.boldLabel).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(20f, -2f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                display = !display;
                e.Use();
            }

            return display;
#else
            return EditorGUILayout.Foldout(display, title, true );
#endif
        }

        static bool FoldoutSubMenu(bool display, string title)
        {
#if USE_TOGGLE_BUTTONS
            var style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.boldLabel).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.padding = new RectOffset(5, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(32f, -2f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 16f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                display = !display;
                e.Use();
            }

            return display;
#else
            return EditorGUILayout.Foldout(display, title, true);
#endif
        }




        // --------------------------------
        //Specify only those that use the m_MaterialEditor method as their UI. For specifying textures and colors on a single line.
        private static class Styles
        {
            public static GUIContent baseColorText = new GUIContent("Base Map", "Base Color : Texture(sRGB) × Color(RGB) Default:White");
            public static GUIContent firstShadeColorText = new GUIContent("1st Shading Map", "1st ShadeColor : Texture(sRGB) × Color(RGB) Default:White");
            public static GUIContent secondShadeColorText = new GUIContent("2nd Shading Map", "2nd ShadeColor : Texture(sRGB) × Color(RGB) Default:White");
            public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map : Texture(bump)");
            public static GUIContent highColorText = new GUIContent("High Light", "High Light : Texture(sRGB) × Color(RGB) Default:Black");
            public static GUIContent highColorMaskText = new GUIContent("High Light Mask", "High Light Mask : Texture(linear)");
            public static GUIContent rimLightMaskText = new GUIContent("Rim Light Mask", "Rim Light Mask : Texture(linear)");
            public static GUIContent matCapSamplerText = new GUIContent("MatCap Sampler", "MatCap Sampler : Texture(sRGB) × Color(RGB) Default:White");
            public static GUIContent matCapMaskText = new GUIContent("MatCap Mask", "MatCap Mask : Texture(linear)");
            public static GUIContent angelRingText = new GUIContent("Angel Ring", "AngelRing : Texture(sRGB) × Color(RGB) Default:Black");
            public static GUIContent emissiveTexText = new GUIContent("Emissive Map", "Emission : Texture(sRGB)× EmissiveMask(alpha) × Color(HDR) Default:Black");
            public static GUIContent shadingGradeMapText = new GUIContent("Shading Grade Map", "Specify shadow-prone areas in UV coordinates. Shading Grade Map : Texture(linear)");
            public static GUIContent firstPositionMapText = new GUIContent("1st Shade Position Map", "Specify the position of fixed shadows that fall in 1st shade color areas in UV coordinates. 1st Position Map : Texture(linear)");
            public static GUIContent secondPositionMapText = new GUIContent("2nd Shade Position Map", "Specify the position of fixed shadows that fall in 2nd shade color areas in UV coordinates. 2nd Position Map : Texture(linear)");
            public static GUIContent outlineSamplerText = new GUIContent("Outline Sampler", "Outline Sampler : Texture(linear)");
            public static GUIContent outlineTexText = new GUIContent("Outline tex", "Outline Tex : Texture(sRGB) Default:White");
            public static GUIContent bakedNormalOutlineText = new GUIContent("Baked NormalMap for Outline", "Unpacked Normal Map : Texture(linear) Note that this is not a standard NORMAL MAP.");
            public static GUIContent clippingMaskText = new GUIContent("Clipping Mask", "Clipping Mask : Texture(linear)");
        }
        // --------------------------------

        public UTS3GUI()
        {

        }

        // never called from the system??
        public override void OnClosed(Material material)
        { 

            base.OnClosed(material);
        }

        
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
//            base.OnGUI(materialEditor, props);


            EditorGUIUtility.fieldWidth = 0;
            FindProperties(props);
            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            UpdateVersionInMaterial(material);

            _autoRenderQueue = (int)material.GetInt(ShaderPropAutoRenderQueue);
            _Transparent_Setting = (_UTS_Transparent)material.GetInt(ShaderPropTransparentEnabled);
            _StencilNo_Setting = material.GetInt(ShaderPropStencilNo);

            //Line 1 horizontal 3 buttons.
            EditorGUILayout.BeginHorizontal();
#if USE_SIMPLE_UI   // disabled SimpleUI
            //Original Inspector Selection Check.
            if (material.HasProperty(ShaderPropSimpleUI))
            {
                var selectedUI = material.GetInt(ShaderPropSimpleUI);
                if (selectedUI == 2)
                {
                    _OriginalInspector = true;  //Original GUI
                }
                else if (selectedUI == 1)
                {
                    _SimpleUI = true;   //UTS2 Beginner GUI
                }
                //Original/Custom GUI toggle button.
                if (_OriginalInspector)
                {
                    if (GUILayout.Button("Change CustomUI", middleButtonStyle))
                    {
                        _OriginalInspector = false;
                        material.SetInt(ShaderPropSimpleUI, 0); //UTS2 Pro GUI
                    }
                    OpenManualLink();
                    //Clear inherited layouts.
                    EditorGUILayout.EndHorizontal();
                    //Show Original GUI.
                    m_MaterialEditor.PropertiesDefaultGUI(props);
                    return;
                }
                if (GUILayout.Button("Show All properties", middleButtonStyle))
                {
                    _OriginalInspector = true;
                    material.SetInt(ShaderPropSimpleUI, 2); //Original GUI
                }
            }
#endif
#if USE_MANUAL_LINK_BUTTONS
            //Open the manual link.
            OpenManualLink();
#endif
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();



            // select UTS technique here.
            DoPopup(workflowModeText, utsTechnique, UtsTechniqueNames);



            _UTS_Technique technique = (_UTS_Technique)material.GetInt(ShaderPropUtsTechniqe);
            switch (technique)
            {
                case _UTS_Technique.DoubleShadeWithFeather:
                    material.DisableKeyword(ShaderDefineSHADINGGRADEMAP);
                    break;
                case _UTS_Technique.ShadingGradeMap:
                    material.EnableKeyword(ShaderDefineSHADINGGRADEMAP);
                    break;
            }
            EditorGUILayout.Space();



            _BasicShaderSettings_Foldout = Foldout(_BasicShaderSettings_Foldout, "Shader Settings");
            if (_BasicShaderSettings_Foldout)
            {
                EditorGUI.indentLevel++;
                //EditorGUILayout.Space(); 
                GUI_SetCullingMode(material);
                GUI_SetRenderQueue(material);
                GUI_Tranparent(material);
                if (StencilShaderPropertyAvailable)
                {
                    GUI_StencilMode(material);

                }


                switch (technique)
                {
                    case _UTS_Technique.DoubleShadeWithFeather:
                        GUILayout.Label("Clipping Shader", EditorStyles.boldLabel);
                        DoPopup(clippingmodeModeText0, clippingMode, System.Enum.GetNames(typeof(_UTS_ClippingMode)));
                        break;
                    case _UTS_Technique.ShadingGradeMap:
                        GUILayout.Label("TransClipping Shader", EditorStyles.boldLabel);
                        DoPopup(clippingmodeModeText1, clippingMode, System.Enum.GetNames(typeof(_UTS_TransClippingMode)));
                        break;
                }

                EditorGUILayout.Space();
                if (IsClippingMaskPropertyAvailable(technique))
                {
                    GUI_SetClippingMask(material);
                    GUI_SetTransparencySetting(material);
                }



                GUI_OptionMenu(material);

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            _BasicThreeColors_Foldout = Foldout(_BasicThreeColors_Foldout, "3 Basic Color and Control Map Settings");
            if (_BasicThreeColors_Foldout)
            {
                EditorGUI.indentLevel++;
                //EditorGUILayout.Space(); 
                GUI_BasicThreeColors(material);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            _StepAndFeather_Foldout = Foldout(_StepAndFeather_Foldout, "Basic Lookdevs : Shading Step and Feather Settings");
            if (_StepAndFeather_Foldout)
            {
                EditorGUI.indentLevel++;
                //EditorGUILayout.Space(); 
                GUI_StepAndFeather(material);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            _HighColor_Foldout = Foldout(_HighColor_Foldout, "High Light Settings");
            if (_HighColor_Foldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                GUI_HighColor(material);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();



            EditorGUILayout.Space();

            _RimLight_Foldout = Foldout(_RimLight_Foldout, "Rim Light Settings");
            if (_RimLight_Foldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                GUI_RimLight(material);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            _MatCap_Foldout = Foldout(_MatCap_Foldout, "MatCap Settings");
            if (_MatCap_Foldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                GUI_MatCap(material);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            if (IsShadingGrademap)
            {
                _AngelRing_Foldout = Foldout(_AngelRing_Foldout, "Angel Ring Projection Settings");
                if (_AngelRing_Foldout)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space();
                    GUI_AngelRing(material);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();
            }

            _Emissive_Foldout = Foldout(_Emissive_Foldout, "Emission Settings");
            if (_Emissive_Foldout)
            {
                EditorGUI.indentLevel++;
                //EditorGUILayout.Space();
                GUI_Emissive(material);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();


            if (material.HasProperty(ShaderPropOutline) && _Transparent_Setting != _UTS_Transparent.On)
            {
                SetuOutline(material);
                _Outline_Foldout = Foldout(_Outline_Foldout, "Outline Settings");
                if (_Outline_Foldout)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space();
                    GUI_Outline(material);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Space();
            }
            else
            {
                SetupOverDrawTransparentObject(material);
            }
            if (material.HasProperty("_TessEdgeLength") && currentRenderPipeline == RenderPipeline.Legacy)
            {
                _Tessellation_Foldout = Foldout(_Tessellation_Foldout, "Legacy Pipeline: Phong Tessellation Settings");
                if (_Tessellation_Foldout)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space();
                    GUI_Tessellation(material);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();
            }

            if (!_SimpleUI)
            {
                _LightColorContribution_Foldout = Foldout(_LightColorContribution_Foldout, "Light Color Effectiveness to Material Settings");
                if (_LightColorContribution_Foldout)
                {
                    EditorGUI.indentLevel++;
                    //EditorGUILayout.Space();
                    GUI_LightColorContribution(material);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();

                _AdditionalLightingSettings_Foldout = Foldout(_AdditionalLightingSettings_Foldout, "Environmental Lighting Effectiveness Settings");
                if (_AdditionalLightingSettings_Foldout)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space();
                    GUI_AdditionalLightingSettings(material);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Space();
                TessellationSetting(material);
                EditorGUILayout.Space();

                RenderingPerChennelsSetting(material);
            }

            ApplyRenderingPerChennelsSetting(material);
            ApplyClippingMode(material);
            ApplyStencilMode(material);
            ApplyAngelRing(material);
            ApplyTessellation(material);
            ApplyMatCapMode(material);
            ApplyQueueAndRenderType(technique, material);



            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialEditor.PropertiesChanged();
            }

        }// End of OnGUI()




        int  MaterialGetInt(Material material, string prop )
        {
#if UNITY_2021_1_OR_NEWER
            return material.GetInteger(prop);
#else
            return material.GetInt(prop);
#endif
        }
        void MaterialSetInt(Material material, string prop, int value)
        {
#if UNITY_2021_1_OR_NEWER
            material.SetInteger(prop, value);
#else
            material.SetInt(prop, value);
#endif
        }
        bool GUI_ToggleShaderKeyword(Material material, string label, string keyword)
        {
            var isEnabled = material.IsKeywordEnabled(keyword);

            EditorGUI.BeginChangeCheck();
            var ret = EditorGUILayout.Toggle(label, isEnabled);
            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialEditor.RegisterPropertyChangeUndo(label);
                if ( ret == false )
                {
                    material.DisableKeyword(keyword);
                }
                else
                {
                    material.EnableKeyword(keyword);
                }
            }
            return ret;
        }
        bool GUI_Toggle(Material material, string label, string prop, bool value)
        {
            EditorGUI.BeginChangeCheck();
            var ret = EditorGUILayout.Toggle(label, value);
            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialEditor.RegisterPropertyChangeUndo(label);
                MaterialSetInt(material, prop, ret ? 1 : 0);
            }
            return ret;
        }

        // --------------------------------

        void CheckUtsTechnique(Material material)
        {
            if (material.HasProperty(ShaderPropUtsTechniqe))//DoubleWithFeather==0 or ShadingGradeMap==1
            {
                if (material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.DoubleShadeWithFeather)   //DWF
                {
                    if (!material.HasProperty(ShaderProp_Set_1st_ShadePosition))
                    {
                        //Change to SGM
                        material.SetInt(ShaderPropUtsTechniqe, (int)_UTS_Technique.ShadingGradeMap);
                    }
                }
                else if (material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.ShadingGradeMap)
                {    //SGM
                    if (!material.HasProperty("_ShadingGradeMap"))
                    {
                        //Change to DWF
                        material.SetInt(ShaderPropUtsTechniqe, (int)_UTS_Technique.DoubleShadeWithFeather);
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }
#if USE_MANUAL_LINK_BUTTONS
        void OpenManualLink()
        {
            if (GUILayout.Button("日本語マニュアル", middleButtonStyle))
            {
                Application.OpenURL("https://github.com/unity3d-jp/UnityChanToonShaderVer2_Project/blob/urp/master/Manual/UTS2_Manual_ja.md");
            }
            if (GUILayout.Button("English manual", middleButtonStyle))
            {
                Application.OpenURL("https://github.com/unity3d-jp/UnityChanToonShaderVer2_Project/blob/urp/master/Manual/UTS2_Manual_en.md");
            }
        }
#endif
        void GUI_SetRTHS(Material material)
        {

            EditorGUILayout.BeginHorizontal();

#if USE_TOGGLE_BUTTONS
            EditorGUILayout.PrefixLabel("Raytraced Hard Shadow");
            var isRTHSenabled = material.IsKeywordEnabled(ShaderDefineUTS_USE_RAYTRACING_SHADOW);
            if (isRTHSenabled)
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.DisableKeyword(ShaderDefineUTS_USE_RAYTRACING_SHADOW);
                }
            }
            else
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.EnableKeyword(ShaderDefineUTS_USE_RAYTRACING_SHADOW);
                }
            }
#else
            var isRTHSenabled = GUI_ToggleShaderKeyword(material, "Raytraced Hard Shadow", ShaderDefineUTS_USE_RAYTRACING_SHADOW);
#endif
            EditorGUILayout.EndHorizontal();
            if (isRTHSenabled)
            {
                EditorGUILayout.LabelField("ShadowRaytracer component must be attached to the camera when this feature is enabled.");
            }
        }

        void GUI_SetRenderQueue(Material material)
        {

            EditorGUILayout.BeginHorizontal();
#if USE_TOGGLE_BUTTONS
            EditorGUILayout.PrefixLabel("Auto Queue");

            if (_autoRenderQueue == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetInt(ShaderPropAutoRenderQueue, _autoRenderQueue = 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetInt(ShaderPropAutoRenderQueue, _autoRenderQueue = 0);
                }
            }
#else
            _autoRenderQueue = GUI_Toggle(material, "Auto Render Queue", ShaderPropAutoRenderQueue, _autoRenderQueue == 1) ? 1 : 0;
#endif
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(_autoRenderQueue == 1);
            _renderQueue = (int)EditorGUILayout.IntField("Render Queue", material.renderQueue);
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
        }


        void GUI_SetCullingMode(Material material)
        {
            const string _CullMode = "_CullMode";
            int _CullMode_Setting = material.GetInt(_CullMode);
            //Convert it to Enum format and store it in the offlineMode variable.
            if ((int)_CullingMode.CullingOff == _CullMode_Setting)
            {
                cullingMode = _CullingMode.CullingOff;
            }
            else if ((int)_CullingMode.FrontCulling == _CullMode_Setting)
            {
                cullingMode = _CullingMode.FrontCulling;
            }
            else
            {
                cullingMode = _CullingMode.BackCulling;
            }
            //GUI description with EnumPopup.
            cullingMode = (_CullingMode)EditorGUILayout.EnumPopup("Culling Mode", cullingMode);
            //If the value changes, write to the material.
            if (_CullMode_Setting != (int)cullingMode)
            {
                switch (cullingMode)
                {
                    case _CullingMode.CullingOff:
                        material.SetInt(_CullMode, 0);
                        break;
                    case _CullingMode.FrontCulling:
                        material.SetInt(_CullMode, 1);
                        break;
                    default:
                        material.SetInt(_CullMode, 2);
                        break;
                }

            }


        }
        void GUI_Tranparent(Material material)
        {
            GUILayout.Label("Transparent Shader", EditorStyles.boldLabel);
            const string _ZWriteMode = "_ZWriteMode";
            const string _ZOverDrawMode = "_ZOverDrawMode";
            DoPopup(transparentModeText, transparentMode, System.Enum.GetNames(typeof(_UTS_Transparent)));


            if (_Transparent_Setting == _UTS_Transparent.On)
            {
                if (material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.DoubleShadeWithFeather)
                {
                    material.SetInt(ShaderPropClippingMode, (int)_UTS_ClippingMode.TransClippingMode);
                }
                else
                {
                    // ShadingGradeMap
                    material.SetInt(ShaderPropClippingMode, (int)_UTS_TransClippingMode.On);
                }
                material.SetInt(_ZWriteMode, 0);
                material.SetFloat(_ZOverDrawMode, 1);
            }
            else
            {
                material.SetInt(_ZWriteMode, 1);
                material.SetFloat(_ZOverDrawMode, 0);
            }

        }

        void GUI_StencilMode(Material material)
        {
            GUILayout.Label("StencilMask or StencilOut Shader", EditorStyles.boldLabel);
            DoPopup(stencilmodeModeText, stencilMode, System.Enum.GetNames(typeof(_UTS_StencilMode)));


            int _Current_StencilNo = _StencilNo_Setting;
            _Current_StencilNo = (int)EditorGUILayout.IntField("Stencil Number", _Current_StencilNo);
            if (_StencilNo_Setting != _Current_StencilNo)
            {
                material.SetInt(ShaderPropStencilNo, _Current_StencilNo);
            }

        }

        void GUI_SetClippingMask(Material material)
        {
            GUILayout.Label("Options for Clipping or TransClipping features", EditorStyles.boldLabel);
            m_MaterialEditor.TexturePropertySingleLine(Styles.clippingMaskText, clippingMask);
#if USE_TOGGLE_BUTTONS
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Inverse Clipping Mask");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropInverseClipping) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropInverseClipping, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropInverseClipping, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
#else
            GUI_Toggle(material, "Inverse Clipping Mask", ShaderPropInverseClipping,MaterialGetInt(material, ShaderPropInverseClipping)!= 0 );
#endif

            m_MaterialEditor.RangeProperty(clipping_Level, "Clipping Level");
        }

        void GUI_SetTransparencySetting(Material material)
        {

            GUILayout.Label("Options for TransClipping or Transparent features", EditorStyles.boldLabel);
            m_MaterialEditor.RangeProperty(tweak_transparency, "Transparency Level");
#if USE_TOGGLE_BUTTONS
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Use BaseMap α as Clipping Mask");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIsBaseMapAlphaAsClippingMask) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsBaseMapAlphaAsClippingMask, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsBaseMapAlphaAsClippingMask, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
#else
            GUI_Toggle(material, "Use BaseMap Alpha as Clipping Mask", ShaderPropIsBaseMapAlphaAsClippingMask, MaterialGetInt(material, ShaderPropIsBaseMapAlphaAsClippingMask) != 0);
#endif
        }

        void GUI_OptionMenu(Material material)
        {
#if USE_SIMPLE_UI // we remove SIMPLE UI feature.
            GUILayout.Label("Option Menu", EditorStyles.boldLabel);
            if (material.HasProperty(ShaderPropSimpleUI))
            {
                if (material.GetInt(ShaderPropSimpleUI) == 1)
                {
                    _SimpleUI = true; //UTS2 Custom GUI Beginner
                }
                else
                {
                    _SimpleUI = false; //UTS2 Custom GUI Pro
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Current UI Type");
            //GUILayout.Space(60);
            if (_SimpleUI == false)
            {
                if (GUILayout.Button("Pro / Full Control", middleButtonStyle))
                {
                    material.SetInt(ShaderPropSimpleUI, 1); //UTS2 Custom GUI Beginner
                }
            }
            else
            {
                if (GUILayout.Button("Beginner", middleButtonStyle))
                {
                    material.SetInt(ShaderPropSimpleUI, 0); //UTS2 Custom GUI Pro
                }
            }
            EditorGUILayout.EndHorizontal();
#endif
#if USE_GAME_RECOMMENDATION // we remove Game Recommendation window feature.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Game Recommendation");
            //GUILayout.Space(60);
            if (GUILayout.Button("Check Settings", middleButtonStyle))
            {
                OpenOptimizationForGameWindow(material);
                _Use_GameRecommend = true;
            }
            EditorGUILayout.EndHorizontal();
            if (_Use_GameRecommend)
            {
                EditorGUILayout.HelpBox("UTS : Applying Game Recommended Settings.", MessageType.Info);
            }
#endif

        }

#if USE_GAME_RECOMMENDATION
        //
        void OpenOptimizationForGameWindow(Material material)
        {
            StoreGameRecommendation(material);
            GameRecommendationWindow.OpenWindow(this, material);
        }

        void StoreGameRecommendation(Material material)
        {
            _GameRecommendationStore.ShaderPropIsLightColor_Base = material.GetFloat(ShaderPropIsLightColor_Base);
            _GameRecommendationStore.ShaderPropIs_LightColor_1st_Shade = material.GetFloat(ShaderPropIs_LightColor_1st_Shade);
            _GameRecommendationStore.ShaderPropIs_LightColor_2nd_Shade = material.GetFloat(ShaderPropIs_LightColor_2nd_Shade);
            _GameRecommendationStore.ShaderPropIs_LightColor_HighColor = material.GetFloat(ShaderPropIs_LightColor_HighColor);
            _GameRecommendationStore.ShaderPropIs_LightColor_RimLight = material.GetFloat(ShaderPropIs_LightColor_RimLight);
            _GameRecommendationStore.ShaderPropIs_LightColor_Ap_RimLight = material.GetFloat(ShaderPropIs_LightColor_Ap_RimLight);
            _GameRecommendationStore.ShaderPropIs_LightColor_MatCap = material.GetFloat(ShaderPropIs_LightColor_MatCap);
            _GameRecommendationStore.ShaderPropIs_LightColor_AR = material.GetFloat(ShaderPropIs_LightColor_AR);
            _GameRecommendationStore.ShaderPropIs_LightColor_Outline = material.GetFloat(ShaderPropIs_LightColor_Outline);
            _GameRecommendationStore.ShaderPropSetSystemShadowsToBase = material.GetFloat(ShaderPropSetSystemShadowsToBase);
            _GameRecommendationStore.ShaderPropIsFilterHiCutPointLightColor = material.GetFloat(ShaderPropIsFilterHiCutPointLightColor);
            _GameRecommendationStore.ShaderPropCameraRolling_Stabilizer = material.GetFloat(ShaderPropCameraRolling_Stabilizer);
            _GameRecommendationStore.ShaderPropIs_Ortho = material.GetFloat(ShaderPropIs_Ortho);
            _GameRecommendationStore.ShaderPropGI_Intensity = material.GetFloat(ShaderPropGI_Intensity);
            _GameRecommendationStore.ShaderPropUnlit_Intensity = material.GetFloat(ShaderPropUnlit_Intensity);
            _GameRecommendationStore.ShaderPropIs_Filter_LightColor = material.GetFloat(ShaderPropIs_Filter_LightColor);
        }

        void RestoreGameRecommendation(Material material)
        {
            material.SetFloat(ShaderPropIsLightColor_Base, _GameRecommendationStore.ShaderPropIsLightColor_Base);
            material.SetFloat(ShaderPropIs_LightColor_1st_Shade, _GameRecommendationStore.ShaderPropIs_LightColor_1st_Shade);
            material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, _GameRecommendationStore.ShaderPropIs_LightColor_2nd_Shade);
            material.SetFloat(ShaderPropIs_LightColor_HighColor, _GameRecommendationStore.ShaderPropIs_LightColor_HighColor);
            material.SetFloat(ShaderPropIs_LightColor_RimLight, _GameRecommendationStore.ShaderPropIs_LightColor_RimLight);
            material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, _GameRecommendationStore.ShaderPropIs_LightColor_Ap_RimLight);
            material.SetFloat(ShaderPropIs_LightColor_MatCap, _GameRecommendationStore.ShaderPropIs_LightColor_MatCap);
            if (material.HasProperty(ShaderPropAngelRing))
            {//When AngelRing is available
                material.SetFloat(ShaderPropIs_LightColor_AR, _GameRecommendationStore.ShaderPropIs_LightColor_AR);
            }
            if (material.HasProperty(ShaderPropOutline))//OUTLINEがある場合.
            {
                material.SetFloat(ShaderPropIs_LightColor_Outline, _GameRecommendationStore.ShaderPropIs_LightColor_Outline);
            }
            material.SetFloat(ShaderPropSetSystemShadowsToBase, _GameRecommendationStore.ShaderPropSetSystemShadowsToBase);
            material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, _GameRecommendationStore.ShaderPropIsFilterHiCutPointLightColor);
            material.SetFloat(ShaderPropCameraRolling_Stabilizer, _GameRecommendationStore.ShaderPropCameraRolling_Stabilizer);
            material.SetFloat(ShaderPropIs_Ortho, _GameRecommendationStore.ShaderPropIs_Ortho);
            material.SetFloat(ShaderPropGI_Intensity, _GameRecommendationStore.ShaderPropGI_Intensity);
            material.SetFloat(ShaderPropUnlit_Intensity, _GameRecommendationStore.ShaderPropUnlit_Intensity);
            material.SetFloat(ShaderPropIs_Filter_LightColor, _GameRecommendationStore.ShaderPropIs_Filter_LightColor);
        }
        void SetGameRecommendation(Material material)
        {
            material.SetFloat(ShaderPropIsLightColor_Base, 1);
            material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
            material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
            material.SetFloat(ShaderPropIs_LightColor_HighColor, 1);
            material.SetFloat(ShaderPropIs_LightColor_RimLight, 1);
            material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 1);
            material.SetFloat(ShaderPropIs_LightColor_MatCap, 1);
            if (material.HasProperty(ShaderPropAngelRing))
            {//When AngelRing is available
                material.SetFloat(ShaderPropIs_LightColor_AR, 1);
            }
            if (material.HasProperty(ShaderPropOutline))// when outline is availbe
            {
                material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
            }
            material.SetFloat(ShaderPropSetSystemShadowsToBase, 1);
            material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, 1);
            material.SetFloat(ShaderPropCameraRolling_Stabilizer, 1);
            material.SetFloat(ShaderPropIs_Ortho, 0);
            material.SetFloat(ShaderPropGI_Intensity, 0);
            material.SetFloat(ShaderPropUnlit_Intensity, 1);
            material.SetFloat(ShaderPropIs_Filter_LightColor, 1);
        }
#endif //#if USE_GAME_RECOMMENDATION
        void GUI_BasicThreeColors(Material material)
        {
#if USE_TOGGLE_BUTTONS
            GUILayout.Label("3 Basic Colors Settings : Textures × Colors", EditorStyles.boldLabel);
#endif

            m_MaterialEditor.TexturePropertySingleLine(Styles.baseColorText, mainTex, baseColor);
            //v.2.0.7 Synchronize _Color to _BaseColor.
            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", material.GetColor("_BaseColor"));
            }
            //
#if USE_TOGGLE_BUTTONS
            if (material.GetFloat(ShaderPropUse_BaseAs1st) == 0)
            {
                if (GUILayout.Button("No Sharing", middleButtonStyle))
                {
                    material.SetFloat(ShaderPropUse_BaseAs1st, 1);
                }
            }
            else
            {
                if (GUILayout.Button("With 1st ShadeMap", middleButtonStyle))
                {
                    material.SetFloat(ShaderPropUse_BaseAs1st, 0);
                }
            }
#else

            EditorGUI.indentLevel += 2;
            var applyTo1st = GUI_Toggle(material, "Apply to 1st shading map", ShaderPropUse_BaseAs1st, MaterialGetInt(material, ShaderPropUse_BaseAs1st) != 0);
            EditorGUI.indentLevel -= 2;
#endif



            EditorGUI.BeginDisabledGroup(applyTo1st);
            m_MaterialEditor.TexturePropertySingleLine(Styles.firstShadeColorText, firstShadeMap, firstShadeColor);
            EditorGUI.EndDisabledGroup();
#if USE_TOGGLE_BUTTONS
            if (material.GetFloat(ShaderPropUse_1stAs2nd) == 0)
            {
                if (GUILayout.Button("No Sharing", middleButtonStyle))
                {
                    material.SetFloat(ShaderPropUse_1stAs2nd, 1);
                }
            }
            else
            {
                if (GUILayout.Button("With 2nd ShadeMap", middleButtonStyle))
                {
                    material.SetFloat(ShaderPropUse_1stAs2nd, 0);
                }
            }
#else
            EditorGUI.indentLevel+=2;
            var applyTo2nd =  GUI_Toggle(material, "Apply to 2nd shading map", ShaderPropUse_1stAs2nd, MaterialGetInt(material, ShaderPropUse_1stAs2nd) != 0);
            EditorGUI.indentLevel-=2;
#endif

            EditorGUI.BeginDisabledGroup(applyTo2nd);
            m_MaterialEditor.TexturePropertySingleLine(Styles.secondShadeColorText, secondShadeMap, secondShadeColor);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();

            _NormalMap_Foldout = FoldoutSubMenu(_NormalMap_Foldout, "NormalMap Settings");
            if (_NormalMap_Foldout)
            {
                //GUILayout.Label("NormalMap Settings", EditorStyles.boldLabel);
                m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMap, bumpScale);
                m_MaterialEditor.TextureScaleOffsetProperty(normalMap);

                EditorGUI.indentLevel++;

                GUILayout.Label("  NormalMap Effectiveness", EditorStyles.boldLabel);
#if USE_TOGGLE_BUTTONS
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("3 Basic Colors");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_NormalMapToBase) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetInt(ShaderPropIs_NormalMapToBase, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetInt(ShaderPropIs_NormalMapToBase, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("HighColor");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropNormalMapToHighColor) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropNormalMapToHighColor, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropNormalMapToHighColor, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("RimLight");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIsNormalMapToRimLight) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIsNormalMapToRimLight, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIsNormalMapToRimLight, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
#else
                GUI_Toggle(material, "3 Basic Colors", ShaderPropIs_NormalMapToBase, MaterialGetInt(material, ShaderPropIs_NormalMapToBase) != 0);
                GUI_Toggle(material, "High Light", ShaderPropNormalMapToHighColor, MaterialGetInt(material, ShaderPropNormalMapToHighColor) != 0);
                GUI_Toggle(material, "Rim Light", ShaderPropIsNormalMapToRimLight, MaterialGetInt(material, ShaderPropIsNormalMapToRimLight) != 0);

#endif
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            _ShadowControlMaps_Foldout = FoldoutSubMenu(_ShadowControlMaps_Foldout, "Shadow Control Maps");
            if (_ShadowControlMaps_Foldout)
            {
                EditorGUI.indentLevel++;
                GUI_ShadowControlMaps(material);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
        }

        void GUI_ShadowControlMaps(Material material)
        {
            if (material.HasProperty(ShaderPropUtsTechniqe))//DoubleWithFeather or ShadingGradeMap
            {
                if (material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.DoubleShadeWithFeather)   //DWF
                {
                    GUILayout.Label("  Mode: Double Shade With Feather", EditorStyles.boldLabel);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.firstPositionMapText, set_1st_ShadePosition);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.secondPositionMapText, set_2nd_ShadePosition);
                }
                else if (material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.ShadingGradeMap)
                {    //SGM
                    GUILayout.Label("  Mode: Shading Grade Map", EditorStyles.boldLabel);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.shadingGradeMapText, shadingGradeMap);
                    m_MaterialEditor.RangeProperty(tweak_ShadingGradeMapLevel, "ShadingGradeMap Level");
                    m_MaterialEditor.RangeProperty(blurLevelSGM, "Blur Level of ShadingGradeMap");
                }
            }
        }

        void GUI_StepAndFeather(Material material)
        {
            GUI_BasicLookdevs(material);

            if (!_SimpleUI)
            {
                GUI_SystemShadows(material);

                if (material.HasProperty("_StepOffset"))//Items not in Mobile & Light Mode.                
                {
#if false
                    //Line();
                    //EditorGUILayout.Space();
                    _AdditionalLookdevs_Foldout = FoldoutSubMenu(_AdditionalLookdevs_Foldout, "● Additional Settings");
                    if (_AdditionalLookdevs_Foldout)
                    {
                        GUI_AdditionalLookdevs(material);
                    }
#else
                    _AdditionalLookdevs_Foldout = FoldoutSubMenu(_AdditionalLookdevs_Foldout, "Point Light Settings");
                    if (_AdditionalLookdevs_Foldout)
                    {
                        GUI_AdditionalLookdevs(material);
                    }
#endif
                }
            }
        }

        void GUI_SystemShadows(Material material)
        {

            GUILayout.Label("System Shadows:", EditorStyles.boldLabel);
#if USE_TOGGLE_BUTTONS
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Receive System Shadows");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropSetSystemShadowsToBase) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropSetSystemShadowsToBase, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropSetSystemShadowsToBase, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
#else
            bool isEnabled = GUI_Toggle(material, "Receive System Shadows", ShaderPropSetSystemShadowsToBase, MaterialGetInt(material,ShaderPropSetSystemShadowsToBase) != 0);
#endif
            //           if (material.GetFloat(ShaderPropSetSystemShadowsToBase) == 1)
            EditorGUI.BeginDisabledGroup(!isEnabled);
            {
                EditorGUI.indentLevel++;
                m_MaterialEditor.RangeProperty(tweak_SystemShadowsLevel, "System Shadow Level");
                GUI_SetRTHS(material);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
        }

        void GUI_BasicLookdevs(Material material)
        {
            if (material.HasProperty(ShaderPropUtsTechniqe))//DoubleWithFeather or ShadingGradeMap
            {
                if (material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.DoubleShadeWithFeather)   //DWF
                {
                    GUILayout.Label("Mode: Double Shade With Feather", EditorStyles.boldLabel);
                    m_MaterialEditor.RangeProperty(baseColor_Step, "Base Color Step");
                    m_MaterialEditor.RangeProperty(baseShade_Feather, "Base/Shade Feather");
                    m_MaterialEditor.RangeProperty(shadeColor_Step, "Shade Color Step");
                    m_MaterialEditor.RangeProperty(first2nd_Shades_Feather, "1st/2nd Shade Feather");
                    //Sharing variables with ShadingGradeMap method.

                    material.SetFloat(ShaderProp1st_ShadeColor_Step, material.GetFloat(ShaderPropBaseColor_Step));
                    material.SetFloat(ShaderProp1st_ShadeColor_Feather, material.GetFloat(ShaderPropBaseShade_Feather));
                    material.SetFloat(ShaderProp2nd_ShadeColor_Step, material.GetFloat(ShaderPropShadeColor_Step));
                    material.SetFloat(ShaderProp2nd_ShadeColor_Feather, material.GetFloat(ShaderProp1st2nd_Shades_Feather));
                }
                else if (material.GetInt(ShaderPropUtsTechniqe) == (int)_UTS_Technique.ShadingGradeMap)
                {    //SGM
                    GUILayout.Label("Mode: Shading Grade Map", EditorStyles.boldLabel);
                    m_MaterialEditor.RangeProperty(first_ShadeColor_Step, "1st Shade Color Step");
                    m_MaterialEditor.RangeProperty(first_ShadeColor_Feather, "1st Shade Color Feather");
                    m_MaterialEditor.RangeProperty(second_ShadeColor_Step, "2nd Shade Color Step");
                    m_MaterialEditor.RangeProperty(second_ShadeColor_Feather, "2nd Shade Color Feather");
                    //Share variables with DoubleWithFeather method.
                    material.SetFloat(ShaderPropBaseColor_Step, material.GetFloat(ShaderProp1st_ShadeColor_Step));
                    material.SetFloat(ShaderPropBaseShade_Feather, material.GetFloat(ShaderProp1st_ShadeColor_Feather));
                    material.SetFloat(ShaderPropShadeColor_Step, material.GetFloat(ShaderProp2nd_ShadeColor_Step));
                    material.SetFloat(ShaderProp1st2nd_Shades_Feather, material.GetFloat(ShaderProp2nd_ShadeColor_Feather));
                }
                else
                {
                    // OutlineObj.
                    return;
                }
            }
            EditorGUILayout.Space();
        }

        void GUI_AdditionalLookdevs(Material material)
        {
//            GUILayout.Label("    Point Light Settings");
            EditorGUI.indentLevel++;
            m_MaterialEditor.RangeProperty(stepOffset, "Step Offset");
#if USE_TOGGLE_BUTTONS

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("PointLights Hi-Cut Filter");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIsFilterHiCutPointLightColor) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
#else
            GUI_Toggle(material, "Point Light High Light Filter", ShaderPropIsFilterHiCutPointLightColor, MaterialGetInt(material, ShaderPropIsFilterHiCutPointLightColor) != 0);
#endif
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }


        void GUI_HighColor(Material material)
        {
            m_MaterialEditor.TexturePropertySingleLine(Styles.highColorText, highColor_Tex, highColor);
            m_MaterialEditor.RangeProperty(highColor_Power, "High Light Power");

            if (!_SimpleUI)
            {
#if USE_TOGGLE_BUTTONS
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Specular Mode");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_SpecularToHighColor) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_SpecularToHighColor, 1);
                        material.SetFloat(ShaderPropIs_BlendAddToHiColor, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_SpecularToHighColor, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Color Blend Mode");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_BlendAddToHiColor) == 0)
                {
                    if (GUILayout.Button("Multiply", shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_BlendAddToHiColor, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button("Additive", shortButtonStyle))
                    {
                        //The additive mode is only available in specular off.
                        if (material.GetFloat(ShaderPropIs_SpecularToHighColor) == 1)
                        {
                            material.SetFloat(ShaderPropIs_BlendAddToHiColor, 1);
                        }
                        else
                        {
                            material.SetFloat(ShaderPropIs_BlendAddToHiColor, 0);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
#else
                EditorGUI.showMixedValue = specularMode.hasMixedValue;

                int mode = (int)specularMode.floatValue;
                EditorGUI.BeginChangeCheck();
                mode = EditorGUILayout.Popup(specularModeText, mode, System.Enum.GetNames(typeof(_UTS_SpeculerMode)));
                if (EditorGUI.EndChangeCheck())
                {
                    m_MaterialEditor.RegisterPropertyChangeUndo(specularModeText.text);
                    switch ((_UTS_SpeculerMode)mode)
                    {
                    case _UTS_SpeculerMode.Solid:
                        break;
                    case _UTS_SpeculerMode.Natural:
                        specularBlendMode.floatValue = 1.0f;
                        break;
                    }


                    specularMode.floatValue = mode;
                }
                EditorGUI.showMixedValue = false;


                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;

                //GUILayout.Space(60);
                EditorGUI.BeginDisabledGroup(mode != 0);
                EditorGUI.showMixedValue = specularBlendMode.hasMixedValue;
                int blendingMode = (int)specularBlendMode.floatValue;
                EditorGUI.BeginChangeCheck();
                blendingMode = EditorGUILayout.Popup(specularBlendModeText, blendingMode, System.Enum.GetNames(typeof(_UTS_SpeculerColorBlendMode)));
                if (EditorGUI.EndChangeCheck())
                {
                    m_MaterialEditor.RegisterPropertyChangeUndo(specularModeText.text);
                    specularBlendMode.floatValue = blendingMode;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndHorizontal();
                EditorGUI.showMixedValue = false;
                EditorGUI.EndDisabledGroup();
#endif

#if USE_TOGGLE_BUTTONS
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PrefixLabel("ShadowMask on HihgColor");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_UseTweakHighColorOnShadow) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_UseTweakHighColorOnShadow, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_UseTweakHighColorOnShadow, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
                if (material.GetFloat(ShaderPropIs_UseTweakHighColorOnShadow) == 1)
                {
                    EditorGUI.indentLevel++;
                    m_MaterialEditor.RangeProperty(tweakHighColorOnShadow, "HighColor Power on Shadow");
                    EditorGUI.indentLevel--;
                }
#else
                var ret = GUI_Toggle(material, "Shadow Mask on High Lights", ShaderPropIs_UseTweakHighColorOnShadow, MaterialGetInt(material, ShaderPropIs_UseTweakHighColorOnShadow) != 0);
                EditorGUI.BeginDisabledGroup(!ret);
                {
                    EditorGUI.indentLevel++;
                    m_MaterialEditor.RangeProperty(tweakHighColorOnShadow, "High Light Power on Shadows");
                    EditorGUI.indentLevel--;
                }
                EditorGUI.EndDisabledGroup();
#endif



            }

            EditorGUILayout.Space();
            //Line();
            //EditorGUILayout.Space();

            GUILayout.Label("    High Light Mask", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            m_MaterialEditor.TexturePropertySingleLine(Styles.highColorMaskText, set_HighColorMask);
            m_MaterialEditor.RangeProperty(tweak_HighColorMaskLevel, "High Lgiht Mask Level");
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
        }

        void GUI_RimLight(Material material)
        {

#if USE_TOGGLE_BUTTONS
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Rim Light");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropRimLight) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropRimLight, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropRimLight, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (material.GetFloat(ShaderPropRimLight) == 1)
            {
                EditorGUI.indentLevel++;
                GUILayout.Label("    RimLight Settings", EditorStyles.boldLabel);
                m_MaterialEditor.ColorProperty(rimLightColor, "RimLight Color");
                m_MaterialEditor.RangeProperty(rimLight_Power, "RimLight Power");

                if (!_SimpleUI)
                {
                    m_MaterialEditor.RangeProperty(rimLight_InsideMask, "RimLight Inside Mask");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("RimLight FeatherOff");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropRimLight_FeatherOff) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropRimLight_FeatherOff, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropRimLight_FeatherOff, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("LightDirection Mask");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropLightDirection_MaskOn) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropLightDirection_MaskOn, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropLightDirection_MaskOn, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    if (material.GetFloat(ShaderPropLightDirection_MaskOn) == 1)
                    {
                        EditorGUI.indentLevel++;
                        m_MaterialEditor.RangeProperty(tweak_LightDirection_MaskLevel, "LightDirection MaskLevel");

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Antipodean(Ap)_RimLight");
                        //GUILayout.Space(60);
                        if (material.GetFloat(ShaderPropAdd_Antipodean_RimLight) == 0)
                        {
                            if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                            {
                                material.SetFloat(ShaderPropAdd_Antipodean_RimLight, 1);
                            }
                        }
                        else
                        {
                            if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                            {
                                material.SetFloat(ShaderPropAdd_Antipodean_RimLight, 0);
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        if (material.GetFloat(ShaderPropAdd_Antipodean_RimLight) == 1)
                        {
                            EditorGUI.indentLevel++;
                            GUILayout.Label("    Ap_RimLight Settings", EditorStyles.boldLabel);
                            m_MaterialEditor.ColorProperty(ap_RimLightColor, "Ap_RimLight Color");
                            m_MaterialEditor.RangeProperty(ap_RimLight_Power, "Ap_RimLight Power");

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("Ap_RimLight FeatherOff");
                            //GUILayout.Space(60);
                            if (material.GetFloat(ShaderPropAp_RimLight_FeatherOff) == 0)
                            {
                                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                                {
                                    material.SetFloat(ShaderPropAp_RimLight_FeatherOff, 1);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                                {
                                    material.SetFloat(ShaderPropAp_RimLight_FeatherOff, 0);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.indentLevel--;
                        }

                        EditorGUI.indentLevel--;

                    }//Light Direction Mask ON

                }

                //EditorGUI.indentLevel++;

                EditorGUILayout.Space();
                //Line();
                //EditorGUILayout.Space();

                GUILayout.Label("    RimLight Mask", EditorStyles.boldLabel);
                m_MaterialEditor.TexturePropertySingleLine(Styles.rimLightMaskText, set_RimLightMask);
                m_MaterialEditor.RangeProperty(tweak_RimLightMaskLevel, "RimLight Mask Level");

                //EditorGUI.indentLevel--;

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
#else
            EditorGUILayout.BeginHorizontal();
            var rimLightEnabled = GUI_Toggle(material, "Rim Light", ShaderPropRimLight, MaterialGetInt(material, ShaderPropRimLight) != 0);
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(!rimLightEnabled);
            EditorGUI.indentLevel++;

            m_MaterialEditor.ColorProperty(rimLightColor, "Rim Light Color");
            m_MaterialEditor.RangeProperty(rimLight_Power, "Rim Light Power");

            if (!_SimpleUI)
            {
                m_MaterialEditor.RangeProperty(rimLight_InsideMask, "Rim Light Inside Mask");

                EditorGUILayout.BeginHorizontal();
                GUI_Toggle(material, "Rim Light Feather Off", ShaderPropRimLight_FeatherOff, MaterialGetInt(material, ShaderPropRimLight_FeatherOff) != 0);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();

                //GUILayout.Space(60);
                var direcitonMaskEnabled = GUI_Toggle(material, "Light Direction Mask", ShaderPropLightDirection_MaskOn, MaterialGetInt(material, ShaderPropLightDirection_MaskOn) != 0);
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginDisabledGroup(!direcitonMaskEnabled);
                {
                    EditorGUI.indentLevel++;
                    m_MaterialEditor.RangeProperty(tweak_LightDirection_MaskLevel, "Light Direction Mask Level");

                    EditorGUILayout.BeginHorizontal();
                    var antipodean_RimLight = GUI_Toggle(material, "Antipodean(Ap)_RimLight", ShaderPropAdd_Antipodean_RimLight, MaterialGetInt(material, ShaderPropAdd_Antipodean_RimLight )!= 0);
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginDisabledGroup(!antipodean_RimLight);
                    {
                        EditorGUI.indentLevel++;

                        m_MaterialEditor.ColorProperty(ap_RimLightColor, "Ap_RimLight Color");
                        m_MaterialEditor.RangeProperty(ap_RimLight_Power, "Ap_RimLight Power");

                        EditorGUILayout.BeginHorizontal();
                        GUI_Toggle(material, "Ap_RimLight FeatherOff", ShaderPropAp_RimLight_FeatherOff, MaterialGetInt(material, ShaderPropAp_RimLight_FeatherOff) != 0);


                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;

                }//Light Direction Mask ON
                EditorGUI.EndDisabledGroup();
            }

            //EditorGUI.indentLevel++;

            EditorGUILayout.Space();
            //Line();
            //EditorGUILayout.Space();

            GUILayout.Label("    RimLight Mask", EditorStyles.boldLabel);
            m_MaterialEditor.TexturePropertySingleLine(Styles.rimLightMaskText, set_RimLightMask);
            m_MaterialEditor.RangeProperty(tweak_RimLightMaskLevel, "Rim Light Mask Level");

            //EditorGUI.indentLevel--;

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            EditorGUI.EndDisabledGroup();
#endif



        }

        void GUI_MatCap(Material material)
        {
#if USE_TOGGLE_BUTTONS

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("MatCap");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropMatCap) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropMatCap, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropMatCap, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (material.GetFloat(ShaderPropMatCap) == 1)
            {
                GUILayout.Label("    MatCap Settings", EditorStyles.boldLabel);
                m_MaterialEditor.TexturePropertySingleLine(Styles.matCapSamplerText, matCap_Sampler, matCapColor);
                EditorGUI.indentLevel++;
                m_MaterialEditor.TextureScaleOffsetProperty(matCap_Sampler);

                if (!_SimpleUI)
                {

                    m_MaterialEditor.RangeProperty(blurLevelMatcap, "Blur Level of MatCap Sampler");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Color Blend Mode");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropIs_BlendAddToMatCap) == 0)
                    {
                        if (GUILayout.Button("Multipy", shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_BlendAddToMatCap, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Additive", shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_BlendAddToMatCap, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    m_MaterialEditor.RangeProperty(tweak_MatCapUV, "Scale MatCapUV");
                    m_MaterialEditor.RangeProperty(rotate_MatCapUV, "Rotate MatCapUV");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("CameraRolling Stabilizer");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropCameraRolling_Stabilizer) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropCameraRolling_Stabilizer, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropCameraRolling_Stabilizer, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("NormalMap for MatCap");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropIs_NormalMapForMatCap) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_NormalMapForMatCap, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_NormalMapForMatCap, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    if (material.GetFloat(ShaderPropIs_NormalMapForMatCap) == 1)
                    {
                        EditorGUI.indentLevel++;
                        GUILayout.Label("       NormalMap for MatCap as SpecularMask", EditorStyles.boldLabel);
                        m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMapForMatCap, bumpScaleMatcap);
                        m_MaterialEditor.TextureScaleOffsetProperty(normalMapForMatCap);
                        m_MaterialEditor.RangeProperty(rotate_NormalMapForMatCapUV, "Rotate NormalMapUV");
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("MatCap on Shadow");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropIs_UseTweakMatCapOnShadow) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_UseTweakMatCapOnShadow, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_UseTweakMatCapOnShadow, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    if (material.GetFloat(ShaderPropIs_UseTweakMatCapOnShadow) == 1)
                    {
                        EditorGUI.indentLevel++;
                        m_MaterialEditor.RangeProperty(tweakMatCapOnShadow, "MatCap Power on Shadow");
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("MatCap Projection Camera");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropIs_Ortho) == 0)
                    {
                        if (GUILayout.Button("Perspective", middleButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_Ortho, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Orthographic", middleButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_Ortho, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space();
                //Line();
                //EditorGUILayout.Space();

                GUILayout.Label("    MatCap Mask", EditorStyles.boldLabel);
                m_MaterialEditor.TexturePropertySingleLine(Styles.matCapMaskText, set_MatcapMask);
                m_MaterialEditor.TextureScaleOffsetProperty(set_MatcapMask);
                m_MaterialEditor.RangeProperty(tweak_MatcapMaskLevel, "MatCap Mask Level");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Inverse Matcap Mask");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropInverse_MatcapMask) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropInverse_MatcapMask, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropInverse_MatcapMask, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            } // MatCap == 1
#else
            EditorGUILayout.BeginHorizontal();
            var matcapEnabled = GUI_Toggle(material, "MatCap", ShaderPropMatCap,MaterialGetInt(material, ShaderPropMatCap) != 0);
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(!matcapEnabled);

            m_MaterialEditor.TexturePropertySingleLine(Styles.matCapSamplerText, matCap_Sampler, matCapColor);
            EditorGUI.indentLevel++;
            m_MaterialEditor.TextureScaleOffsetProperty(matCap_Sampler);

            if (!_SimpleUI)
            {

                m_MaterialEditor.RangeProperty(blurLevelMatcap, "Blur Level of MatCap Sampler");

                EditorGUILayout.BeginHorizontal();
                DoPopup(matcapBlendModeText, matcapBlendMode, System.Enum.GetNames(typeof(_UTS_MatcapColorBlendMode)));
                EditorGUILayout.EndHorizontal();

                m_MaterialEditor.RangeProperty(tweak_MatCapUV, "Scale MatCap UV");
                m_MaterialEditor.RangeProperty(rotate_MatCapUV, "Rotate MatCap UV");

                EditorGUILayout.BeginHorizontal();
                GUI_Toggle(material, "Camera Rolling Stabilizer", ShaderPropCameraRolling_Stabilizer, MaterialGetInt(material, ShaderPropCameraRolling_Stabilizer) != 0);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                var isNormalMapForMatCap = GUI_Toggle(material, "NormalMap Specular Mask for MatCap", ShaderPropIs_NormalMapForMatCap, MaterialGetInt(material, ShaderPropIs_NormalMapForMatCap) != 0);

                //GUILayout.Space(60);

                EditorGUILayout.EndHorizontal();
                EditorGUI.BeginDisabledGroup(!isNormalMapForMatCap);
                {
                    EditorGUI.indentLevel++;
                    m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, normalMapForMatCap, bumpScaleMatcap);
                    m_MaterialEditor.TextureScaleOffsetProperty(normalMapForMatCap);
                    m_MaterialEditor.RangeProperty(rotate_NormalMapForMatCapUV, "Rotate NormalMap UV");
                    EditorGUI.indentLevel--;
                }
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.BeginHorizontal();
                var tweakMatCapOnShadows = GUI_Toggle(material, "MatCap on Shadows", ShaderPropIs_UseTweakMatCapOnShadow, MaterialGetInt(material, ShaderPropIs_UseTweakMatCapOnShadow) != 0);
 
                EditorGUILayout.EndHorizontal();
                EditorGUI.BeginDisabledGroup(!tweakMatCapOnShadows);
                {
                    EditorGUI.indentLevel++;
                    m_MaterialEditor.RangeProperty(tweakMatCapOnShadow, "MatCap Power on Shadows");
                    EditorGUI.indentLevel--;
                }
                EditorGUI.EndDisabledGroup();
                DoPopup(matcapOrthoText, matcapCameraMode, System.Enum.GetNames(typeof(_CameraProjectionType)));
            }

            EditorGUILayout.Space();
            //Line();
            //EditorGUILayout.Space();

            GUILayout.Label("    MatCap Mask", EditorStyles.boldLabel);
            m_MaterialEditor.TexturePropertySingleLine(Styles.matCapMaskText, set_MatcapMask);
            m_MaterialEditor.TextureScaleOffsetProperty(set_MatcapMask);
            m_MaterialEditor.RangeProperty(tweak_MatcapMaskLevel, "MatCap Mask Level");

            GUI_Toggle(material, "Inverse MatCap Mask", ShaderPropInverse_MatcapMask, MaterialGetInt(material, ShaderPropInverse_MatcapMask) != 0);


            EditorGUI.indentLevel--;

            EditorGUI.EndDisabledGroup();
#endif

            //EditorGUILayout.Space();
        }

        void GUI_AngelRing(Material material)
        {
#if USE_TOGGLE_BUTTONS
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("AngelRing Projection");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropAngelRing) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropAngelRing, 1);



                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropAngelRing, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
                        if (material.GetFloat(ShaderPropAngelRing) == 1)
            {
                GUILayout.Label("    AngelRing Sampler Settings", EditorStyles.boldLabel);
                m_MaterialEditor.TexturePropertySingleLine(Styles.angelRingText, angelRing_Sampler, angelRing_Color);
                EditorGUI.indentLevel++;
                //m_MaterialEditor.TextureScaleOffsetProperty(angelRing_Sampler);
                m_MaterialEditor.RangeProperty(ar_OffsetU, "Offset U");
                m_MaterialEditor.RangeProperty(ar_OffsetV, "Offset V");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Use α channel as Clipping Mask");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropARSampler_AlphaOn) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropARSampler_AlphaOn, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropARSampler_AlphaOn, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

            }

#else
            var angelRingEnabled = GUI_Toggle(material, "Angel Ring Projection", ShaderPropAngelRing, MaterialGetInt(material, ShaderPropAngelRing) != 0);
            EditorGUI.BeginDisabledGroup(!angelRingEnabled);
            {
                m_MaterialEditor.TexturePropertySingleLine(Styles.angelRingText, angelRing_Sampler, angelRing_Color);
                EditorGUI.indentLevel++;
                //m_MaterialEditor.TextureScaleOffsetProperty(angelRing_Sampler);
                m_MaterialEditor.RangeProperty(ar_OffsetU, "Offset U");
                m_MaterialEditor.RangeProperty(ar_OffsetV, "Offset V");

                GUI_Toggle(material, "Alpha channel as Clipping Mask", ShaderPropARSampler_AlphaOn, MaterialGetInt(material, ShaderPropARSampler_AlphaOn) != 0);

                EditorGUI.indentLevel--;

            }
            EditorGUI.EndDisabledGroup();
#endif

        }
        void ApplyQueueAndRenderType(_UTS_Technique technique, Material material)
        {

            if (_autoRenderQueue == 1)
            {
                 material.renderQueue = -1; //  (int)UnityEngine.Rendering.RenderQueue.Geometry;
            }

            const string OPAQUE = "Opaque";
            const string TRANSPARENTCUTOUT = "TransparentCutOut";
            const string TRANSPARENT = "Transparent";
            const string RENDERTYPE = "RenderType";
            const string IGNOREPROJECTION = "IgnoreProjection";
            const string DO_IGNOREPROJECTION = "True";
            const string DONT_IGNOREPROJECTION = "False";
            var renderType = OPAQUE;
            var ignoreProjection = DONT_IGNOREPROJECTION;

            if (_Transparent_Setting == _UTS_Transparent.On)
            {
                renderType = TRANSPARENT;
                ignoreProjection = DO_IGNOREPROJECTION;
            }
            else
            {
                switch (technique)
                {
                    case _UTS_Technique.DoubleShadeWithFeather:
                        {
                            _UTS_ClippingMode clippingMode = (_UTS_ClippingMode)material.GetInt(ShaderPropClippingMode);
                            if (clippingMode == _UTS_ClippingMode.Off)
                            {

                            }
                            else
                            {
                                renderType = TRANSPARENTCUTOUT;

                            }

                            break;
                        }
                    case _UTS_Technique.ShadingGradeMap:
                        {
                            _UTS_TransClippingMode transClippingMode = (_UTS_TransClippingMode)material.GetInt(ShaderPropClippingMode);
                            if (transClippingMode == _UTS_TransClippingMode.Off)
                            {
                            }
                            else
                            {
                                renderType = TRANSPARENTCUTOUT;

                            }

                            break;
                        }
                }

            }
            if (_autoRenderQueue == 1)
            {
                SetReqnderQueueAuto(material);
            }
            else
            {
                material.renderQueue = _renderQueue;
            }

            material.SetOverrideTag(RENDERTYPE, renderType);
            material.SetOverrideTag(IGNOREPROJECTION, ignoreProjection);
        }

        void SetReqnderQueueAuto(Material material)
        {
            var stencilMode = (_UTS_StencilMode)material.GetInt(ShaderPropStencilMode);
            if (_Transparent_Setting == _UTS_Transparent.On)
            {
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            else if (stencilMode == _UTS_StencilMode.StencilMask)
            {
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest - 1;
            }
            else if (stencilMode == _UTS_StencilMode.StencilOut)
            {
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
            }
            if (_Transparent_Setting == _UTS_Transparent.On)
            {
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            else if (stencilMode == _UTS_StencilMode.StencilMask)
            {
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest - 1;
            }
            else if (stencilMode == _UTS_StencilMode.StencilOut)
            {
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
            }
        }
        void ApplyMatCapMode(Material material)
        {
            if (material.GetInt(ShaderPropClippingMode) == 0)
            {
                if (material.GetFloat(ShaderPropMatCap) == 1)
                    material.EnableKeyword(ShaderPropMatCap);
                else
                    material.DisableKeyword(ShaderPropMatCap);
            }
            else
            {
                material.DisableKeyword(ShaderPropMatCap);
            }
        }


        void ApplyAngelRing(Material material)
        {
            int angelRingEnabled = material.GetInt(ShaderPropAngelRing);
            if (angelRingEnabled == 0)
            {
                material.DisableKeyword(ShaderDefineANGELRING_ON);
                material.EnableKeyword(ShaderDefineANGELRING_OFF);
            }
            else
            {
                material.EnableKeyword(ShaderDefineANGELRING_ON);
                material.DisableKeyword(ShaderDefineANGELRING_OFF);

            }
        }

        void ApplyStencilMode(Material material)
        {
            _UTS_StencilMode mode = (_UTS_StencilMode)(material.GetInt(ShaderPropStencilMode));
            switch (mode)
            {
                case _UTS_StencilMode.Off:
                    //    material.SetInt(ShaderPropStencilNo,0);
                    material.SetInt(ShaderPropStencilComp, (int)_StencilCompFunction.Disabled);
                    material.SetInt(ShaderPropStencilOpPass, (int)_StencilOperation.Keep);
                    material.SetInt(ShaderPropStencilOpFail, (int)_StencilOperation.Keep);
                    break;
                case _UTS_StencilMode.StencilMask:
                    //    material.SetInt(ShaderPropStencilNo,0);
                    material.SetInt(ShaderPropStencilComp, (int)_StencilCompFunction.Always);
                    material.SetInt(ShaderPropStencilOpPass, (int)_StencilOperation.Replace);
                    material.SetInt(ShaderPropStencilOpFail, (int)_StencilOperation.Replace);
                    break;
                case _UTS_StencilMode.StencilOut:
                    //    material.SetInt(ShaderPropStencilNo,0);
                    material.SetInt(ShaderPropStencilComp, (int)_StencilCompFunction.NotEqual);
                    material.SetInt(ShaderPropStencilOpPass, (int)_StencilOperation.Keep);
                    material.SetInt(ShaderPropStencilOpFail, (int)_StencilOperation.Keep);

                    break;
            }



        }
        void ApplyClippingMode(Material material)
        {

            if (!IsShadingGrademap)
            {


                material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
                material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);

                switch (material.GetInt(ShaderPropClippingMode))
                {
                    case 0:
                        material.EnableKeyword(ShaderDefineIS_CLIPPING_OFF);
                        material.DisableKeyword(ShaderDefineIS_CLIPPING_MODE);
                        material.DisableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                        material.EnableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_NO);
                        material.DisableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_YES);
                        break;
                    case 1:
                        material.DisableKeyword(ShaderDefineIS_CLIPPING_OFF);
                        material.EnableKeyword(ShaderDefineIS_CLIPPING_MODE);
                        material.DisableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                        material.DisableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_NO);
                        material.EnableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_YES);
                        break;
                    default:
                        material.DisableKeyword(ShaderDefineIS_CLIPPING_OFF);
                        material.DisableKeyword(ShaderDefineIS_CLIPPING_MODE);
                        material.EnableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                        material.DisableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_NO);
                        material.EnableKeyword(ShaderDefineIS_OUTLINE_CLIPPING_YES);
                        break;
                }
            }
            else
            {


                material.DisableKeyword(ShaderDefineIS_CLIPPING_OFF);
                material.DisableKeyword(ShaderDefineIS_CLIPPING_MODE);
                material.DisableKeyword(ShaderDefineIS_CLIPPING_TRANSMODE);
                switch (material.GetInt(ShaderPropClippingMode))
                {
                    case 0:
                        material.EnableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
                        material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);
                        break;
                    default:
                        material.DisableKeyword(ShaderDefineIS_TRANSCLIPPING_OFF);
                        material.EnableKeyword(ShaderDefineIS_TRANSCLIPPING_ON);
                        break;

                }

            }

        }
        void GUI_Emissive(Material material)
        {
#if USE_TOGGLE_BUTTONS
            GUILayout.Label("Emissive Tex × HDR Color", EditorStyles.boldLabel);
            GUILayout.Label("(Bloom Post-Processing Effect necessary)");
            EditorGUILayout.Space();
            m_MaterialEditor.TexturePropertySingleLine(Styles.emissiveTexText, emissive_Tex, emissive_Color);
            m_MaterialEditor.TextureScaleOffsetProperty(emissive_Tex);

            int _EmissiveMode_Setting = material.GetInt("_EMISSIVE");
            if ((int)_EmissiveMode.SimpleEmissive == _EmissiveMode_Setting)
            {
                emissiveMode = _EmissiveMode.SimpleEmissive;
            }
            else if ((int)_EmissiveMode.EmissiveAnimation == _EmissiveMode_Setting)
            {
                emissiveMode = _EmissiveMode.EmissiveAnimation;
            }
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Emissive Animation");
            //GUILayout.Space(60);
            if (emissiveMode == _EmissiveMode.SimpleEmissive)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat("_EMISSIVE", 1);
                    material.EnableKeyword("_EMISSIVE_ANIMATION");
                    material.DisableKeyword("_EMISSIVE_SIMPLE");

                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat("_EMISSIVE", 0);
                    material.EnableKeyword("_EMISSIVE_SIMPLE");
                    material.DisableKeyword("_EMISSIVE_ANIMATION");
                }
            }
            EditorGUILayout.EndHorizontal();

            if (emissiveMode == _EmissiveMode.EmissiveAnimation)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                m_MaterialEditor.FloatProperty(base_Speed, "Base Speed (Time)");
                //EditorGUILayout.PrefixLabel("Select Scroll Coord");
                //GUILayout.Space(60);
                if (!_SimpleUI)
                {
                    if (material.GetFloat(ShaderPropIs_ViewCoord_Scroll) == 0)
                    {
                        if (GUILayout.Button("UV Coord Scroll", shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_ViewCoord_Scroll, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("View Coord Scroll", shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_ViewCoord_Scroll, 0);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                m_MaterialEditor.RangeProperty(scroll_EmissiveU, "Scroll U/X direction");
                m_MaterialEditor.RangeProperty(scroll_EmissiveV, "Scroll V/Y direction");
                m_MaterialEditor.FloatProperty(rotate_EmissiveUV, "Rotate around UV center");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("PingPong Move for Base");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_PingPong_Base) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_PingPong_Base, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_PingPong_Base, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                if (!_SimpleUI)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("ColorShift with Time");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropIs_ColorShift) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_ColorShift, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_ColorShift, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    if (material.GetFloat(ShaderPropIs_ColorShift) == 1)
                    {
                        m_MaterialEditor.ColorProperty(colorShift, "Destination Color");
                        m_MaterialEditor.FloatProperty(colorShift_Speed, "ColorShift Speed (Time)");
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("ViewShift of Color");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropIs_ViewShift) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_ViewShift, 1);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_ViewShift, 0);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    if (material.GetFloat(ShaderPropIs_ViewShift) == 1)
                    {
                        m_MaterialEditor.ColorProperty(viewShift, "ViewShift Color");
                    }
                    EditorGUI.indentLevel--;
                }//!_SimpleUI
            }
            EditorGUILayout.Space();
#else

            m_MaterialEditor.TexturePropertySingleLine(Styles.emissiveTexText, emissive_Tex, emissive_Color);
            m_MaterialEditor.TextureScaleOffsetProperty(emissive_Tex);

            int _EmissiveMode_Setting = material.GetInt("_EMISSIVE");
            if ((int)_EmissiveMode.SimpleEmissive == _EmissiveMode_Setting)
            {
                emissiveMode = _EmissiveMode.SimpleEmissive;
            }
            else if ((int)_EmissiveMode.EmissiveAnimation == _EmissiveMode_Setting)
            {
                emissiveMode = _EmissiveMode.EmissiveAnimation;
            }
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            var label = "Emissive Animation";
            var ret = EditorGUILayout.Toggle("Emissive Animation", emissiveMode != _EmissiveMode.SimpleEmissive);
            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialEditor.RegisterPropertyChangeUndo(label);
                if (ret )
                {
                    material.SetFloat("_EMISSIVE", 1);
                    material.EnableKeyword("_EMISSIVE_ANIMATION");
                    material.DisableKeyword("_EMISSIVE_SIMPLE");
                }
                else
                {
                    material.SetFloat("_EMISSIVE", 0);
                    material.EnableKeyword("_EMISSIVE_SIMPLE");
                    material.DisableKeyword("_EMISSIVE_ANIMATION");
                }
            }



            EditorGUI.BeginDisabledGroup(emissiveMode != _EmissiveMode.EmissiveAnimation);
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                m_MaterialEditor.FloatProperty(base_Speed, "Base Speed (Time)");

                if (!_SimpleUI)
                {
                    int mode = MaterialGetInt(material, ShaderPropIs_ViewCoord_Scroll);
                    EditorGUI.BeginChangeCheck();
                    mode = EditorGUILayout.Popup("", (int)mode, EmissiveScrollMode);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_MaterialEditor.RegisterPropertyChangeUndo("Emissive Scroll Mode");
                        MaterialSetInt(material, ShaderPropIs_ViewCoord_Scroll, mode);
                    }
                }
                EditorGUILayout.EndHorizontal();

                m_MaterialEditor.RangeProperty(scroll_EmissiveU, "Scroll U/X direction");
                m_MaterialEditor.RangeProperty(scroll_EmissiveV, "Scroll V/Y direction");
                m_MaterialEditor.FloatProperty(rotate_EmissiveUV, "Rotate around UV center");

                GUI_Toggle(material, "Ping-pong moves for base", ShaderPropIs_PingPong_Base, MaterialGetInt(material, ShaderPropIs_PingPong_Base) != 0);


                if (!_SimpleUI)
                {
                    EditorGUILayout.Space();

                    //GUILayout.Space(60);
                    var isColorShiftEnabled = GUI_Toggle(material, "Color shift with time", ShaderPropIs_ColorShift, MaterialGetInt(material, ShaderPropIs_ColorShift) != 0 );


                    EditorGUI.indentLevel++;
                    EditorGUI.BeginDisabledGroup(!isColorShiftEnabled);
                    {
                        m_MaterialEditor.ColorProperty(colorShift, "Destination Color");
                        m_MaterialEditor.FloatProperty(colorShift_Speed, "ColorShift Speed (Time)");
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space();

                    var isViewShiftEnabled = GUI_Toggle(material, "ViewShift of Color", ShaderPropIs_ViewShift, MaterialGetInt(material, ShaderPropIs_ViewShift) != 0);


                    EditorGUI.indentLevel++;
                    EditorGUI.BeginDisabledGroup(!isViewShiftEnabled);
                    m_MaterialEditor.ColorProperty(viewShift, "ViewShift Color");
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;
                }//!_SimpleUI
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
#endif
        }


        const string srpDefaultColorMask = "_SPRDefaultUnlitColorMask";
        const string srpDefaultCullMode = "_SRPDefaultUnlitColMode";

        void SetupOverDrawTransparentObject(Material material)
        {
            var srpDefaultLightModeTag = material.GetTag("LightMode", false, srpDefaultLightModeName);
            if (srpDefaultLightModeTag == srpDefaultLightModeName)
            {
                material.SetShaderPassEnabled(srpDefaultLightModeName, true);
                material.SetInt(srpDefaultColorMask, 0);
                material.SetInt(srpDefaultCullMode, (int)_CullingMode.BackCulling);
            }
        }
        void SetuOutline(Material material)
        {
            var srpDefaultLightModeTag = material.GetTag("LightMode", false, srpDefaultLightModeName);
            if (srpDefaultLightModeTag == srpDefaultLightModeName)
            {
                material.SetInt(srpDefaultColorMask, 15);
                material.SetInt(srpDefaultCullMode, (int)_CullingMode.FrontCulling);
            }
        }
        void GUI_Outline(Material material)
        {
            const string kDisableOutlineKeyword = "_DISABLE_OUTLINE";
            bool isLegacy = (srpDefaultLightModeName == "Always");

            var srpDefaultLightModeTag = material.GetTag("LightMode", false, srpDefaultLightModeName);
            bool isOutlineEnabled = true;
#if USE_TOGGLE_BUTTONS
            if (srpDefaultLightModeTag == srpDefaultLightModeName)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Outline");
                if (isOutlineEnabled = material.GetShaderPassEnabled(srpDefaultLightModeName))
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        if ( isLegacy)
                        {
                            material.EnableKeyword(kDisableOutlineKeyword);
                        }

                        material.SetShaderPassEnabled(srpDefaultLightModeName, false);

                    }
                }
                else
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        if (isLegacy)
                        {
                            material.DisableKeyword(kDisableOutlineKeyword);
                        }

                        material.SetShaderPassEnabled(srpDefaultLightModeName, true);

                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            if (!isOutlineEnabled)
            {
                return;
            }
            //
            //Express Shader property [KeywordEnum(NML,POS)] by EumPopup.
            //Load the outline mode settings in the material.
            int _OutlineMode_Setting = material.GetInt(ShaderPropOutline);
            //Convert it to Enum format and store it in the offlineMode variable.

            if ((int)_OutlineMode.NormalDirection == _OutlineMode_Setting)
            {
                outlineMode = _OutlineMode.NormalDirection;
            }
            else if ((int)_OutlineMode.PositionScaling == _OutlineMode_Setting)
            {
                outlineMode = _OutlineMode.PositionScaling;
            }
            //GUI description with EnumPopup.
            outlineMode = (_OutlineMode)EditorGUILayout.EnumPopup("Outline Mode", outlineMode);
            //If the value changes, write to the material.
            if (outlineMode == _OutlineMode.NormalDirection)
            {
                material.SetFloat(ShaderPropOutline, 0);
                //The keywords on the UTCS_Outline.cginc side are also toggled around.
                material.EnableKeyword("_OUTLINE_NML");
                material.DisableKeyword("_OUTLINE_POS");
            }
            else if (outlineMode == _OutlineMode.PositionScaling)
            {
                material.SetFloat(ShaderPropOutline, 1);
                material.EnableKeyword("_OUTLINE_POS");
                material.DisableKeyword("_OUTLINE_NML");
            }

            m_MaterialEditor.FloatProperty(outline_Width, "Outline Width");
            m_MaterialEditor.ColorProperty(outline_Color, "Outline Color");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Blend BaseColor to Outline");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_BlendBaseColor) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_BlendBaseColor, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_BlendBaseColor, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            m_MaterialEditor.TexturePropertySingleLine(Styles.outlineSamplerText, outline_Sampler);
            m_MaterialEditor.FloatProperty(offset_Z, "Offset Outline with Camera Z-axis");

            if (!_SimpleUI)
            {

                _AdvancedOutline_Foldout = FoldoutSubMenu(_AdvancedOutline_Foldout, "● Advanced Outline Settings");
                if (_AdvancedOutline_Foldout)
                {
                    EditorGUI.indentLevel++;
                    GUILayout.Label("    Camera Distance for Outline Width");
                    m_MaterialEditor.FloatProperty(farthest_Distance, "● Farthest Distance to vanish");
                    m_MaterialEditor.FloatProperty(nearest_Distance, "● Nearest Distance to draw with Outline Width");
                    EditorGUI.indentLevel--;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Use Outline Texture");
                    //GUILayout.Space(60);
                    if (material.GetFloat(ShaderPropIs_OutlineTex) == 0)
                    {
                        if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_OutlineTex, 1);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                        {
                            material.SetFloat(ShaderPropIs_OutlineTex, 0);
                        }
                        EditorGUILayout.EndHorizontal();
                        m_MaterialEditor.TexturePropertySingleLine(Styles.outlineTexText, outlineTex);
                    }

                    if (outlineMode == _OutlineMode.NormalDirection)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("Use Baked Normal for Outline");
                        //GUILayout.Space(60);
                        if (material.GetFloat(ShaderPropIs_BakedNormal) == 0)
                        {
                            if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                            {
                                material.SetFloat(ShaderPropIs_BakedNormal, 1);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                        else
                        {
                            if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                            {
                                material.SetFloat(ShaderPropIs_BakedNormal, 0);
                            }
                            EditorGUILayout.EndHorizontal();
                            m_MaterialEditor.TexturePropertySingleLine(Styles.bakedNormalOutlineText, bakedNormal);
                        }
                    }
                }
            }
#else
            if (srpDefaultLightModeTag == srpDefaultLightModeName)
            {
                const string kOutline = "Outline";
                isOutlineEnabled = material.GetShaderPassEnabled(srpDefaultLightModeName);
                EditorGUI.BeginChangeCheck();
                isOutlineEnabled = EditorGUILayout.Toggle(kOutline, isOutlineEnabled);
                if (EditorGUI.EndChangeCheck())
                {
                    m_MaterialEditor.RegisterPropertyChangeUndo(kOutline);
                    if (isOutlineEnabled)
                    {
                        if (isLegacy)
                        {
                            material.DisableKeyword(kDisableOutlineKeyword);
                        }

                        material.SetShaderPassEnabled(srpDefaultLightModeName, true);
                    }
                    else
                    {
                        if (isLegacy)
                        {
                            material.EnableKeyword(kDisableOutlineKeyword);
                        }
                        material.SetShaderPassEnabled(srpDefaultLightModeName, false);
                    }
                }
            }
            EditorGUI.BeginDisabledGroup(!isOutlineEnabled);
            //
            //Express Shader property [KeywordEnum(NML,POS)] by EumPopup.
            //Load the outline mode settings in the material.
            int _OutlineMode_Setting = material.GetInt(ShaderPropOutline);
            //Convert it to Enum format and store it in the offlineMode variable.

            if ((int)_OutlineMode.NormalDirection == _OutlineMode_Setting)
            {
                outlineMode = _OutlineMode.NormalDirection;
            }
            else if ((int)_OutlineMode.PositionScaling == _OutlineMode_Setting)
            {
                outlineMode = _OutlineMode.PositionScaling;
            }
            //GUI description with EnumPopup.
            outlineMode = (_OutlineMode)EditorGUILayout.EnumPopup("Outline Mode", outlineMode);
            //If the value changes, write to the material.
            if (outlineMode == _OutlineMode.NormalDirection)
            {
                material.SetFloat(ShaderPropOutline, 0);
                //The keywords on the UTCS_Outline.cginc side are also toggled around.
                material.EnableKeyword("_OUTLINE_NML");
                material.DisableKeyword("_OUTLINE_POS");
            }
            else if (outlineMode == _OutlineMode.PositionScaling)
            {
                material.SetFloat(ShaderPropOutline, 1);
                material.EnableKeyword("_OUTLINE_POS");
                material.DisableKeyword("_OUTLINE_NML");
            }

            m_MaterialEditor.FloatProperty(outline_Width, "Outline Width");
            m_MaterialEditor.ColorProperty(outline_Color, "Outline Color");

            GUI_Toggle(material,"Blend BaseColor to Outline", ShaderPropIs_BlendBaseColor, MaterialGetInt(material, ShaderPropIs_BlendBaseColor) != 0);

            m_MaterialEditor.TexturePropertySingleLine(Styles.outlineSamplerText, outline_Sampler);
            m_MaterialEditor.FloatProperty(offset_Z, "Offset Outline with Camera Z-axis");

            if (!_SimpleUI)
            {

                _AdvancedOutline_Foldout = FoldoutSubMenu(_AdvancedOutline_Foldout, "Advanced Outline Settings");
                if (_AdvancedOutline_Foldout)
                {
                    EditorGUI.indentLevel++;
                    GUILayout.Label("    Camera Distance for Outline Width");
                    m_MaterialEditor.FloatProperty(farthest_Distance, "Farthest Distance to vanish");
                    m_MaterialEditor.FloatProperty(nearest_Distance, "Nearest Distance to draw with Outline Width");
                    EditorGUI.indentLevel--;

                    var useOutlineTexture =  GUI_Toggle(material, "Use Outline Texture", ShaderPropIs_OutlineTex, MaterialGetInt(material, ShaderPropIs_OutlineTex)!=0); ;
                    EditorGUI.BeginDisabledGroup(!useOutlineTexture);
                    m_MaterialEditor.TexturePropertySingleLine(Styles.outlineTexText, outlineTex);
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.BeginDisabledGroup(outlineMode != _OutlineMode.NormalDirection);
                    {
                        var isBackedNormal = GUI_Toggle(material, "Use Baked Normal for Outline", ShaderPropIs_BakedNormal, MaterialGetInt(material, ShaderPropIs_BakedNormal) != 0);
                        EditorGUI.BeginDisabledGroup(!isBackedNormal);
                        m_MaterialEditor.TexturePropertySingleLine(Styles.bakedNormalOutlineText, bakedNormal);
                        EditorGUI.EndDisabledGroup();
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUI.EndDisabledGroup(); //!isOutlineEnabled
            }
#endif
            EditorGUILayout.Space();
        }

        void GUI_Tessellation(Material material)
        {
//            GUILayout.Label("Technique : DX11 Phong Tessellation", EditorStyles.boldLabel);
            m_MaterialEditor.RangeProperty(tessEdgeLength, "Edge Length");
            m_MaterialEditor.RangeProperty(tessPhongStrength, "Phong Strength");
            m_MaterialEditor.RangeProperty(tessExtrusionAmount, "Extrusion Amount");

            EditorGUILayout.Space();
        }

        void UnrecommendableGUIColor()
        {
            GUI.color = Color.red;
        }

        void RestoreGUIColor()
        {
            GUI.color = Color.white;
        }

        internal void GUI_GameRecommendation(Material material, EditorWindow window)
        {
#if USE_GAME_RECOMMENDATION

            var labelWidthStore = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 200;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Recommendable Setting", longButtonStyle))
            {
                SetGameRecommendation(material);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Base Color");
            //GUILayout.Space(60);
            RestoreGUIColor();
            if (material.GetFloat(ShaderPropIsLightColor_Base) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsLightColor_Base, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsLightColor_Base, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("1st ShadeColor");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_1st_Shade) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("2nd ShadeColor");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_2nd_Shade) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("HighColor");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_HighColor) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_HighColor, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_HighColor, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("RimLight");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_RimLight) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_RimLight, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_RimLight, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Ap_RimLight");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_Ap_RimLight) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("MatCap");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_MatCap) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_MatCap, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_MatCap, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            if (IsShadingGrademap)//If AngelRing is available.
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Angel Ring");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_LightColor_AR) == 0)
                {
                    UnrecommendableGUIColor();
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_AR, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_AR, 0);
                    }
                }
                RestoreGUIColor();
                EditorGUILayout.EndHorizontal();
            }

            if (material.HasProperty(ShaderPropOutline))//If OUTLINE is available.
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Outline");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_LightColor_Outline) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_Outline, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Receive System Shadows");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropSetSystemShadowsToBase) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropSetSystemShadowsToBase, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropSetSystemShadowsToBase, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("PointLights Hi-Cut Filter");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIsFilterHiCutPointLightColor) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsFilterHiCutPointLightColor, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("MatCap Projection Camera");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_Ortho) == 0)
            {

                if (GUILayout.Button("Perspective", middleButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_Ortho, 1);
                }
            }
            else
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button("Orthographic", middleButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_Ortho, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();


            if (material.GetFloat(ShaderPropGI_Intensity) != 0.0f)
            {
                UnrecommendableGUIColor();
            }
            m_MaterialEditor.RangeProperty(gi_Intensity, "GI Intensity");
            RestoreGUIColor();
            if (material.GetFloat(ShaderPropUnlit_Intensity) != 1.0f)
            {
                UnrecommendableGUIColor();
            }
            m_MaterialEditor.RangeProperty(unlit_Intensity, "Unlit Intensity");
            RestoreGUIColor();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Scene Light Hi-Cut Filter");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_Filter_LightColor) == 0)
            {
                UnrecommendableGUIColor();
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_Filter_LightColor, 1);
                    material.SetFloat(ShaderPropIsLightColor_Base, 1);
                    material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
                    material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
                    if (material.HasProperty(ShaderPropOutline))//If OUTLINE is available.
                    {
                        material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
                    }
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_Filter_LightColor, 0);
                }
            }
            RestoreGUIColor();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("OK", shortButtonStyle))
            {
                EditorGUIUtility.labelWidth = labelWidthStore;
                window.Close();
            }
            if (GUILayout.Button("Discard", shortButtonStyle))
            {
                RestoreGameRecommendation(material);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = labelWidthStore;
#endif //USE_GAME_RECOMMENDATION
        }
        void GUI_LightColorContribution(Material material)
        {
//            GUILayout.Label("Realtime Light Color Effectiveness to each color", EditorStyles.boldLabel);
#if USE_TOGGLE_BUTTONS
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Base Color");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIsLightColor_Base) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsLightColor_Base, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIsLightColor_Base, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("1st ShadeColor");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_1st_Shade) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("2nd ShadeColor");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_2nd_Shade) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("HighColor");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_HighColor) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_HighColor, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_HighColor, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("RimLight");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_RimLight) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_RimLight, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_RimLight, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Ap_RimLight");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_Ap_RimLight) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_Ap_RimLight, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("MatCap");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_LightColor_MatCap) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_MatCap, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_LightColor_MatCap, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (IsShadingGrademap)//If AngelRing is available.
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Angel Ring");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_LightColor_AR) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_AR, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_AR, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (material.HasProperty(ShaderPropOutline))//If OUTLINE is available.
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Outline");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropIs_LightColor_Outline) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropIs_LightColor_Outline, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
#else
            GUI_Toggle(material, "Base Color", ShaderPropIsLightColor_Base, MaterialGetInt(material, ShaderPropIsLightColor_Base)!= 0);
            GUI_Toggle(material, "1st Shading Color", ShaderPropIs_LightColor_1st_Shade, MaterialGetInt(material, ShaderPropIs_LightColor_1st_Shade) != 0);
            GUI_Toggle(material, "2nd Shading Color", ShaderPropIs_LightColor_2nd_Shade, MaterialGetInt(material, ShaderPropIs_LightColor_2nd_Shade) != 0);
            GUI_Toggle(material, "High Light", ShaderPropIs_LightColor_HighColor, MaterialGetInt(material, ShaderPropIs_LightColor_HighColor) != 0);
            GUI_Toggle(material, "Rim Light", ShaderPropIs_LightColor_RimLight, MaterialGetInt(material, ShaderPropIs_LightColor_RimLight) != 0);
            GUI_Toggle(material, "Ap_RimLight", ShaderPropIs_LightColor_Ap_RimLight, MaterialGetInt(material, ShaderPropIs_LightColor_Ap_RimLight) != 0);
            GUI_Toggle(material, "MatCap", ShaderPropIs_LightColor_MatCap, MaterialGetInt(material, ShaderPropIs_LightColor_MatCap) != 0);
            GUI_Toggle(material, "Outline", ShaderPropIs_LightColor_Outline, MaterialGetInt(material, ShaderPropIs_LightColor_Outline) != 0);
#endif
            EditorGUILayout.Space();
        }

        void GUI_AdditionalLightingSettings(Material material)
        {
            m_MaterialEditor.RangeProperty(gi_Intensity, "GI Intensity");
#if USE_TOGGLE_BUTTONS
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("SceneLights Hi-Cut Filter");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_Filter_LightColor) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_Filter_LightColor, 1);
                    material.SetFloat(ShaderPropIsLightColor_Base, 1);
                    material.SetFloat(ShaderPropIs_LightColor_1st_Shade, 1);
                    material.SetFloat(ShaderPropIs_LightColor_2nd_Shade, 1);
                    if (material.HasProperty(ShaderPropOutline))//If OUTLINE is available.
                    {
                        material.SetFloat(ShaderPropIs_LightColor_Outline, 1);
                    }
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_Filter_LightColor, 0);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Built-in Light Direction");
            //GUILayout.Space(60);
            if (material.GetFloat(ShaderPropIs_BLD) == 0)
            {
                if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_BLD, 1);
                }
            }
            else
            {
                if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                {
                    material.SetFloat(ShaderPropIs_BLD, 0);
                }
            }
            EditorGUILayout.EndHorizontal();
            if (material.GetFloat(ShaderPropIs_BLD) == 1)
            {
                GUILayout.Label("    Built-in Light Direction Settings");
                EditorGUI.indentLevel++;
                m_MaterialEditor.RangeProperty(offset_X_Axis_BLD, "● Offset X-Axis Direction");
                m_MaterialEditor.RangeProperty(offset_Y_Axis_BLD, "● Offset Y-Axis Direction");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("● Inverse Z-Axis Direction");
                //GUILayout.Space(60);
                if (material.GetFloat(ShaderPropInverse_Z_Axis_BLD) == 0)
                {
                    if (GUILayout.Button(STR_OFFSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropInverse_Z_Axis_BLD, 1);
                    }
                }
                else
                {
                    if (GUILayout.Button(STR_ONSTATE, shortButtonStyle))
                    {
                        material.SetFloat(ShaderPropInverse_Z_Axis_BLD, 0);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }

#else

            EditorGUI.BeginChangeCheck();
            var prop = ShaderPropIs_Filter_LightColor;
            var label = "Scene Light Hi-Cut Filter";
            var value = MaterialGetInt(material, prop);
            var ret = EditorGUILayout.Toggle(label, value != 0);
            if (EditorGUI.EndChangeCheck())
            {
                var boolValue = ret ? 1 : 0;
                m_MaterialEditor.RegisterPropertyChangeUndo(label);
                MaterialSetInt(material, prop, boolValue);
                if ( boolValue != 0 )
                {
                    MaterialSetInt(material, ShaderPropIs_Filter_LightColor, boolValue);
                    MaterialSetInt(material, ShaderPropIsLightColor_Base, boolValue);
                    MaterialSetInt(material, ShaderPropIs_LightColor_1st_Shade, boolValue);
                    MaterialSetInt(material, ShaderPropIs_LightColor_2nd_Shade, boolValue);
                    if (material.HasProperty(ShaderPropOutline))//If OUTLINE is available.
                    {
                        MaterialSetInt(material, ShaderPropIs_LightColor_Outline, boolValue);
                    }
                }
            }
            m_MaterialEditor.RangeProperty(unlit_Intensity, "Built-in Light Intensity");
            var isBold = GUI_Toggle(material, "Built-in Light Direction", ShaderPropIs_BLD, MaterialGetInt(material, ShaderPropIs_BLD) != 0);
            EditorGUI.BeginDisabledGroup(!isBold);

            EditorGUI.indentLevel++;
            m_MaterialEditor.RangeProperty(offset_X_Axis_BLD, "Offset X-Axis Direction");
            m_MaterialEditor.RangeProperty(offset_Y_Axis_BLD, "Offset Y-Axis Direction");

            GUI_Toggle(material, "Invert Z - Axis Direction", ShaderPropInverse_Z_Axis_BLD, MaterialGetInt(material,ShaderPropInverse_Z_Axis_BLD) != 0);

            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();
#endif
            EditorGUILayout.Space();
        }

        public void DoPopup(GUIContent label, MaterialProperty property, string[] options)
        {
            DoPopup(label, property, options, m_MaterialEditor);
        }

        public static void DoPopup(GUIContent label, MaterialProperty property, string[] options, MaterialEditor materialEditor)
        {
            if (property == null)
                throw new System.ArgumentNullException("property");

            EditorGUI.showMixedValue = property.hasMixedValue;

            var mode = property.floatValue;
            EditorGUI.BeginChangeCheck();
            mode = EditorGUILayout.Popup(label, (int)mode, options);
            if (EditorGUI.EndChangeCheck())
            {
                materialEditor.RegisterPropertyChangeUndo(label.text);
                property.floatValue = mode;
            }

            EditorGUI.showMixedValue = false;
        }


    } 


}
