#!/usr/bin/env bash

printf "Running MariaDB...\n\n"
docker run --name dictum-db -v $(pwd)/$(dirname "$0")/../db:/var/lib/mysql --rm -e MYSQL_ALLOW_EMPTY_PASSWORD=true -d -p 3306:3306 mariadb:latest