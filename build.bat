@cd "%~dp0"

@set UNITY="C:\Program Files\Unity\Editor\Unity.exe"
@set PROJECT_PATH="%~dp0."

:: Win64
@echo Running Win64 Build for: %PROJECT_PATH%
%UNITY% -batchmode -quit ^
	-projectPath	%PROJECT_PATH% ^
	-executeMethod	RoaringFangs.Editor.BuildManager.Build ^
	-platform		Win64 ^
	-configuration	Release ^
	-cleanedLogFile Build\win64-build-log.txt

:: TODO: other platforms

@pause
