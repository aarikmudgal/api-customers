FROM microsoft/dotnet:2.1-sdk
RUN mkdir /app && cd /app
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["customers", "out/eshop.api.customer.dll"]
