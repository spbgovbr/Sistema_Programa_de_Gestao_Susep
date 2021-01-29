######################
# Build
FROM mcr.microsoft.com/dotnet/sdk:3.1 as builder

RUN apt-get update \
 && apt-get install -y nodejs npm --no-install-recommends \
 && npm i npm@latest -g

COPY src/ src/
WORKDIR src/

#RUN dotnet build --configuration Release
RUN dotnet publish --configuration Release

######################
# Deploy
FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

RUN addgroup --group susep \
 && adduser --ingroup susep --no-create-home pgd \
 && chown -R pgd:susep /app

USER pgd

# WebApi
COPY --chown=pgd:susep --from=builder /src/Susep.SISRH.WebApi/bin/Release/netcoreapp3.1/publish /app/api/
# ApiGateway
COPY --chown=pgd:susep --from=builder /src/Susep.SISRH.ApiGateway/bin/Release/netcoreapp3.1/publish /app/gateway/
# WebApp
COPY --chown=pgd:susep --from=builder /src/Susep.SISRH.WebApp/bin/Release/netcoreapp3.1/publish /app/app/
