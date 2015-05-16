@echo off

msbuild.exe /p:Configuration="%config%" /p:Platform="x64" ^
  /verbosity:minimal ^
  /nologo ^
  /fl ^
  /flp:LogFile=clean.log;Verbosity=detailed ^
  /t:clean ^
  grobid.sln

