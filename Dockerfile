FROM node:14.15.5-alpine AS build-frontend
WORKDIR /app-frontend
COPY ./Frontend .
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:5.0 as backend-build
WORKDIR /backend
COPY ./Backend .
COPY --from=build-frontend /app-frontend/dist/files-management ./wwwroot
#COPY ./Frontent/dist/files-management ./wwwroot
RUN ls
RUN dotnet restore
RUN dotnet build -o /app -c Release

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=backend-build /app .
ENTRYPOINT ["dotnet", "Files.Management.dll"]