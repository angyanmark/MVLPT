# MVLPT 👑

Computer Security Homework 2020, Budapest University of Technology and Economics

## How to start up project

1. Run `npm install` from the `ImageStore.Frontend` folder
2. Open `ImageStore.sln` in Visual Studio and build the solution
3. Set `ImageStore.Backend.Web` as startup project and `ImageStore.Backend.Dal` as default project in Package Manager Console then run the `Update-Database` command
4. Set `ImageStore.Backend.Web` as startup project, then run it with the `Frontend & Backend` configuration
5. The webapp will open up in a browser window (ignore any errors in the console output), register a new user, or use the pre-generated admin account (configurable in `appsettings.Development.json`) to login

## Useful commands & links

### Backend

- `Backend.Web` \ `Backend Only`: run backend & swagger on `http://localhost:5000/`
- `Backend.Web` \ `Frontend & Backend`: run backend & swagger & angular on `http://localhost:5000/`
- `Backend.Dal` \ `Add-Migration migration-name`: create migration for db changes
- `Backend.Dal` \ `Update-Database`: apply last migration to db

### Frontend

- `npm install`: initialize `node_modules`
- `npm start`: start frontend on `http://localhost:4200/`
- `npm run swagger`: generates `api/app.generated.ts`
- `npm ci`: reinitialize `node_modules`
- design: https://material.angular.io/
- routing: https://medium.com/@shairez/angular-routing-a-better-pattern-for-large-scale-apps-f2890c952a18
