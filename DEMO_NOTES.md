# Live Demo Notes

## 1\. Problem

Vehicle owners can forget service dates and lose maintenance history. This app keeps vehicle, booking, service history, and cost information in one place.

## 2\. Features to Show

1. Add a vehicle.
2. Search for a vehicle by registration or owner name.
3. Create a service booking.
4. Add a completed maintenance record.
5. View the service history report.teh
6. View the cost summary report.
7. Show upcoming service reminder count on the main screen.

## 3\. OOP Code to Explain

* `Person` and `Owner`: inheritance.
* `Vehicle` and `Car`: abstract class and inheritance.
* `GetDisplayName()` override in `Car`: polymorphism.
* Private fields and properties in `Vehicle` and `MaintenanceRecord`: encapsulation.
* `IServiceReport`, `ServiceHistoryReport`, and `CostSummaryReport`: abstraction and polymorphism.
* `try-catch` blocks in forms and `ServiceManager`: exception handling.

## 4\. GitHub Commit Ideas

Do not fake old commits. Commit your real work as you do it. Possible commit messages:

* Add project structure and main form
* Add vehicle and owner model classes
* Add service booking model
* Add maintenance record model
* Add service manager file storage
* Add vehicle form validation
* Add booking form
* Add maintenance record form
* Add reporting interface and report classes
* Update README and demo notes

