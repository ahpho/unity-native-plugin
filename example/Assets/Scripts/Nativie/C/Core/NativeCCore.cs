using System;
using System.Runtime.InteropServices;

public static class NativeCCore
{
#if UNITY_IPHONE && !UNITY_EDITOR
		private const string UNITYDLL = "__Internal";
#else
        private const string UNITYDLL = "tstunity";
#endif

    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tst_add(int a, int b);

    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tst_sub(int a, int b);

    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tst_div(int a, int b);

    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tst_mul(int a, int b);

    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void reg_func(IntPtr fn);

    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void call_func();


    public delegate void FrameMoveCallback(int nParam1);
    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void RegisterFrameMoveCallback(int nParam1, FrameMoveCallback callback);
    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void FrameMove();
    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CrashNoTry();
    [DllImport(UNITYDLL, CallingConvention = CallingConvention.Cdecl)]
    public static extern void CrashTry();


#if UNITY_IPHONE && !UNITY_EDITOR
	private const string CLIENT_SCENE = "__Internal";
#else
    private const string CLIENT_SCENE = "Client";
#endif
	public delegate void MapCallback(string szParam1, string szParam2, int nParam, int nParam2);
    [DllImport(CLIENT_SCENE, CallingConvention = CallingConvention.Cdecl)]
    public static extern void RegisterMapCallback(int nType, MapCallback fnCallback);
    [DllImport(CLIENT_SCENE, CallingConvention = CallingConvention.Cdecl)]
    public static extern void TestMapCallback();
    
}
