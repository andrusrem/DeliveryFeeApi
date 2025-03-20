# DeliveryFeeApi - Test assignment for Fujitsu!

**Navigation**
- [Goal](#goal)
- [Database](#database)
- [Scheduled Task](#scheduled-task)
- [Testing](#testing)
- [Endpoints](#endpoints)

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
![Screenshot 2025-03-19 174741](https://github.com/user-attachments/assets/12eb10d5-4d2b-476f-a873-04166b4ef65f)

## Endpoints:
### 1. The first and main endpoint provides the delivery price.
This is a **GET** method with two request parameters: **station**(Tallinn, Tartu and Pärnu), **vehicle**(Car, Scooter and Bike) - Both parameters are of type **string**. <br>
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


