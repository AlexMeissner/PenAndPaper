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

# Proxy (Caddy)
## Download
``docker pull caddy``

## Initialize
To override the default Caddyfile, you can mount a new one at ``/etc/caddy/Caddyfile``
```
docker run -d -p 80:80 \
    -v $PWD/Caddyfile:/etc/caddy/Caddyfile \
    -v caddy_data:/data \
    caddy
```

## Caddy network
``docker network create caddy-net``

## Run
``docker run -d --cap-add=NET_ADMIN --name caddy-proxy --network caddy-net -v /home/dockeruser/Caddyfile:/etc/caddy/Caddyfile -v /site:/srv -v caddy_data:/data -v caddy_config:/config -p 80:80 -p 443:443 caddy``