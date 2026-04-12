@echo off
set SCRIPT_DIR=%~dp0
cd /d "%SCRIPT_DIR%"
where git >nul 2>nul
if %errorlevel% neq 0 (
    echo Error: Git is not installed or not in PATH.
    pause
    exit /b
)

git add .
set /p commitMessage="Enter the commit message: "

git commit -m "%commitMessage%"

set /p branch="Enter the name of the branch (Usually its main): "
git push origin %branch%

echo. 
echo Changes have been pushed to the repository.
echo Press any key to exit...
pause
