FROM microsoft/dotnet:2.1-sdk
WORKDIR ./
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["customers", "out/eshop.api.customer.dll"]
