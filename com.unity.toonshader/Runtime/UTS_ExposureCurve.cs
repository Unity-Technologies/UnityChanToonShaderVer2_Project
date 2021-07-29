using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityObject = UnityEngine.Object;
namespace Unity.Rendering.Toon
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class UTS_ExposureCurve : MonoBehaviour
    {
        // flags
        bool m_initialized = false;
        bool m_srpCallbackInitialized = false;

        const int k_ExposureCurvePrecision = 128;
#if false
        const string kLightAdjustmentPropName = "_UTS_LightAdjustment";
#endif
        const string kExposureAdjustmentPorpName = "_UTS_ExposureAdjustment";
        const string kExposureArrayPropName = "_UTS_ExposureArray";
        const string kExposureMinPropName   = "_UTS_ExposureMin";
        const string kExposureMaxPropName   = "_UTS_ExposureMax";
        readonly Color[] m_ExposureCurveColorArray = new Color[k_ExposureCurvePrecision];
        [SerializeField]
        public bool m_LightAdjustment = false;
        [SerializeField]
        public bool m_ExposureAdjustmnt = false;
        [SerializeField]
        public AnimationCurve m_AnimationCurve = AnimationCurve.Linear(-10f, -10f, 16f, 16f);
        [SerializeField]
        public float[] m_ExposureArray;
        public float m_Max, m_Min;
        public bool m_DebugUI;

#if UNITY_EDITOR
#pragma warning restore CS0414
        bool m_isCompiling = false;
#endif


        void Update()
        {

            Initialize();



            // Fail safe in case the curve is deleted / has 0 point
            var curve = m_AnimationCurve;


            if (curve == null || curve.length == 0)
            {
                m_Min = 0f;
                m_Max = 0f;

                for (int i = 0; i < k_ExposureCurvePrecision; i++)
                    m_ExposureArray[i] = 0.0f;
            }
            else
            {
                m_Min = curve[0].time;
                m_Max = curve[curve.length - 1].time;
                float step = (m_Max - m_Min) / (k_ExposureCurvePrecision - 1f);

                for (int i = 0; i < k_ExposureCurvePrecision; i++)
                    m_ExposureArray[i] = curve.Evaluate(m_Min + step * i);
            }


#if UNITY_EDITOR
            // handle script recompile
            if (EditorApplication.isCompiling && !m_isCompiling)
            {
                // on compile begin
                m_isCompiling = true;
                Release();
            }
            else if (!EditorApplication.isCompiling && m_isCompiling)
            {
                // on compile end
                m_isCompiling = false;
            }
#endif
            Shader.SetGlobalFloatArray(kExposureArrayPropName, m_ExposureArray);
            Shader.SetGlobalFloat(kExposureMinPropName, m_Min);
            Shader.SetGlobalFloat(kExposureMaxPropName, m_Max);
            Shader.SetGlobalInt(kExposureAdjustmentPorpName, m_ExposureAdjustmnt ? 1 : 0);
#if false
            Shader.SetGlobalInt(kLightAdjustmentPropName, m_LightAdjustment ? 1 : 0);
#endif
            var pixels = m_ExposureCurveColorArray;

        }

        void EnableSrpCallbacks()
        {

            if (!m_srpCallbackInitialized)
            {
                m_srpCallbackInitialized = true;
            }
        }
        void DisableSrpCallbacks()
        {
            if (m_srpCallbackInitialized)
            {
                m_srpCallbackInitialized = false;
            }
        }

        void OnEnable()
        {

            Initialize();

            EnableSrpCallbacks();

        }

        void OnDisable()
        {
            DisableSrpCallbacks();

            Release();
        }

        void Initialize()
        {
            if (m_initialized)
            {
                return;
            }
#if UNITY_EDITOR
            // initializing renderer can interfere GI baking. so wait until it is completed.

            if (EditorApplication.isCompiling)
                return;
#endif

            if (m_ExposureArray == null)
            {
                m_ExposureArray = new float[k_ExposureCurvePrecision];
            }
            m_initialized = true;
        }


        void Release()
        {
            if (m_initialized)
            {
            }

            m_initialized = false;

        }
/*
        public static void DestroyUnityObject(UnityObject obj)
        {
            if (obj != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    UnityObject.Destroy(obj);
                else
                    UnityObject.DestroyImmediate(obj);
#else
                UnityObject.Destroy(obj);
#endif
            }
        }
*/
    }


}
