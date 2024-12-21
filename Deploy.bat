@echo off

REM Get current year and month using WMIC (consistent format)
for /f "skip=1" %%x in ('wmic os get localdatetime') do if not defined NOW set NOW=%%x
set YEAR=%NOW:~0,4%
set MONTH=%NOW:~4,2%

REM Get git commit count
for /f %%i in ('git rev-list --count --all') do set COMMIT_COUNT=%%i

REM Build version string
set VERSION=%YEAR%.%MONTH%.%COMMIT_COUNT%

REM Building docker image
echo Building docker image for version %VERSION%
docker build -t pen-and-paper-host:%VERSION% .