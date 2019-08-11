FROM node:12-alpine as node
WORKDIR /app

COPY ./src/Quote.Web/package.json ./
RUN npm i
COPY ./src/Quote.Web/ .
RUN npm run build

FROM nginx:alpine
COPY --from=node /app/build /usr/share/nginx/html/