#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InstantaneousGram-LikeAndComment.csproj", "."]
RUN dotnet restore "./InstantaneousGram-LikeAndComment.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "InstantaneousGram-LikeAndComment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InstantaneousGram-LikeAndComment.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InstantaneousGram-LikeAndComment.dll"]