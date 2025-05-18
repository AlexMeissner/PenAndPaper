# PenAndPaper

[![Publish Client](https://github.com/AlexMeissner/PenAndPaper/actions/workflows/publish-client.yml/badge.svg)](https://github.com/AlexMeissner/PenAndPaper/actions/workflows/publish-client.yml)
[![Update Server](https://github.com/AlexMeissner/PenAndPaper/actions/workflows/update-server.yml/badge.svg)](https://github.com/AlexMeissner/PenAndPaper/actions/workflows/update-server.yml)


# Postgres

``docker pull postgres``

``docker run -d --name postgres --network <network-name> -e POSTGRES_USER=<username> -e POSTGRES_PASSWORD=<password> -p 5432:5432 --restart unless-stopped postgres:latest``

``docker exec -it postgres psql -U postgres -c "CREATE ROLE <role_name> WITH LOGIN PASSWORD '<different_password>';"``
``docker exec -it postgres psql -U postgres -c "ALTER ROLE <role_name> CREATEDB;"``

# Backend

``docker run --name PenAndPaperServer --network <network-name> -d -e "ConnectionStrings__PostgresDb=Host=postgres;Port=5432;Database=penandpaperdb;Username=<role_name>;Password=<different_password>" \ -p 8080:8080 alexmeissner/pen_and_paper_server``