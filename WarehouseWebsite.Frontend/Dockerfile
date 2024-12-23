# Используйте официальный образ Node.js
FROM node:22-alpine

# Установите рабочую директорию
WORKDIR /usr/src/app

# Копируйте файлы package.json и package-lock.json (если он есть)
COPY package.json ./

# Установите Angular CLI глобально
RUN npm install -g @angular/cli

# Установите зависимости проекта
RUN npm install

# Копируйте остальные файлы проекта
COPY . .

# Откройте порт, который будет использоваться для доступа к приложению
EXPOSE 8081

# Запустите ваше приложение в продакшн режиме
CMD ["ng", "serve", "--host", "0.0.0.0", "--configuration", "production"]
