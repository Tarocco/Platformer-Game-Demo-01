@cd "%~dp0"

@set UNITY="C:\Program Files\Unity\Editor\Unity.exe"
@set PROJECT_PATH="%~dp0."

:: WebGL
@echo Running WebGL Build for: %PROJECT_PATH%
%UNITY% -batchmode -quit ^
	-projectPath	%PROJECT_PATH% ^
	-executeMethod	RoaringFangs.Editor.BuildManager.Build ^
	-platform		WebGL ^
	-configuration	Release ^
	-cleanedLogFile Build\webgl-build-log.txt

:: TODO: other platforms

@pause
