FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .

# Change executer to non user
RUN useradd -u 7999 appexecuter
RUN chown -R appexecuter .
USER appexecuter

EXPOSE 80

ENTRYPOINT ["dotnet", "PoliFemoBackend.dll"]
