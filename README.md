# CarStockApi

---

## Run This Project Locally

### 1. Clone the repository

Clone the project repository to your local machine:

```bash
git clone https://github.com/dh-giang-vu/CarStockAPI.git
cd CarStockAPI
```

### 2. Run the project

If using Visual Studio 2022, just open the project and click the run button (Ctrl + F5). Otherwise, here is how to use .NET CLI to run this project:

```bash
# Navigate to the root of CarStockAPI directory first, i.e. cd CarStockAPI
dotnet run --launch-profile https
```

The project should launch at `https://localhost:8080/`.

---

## Quick Notes

- Must register a Dealer "account" and "log in" first to access any endpoints involving cars.
- If log in is successful, a JWT will be given. Send this token in the `Authorization` header of every request to endpoints involving cars.
- The JWT Token is used by the server to identify which Dealer is making the request.
- Examples of using all API endpoints are available below (requests & responses).

---

## API Endpoints

| HTTP Method | Endpoint                | Description                                                       |
|-------------|-------------------------|-------------------------------------------------------------------|
| `POST`      | `/api/dealers/register` | Create a new Dealer                                               |
| `POST`      | `/api/dealers/login`    | Authenticate a Dealer and return a JWT if login details are valid |
| `GET`       | `/api/cars`             | List cars and stock levels                                        |
| `POST`      | `/api/cars`             | Add new car                                                       |
| `PUT`       | `/api/cars`             | Update car stock level                                            |
| `DELETE`    | `/api/cars/{CarId}`     | Remove car                                                        |
| `GET`       | `/api/cars/search`      | Search car by make and/or model                                   |

## Example Usage

Note: not all responses are shown.

- **Create a new Dealer**:
  ```http
  POST /api/dealers/register
  Content-Type: application/json

  {
    "name": "My Name",
    "credentials": {
        "username": "myUsername",
        "password": "mypassword123"
    }
  }
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": "Dealer registered successfully.",
    "details": null
  }
  ```

  ```json
  // HTTP 400 Bad Request
  {
    "message": "Username is already taken.",
    "details": "myUsername"
  }
  ```

- **Authenticate Dealer**:
  ```http
  POST /api/dealers/login
  Content-Type: application/json

  {
    "credentials": {
        "username": "myUsername",
        "password": "mypassword123"
    }
  }
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": "Login success.",
    "token": "JWT Token to send with your resquests to /api/cars/*"
  }
  ```

- **List cars and stock levels**:
  ```http
  GET /api/cars
  Authorization: Bearer jwt-token-from-authenticate-dealer
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": null,
    "carResponses": [
        {
            "id": 1,
            "make": "Toyota",
            "model": "Corolla",
            "year": 2020,
            "stockLevel": 10
        },
        {
            "id": 3,
            "make": "Honda",
            "model": "Civic",
            "year": 2020,
            "stockLevel": 8
        }
    ]
  }
  ```

- **Add a new car**:
  ```http
  POST /api/cars
  Authorization: Bearer jwt-token-from-authenticate-dealer
  Content-Type: application/json

  {
    "make": "Audi",
    "model": "A4",
    "year": 2016,
    "stockLevel": 88
  }
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": "Car added successfully.",
    "details": {
        "make": "Audi",
        "model": "A4",
        "year": 2016,
        "stockLevel": 88
    }
  }
  ```

  ```json
  // HTTP 400 Bad Request - adding existing car to the same dealer
  {
    "message": "This car already exists.",
    "details": {
        "make": "Audi",
        "model": "A4",
        "year": 2016,
        "stockLevel": 88
    }
  }
  ```

- **Update car stock level**:
  ```http
  PUT /api/cars
  Authorization: Bearer jwt-token-from-authenticate-dealer
  Content-Type: application/json

  {
    "carId": 1,
    "newStockLevel": 99
  }
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": "Stock level updated successfully.",
    "details": 1
  }
  ```

  ```json
  // HTTP 401 Unauthorized
  {
    "message": "Not authorised to update stock level of car.",
    "details": 1
  }
  ```

- **Remove car**:
  ```http
  DELETE /api/cars/1
  Authorization: Bearer jwt-token-from-authenticate-dealer
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": "Car removed successfully.",
    "details": 1
  }
  ```

  ```json
  // HTTP 401 Unauthorized
  {
    "message": "Not authorised to remove car.",
    "details": 1
  }

- **Search car by make**:

  Leaving `model` field `null` or as an empty string is equivalent to searching for **any model**.
  ```http
  GET /api/cars/search
  Authorization: Bearer jwt-token-from-authenticate-dealer
  Content-Type: application/json

  {
    "make": "Toyota"
  }
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": null,
    "carResponses": [
        {
            "id": 1,
            "make": "Toyota",
            "model": "Corolla",
            "year": 2020,
            "stockLevel": 10
        }
    ]
  }
  ```

- **Search car by model**:

  Leaving `make` field `null` or as an empty string is equivalent to searching for **any make**.
  ```http
  GET /api/cars/search
  Authorization: Bearer jwt-token-from-authenticate-dealer
  Content-Type: application/json

  {
    "model": "Corolla"
  }
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": null,
    "carResponses": [
        {
            "id": 1,
            "make": "Toyota",
            "model": "Corolla",
            "year": 2020,
            "stockLevel": 10
        }
    ]
  }
  ```

- **Search car by make and model**:
  ```http
  GET /api/cars/search
  Authorization: Bearer jwt-token-from-authenticate-dealer
  Content-Type: application/json

  {
    "make": "Toyota",
    "model": "Corolla"
  }
  ```

  Example Response(s):
  ```json
  // HTTP 200 OK
  {
    "message": null,
    "carResponses": [
        {
            "id": 1,
            "make": "Toyota",
            "model": "Corolla",
            "year": 2020,
            "stockLevel": 10
        }
    ]
  }
  ```


---

## License

This project is licensed under the [MIT License](./LICENSE.txt).