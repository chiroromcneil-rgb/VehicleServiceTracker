# Vehicle Service Booking and Maintenance Tracker

## Project Description

This is a C# Windows Forms application for ITS203 Object-Oriented Design and Programming. The system helps vehicle owners keep track of vehicles, service bookings, completed maintenance records, upcoming reminders, and total maintenance costs.

## Main Features

* Add, view, search, update, and delete vehicle records.
* Store owner details for each vehicle.
* Create service bookings with booking date, service type, workshop name, and status.
* Add completed maintenance records with date, description, and cost.
* View upcoming service reminders.
* Generate service history and cost summary reports.
* Save data using CSV file-based storage inside the application Data folder.
* Validate user input and show clear error messages using the exception handling.

## OOP Principles Used

* **Classes and Objects:** Person, Owner, Vehicle, Car, ServiceBooking, MaintenanceRecord, ServiceManager.
* **Encapsulation:** Private fields and public properties are used to control data access.
* **Inheritance:** Owner inherits from Person. Car inherits from Vehicle.
* **Polymorphism:** GetDisplayName() and GetContactSummary() are overridden. IServiceReport is used by different report classes.
* **Abstraction:** Vehicle is an abstract class and IServiceReport is an interface.
* **Exception Handling:** Try-catch blocks are used in forms and file loading/saving logic.

## How to Run the Application

1. Open Visual Studio.
2. Select **Open a project or solution**.
3. Open `VehicleServiceTracker.sln`.
4. Build the project.
5. Press **Start** to run the Windows Forms application.

## Project Structure

* `Models` contains the main entity classes.
* `Services` contains the business logic, reports, and data storage manager.
* `Forms` contains the Windows Forms screens.
* `docs` contains the milestone idea proposal document.

## Data Storage

The application uses CSV files. When the program runs, it creates a `Data` folder inside the build output folder. Records are saved into:

* `vehicles.csv`
* `bookings.csv`
* `maintenance.csv`

## References and Tools Used

* Microsoft Learn C# documentation.
* Microsoft Learn Windows Forms documentation.
* ## OOP Concepts Used
The project uses classes, objects, inheritance, encapsulation, abstraction, polymorphism, and exception handling.

## References and Tools Used
- Microsoft Visual Studio
- C# Windows Forms
- ChatGPT for study support and debugging guidance