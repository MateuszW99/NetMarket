﻿FROM node:latest as build

WORKDIR /usr/local/app

COPY ./ /usr/local/app/

ENV NODE_OPTIONS=--openssl-legacy-provider

RUN npm install
RUN npm run build

FROM nginx:latest
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/local/app/dist/Client /usr/share/nginx/html

EXPOSE 80