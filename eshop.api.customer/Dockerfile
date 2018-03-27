FROM microsoft/dotnet:2.1-sdk
RUN mkdir /app && cd /app
WORKDIR /app
COPY *.csproj /app
RUN dotnet restore
COPY . /app
#RUN dotnet publish -c Release -o out && cd /app/out
#WORKDIR /app/out
#ENTRYPOINT ["customers", "/app/out/eshop.api.customer.dll"]
EXPOSE 12345
CMD [ "dotnet", "run" ]
