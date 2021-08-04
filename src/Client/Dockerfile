FROM node:16-buster AS build
WORKDIR /app
COPY *.json ./
RUN npm install -g @angular/cli@12.0.2
RUN npm install
COPY . .
RUN ng build --prod

FROM nginx:alpine AS deploy
WORKDIR /app
COPY --from=build /app/dist/Client/*.* /usr/share/nginx/html/

