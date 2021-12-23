@echo off
set folder2=..\..\..

IF EXIST "%1TestData" (
   IF NOT EXIST "%folder2%\TestData" mkdir "%folder2%\TestData"
   xcopy "%1TestData" "%folder2%\TestData" /E /S /Y
)
rem pause