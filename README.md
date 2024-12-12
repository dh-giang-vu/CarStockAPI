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

If using Visual Studio 2022, just open the project and click run normally (Ctrl + F5). Otherwise, here is how to use .NET CLI to run this project:

```bash
# Navigate to the root of CarStockAPI directory first, i.e. cd CarStockAPI
dotnet run --launch-profile https
```

The project should launch at `https://localhost:8080/`.

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

Usage here.

---

## License

This project is licensed under the [MIT License](./LICENSE.txt).