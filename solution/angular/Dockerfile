FROM node:18-alpine

WORKDIR /app
COPY sgcan/package.json sgcan/package-lock.json ./
RUN npm install
COPY sgcan ./
RUN npx ng analytics off
EXPOSE 4200
CMD ["npm", "start", "--", "--host", "0.0.0.0"]