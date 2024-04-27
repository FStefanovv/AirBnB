A project consisting of a monolith application for managing flights and flight tickets and a microservice implementation of an AirBnB-like application.
Backends of both apps were implemented as .NET Core 5 WEB APIs. Each backend has its' own front-end Angular app for seamless communication with the APIs.
<br>
In order to interact with the APIs, a user needs to be registered and then authenticated/authorized with each request via a JWT that includes claims such as ID, 
email address and role. Unregistered users have a limited access to the APIs and can only preview the flights/accommodations. Apart from JWT authentication, 
the flight managemenet app also provides it's users with API keys that can be used for validating user's identity on the backend. 
<br>
The AirBnB app consists of multiple microservices, with each of them having a single responsibility. Based on the functional and non-functional requirements, 
the app is designed to consist of the following microservices: user accounts, accommodation, reservations, ratings, notifications and last but not least, flight recommendations.
The microservices are accessed via an Ocelot API gateway that performs authentication/authorization and routing to the appropriate microservice endpoint. 
<br>
In scenarios where functionalities exceed the capabilities of the business logic incorporated within a single microservice, the gRPC protocol ensures performant and reliable
communication between the responsible services in order to process the request. <br>
In order to provide consistency accross the system (related data stored in separate databases/containers), a SAGA pattern was employed with accommodation host account 
deletion so that no accommodation can be available without having an active host account. <br>
