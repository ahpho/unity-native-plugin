using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NativeCDemo : MonoBehaviour
{
    private int m_ret1;
    private int m_ret2;
    private int m_ret3;
    private int m_ret4;
    private string m_strRD;

    private void Start()
    {
        Debug.Log($"unity: ......Before NativeCDemo.Start()");
        NativeCSBinder.Register();
        Debug.Log($"unity: ......After NativeCDemo.Start()");


        Debug.Log($"unity: ......调用C代码");
        // m_fnFrameMoveCallback = new NativeCCore.FrameMoveCallback(OnFrameMoveCallBack);
        // NativeCCore.RegisterFrameMoveCallback(42, m_fnFrameMoveCallback);
        // NativeCCore.FrameMove();
		
        //int emLOAD_MAP = 1;
        //Debug.Log($"unity: ......NativeCCore.RegisterMapCallback. emLOAD_MAP={emLOAD_MAP}");
        //NativeCCore.RegisterMapCallback(emLOAD_MAP, NearMapCallback);
        //NativeCCore.TestMapCallback();
        //Debug.Log($"unity: ......调用C代码结束");
    }

    // 我自己的写法
    private static NativeCCore.FrameMoveCallback m_fnFrameMoveCallback;

    [MonoPInvokeCallback(typeof(NativeCCore.FrameMoveCallback))]
	public static void OnFrameMoveCallBack(int nParam)
    {
        ScriptCrash(nParam);
    }

    // Client的写法
    //[MonoPInvokeCallback(typeof(NativeCCore.MapCallback))]
    //public static void NearMapCallback(string szMapName, string szInfoDir, int nMapTemplateId, int nNotResetCamera)
    //{
    //    Debug.Log($"unity: ......NearMapCallback, szMapName={szMapName}");
    //    ScriptCrash(nMapTemplateId);
    //}

    private static void ScriptCrash(int nParam)
    {
        Debug.Log($"unity: ScriptCrash: {nParam}");

        // Renderer[] renderers = null;
        // foreach (Renderer renderer in renderers) {
        //     Material m2 = null;
        //     m2.EnableKeyword("123");
        // }
        
        CrashWithPointer();
        Debug.Log($"unity: Fuck done! ScriptCrash end...");
    }

    private static unsafe void CrashWithPointer()
    {
        // 创建空指针并解引用
        int* ptr = null;
        *ptr = 42; // 这会触发访问违规，导致原生崩溃
    }

    private static void CSharpThrow()
    {
        throw new Exception("throwing CSharpThrow...");
    }

    private void CallFunction()
    {
        m_ret1 = NativeCCore.tst_add(10, 2);
        m_ret2 = NativeCCore.tst_sub(10, 2);
        m_ret3 = NativeCCore.tst_div(10, 2);
        m_ret4 = NativeCCore.tst_mul(10, 2);

        Debug.Log("10+2=" + m_ret1);
        Debug.Log("10-2=" + m_ret2);
        Debug.Log("10/2=" + m_ret3);
        Debug.Log("10*2=" + m_ret4);
    }

    private void PInvokeFunction()
    {
        NativeCCore.call_func();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

        int retHeight = 100;
        if (GUILayout.Button("返回Launcher", GUILayout.Height(retHeight))) {
            SceneManager.LoadScene("Launcher");
        }

        /*if (GUILayout.Button("调用C代码", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            CallFunction();

            Debug.Log($"......调用C代码");
            m_fnFrameMoveCallback = new NativeCCore.FrameMoveCallback(OnFrameMoveCallBack);
            NativeCCore.RegisterFrameMoveCallback(42, m_fnFrameMoveCallback);
            NativeCCore.FrameMove();
            Debug.Log($"......调用C代码结束");
        }
        if (GUILayout.Button("C调用CS", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            PInvokeFunction();
        }*/
        if (GUILayout.Button("NativeCCore.DllCanUnloadNow", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            Debug.Log($"......NativeCCore.DllCanUnloadNow");
            try {
                NativeCCore.DllCanUnloadNow();
            }
            catch (Exception e) {
                Debug.Log($"NativeCCore.DllCanUnloadNow, ex: {e.Message}");
            }          
            Debug.Log($"......NativeCCore.DllCanUnloadNow end");
        }
        if (GUILayout.Button("TryAndCallSelf", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            Debug.Log($"unity: ......TryAndCallSelf");
            NativeCCore.TryAndCallSelf();
            Debug.Log($"unity: ......TryAndCallSelf end");
        }

#if OLD_TEST
        if (GUILayout.Button("RD.cmd.CommitHash", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            m_strRD = RenderDocCmdCore.RENDERDOC_CanGlobalHook().ToString();
        }
        if (GUILayout.Button("RD.cmd.usleep", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            Debug.Log($"Before RenderDocCmdCore.usleep()");
            m_strRD = RenderDocCmdCore.usleep((IntPtr)6000000).ToString();
            Debug.Log($"After RenderDocCmdCore.usleep()"); 
        }
        if (GUILayout.Button("RD.vklayer.RENDERDOC_SetDebugLogFile", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            VkLayerCore.RENDERDOC_SetDebugLogFile("./fuck123.log");
        }
        if (GUILayout.Button("RD.vklayer.force_include_libentry", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            m_strRD = VkLayerCore.force_include_libentry().ToString();
        }
        if (GUILayout.Button("RD.vklayer.usleep", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            Debug.Log($"Before VkLayerCore.usleep()");
            m_strRD = VkLayerCore.usleep((IntPtr)6000000).ToString();
            Debug.Log($"After VkLayerCore.usleep()"); 
        }
#endif
        if (GUILayout.Button("[RDW] Module", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            RDWInterface.RDWDll.RdwInitModule(@"libVkLayer_GLES_RenderDoc.so");
        }
        if (GUILayout.Button("[RDW] GetTemplate", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            m_strRD = RDWInterface.RDWDll.RdwGetPathTemplate();
        }
        if (GUILayout.Button("[RDW] SetTemplate", GUILayout.Height((Screen.height - retHeight) >> 4))){
            RDWInterface.RDWDll.RdwSetPathTemplate(@"/sdcard/Android/data/com.hellowod.example/files/RenderDoc/FuckFrame_"); 
        }
        if (GUILayout.Button("[RDW] StartCapture", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            RDWInterface.RDWDll.RdwStartCapture(IntPtr.Zero, IntPtr.Zero);
        }
        if (GUILayout.Button("[RDW] EndCapture", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            RDWInterface.RDWDll.RdwEndCapture(IntPtr.Zero, IntPtr.Zero);
        }
        if (GUILayout.Button("[RDW] TriggerCapture", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            RDWInterface.RDWDll.RdwTriggerCapture();
        }
        if (GUILayout.Button("[RDW] IsRelease", GUILayout.Height((Screen.height - retHeight) >> 4))) {
            //Debug.Log($"RENDERDOC_GetVersionString={RDWInterface.RDWDll.RENDERDOC_GetVersionString()}");
            Debug.Log($"RENDERDOC_IsReleaseBuild={RDWInterface.RDWDll.RENDERDOC_IsReleaseBuild()}");
        }

        GUILayout.TextArea(
           "10+2=" + m_ret1 + "\n" + 
           "10-2=" + m_ret2 + "\n" + 
           "10/2=" + m_ret3 + "\n" + 
           "10*2=" + m_ret4 + "\n" +
           "RDstr="+ m_strRD+ "\n", 
           GUILayout.Height((Screen.height - retHeight) >> 1)
        );

        GUILayout.EndArea();
    }
}
