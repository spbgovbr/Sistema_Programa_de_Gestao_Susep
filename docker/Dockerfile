######################
# Build
FROM mcr.microsoft.com/dotnet/sdk:3.1-bullseye as builder

RUN apt-get update \
 && apt-get install -y nodejs npm --no-install-recommends

COPY src/ src/
WORKDIR src/

ARG diretorio="install/1. Susep.SISRH.WebApi"

COPY ${diretorio}/SUSEP.Framework.CoreFilters.dll Susep.libs/SUSEP.Framework.CoreFilters.dll
COPY ${diretorio}/SUSEP.Framework.Data.dll Susep.libs/SUSEP.Framework.Data.dll
COPY ${diretorio}/SUSEP.Framework.MessageBroker.dll Susep.libs/SUSEP.Framework.MessageBroker.dll
COPY ${diretorio}/SUSEP.Framework.Messages.dll Susep.libs/SUSEP.Framework.Messages.dll
COPY ${diretorio}/SUSEP.Framework.Result.dll Susep.libs/SUSEP.Framework.Result.dll
COPY ${diretorio}/SUSEP.Framework.SeedWorks.dll Susep.libs/SUSEP.Framework.SeedWorks.dll
COPY ${diretorio}/SUSEP.Framework.Utils.dll Susep.libs/SUSEP.Framework.Utils.dll

#RUN dotnet build --configuration Release
RUN dotnet publish --configuration Release
#  --verbosity diag

######################
# Deploy
FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

RUN addgroup --group susep --gid 1000 \
 && adduser --ingroup susep --no-create-home --uid 1000 pgd \
 && chown -R pgd:susep /app

USER pgd

# WebApi
COPY --chown=pgd:susep --from=builder /src/Susep.SISRH.WebApi/bin/Release/netcoreapp3.1/publish /app/api/
# ApiGateway
COPY --chown=pgd:susep --from=builder /src/Susep.SISRH.ApiGateway/bin/Release/netcoreapp3.1/publish /app/gateway/
# WebApp
COPY --chown=pgd:susep --from=builder /src/Susep.SISRH.WebApp/bin/Release/netcoreapp3.1/publish /app/app/
