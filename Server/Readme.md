# Connect to server
``ssh username@ip``

# MYSQL
## Download
``sudo docker pull mysql``

## Start
``sudo docker run -d --name MysqlDatabase -e MYSQL_ROOT_PASSWORD=Wurst007 -p 3306:3306 mysql``