DESIGN AND THOUGHS

Database Design
I chose a single table design for the casino wagers. This approach simplifies my database schema and ensures that all related data is stored in one place, making it easier to manage and query.

ORM Choice
I opted for Entity Framework Core because it provides a robust and user-friendly ORM for managing database operations, making my life easier when handling data.

Design Principles
Separation of Concerns: I divided the application into distinct layers, each with a specific responsibility. This makes my code easier to manage and understand.

Dependency Injection: Instead of hardcoding dependencies, I injected them into classes, making my application more flexible, easier to test, and maintainable.

Single Responsibility Principle: I ensured that each class and method has a single responsibility, which keeps my codebase clean and focused.

Overall Approach
By adhering to SOLID principles, I've made my application modular, easy to maintain, and scalable. This way, my code can grow and adapt without becoming a tangled mess.

Challenges Experienced


Message Handling and Reliability
Ensuring that messages are reliably published to RabbitMQ and consumed by the service without loss was quite a challenge. It's crucial that every message is correctly delivered and processed

Database Performance
Initially, managing database performance, especially under heavy loads, was problematic. Ensuring that the database could handle multiple simultaneous requests without slowing down required optimizing queries, indexing, and occasionally revisiting the database design. 
these are some of the challenges i faced when creating the app