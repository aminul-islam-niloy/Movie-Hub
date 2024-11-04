# Movie Fair

Movie Fair is a web Movie hub application developed using ASP.NET Core MVC, Identity, and Entity Framework Core. It serves as a platform for users to browse, view, download, and manage movies. Administrators have the capability to add and manage movies, while customers can browse, view, download, add to favorites, and provide reviews and ratings for movies.

### Live link:

```
http://moviesfair.runasp.net
```

## Table of Contents

- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

Before running the application, make sure you have the following software installed:

- [.NET SDK](https://dotnet.microsoft.com/download) 6 or upper
- [Visual Studio](https://visualstudio.microsoft.com/) or any other preferred IDE
- [Git](https://git-scm.com/)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/aminul-islam-niloy/Movie-Hub.git
   ```

2. Navigate to the project directory:
   install:

   ```
   EntityFrameworkCore
    Identity.UI
    Session
    SqlServer
    EntityFrameworkCore.Tools
    X.PagedList.Mvc.Core
   ```

3. Open the solution file (`MovieHub.sln`) in Visual Studio.

4. Restore NuGet packages and build the solution.

## Usage

Delete previous migration and add new migration in nuget package manager: Add-Migration YourMigrationName

1. Start the application by running the project in Visual Studio.
2. Navigate to the URL provided by the IDE (typically `https://localhost:port`).
3. log in and access the admin dashboard to manage movies.
4. Regular users can browse movies, view details, download, add to favorites, and provide reviews and ratings after logging in.

## Contributing

Contributions are welcome! If you'd like to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create your feature branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -am 'Add some feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Create a new Pull Request.

## License

This project is licensed under the [MIT License](LICENSE.md).

---

Feel free to customize this README according to your project's specific details and requirements. If you have any questions or need further assistance, don't hesitate to ask!
