# Connect to server
``ssh username@ip``

# MYSQL
## Download
``docker pull mysql``

## Create
``docker run -d --name MysqlDatabase -e MYSQL_ROOT_PASSWORD=Wurst007 -p 3306:3306 mysql``

## Start
``docker start MysqlDatabase``

# Pen and Paper Docker Image
## Download
``docker login``

``docker pull alexmeissner/pen_and_paper_server``

## Create
``docker run -d --name PenAndPaperServer --network caddy-net -p 7099:8080 alexmeissner/pen_and_paper_server``

## Start
``docker start PenAndPaperServer``

# Container Output
``docker logs <containername>``

# Proxy (Caddy)
## Download
``docker pull caddy``

## Caddy network
``docker network create caddy-net``

## Run
``docker run -d --cap-add=NET_ADMIN --name caddy-proxy --network caddy-net -v /home/dockeruser/Caddyfile:/etc/caddy/Caddyfile -v /site:/srv -v caddy_data:/data -v caddy_config:/config -p 80:80 -p 443:443 caddy``