# Online Betting Data Capture and Analytics System

## Scenario
In the dynamic world of online betting, capturing and analyzing data is paramount for business success. Every time a player initiates a spin in a casino game, it's essential to capture and store relevant data. This project aims to develop an API and service capable of receiving, storing, and retrieving player casino data.

## Pre-Requisites
- **RabbitMQ:** Setup a local RabbitMQ instance running on `localhost` with credentials `guest/guest`.
    > [Run RabbitMQ Windows Service](https://www.rabbitmq.com/docs/install-windows#installer) or
 
    > [Docker Compose Rabbit Image](docker/RabbitMQ/docker-up.bat)
- **SQL Server:** Setup a local SQL Server 2022 Developer Edition with the connection string: `"SERVER=localhost; DATABASE=OT_Assessment_DB; Integrated Security=SSPI;"`.

## Requirements Tasks
### 1. .NET 8 API 
The API (OT.Assessment.App) must at least include the following endpoints:
- **POST** `api/player/casinowager`: Receives player casino wager events to publish to the local RabbitMQ queue.
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
- **GET** `api/player/{playerId}/casino?pageSize=10&page=1`: Returns a paginated list of the latest casino wagers for a specific player.
    ```json
    {
      "data": [
        {
          "wagerId": "aa6700eb-1a06-483e-9739-d293dc7a9383",
          "game": "Ergonomic Granite Cheese",
          "provider": "Ergonomic Soft Fish",
          "amount": 500,
          "createdDate": "2024-09-10T07:25:13.577Z"
        },
        {
          "wagerId": "aa6700eb-1a06-483e-9739-d293dc7a9383",
          "game": "Test",
          "provider": "Test",
          "amount": 250,
          "createdDate": "2024-09-10T07:25:13.577Z"
        },
        //etc
      ],
      "page": 1,
      "pageSize": 10,
      "total": 35,
      "totalPages": 4
    }
    ```
- **GET** `api/player/topSpenders?count=10`: Returns the top {count} players based on total spending. Highest to Lowest.
    ```json
    [
      {
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "username": "testUser1",
        "totalAmountSpend": 150
      },
      {
        "accountId": "aa6700eb-5717-4562-b3fc-2c963f66bev7",
        "username": "testUser21",
        "totalAmountSpend": 100
      },
      //etc
    ]
    ```

### 2. .NET Service
This service (OT.Assessment.Consumer) needs to consume messages published to the aforementioned queue and store consumed messages in the SQL database.

### 3. SQL Database
Design tables, indexes, keys, stored procedures, etc. necessary for populating and retrieving the data as required above. Ensure that the data is stored using best practices and try minimalise duplication. Not all the data is required. Use your descretion for storing and retrieving necessary data to complete the requirements.

**DatabaseGenerate.sql**: Populate the file in the root of the directory with the necessary statements to recreate the database.

### 4. Benchmark and Test
Test your application by running the <b>OT.Assessment.Tester</b>.

1. First, run API Application and the Consumer Service simultaneously.
2. Then, run the Tester Project. 
3. The test application should initiate communication with your api endpoints, populating the queue with relevant data. Concurrently, your consumer application should process incoming messages from the queue and store it within your database.
4. Debug, optimise and ensure your application can handle the load by storing all the messages.
5. Ensure the both GET endpoints return data both, during the load test and after the load test.

### 5. BONUS
Surprise us! Add some things to your project you think are valuable additions.

### 6. Notes
Populate the Notes.md with some of your design choices and thoughts for your code submission. Please also share any challenges experienced or improvements and suggestions for a fully production ready version of solution.

## Scoring Criteria
- Completion of all requirement tasks as specified above.
- Architecture and project structure.
- Naming conventions for variables, projects, tables, and stored procedures.
- Adherence to SOLID principles.
- Readability and maintainability of the code.
- Efficient database design.
- Efficiently consuming all the data from the tester application as quickly as possible.
- Bonus features and additions.
- Proficiency and adherence to best practices for .NET 8 Projects, Background services, RabbitMQ, RESTful API's, Git, and SQL Server.

## FAQ
- Can I modify the suggested solutions, projects, and classes? <b> Yes, but ensure all pre-requisites and tasks are completed as stated above.</b>
- What technologies should I use? <b>Feel free to use any packages you're comfortable with in the .NET ecosystem.</b>
- How do I test my application? <b>Run the tester application to simulate sending casino game data to your API. Ensure both GET routes return the correct data</b>
- How is my application tested and assessed? <b>We'll generate the database from your schema script, start the API and consumer, run the tester application, and evaluate against the scoring criteria</b>
- Need clarification? <b>Compile a list of questions and send them to your OT representative for prompt assistance</b>
- What's next after completing and testing the solution? <b>Ensure your database schema script is ready, commit your code to a public GitHub repository, share the link with your OT representative, and consider enhancements for production readiness</b>

Try to remember and briefly document the decisions you made and why you made them. We may ask you in a follow-up interview.

## Final Note
Use this solution to showcase to us your standards, experience and passion for software development. Ensure that the solution put forward to us will be a real world indication of what we can expect from you if you join our team. Good luck and have fun.
