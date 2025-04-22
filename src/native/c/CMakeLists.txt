cmake_minimum_required(VERSION 2.8)

project(tstunity)


set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
# 确保启用异常支持
# 对于大多数编译器，异常是默认启用的，但这里显式指定以确保
if(MSVC)
    # Visual Studio编译器选项
    # /EHsc 启用C++异常处理 (s:同步异常, c:extern "C"函数不抛出异常)
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /EHsc")
    add_compile_options(/DLIBTST_BUILD)
else()
    # GCC/Clang编译器选项
    # -fexceptions 启用异常支持
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fexceptions") #-fno-exceptions
    add_compile_options(-DLIBTST_BUILD)
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
else ( )
	add_definitions("-DLIBTST_BUILD")
    add_library(tstunity SHARED
        ${TSTUNITY_CORE}
    )
endif ( )
	
if ( WIN32 AND NOT CYGWIN )
    target_link_libraries(tstunity
		ws2_32
		${THIRDPART_LIB} 
    )
else ( )
    target_link_libraries(tstunity
		m
		log
		${THIRDPART_LIB} 
	)
endif ( )