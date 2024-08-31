using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RDWInterface
{
	public static class RDWDll
	{
        // 系统api
#if UNITY_EDITOR && !UNITY_EDITOR_OSX
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
#else
        //需要与loadLib/getProc等效的Posix实现
#endif


        //直接调用以确保此so来自本app，而并非来自org.renderdoc.renderdoccmd.arm64.apk（adb -s XXX shell settings put global gpu_debug_layer_app org.renderdoc.renderdoccmd.arm64）
        //与在Unity中勾选此so的"Load on start"有着相同目的
#if UNITY_EDITOR
    #if !UNITY_EDITOR_OSX
        public const string RENDERDOC = "renderdoc";
    #else
        public const string RENDERDOC = "renderdoc_Mac_Bundle";
    #endif
#else
    #if UNITY_IPHONE || UNITY_IOS
		public const string RENDERDOC = "__Internal";
    #else
		public const string RENDERDOC = "VkLayer_GLES_RenderDoc";
    #endif
#endif
        [DllImport(RENDERDOC, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string RENDERDOC_GetVersionString();
        [DllImport(RENDERDOC, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool RENDERDOC_IsReleaseBuild();


        //wrap调用
#if UNITY_EDITOR
    #if !UNITY_EDITOR_OSX
        public const string RENDERDOCWRAP = "tstunity";
    #else
        public const string RENDERDOCWRAP = "tstunity_Mac_Bundle";
    #endif
#else
    #if UNITY_IPHONE || UNITY_IOS
		public const string RENDERDOCWRAP = "__Internal";
    #else
		public const string RENDERDOCWRAP = "tstunity";
    #endif
#endif
        [DllImport(RENDERDOCWRAP, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int RdwInitModule(string szLibraryName);
        [DllImport(RENDERDOCWRAP, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern string RdwGetPathTemplate();
        [DllImport(RENDERDOCWRAP, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RdwSetPathTemplate(string szPathTemplate);
        [DllImport(RENDERDOCWRAP, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RdwStartCapture(IntPtr devicePtr, IntPtr wndHandle);
        [DllImport(RENDERDOCWRAP, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RdwEndCapture(IntPtr devicePtr, IntPtr wndHandle);
        [DllImport(RENDERDOCWRAP, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RdwTriggerCapture();
    }
}
