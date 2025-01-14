```markdown
# WPF Zoo Project

The WPF Zoo Project is a Windows Presentation Foundation (WPF) application designed to manage zoo and animal data. The application allows users to perform CRUD (Create, Read, Update, Delete) operations on zoo and animal records stored in a SQL database.

## Features

- **View Zoos**: Display a list of all zoos.
- **View Animals**: Display a list of all animals.
- **View Animals in Selected Zoo**: Display animals associated with a selected zoo.
- **Add Zoo**: Add a new zoo to the database.
- **Add Animal**: Add a new animal to the database.
- **Add Animal to Zoo**: Associate an animal with a zoo.
- **Update Zoo**: Update the location of a selected zoo.
- **Update Animal**: Update the name of a selected animal.
- **Delete Zoo**: Remove a zoo from the database.
- **Delete Animal**: Remove an animal from the database.
- **Remove Animal from Zoo**: Disassociate an animal from a zoo.

## How to Use

### Viewing Data
- **Zoos**: The list of zoos is displayed in a list box. Select a zoo to view its details.
- **Animals**: The list of all animals is displayed in a list box. Select an animal to view its details.
- **Animals in Selected Zoo**: Select a zoo to view the animals associated with it.

### Adding Data
- **Add Zoo**: Enter the location in the input box and click the "Add Zoo" button.
- **Add Animal**: Enter the name in the input box and click the "Add Animal" button.
- **Add Animal to Zoo**: Select a zoo and an animal, then click the "Add Animal to Zoo" button.

### Updating Data
- **Update Zoo**: Select a zoo, enter the new location in the input box, and click the "Update Zoo" button.
- **Update Animal**: Select an animal, enter the new name in the input box, and click the "Update Animal" button.

### Deleting Data
- **Delete Zoo**: Select a zoo and click the "Delete Zoo" button.
- **Delete Animal**: Select an animal and click the "Delete Animal" button.
- **Remove Animal from Zoo**: Select a zoo and an associated animal, then click the "Remove Animal" button.

## Error Handling

The application includes basic error handling to display error messages in case of exceptions during database operations.

## Requirements

- .NET Framework 
- SQL Server with a database containing `Zoo`, `Animal`, and `ZooAnimal` tables.
```
