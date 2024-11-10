## Project Structure :

I've organized the  project using the Repository pattern and separated functionalities into class libraries:

- **OT.Assessment.App**: This is the main API project.
- **OT.Assessment.Consumer**: Manages published messages and sends them to the service for processing.
- **OT.Assessment.Migrations**: Handles database migrations and entity configurations; this is where the context class is located.
- **OT.Assessment.Model**: Stores all entities, DTOs, request models, and response classes.
- **OT.Assessment.Core**: Contains enums, extension methods, helper classes, and static response classes.
- **OT.Assessment.Repository**: Includes all generic repository classes and the unit of work.
- **OT.Assessment.Services**: Contains business logic and the producer/publisher, which I considered separating but decided against to avoid an overkill.
