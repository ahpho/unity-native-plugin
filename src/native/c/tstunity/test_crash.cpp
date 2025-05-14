#define TST_LIB

#include "math_helper.h"
#include "log_helper.h"
#include <string>

int g_nParam1 = 0;
FrameMoveCallback* g_fnCallBack = NULL;

TST_API void FrameMove()
{
	if (g_fnCallBack)
	{
		g_fnCallBack(g_nParam1 + 1);
		g_fnCallBack = NULL;
		g_nParam1 = 0;
	}
}

TST_API void RegisterFrameMoveCallback(int nParam1, FrameMoveCallback* callback)
{
	g_nParam1 = nParam1;
	g_fnCallBack = callback;
}

TST_API void CrashNoTry()
{
	LOGD(".....................CrashNoTry\n");
	volatile int* p = NULL;
	*p = 1;
}

#define ENABLE_CPP_EXCEPTION 1

TST_API void CrashTry()
{
	LOGD(".....................CrashTry\n");

#if ENABLE_CPP_EXCEPTION == 1
	try
	{// begin try (depth: 1)
		volatile int* p = NULL;
		*p = 1;
	}// end try (depth: 1)
	catch (std::exception& e)
	{
		LOGD("std::exception=[%s]\n", e.what());
	}
#else
	LOGD("..................... doing nothing.\n");
#endif
}

TST_API void ThrowCppException()
{
#if defined(ANDROID) || defined(__ANDROID__)
	__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[tstunity] ThrowCppException 1");
	throw std::runtime_error("ThrowCppException");
	//throw 1;
	//throw "test exception";
	//throw std::exception();
	//throw std::bad_alloc();
	//throw std::out_of_range("test exception");
	//throw std::invalid_argument("test exception");
	//throw std::logic_error("test exception");
	//throw std::bad_exception();
	//throw std::bad_typeid();
	//throw std::bad_cast();
	__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[tstunity] ThrowCppException 2");
#endif
}

TST_API void TryAndCallSelf()
{
#if defined(ANDROID) || defined(__ANDROID__)
	__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[TryAndCallSelf] It's Android.");

	// RTLD_NOLOAD, we'll not try loading *.so dynamically.
	void* mod = dlopen("libtstunity2.so", RTLD_NOW/* | RTLD_NOLOAD*/);
	if (!mod)
	{
		__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[TryAndCallSelf] init failed! dlopen libil2cppRF.so, mod=%p", mod);
		return;
	}
	typedef void (* pThrowCppException)();
	pThrowCppException ThrowCppException_ = NULL;
	ThrowCppException_ = (pThrowCppException)dlsym(mod, "ThrowCppException");
	if (!ThrowCppException_)
	{
		__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[TryAndCallSelf] dlsym failed! ThrowCppException_=%p", ThrowCppException_);
		return;
	}

	__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[TryAndCallSelf] ThrowCppException.");
	#if false
		ThrowCppException_();
	#else
		try {
			ThrowCppException_();
		}
		catch (const std::exception& e) {
			// 捕获所有标准C++异常及其派生类
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "标准异常: %s", e.what());
		}
		catch (const std::string& e) {
			// 捕获std::string类型的异常(有些代码会抛出字符串)
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "C++字符串异常: %s", e.c_str());
		}
		catch (const char* e) {
			// 捕获C字符串异常
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "C字符串异常: %s", e);
		}
		catch (...) {
			// 捕获其他所有类型的异常
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "未知类型的异常");
		}
	#endif
#else

#endif	
}

TST_API void TryAndCallIl2cpp()
{
#if defined(ANDROID) || defined(__ANDROID__)
	__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[tstunity] It's Android.");

	// RTLD_NOLOAD, we'll not try loading *.so dynamically.
	void* mod = dlopen("libil2cppRF.so", RTLD_NOW/* | RTLD_NOLOAD*/);
	if (!mod)
	{
		__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[tstunity] init failed! dlopen libil2cppRF.so, mod=%p", mod);
		return;
	}
	typedef long (* pDllCanUnloadNow)();
	pDllCanUnloadNow DllCanUnloadNow = NULL;
	DllCanUnloadNow = (pDllCanUnloadNow)dlsym(mod, "DllCanUnloadNow");
	if (!DllCanUnloadNow)
	{
		__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[tstunity] dlsym failed! DllCanUnloadNow=%p", DllCanUnloadNow);
		return;
	}

	__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[tstunity] DllCanUnloadNow.");
	long ret = 0;
	#if false
		ret = DllCanUnloadNow();
	#else
		try {
			// 假如il2cpp代码中抛出异常，上溯栈帧到libClient.so时，若上下文中没try则相安无事。
			// 但如果有try，C++异常传递时要求.so之间是ABI兼容的。否则大概率宕机。
			// 
			// 作为负责重现而抛出异常的测试例子，打开导出的安卓gradle项目rf_project_release，导航到：
			// unityLibrary\src\main\Il2CppOutputProject\IL2CPP\libil2cpp\vm\COMEntryPoints.cpp
			// 导航到long STDCALL DllCanUnloadNow()函数入口，添加一行：
			// il2cpp::vm::Exception::RaiseNullReferenceException();
			// 理论上libil2cppRF.so任意其他导出函数皆可。
			ret = DllCanUnloadNow();
		}
		catch (const std::exception& e) {
			// 捕获所有标准C++异常及其派生类
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "标准异常: %s", e.what());
		}
		catch (const std::string& e) {
			// 捕获std::string类型的异常(有些代码会抛出字符串)
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "C++字符串异常: %s", e.c_str());
		}
		catch (const char* e) {
			// 捕获C字符串异常
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "C字符串异常: %s", e);
		}
		catch (...) {
			// 捕获其他所有类型的异常
			__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "未知类型的异常");
		}
	#endif
	
	__android_log_print(ANDROID_LOG_DEBUG, "Unity", "%s", "[TestMapCallback] called! ret=%d", ret);
#else

#endif
}
