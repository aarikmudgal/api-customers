FROM microsoft/dotnet:2.1-sdk
RUN mkdir /app && cd /app
COPY *.csproj /app
RUN dotnet restore
COPY ../eshop.api.customer /app
COPY ../eshop.api.customer.dal /app
WORKDIR /app/eshop.api.customer
CMD [ "dotnet", "run" ]
EXPOSE 8001
