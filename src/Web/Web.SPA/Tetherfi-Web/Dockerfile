FROM node:18.13 AS build
WORKDIR /usr/src/app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build
FROM nginx:1.17.1-alpine
COPY --from=build /usr/src/app/dist/tetherfi-web /usr/share/nginx/html
EXPOSE 80
