# Step 1: Use a Node.js image to build the React app
FROM node:16 AS build

# Set the working directory in the container
WORKDIR /app

# Copy package.json and package-lock.json (if present)
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application
COPY . ./

# Build the React app
RUN npm run build

# Step 2: Use Nginx to serve the React app
FROM nginx:alpine

# Copy the build folder from the previous step
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80 to access the app
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
