echo off

set BUILD32=build32
set BUILD64=build64

set SRC_DLL_X32=%BUILD32%\Release
set DST_DLL_X32=..\..\..\example\Assets\Plugins\Native\C\x86

set SRC_DLL_X64=%BUILD64%\Release
set DST_DLL_X64=..\..\..\example\Assets\Plugins\Native\C\x86_64

set DLL_NAME=tstunity.dll

echo **************************build32**********************************

if exist %BUILD32% (
	rd /s/q %BUILD32%
)
mkdir %BUILD32% & pushd %BUILD32%
cmake -G"Visual Studio 17 2022" -A Win32 ..
popd
cmake --build %BUILD32% --config Release
if exist %DST_DLL_X32% (
	rd /s/q %DST_DLL_X32%
)
mkdir %DST_DLL_X32%
copy /Y %SRC_DLL_X32%\%DLL_NAME% %DST_DLL_X32%\%DLL_NAME%

echo **************************build64**********************************

if exist %BUILD64% (
	rd /s/q %BUILD64%
)
mkdir %BUILD64% & pushd %BUILD64%
cmake -G"Visual Studio 17 2022" -A x64 ..
popd
cmake --build %BUILD64% --config Release
if exist %DST_DLL_X64% (
	rd /s/q %DST_DLL_X64%	
)
mkdir %DST_DLL_X64%	
copy /Y %SRC_DLL_X64%\%DLL_NAME% %DST_DLL_X64%\%DLL_NAME%

echo **************************clearall**********************************

if exist %BUILD32% (
	rd /s/q %BUILD32%
)
if exist %BUILD64% (
	rd /s/q %BUILD64%
)

pause