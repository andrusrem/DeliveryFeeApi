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



