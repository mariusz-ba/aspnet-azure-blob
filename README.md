# AzureBlob

Example ASP.NET Core application using Azure Blob Storage.

## Running

For local development in order not to connect to Azure Blob Storage hosted in the cloud - the azurite
docker image is used. The docker container can be started using:

```bash
docker-compose up
```

Now blob storage is running on `localhost:10000`. The connection string provided in 
`appsettings.json` points to exactly this instance.

API

``` bash
# Get list of all blobs in the "images" container
GET localhost:5000/azure

# Get single blob by its name
GET localhost:5000/azure/path/to/blob.jpg

# Upload file to blob storage
POST localhost:5000/azure
Headers:
Content-Type: multipart/form-data
Body:
{
    file: <file>
}
```

The last uploaded file will appear on home page `localhost:5000`.