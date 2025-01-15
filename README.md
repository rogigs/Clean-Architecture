
# ProjectTrack

The project was developed with the aim of studying and applying the concepts of Clean Architecture using .NET Core. The goal is to structure an application in a modular and decoupled way, promoting the separation of responsibilities and code maintainability. The project also includes the implementation of microservices to further enhance scalability and maintainability.

Additionally, the application performs CRUD operations in a scenario where project monitoring is required, with responsibilities for tracking project progress and the users involved.

## Documentation
 
[Wiki](https://github.com/rogigs/Clean-Architecture/wiki) - Hire have more about patterns, performance, system design , decisions and queues.


## Screenshots

![services-endpoints](https://github.com/user-attachments/assets/4511eb4c-e9e0-4493-ad18-65f64700ce68)


# Environment Variables

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=your_db;User=your_user;Password=your_password"
  }
}
```

## Lessons Learned

- [Introduction](#introduction)
- [Fundamental Principles](#fundamental-principles)
- [Lessons Learned](#lessons-learned)
  - [1. Separation of Concerns](#1-separation-of-concerns)
  - [2. Framework Independence](#2-framework-independence)
  - [3. Testability](#3-testability)
  - [4. Decoupling and Interfaces](#4-decoupling-and-interfaces)
  - [5. Data Flow and Dependencies](#5-data-flow-and-dependencies)
- [Conclusion](#conclusion)

## Introduction

Clean Architecture, proposed by Robert C. Martin (also known as Uncle Bob), aims to create systems that are independent of external details such as databases and frameworks. This facilitates changing parts of the system without impacting others, promoting a cleaner and more organized code structure.

![clean-arch](https://github.com/user-attachments/assets/7a11a476-8ee5-4045-8835-3a73c10d046d)

## Fundamental Principles

1. **Dependency Inversion**: Dependencies should always point to abstractions and never to concrete implementations.
2. **Layers**: The system should be divided into layers, each with a specific responsibility.
3. **Testability**: Each component should be independently testable.

## Lessons Learned

### 1. Separation of Concerns

A clear separation between the presentation, application, domain, and infrastructure layers facilitates maintenance and evolution of the system. Each layer can be modified or replaced without impacting the others, making the system more modular.

### 2. Framework Independence

By avoiding direct dependencies on frameworks, we can easily swap out the underlying technology without affecting the rest of the system. This is especially useful in an environment where technologies are constantly changing.

### 3. Testability

Clean Architecture allows for more effective automated testing. Each layer can be tested in isolation, resulting in a more robust and reliable test coverage. Using mocks and stubs becomes simpler when dependencies are injected.

### 4. Decoupling and Interfaces

Defining interfaces for dependencies helps decouple different parts of the system. This not only promotes cleaner code but also facilitates the implementation of new features or the replacement of existing components.

### 5. Data Flow and Dependencies

Maintaining a clear flow of data between layers is essential. Communication should occur in a way that dependencies flow from the outside in, following the principle of dependency inversion. This helps to avoid coupling issues.

## References
[Live Coding: Clean architecture na prática com Rodrigo Branas](https://www.youtube.com/watch?v=s3QsigPsXKI&t=7711s&pp=ygUZY2xlYW4gY29kZSByb2RyaWdvIGJyYW5hcw%3D%3D)

[Clean Architecture: descubra o que é e onde aplicar Arquitetura Limpa](https://zup.com.br/blog/clean-architecture-arquitetura-limpa)





