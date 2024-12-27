@echo off

echo "*******************************************************************************";
echo "*                                                                             *";
echo "*  ____                      ___       ____                                   *";
echo "* |  _ \    ___   _ __      ( _ )     |  _ \    __ _   _ __     ___   _ __    *";
echo "* | |_) |  / _ \ | '_ \     / _ \/\   | |_) |  / _` | | '_ \   / _ \ | '__|   *";
echo "* |  __/  |  __/ | | | |   | (_>  <   |  __/  | (_| | | |_) | |  __/ | |      *";
echo "* |_|      \___| |_| |_|    \___/\/   |_|      \__,_| | .__/   \___| |_|      *";
echo "*  ____                   _                           |_|                _    *";
echo "* |  _ \    ___   _ __   | |   ___    _   _   _ __ ___     ___   _ __   | |_  *";
echo "* | | | |  / _ \ | '_ \  | |  / _ \  | | | | | '_ ` _ \   / _ \ | '_ \  | __| *";
echo "* | |_| | |  __/ | |_) | | | | (_) | | |_| | | | | | | | |  __/ | | | | | |_  *";
echo "* |____/   \___| | .__/  |_|  \___/   \__, | |_| |_| |_|  \___| |_| |_|  \__| *";
echo "*                |_|                  |___/                                   *";
echo "*                                                                             *";
echo "*******************************************************************************";

REM GET USER SECRETS
set /p "IP_ADDRESS=IP: "
set /p "USER_NAME=Username: "
set /p "GOOGLE_CLIENT_ID=Google ClientId: "
set /p "GOOGLE_SECRET=Google Secret: "

REM GET VERSION WITH FORMAT (YEAR.MONTH.COMMIT_COUNT)
for /f "skip=1" %%x in ('wmic os get localdatetime') do if not defined NOW set NOW=%%x
set YEAR=%NOW:~0,4%
set MONTH=%NOW:~4,2%

for /f %%i in ('git rev-list --count --all') do set COMMIT_COUNT=%%i

set VERSION=%YEAR%.%MONTH%.%COMMIT_COUNT%
echo Current Version: %VERSION%

REM BUILD DOCKER IMAGE FOR API
echo Building docker image for the API...
set API_CONTAINER_NAME=pen-and-paper-api
docker build -f Api.Dockerfile -t %API_CONTAINER_NAME%:%VERSION% .
echo Finished building docker image for the API!

REM BUILD DOCKER IMAGE FOR FRONTEND
echo Building docker image for the frontend...
set FRONTEND_CONTAINER_NAME=pen-and-paper-frontend
docker build -f Website.Dockerfile -t %FRONTEND_CONTAINER_NAME%:%VERSION% .
echo Finished building docker image for the frontend!

REM PUSH DOCKER IMAGES TO SERVER
echo Pushing API docker image to server...
docker save %API_CONTAINER_NAME% | ssh %USER_NAME%@%IP_ADDRESS% docker load
echo Finished pushing API docker image to server!

echo Pushing frontend docker image to server...
docker save %FRONTEND_CONTAINER_NAME% | ssh %USER_NAME%@%IP_ADDRESS% docker load
echo Finished pushing frontend docker image to server!

REM STOP RUNNING CONTAINERS ON SERVER WITH SAME IMAGE
echo Stopping running API container...
ssh %USER_NAME%@%IP_ADDRESS% "docker ps --filter ancestor=%API_CONTAINER_NAME% --format '{{.ID}}' | xargs --no-run-if-empty docker stop"
echo API container stopped!

echo Stopping running frontend container...
ssh %USER_NAME%@%IP_ADDRESS% "docker ps --filter ancestor=%FRONTEND_CONTAINER_NAME% --format '{{.ID}}' | xargs --no-run-if-empty docker stop"
echo Frontend container stopped!

REM START DOCKER CONTAINER
echo Starting API docker container...
ssh %USER_NAME%@%IP_ADDRESS% "docker run --name PenAndPaperAPI --network caddy-net -d --restart unless-stopped -p 8081:8080 %API_CONTAINER_NAME%:%VERSION%"
echo API docker container started!

echo Starting frontend docker container...
ssh %USER_NAME%@%IP_ADDRESS% "docker run --name PenAndPaperFrontend --network caddy-net -d --restart unless-stopped -p 7070:7070 -e ApiHost='http://PenAndPaperAPI:8080/' -e Google__ClientId=%GOOGLE_CLIENT_ID% -e Google__ClientSecret=%GOOGLE_SECRET% %FRONTEND_CONTAINER_NAME%:%VERSION%"
echo Frontend docker container started!

REM FINISHED
echo Deployment finished!
PAUSE