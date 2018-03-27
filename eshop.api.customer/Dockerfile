FROM microsoft/dotnet:2.1-sdk
RUN mkdir /app && cd /app
WORKDIR /app
COPY *.csproj /app
RUN dotnet restore
COPY . /app
RUN dotnet publish -c Release -o out
CMD dotnet run
#ENTRYPOINT ["customers", "out/eshop.api.customer.dll"]
