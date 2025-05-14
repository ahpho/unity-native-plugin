cmake_minimum_required(VERSION 2.8)

project(tstunity)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)
set(ANDROID_STL c++_static)
set(ANDROID_PLATFORM android-22)
set(CMAKE_ANDROID_API 22)

# 确保启用异常支持
# 对于大多数编译器，异常是默认启用的，但这里显式指定以确保
if(MSVC)
    # Visual Studio编译器选项
    # /EHsc 启用C++异常处理 (s:同步异常, c:extern "C"函数不抛出异常)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /EHsc")
    add_compile_options(/DLIBTST_BUILD)
else()
    # GCC/Clang编译器选项
    # -fexceptions 启用异常支持, -fno-exceptions关闭
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -static-libstdc++ -std=c++11 -Wno-register -fexceptions ")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -U_FORTIFY_SOURCE -fno-rtti -funwind-tables -fPIC -g ")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fdiagnostics-format=msvc -fdiagnostics-absolute-paths -no-canonical-prefixes ")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fsigned-char -fno-strict-overflow -fomit-frame-pointer -fvisibility=hidden -fstack-protector ")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fstrict-aliasing -ffunction-sections -fdata-sections -fmessage-length=0 -fcolor-diagnostics ")
    #set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wl,--wrap,sigaction -Wl,--wrap,sigaction -Wl,--gc-sections -Wl,--fatal-warnings -Wl,--no-undefined -Wl,--build-id -Wl,-z,noexecstack -shared -rdynamic -Wl,-z,relro ")
    add_compile_options(-DLIBTST_BUILD)
    add_compile_options(-DANDROID)
    add_compile_options(-DANDROID_PLATFORM=android-22)
    add_compile_options(-DANDROID_NATIVE_API_LEVEL=android-22)
endif()


include_directories(
	tstunity
)

set ( TSTUNITY_CORE 
	tstunity/math_helper.cpp
	tstunity/unity_helper.cpp
    tstunity/test_crash.cpp
)

if (APPLE)
    if (IOS)
        set(CMAKE_OSX_ARCHITECTURES "$(ARCHS_STANDARD)")
        add_library(tstunity STATIC
           ${TSTUNITY_CORE}
        )
    else ()
        set(CMAKE_OSX_ARCHITECTURES "$(ARCHS_STANDARD_64_BIT)")
        add_library(tstunity MODULE
            ${TSTUNITY_CORE}
        )
        set_target_properties ( tstunity PROPERTIES BUNDLE TRUE )
    endif ()
else ()
	add_definitions("-DLIBTST_BUILD")
    add_library(tstunity SHARED
        ${TSTUNITY_CORE}
    )
endif ()
	
if (WIN32 AND NOT CYGWIN)
    target_link_libraries(tstunity
		ws2_32
		${THIRDPART_LIB} 
    )
else ()
    target_link_libraries(tstunity
		m
		log
		${THIRDPART_LIB} 
	)
endif ()
