#ifndef MATH_HELPER_
#define MATH_HELPER_

#ifdef _WIN32
    #define WIN32_LEAN_AND_MEAN
    #include <windows.h>
#else
    #include <dlfcn.h>              // dlopen
#endif
#include <stddef.h>                 // printf
#include <stdbool.h>                // bool
#include <stdexcept>
#include "tstunity_conf.h"

TST_API int tst_add(int a, int b);
TST_API int tst_sub(int a, int b);
TST_API int tst_div(int a, int b);
TST_API int tst_mul(int a, int b);

TST_API bool RdwInitModule(const char* szModuleName);
TST_API const char* RdwGetPathTemplate(void);
TST_API void RdwSetPathTemplate(const char* szTemplate);
TST_API void RdwStartCapture(void* devicePtr, void* wndHandle);
TST_API void RdwEndCapture(void* devicePtr, void* wndHandle);
TST_API void RdwTriggerCapture();

typedef void (CALLBACK FrameMoveCallback)(int nParam1);
TST_API void RegisterFrameMoveCallback(int nParam1, FrameMoveCallback* callback);
TST_API void FrameMove();
TST_API void CrashNoTry();
TST_API void CrashTry();
TST_API void ThrowCppException();
TST_API void TryAndCallIl2cpp();
TST_API void TryAndCallSelf();

#endif//MATH_HELPER_
