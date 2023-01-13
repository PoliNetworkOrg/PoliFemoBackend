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

# Set timezone to Italy
RUN sh -c 'rm -rf /etc/localtime; ln -s /usr/share/zoneinfo/Europe/Rome /etc/localtime;'
RUN date

EXPOSE 443

# Change executer to non user
RUN useradd -u 7999 appexecuter
RUN chown -R appexecuter .
USER appexecuter

ENTRYPOINT ["sh", "-c", "dotnet PoliFemoBackend.dll 2"]
