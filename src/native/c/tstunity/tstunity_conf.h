/*
conf
*/

#if defined(_WIN32) || defined(_MSC_VER)
    #ifdef LIBTST_BUILD
        #define TST_API extern "C" __declspec(dllexport)
    #else
        #define TST_API extern "C" __declspec(dllimport)
    #endif
#elif defined(__ANDROID__) || defined(ANDROID)
	#ifdef LIBTST_BUILD
		#define TST_API extern "C" __attribute__((visibility("default")))
	#else
		#define TST_API extern "C" 
	#endif
#else
    #define TST_API
#endif

#if defined(_WIN32) || defined(_MSC_VER)
	#define CALLBACK __stdcall
#elif defined(__APPLE__)
	#define CALLBACK
#elif defined(__ANDROID__) || defined(ANDROID)
	#define CALLBACK 
#endif

typedef void (*CSFunction)();
