## The “HouseRentingSystem” Project

**House Renting System Website** is my defense project for **ASP.NET Core MVC** course at SoftUni Svetlina.

## :pencil2: Overview

**House Renting System** ASP.NET Core MVC App is a Web application for house renting. Houses can be added, read, edited, deleted, rented and left.

* **All users** have access to see the houses and their details.
* **Authorized users**, who are **not agents**, can rent free houses. They can also leave the houses they have rented.
* **Authorized users**, who are **not agents**, can become agents if they have no rented houses.
* **Agents** cannot become ordinary users again.
* **Authorized users**, who are **agents**, can add houses. They can also edit and delete the ones they added.
* **Admin** has a full access to edit and delete all houses. They can also add new houses, rent free ones, and leave those, which they have rented.
* Only the **Admin** can see all rents and all users in the app. Those pages are reloaded on every 5 minutes, because of caching.

## :performing_arts: User Types

**Administrator** - created from site owner – username: “admin@mail.com”, password: “admin123”
* Add, read, edit and delete all houses on the site (the administrator is an agent).
* Rent free houses.
* Leave houses that they have rented.
* See all rents.
* See all users.

**Agent** - logged-in user, who has become an agent through the “Become Agent” functionality and has a phone number
* Read all houses on the site.
* Add houses.
* Edit and delete the houses they created.
* Agents cannot rent and leave houses!

**User** - logged-in user, who is not an agent
* Read all houses on the site.
* Rent houses, which are free.
* Leave houses that they have rented.
* Can become an agent.

## :hammer: Built With
* ASP.NET [CORE 5.0] MVC
	- **3 controllers** (with **1 API controller**) + **5** more in the “Admin” area
	- **4 entity models**
	- **11 views** + **4** more in the “Admin” area + **partial views** where appropriate + **layout** views
	- **Services** (for the whole business logic; used in controllers and views)
	- Service, view and form **models** with validations and restrictions
	- etc.
* ASP.NET Core Default Site Template
* **Microsoft SQL Server**
* **Entity Framework Core**
    - Using migrations
    - Database has seeded data: 3 users, 2 agents, 3 categories and 3 houses
* Using Razor in views
    - Different navigation bar, according to the user
	- Paging, sorting and searching functionalities on the "All Houses" page
* Custom error pages for "400 Bad Request", "401 Unauthorized" and other error codes
* AJAX request to the API controller
* Secured URLs on the "Details" page
* Using **ASP.NET Identity System**
	- Seeded Admin in a special Administrator role
* Separated solution to layers and projects, which depend on one another	
* MVC **Areas**
    - Admin area 
* **AutoMapper** for mappings
* **In-Memmory Cache** for the "All Rents" and "All Users" pages on the Admin area
* **TempData** messages
* **NUnit**
	- Unit tests for service methods with code coverage of 88%
	- Integration tests for some controller actions
	- Mocked external dependencies
* Deployed to **Replit** and available on https://houserentingsystem.evandonova.repl.co 

## :clipboard: Tests Coverage
![image](https://github.com/evandonova/House-Renting-System-App/assets/69080997/b10604d5-d3e8-4f31-bda3-ff4f47862b41)

## :wrench: DB Diagram
![image](https://user-images.githubusercontent.com/69080997/156184077-0e378df0-caba-4fbc-9af5-5c84bcd8bd88.png)
