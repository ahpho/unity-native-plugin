#define TST_LIB

#include "math_helper.h"
#include "log_helper.h"

int g_nParam1 = 0;
FrameMoveCallback* g_fnCallBack = NULL;

TST_API void FrameMove()
{
	if (g_fnCallBack)
	{
		g_fnCallBack(g_nParam1);
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

TST_API void CrashTry()
{
	LOGD(".....................CrashTry\n");
	try
	{// begin try (depth: 1)
		volatile int* p = NULL;
		*p = 1;
	}// end try (depth: 1)
	catch (std::exception& e)
	{
		LOGD("std::exception=[%s]\n", e.what());
	}
}
