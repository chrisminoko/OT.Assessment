## Project Structure :

I've organized the  project using the Repository pattern and separated functionalities into class libraries:

- **OT.Assessment.App**: This is the main API project.
- **OT.Assessment.Consumer**: Manages published messages and sends them to the service for processing.
- **OT.Assessment.Migrations**: Handles database migrations and entity configurations; this is where the context class is located.
- **OT.Assessment.Model**: Stores all entities, DTOs, request models, and response classes.
- **OT.Assessment.Core**: Contains enums, extension methods, helper classes, and static response classes.
- **OT.Assessment.Repository**: Includes all generic repository classes and the unit of work.
- **OT.Assessment.Services**: Contains business logic and the producer/publisher, which I considered separating but decided against to avoid an overkill.
- docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3-management please run that command to set up rabbitmq


## Thoughts Regarding this request

### POST api/player/casinowager
Receives player casino wager events to publish to the local RabbitMQ queue.

```json
{
  "wagerId": "aa6700eb-1a06-483e-9739-d293dc7a9383",
  "theme": "adventure",
  "provider": "Ergonomic Soft Fish",
  "gameName": "Ergonomic Granite Cheese",
  "transactionId": "410b7161-3473-4d74-85c3-a533d050a9d3",
  "brandId": "8a2016f8-c4c4-471f-9a9c-337a54664650",
  "accountId": "5ac75fec-23e9-27d1-b660-179eee70003d",
  "Username": "Jay.Bernhard67",
  "externalReferenceId": "0267dbca-2760-4a9e-ab42-5ce766fa8ca0",
  "transactionTypeId": "8aaece0c-5d53-4225-a937-adb454c4da31",
  "amount": 38273.974454660885,
  "createdDateTime": "2024-05-04T02:25:05.9906387+02:00",
  "numberOfBets": 3,
  "countryCode": "BS",
  "sessionData": "Central Chile Awesome Cotton Gloves cross-platform Handmade Rubber Shoes portals leading-edge Coordinator Data Producer end-to-end encoding Gorgeous Clothing View Health, Outdoors & Music embrace Metrics Facilitator morph",
  "duration": 1827254
}

```
I was very concerned about this part: "receives player casino wager events to publish to the local RabbitMQ queue." Should I just publish and move on? How can I notify the requester about the status of their request?
It seems like my best option is to "publish and forget," since I don't want the requester to have to wait for RabbitMQ to finish processing.
The lest i did was to just check if the publishing to the queue was successful and return an appropriate response. However, this might be misleading, as the publish can succeed, but issues could still arise later, such as validation errors or unexpected problems during SQL operations.


## RabbitMQ Structure :

I watched a lot of videos on how to do this here, I did not really know some of the best practices on how to implement it. I watched this here : https://www.youtube.com/watch?v=bfVddTJNiAw&t=2827s ,https://www.youtube.com/watch?v=LHYOBHR2Ms4&t=629s,
They really helped set up the foundation but none of them did it in way that is dynamic to consume different topics: so I did some googling and with some of my experience with Kafka , I tried implemnting a similar pattern am used too:
I would have wanted to add retrial but I want to finish before Monday - My upcomming week will be busy.



## General overview Structure :

I made a lot of changes, we can discuss if am sucessful , There are design choice I disagree with myself but it works . And a