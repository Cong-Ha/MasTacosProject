FROM node:16-alpine

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the app
COPY . .

# Expose port
EXPOSE 8080

# Start development server with hot-reload
CMD ["npm", "run", "serve", "--", "--host", "0.0.0.0"]