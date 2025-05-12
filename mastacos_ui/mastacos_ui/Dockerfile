FROM node:16-alpine
WORKDIR /app
EXPOSE 8080
# The CMD will be overridden by the command in docker-compose.yml
CMD ["npm", "run", "serve", "--", "--host", "0.0.0.0"]