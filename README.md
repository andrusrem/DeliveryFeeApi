# DeliveryFeeApi - Test assignment for Fujitsu!

**Navigation**
- [Goal](#goal)
- [Database](#database)
- [Scheduled Task](#scheduled-task)
- [Testing](#testing)
- [Endpoints](#endpoints)
  - [1. The first and main endpoint provides the delivery price](#1-the-first-and-main-endpoint-provides-the-delivery-price)
  - [Enums used in next endpoints](#enums-used-in-next-endpoints)
  - [2. CRUD Endpoints for Regional Base Fee Management](#2-crud-endpoints-for-regional-base-fee-management)
  - [3. CRUD Endpoints for Air Temperature Extra Fee Management](#3-crud-endpoints-for-air-temperature-extra-fee-management)
  - [4. CRUD Endpoints for Wind Speed Extra Fee Management](#4-crud-endpoints-for-wind-speed-extra-fee-management)
  - [5. CRUD Endpoints for Weather Phenomenon Extra Fee Management](#5-crud-endpoints-for-weather-phenomenon-extra-fee-management)

## Goal:
Create an **API** that calculates the delivery fee based on the base fee, vehicle type, and weather conditions.

## Database: 
1. **SQLite** was chosen for this project due to its simplicity and ease of use, making it an ideal fit for the current requirements.
2. All existing fee data has been **seeded** into the database. This allows for easy retrieval and updating of fee rates as needed.
#### Data Model Structure
The data model is visually represented below: <br>
![Screenshot 2025-03-20 160559](https://github.com/user-attachments/assets/9bede67f-0625-4bdd-a240-8968c20db9cc)



## Scheduled Task:
1. **Quartz** was utilized to create a recurring job that imports weather data for **Tallinn**, **Tartu**, and **Pärnu** every hour. Instead of binding the task to specific minutes, it was set to start when the API is launched and then execute every hour thereafter.

## Testing:
1. **NUnit/XUnit** was selected for unit testing due to prior experience with these frameworks.
2. For code coverage, **reportgenerator-globaltool** was employed. Using a **PowerShell script**, I converted coverage.cobertura.**xml** into coverage.cobertura.**html**, allowing for a detailed view of all branches and lines. This method also provides a "crap score" to identify areas of the code that may need further improvement.
![Screenshot 2025-03-22 204502](https://github.com/user-attachments/assets/fc2a05c9-c694-443e-ac11-420a6858e7d1)


## Endpoints:
## 1. The first and main endpoint provides the delivery price.
This is a **GET** method with two query parameters: **station**(Tallinn, Tartu and Pärnu), **vehicle**(Car, Scooter and Bike) - Both parameters are of type **string**. <br>
* **Endpoint URL:** {host}/api/DeliveryPrice/calculate <br>
* **For local testing use:** https://localhost:7031/api/DeliveryPrice/calculate <br>
![Screenshot 2025-03-20 111520](https://github.com/user-attachments/assets/92352976-ad7b-407e-bb69-38545cb0938a)
#### Posible responses: 
	
 1. **Response body (OK - Status 200):** Station Tartu, Vehicle Bike, and good weather
```json
{
  "total": 2.5,
  "forbitten": false,
  "errorMessage": null,
}
```

 2. **Response body (OK - Status 200):** Station Tartu, Vehicle Bike, and wind speed exceeds 20 m/s
```json
{
  "total": null,
  "forbitten": true,
  "errorMessage": "Usage of selected vehicle type is forbidden",
}
```

 3. **Bad Request (400):** If the request parameters contain invalid values (i.e., anything other than Tallinn, Tartu, Pärnu for station or Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-20 113909](https://github.com/user-attachments/assets/0f8dc663-d919-471e-b7d3-85b1355dc3f5)

 4. **Not Found (404):** If the weather data is missing from the database, we cannot calculate the fee because it is based on that data.
![Screenshot 2025-03-20 130920](https://github.com/user-attachments/assets/974540a2-84b1-48a5-9e65-8d408bf2f937)

## Enums used in next endpoints:
**Stations Enum** explain:
```json
{
  "Tallinn": 0,
  "Tartu": 1,
  "Pärnu": 2,
}
```
**Vehicle Enum** explain:
```json
{
  "Car": 0,
  "Scooter": 1,
  "Bike": 2,
}
```

## 2. CRUD Endpoints for Regional Base Fee management:
**1. Get method**, that return list of all base fees (**No query params need**)<br>
* **Endpoint URL:** {host}/api/RegionalBaseFee/all <br>
* **For local testing use:** https://localhost:7031/api/RegionalBaseFee/all <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Always return list
```json
[
  {
    "id": 0,
    "vehicleType": 0,
    "stationName": 1,
    "price": 3.5,
  },
]
```
**2. Put method**, that return updated fee or error, request params needed: **station**(Tallinn, Tartu and Pärnu), **vehicle**(Car, Scooter and Bike) and **price** double <br>
* **Endpoint URL:** {host}/api/RegionalBaseFee/update <br>
* **For local testing use:** https://localhost:7031/api/RegionalBaseFee/update <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Return updated fee
```json
{
    "id": 0,
    "vehicleType": 0,
    "stationName": 1,
    "price": 3.5,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other than Tallinn, Tartu, Pärnu for station or Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 102052](https://github.com/user-attachments/assets/81c77aba-3d9d-4d06-a514-eac94ac1a942)


**Not Found (404):** If fee is missing from the database, we cannot update the fee.
![Screenshot 2025-03-23 104302](https://github.com/user-attachments/assets/723decbc-5b81-40a7-b15f-1161d86d74cc)



**3. Post method**, that return created fee or error, request params needed: **station**(Tallinn, Tartu and Pärnu), **vehicle**(Car, Scooter and Bike) and **price** double <br>
* **Endpoint URL:** {host}/api/RegionalBaseFee/create <br>
* **For local testing use:** https://localhost:7031/api/RegionalBaseFee/create <br>
#### Posible responses: 
**Response Body (CreatedAtAction - Status 201):** Return created fee
```json
{
    "id": 1,
    "vehicleType": 1,
    "stationName": 1,
    "price": 3,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other than Tallinn, Tartu, Pärnu for station or Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 102650](https://github.com/user-attachments/assets/12b5016e-d5ce-423a-b904-49a6ce4617de)

**Bad Request (400):** If fee is already exists.
![Screenshot 2025-03-23 102722](https://github.com/user-attachments/assets/760ab5ca-a1f0-434d-8ed7-66a98d749d14)

**4. Delete method**, that return no content if fee was deleted or error, request params needed: **id** int <br>
* **Endpoint URL:** {host}/api/RegionalBaseFee/delete <br>
* **For local testing use:** https://localhost:7031/api/RegionalBaseFee/delete <br>
#### Posible responses: 
**No Contents (204):** If fee was deleted<br>
**Not Found (404):** If fee not found in database.
![Screenshot 2025-03-23 103507](https://github.com/user-attachments/assets/96c5182c-ef2f-4201-83a5-3bac6a13d2d1)

## 3. CRUD Endpoints for Air Temperature Extra Fee management:
**1. Get method**, that return list of all air temperature fees (**No query params need**)<br>
* **Endpoint URL:** {host}/api/AirTemperatureExtraFee/all <br>
* **For local testing use:** https://localhost:7031/api/AirTemperatureExtraFee/all <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Always return list
```json
[
  {
    "id": 0,
    "lowerTemperature": -10,
    "upperTemperature": 0,
    "vehicleType": 1,
    "price": 0.5,
  },
]
```
**2. Put method**, that return updated fee or error, request params needed: **vehicle**(Car, Scooter and Bike), **lower temperature** double, **upper temperature** double and **price** double <br>
* **Endpoint URL:** {host}/api/AirTemperatureExtraFee/update <br>
* **For local testing use:** https://localhost:7031/api/AirTemperatureExtraFee/update <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Return updated fee
```json
{
    "id": 0,
    "lowerTemperature": -10,
    "upperTemperature": 0,
    "vehicleType": 1,
    "price": 1,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 104359](https://github.com/user-attachments/assets/ceb96344-b956-4e65-a394-d57bf7aa0027)


**Not Found (404):** If fee is missing from the database, we cannot update the fee.
![Screenshot 2025-03-23 104302](https://github.com/user-attachments/assets/b2c94d73-3c56-4f2c-8623-b712b9972405)


**3. Post method**, that return created fee or error, request params needed:**vehicle**(Car, Scooter and Bike), **lower temperature** double, **upper temperature** double and **price** double <br>
* **Endpoint URL:** {host}/api/AirTemperatureExtraFee/create <br>
* **For local testing use:** https://localhost:7031/api/AirTemperatureExtraFee/create <br>
#### Posible responses: 
**Response Body (CreatedAtAction - Status 201):** Return created fee
```json
{
    "id": 3,
    "lowerTemperature": -10,
    "upperTemperature": 10,
    "vehicleType": 2,
    "price": 0.5,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other than Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 104440](https://github.com/user-attachments/assets/6df765c5-be3f-49a4-8464-563ab9ebf115)


**Bad Request (400):** If fee is already exists.
![Screenshot 2025-03-23 104504](https://github.com/user-attachments/assets/24c2c2be-43b2-4797-ab69-23c88a5b4cdd)


**4. Delete method**, that return no content if fee was deleted or error, request params needed: **id** int <br>
* **Endpoint URL:** {host}/api/AirTemperatureExtraFee/delete <br>
* **For local testing use:** https://localhost:7031/api/AirTemperatureExtraFee/delete <br>
#### Posible responses: 
**No Contents (204):** If fee was deleted<br>
**Not Found (404):** If fee not found in database.
![Screenshot 2025-03-23 103507](https://github.com/user-attachments/assets/96c5182c-ef2f-4201-83a5-3bac6a13d2d1)

## 4. CRUD Endpoints for Wind Speed Extra Fee management:
**1. Get method**, that return list of all wind speed fees (**No query params need**)<br>
* **Endpoint URL:** {host}/api/WindSpeedExtraFee/all <br>
* **For local testing use:** https://localhost:7031/api/WindSpeedExtraFee/all <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Always return list
```json
[
  {
    "id": 0,
    "lowerSpeed": 10,
    "upperSpeed": 20,
    "vehicleType": 1,
    "price": 0.5,
    "forbitten": false,
  },
]
```
**2. Put method**, that return updated fee or error, request params needed: **vehicle**(Car, Scooter and Bike), **lower speed** double, **upper speed** double, **price** double(nullable) and **forbitten** boolean(nullable) <br>
* **Endpoint URL:** {host}/api/WindSpeedExtraFee/update <br>
* **For local testing use:** https://localhost:7031/api/WindSpeedExtraFee/update <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Return updated fee
```json
{
    "id": 0,
    "lowerSpeed": 10,
    "upperSpeed": 20,
    "vehicleType": 1,
    "price": 1,
    "forbitten": false,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 104359](https://github.com/user-attachments/assets/ceb96344-b956-4e65-a394-d57bf7aa0027)


**Not Found (404):** If fee is missing from the database, we cannot update the fee.
![Screenshot 2025-03-23 104302](https://github.com/user-attachments/assets/b2c94d73-3c56-4f2c-8623-b712b9972405)


**3. Post method**, that return created fee or error, request params needed:**vehicle**(Car, Scooter and Bike), **lower speed** double, **upper speed** double, **price** double(nullable) and **forbitten** boolean(nullable) <br>
* **Endpoint URL:** {host}/api/WindSpeedExtraFee/create <br>
* **For local testing use:** https://localhost:7031/api/WindSpeedExtraFee/create <br>
#### Posible responses: 
**Response Body (CreatedAtAction - Status 201):** Return created fee
```json
{
    "id": 0,
    "lowerSpeed": 10,
    "upperSpeed": 20,
    "vehicleType": 1,
    "price": 1,
    "forbitten": false,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other than Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 104440](https://github.com/user-attachments/assets/6df765c5-be3f-49a4-8464-563ab9ebf115)


**Bad Request (400):** If fee is already exists.
![Screenshot 2025-03-23 104504](https://github.com/user-attachments/assets/24c2c2be-43b2-4797-ab69-23c88a5b4cdd)


**4. Delete method**, that return no content if fee was deleted or error, request params needed: **id** int <br>
* **Endpoint URL:** {host}/api/WindSpeedExtraFee/delete <br>
* **For local testing use:** https://localhost:7031/api/WindSpeedExtraFee/delete <br>
#### Posible responses: 
**No Contents (204):** If fee was deleted<br>
**Not Found (404):** If fee not found in database.
![Screenshot 2025-03-23 103507](https://github.com/user-attachments/assets/96c5182c-ef2f-4201-83a5-3bac6a13d2d1)

## 5. CRUD Endpoints for Weather Phenomenon Extra Fee management:
**1. Get method**, that return list of all phenomenon fees (**No query params need**)<br>
* **Endpoint URL:** {host}/api/WeatherPhenomenonExtraFee/all <br>
* **For local testing use:** https://localhost:7031/api/WeatherPhenomenonExtraFee/all <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Always return list
```json
[
  {
    "id": 0,
    "phenomenon": "Light rain",
    "vehicleType": 1,
    "price": 0.5,
    "forbitten": false,
  },
]
```
**2. Put method**, that return updated fee or error, request params needed: **vehicle**(Car, Scooter and Bike), **phenomenon** string, **price** double(nullable) and **forbitten** boolean(nullable) <br>
* **Endpoint URL:** {host}/api/WeatherPhenomenonExtraFee/update <br>
* **For local testing use:** https://localhost:7031/api/WeatherPhenomenonExtraFee/update <br>
#### Posible responses: 
**Response Body (OK - Status 200):** Return updated fee
```json
{
    "id": 0,
    "phenomenon": "Light rain",
    "vehicleType": 1,
    "price": 1,
    "forbitten": false,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 104359](https://github.com/user-attachments/assets/ceb96344-b956-4e65-a394-d57bf7aa0027)


**Not Found (404):** If fee is missing from the database, we cannot update the fee.
![Screenshot 2025-03-23 104302](https://github.com/user-attachments/assets/b2c94d73-3c56-4f2c-8623-b712b9972405)


**3. Post method**, that return created fee or error, request params needed:**vehicle**(Car, Scooter and Bike), **phenomenon** string, **price** double(nullable) and **forbitten** boolean(nullable) <br>
* **Endpoint URL:** {host}/api/WeatherPhenomenonExtraFee/create <br>
* **For local testing use:** https://localhost:7031/api/WeatherPhenomenonExtraFee/create <br>
#### Posible responses: 
**Response Body (CreatedAtAction - Status 201):** Return created fee
```json
{
    "id": 1,
    "phenomenon": "Light rain",
    "vehicleType": 2,
    "price": 1,
    "forbitten": false,
}
```
**Bad Request (400):** If the request parameters contain invalid values (i.e., anything other than Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-23 104440](https://github.com/user-attachments/assets/6df765c5-be3f-49a4-8464-563ab9ebf115)


**Bad Request (400):** If fee is already exists.
![Screenshot 2025-03-23 104504](https://github.com/user-attachments/assets/24c2c2be-43b2-4797-ab69-23c88a5b4cdd)


**4. Delete method**, that return no content if fee was deleted or error, request params needed: **id** int <br>
* **Endpoint URL:** {host}/api/WeatherPhenomenonExtraFee/delete <br>
* **For local testing use:** https://localhost:7031/api/WeatherPhenomenonExtraFee/delete <br>
#### Posible responses: 
**No Contents (204):** If fee was deleted<br>
**Not Found (404):** If fee not found in database.
![Screenshot 2025-03-23 103507](https://github.com/user-attachments/assets/96c5182c-ef2f-4201-83a5-3bac6a13d2d1)

