# ----- STAGE 1: BUILD ANGULAR -----
FROM node:18.19.0 AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build --prod

# ----- STAGE 2: SERVE WITH NGINX -----
FROM nginx:1.25
WORKDIR /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
