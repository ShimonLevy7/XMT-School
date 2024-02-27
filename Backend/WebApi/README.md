# XMT School Web API

**Please note!**

You have to manually add a token for a MySQL database in `..\xmt_school_server_cs_asp.net\XmtSchoolWebApi\appsettings.json`.

Otherwise, the API will **not** work!

Welcome to the XMT School Web API documentation. This API serves as the backbone of the XMT School project, facilitating communication between the frontend, backend, and database. This documentation will outline the features of the API and provide basic instructions for deployment.

## Features

### Secure Communication

The API ensures secure communication between the frontend and backend, protecting sensitive data.

### Efficient Data Handling

With a focus on efficiency, the API efficiently handles data requests and responses, ensuring optimal performance.

### Swagger API

The API is documented using Swagger, providing a clear and interactive documentation for developers.

### Project Structure

![Project Structure](https://tiro-finale.com/images/HackerU/Structure.png)

### Endpoints - Login Endpoint

![Login Endpoint](https://tiro-finale.com/images/HackerU/Login.png)

### Endpoints - Users Management Endpoint

![Users Management Endpoint](https://tiro-finale.com/images/HackerU/Users.png)

### Endpoints - Tests Management Endpoint

![Tests Management Endpoint](https://tiro-finale.com/images/HackerU/Tests.png)

## Deployment

To deploy the XMT School Web API, follow these steps:

1. Clone the repository:
   `git clone https://github.com/your/repository.git`
2. Open the solution file `XmtSchoolWebApi.sln` in Visual Studio.
3. Navigate to the `appsettings.json` file in the `XmtSchoolWebApi` project and add a token for your MySQL database.
4. Build the solution.
5. Run the API project (`XmtSchoolWebApi`) in Visual Studio.
6. The API will be running on `http://localhost:port` by default.

For more detailed deployment instructions, refer to the documentation in the repository.

Thank you for using XMT School Web API!
