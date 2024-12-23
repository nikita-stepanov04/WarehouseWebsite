# Warehouse Website

![ASP.NET](https://img.shields.io/badge/ASP.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) 
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=entity-framework&logoColor=white) 
![Angular](https://img.shields.io/badge/Angular-ad0c00?style=for-the-badge&logo=entity-framework&logoColor=white)

![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?style=for-the-badge&logo=postgresql&logoColor=white) 
![Azure Blob Storage](https://img.shields.io/badge/Azure%20Blob%20Storage-0078D4?style=for-the-badge&logo=microsoft-azure&logoColor=white) 

![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white) 
![NUnit](https://img.shields.io/badge/NUnit-25A162?style=for-the-badge&logo=nunit&logoColor=white) 
![Quartz](https://img.shields.io/badge/Quartz-512BD4?style=for-the-badge&logo=quartz-scheduler&logoColor=white)


## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/nikita-stepanov04/WarehouseWebsite.git
    ```
2. Navigate to the project directory:
    ```bash
    cd WarehouseWebsite
    ```
3. Build and run the Docker containers:
    ```bash
    docker-compose up
    ```

## Usage

1. **Access the Website**

    - **Local Environment**

        Once you have built and started the Docker containers, the swagger ui will be available at:

        - **Localhost**: <http://localhost:8080/swagger/index.html>
         
       Frontend will be available at:

        - **Localhost**: <http://localhost:8081>
       
        Smtp server client will be available at:

        - **Localhost**: <http://localhost:8025>
  
2. **Log in** with one of the predefined admin accounts and get an access token:
    - **Email**: *admin1@mail.com*, **Password**: *password*
    - **Email**: *admin2@mail.com*, **Password**: *password*
    - **Email**: *admin3@mail.com*, **Password**: *password*
    
 3. **Set the authorization header** in Swagger to the value: Bearer \$\{access token}

   


