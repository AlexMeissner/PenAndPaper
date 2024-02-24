# Connect to server
``ssh username@ip``

# MYSQL
## Download
``sudo docker pull mysql``

## Create
``sudo docker run -d --name MysqlDatabase -e MYSQL_ROOT_PASSWORD=Wurst007 -p 3306:3306 mysql``

## Start
``sudo docker start MysqlDatabase``

# Pen and Paper Docker Image
## Download
**sudo should not be used here**

``sudo docker login``

``sudo docker pull alexmeissner/pen_and_paper_server``

## Create
**sudo should not be used here**

``sudo docker run -d --name PenAndPaperServer -p 7099:7099 -p 443:443 alexmeissner/pen_and_paper_server``

## Start
``sudo docker start PenAndPaperServer``

# Container Output
``sudo docker logs <containername>``