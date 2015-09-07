@echo off

rem .paket\paket.bootstrapper.exe
rem if errorlevel 1 (
rem   exit /b %errorlevel%
rem )

rem .paket\paket.exe restore 
rem if errorlevel 1 (
rem   exit /b %errorlevel%
rem )

packages\FAKE\tools\FAKE.exe build.fsx %*