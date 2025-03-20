# DeliveryFeeApi - Test assignment for Fujitsu!

## Goal:
### Create an API that calculates the delivery fee based on the base fee, vehicle type, and weather conditions.

## Database: 
#### 1. I decided to use an SQLite database for this project because of its simplicity and ease of use in this scenario.
#### 2. I decided to seed all existing fee data into the database. Now, I can easily retrieve this data when needed and update the fee rates.
### Here is how my data models is structured:
![DataStructureEd](https://github.com/user-attachments/assets/81261318-4e38-43d0-b615-bb7fea8b60d0)


## Scheduled Task:
#### 1. I used Quartz to create a recurring job that imports weather data for Tallinn, Tartu, and Pärnu every hour. The decision was made not to tie it to specific minutes of the hour but to start running it when the API is launched and then execute it every hour thereafter.

## Testing:
#### 1. For testing, I chose the N/XUnit library because I had experience writing unit tests with it.
#### 2. For code coverage, I used the reportgenerator-globaltool. With a small trick using a PowerShell script, I converted coverage.cobertura.xml into coverage.cobertura.html. This trick helps you see all branches and lines, and it also provides a "crap score" for your code, which can help identify areas that need improvement.
![Screenshot 2025-03-19 174741](https://github.com/user-attachments/assets/12eb10d5-4d2b-476f-a873-04166b4ef65f)

## Endpoints:
### 1. The first and main endpoint provides the delivery price.
#### Endpoint URL: {host}/api/DeliveryPrice/calculate
#### For local testing use: https://localhost:7031/api/DeliveryPrice/calculate
##### This is a GET method with two request parameters: station(Tallinn, Tartu and Pärnu), vehicle(Car, Scooter and Bike) - Both parameters are of type string.
![Screenshot 2025-03-20 111520](https://github.com/user-attachments/assets/92352976-ad7b-407e-bb69-38545cb0938a)
#### Posible responses: 
	
##### Response body (OK - Status 200): Station Tartu, Vehicle Bike, and good weather
```json
{
  "total": 2.5,
  "forbitten": false,
  "errorMessage": null,
}
```

##### Response body (OK - Status 200): Station Tartu, Vehicle Bike, and wind speed exceeds 20 m/s
```json
{
  "total": null,
  "forbitten": true,
  "errorMessage": "Usage of selected vehicle type is forbidden",
}
```

##### Bad Request (400): If the request parameters contain invalid values (i.e., anything other than Tallinn, Tartu, Pärnu for station or Car, Scooter, Bike for vehicle).
![Screenshot 2025-03-20 113909](https://github.com/user-attachments/assets/0f8dc663-d919-471e-b7d3-85b1355dc3f5)

##### Not Found (404): If the weather data is missing from the database, we cannot calculate the fee because it is based on that data.
![Screenshot 2025-03-20 130920](https://github.com/user-attachments/assets/974540a2-84b1-48a5-9e65-8d408bf2f937)


