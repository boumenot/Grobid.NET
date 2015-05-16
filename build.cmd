@echo off
set config=%1

if "%config%" == "" (
   set config=Debug
)

msbuild.exe /p:Configuration="%config%" /p:Platform="x64" ^
  /maxcpucount ^
  /verbosity:minimal ^
  /nologo ^
  /fl ^
  /flp:LogFile=build.log;Verbosity=detailed ^
  build\build.proj
  
  
  