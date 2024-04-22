Project features
- the app is a console-based application that manages a list of contacts and allows users to add, edit, delete, and list contacts
- it has 2 hardcoded example contacts for a demonstration purposes
- pagination for fetching records form DB is not implemented as is not required so logic is built on assumption that all existing records are on UI;
    should be done in the real world
- log messages for possible errors are put on the console for simplicity; should be put in log file in the real world app;
- the value for sorting by specific field is persisted and used for upcoming queries; by default records are sorted by first name
- values for "search by" logic are not persisted and are not used for upcoming queries

Description of objects
Program.cs
- app entry point, 
- registers app services and runs UiService 

UiService.cs
- presents console base UI for user input/output operations
- implements sync message loop
- has validation service to output user input validation errors
- has logger service tuned to output app error messages to console (in real world it would create a log file)

ContactService.cs
- used for CRUD operations between UI and DB 
- has DB simulator as a DB analogue
- has a mapper to convert "contact" object between ui and DB formats

ValidationService.cs
- validates correctness of email, phone number and string fields
- returns result and error message if validation failed

FileService.cs
- exports contacts to a CSV file
- file location is defined by user, file name consist of time stamp following by -contacts prefix
- example : 20240422121643-contacts.csv

Uniit tests
To ensure the code correctness and stability for upcoming changes unit tests are written for all classes except UiService.
Since console UI primarily involve reading input from the console and writing output to the console,
the focus of unit testing is on the business logic and functionality that drives these interactions, that's why UiService is not unit tested.
