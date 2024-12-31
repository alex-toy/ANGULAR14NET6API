# ANGULAR 14 .NET 6 API 

<img src="/pictures/app.png" title="web app"  width="900">

## Packages
```
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools
```

## Commands
```
cd SoccerPlayerApi/clientapp
ng s --configuration=production
```


## Deploy To Azure

### Web App

#### Web App Backend

- create Web App for backend
<img src="/pictures/webapp.png" title="web app"  width="900">

- enable basic authentication
<img src="/pictures/webapp1.png" title="web app"  width="900">

- allow origin for frontend
<img src="/pictures/webapp2.png" title="web app"  width="900">

#### Application Insights

- use it for debugging purposes
<img src="/pictures/insight.png" title="web app"  width="900">
<img src="/pictures/insight1.png" title="web app"  width="900">

#### Web App Frontend

- create Web App for frontend
<img src="/pictures/webappfront.png" title="web app front"  width="900">

- run
```
cd SoccerPlayerApi/clientapp
ng build --configuration=production
```

- deploy to web app. Browse until *clientapp* in the *dist* folder
<img src="/pictures/webappfront1.png" title="web app front"  width="900">

- manual deploy
```
cd clientapp\dist\clientapp
az webapp deploy --resource-group alexeirg --name soccerplayerclient  --src-path ./ --type static
```


### SQL Azure

- create SQL databse
<img src="/pictures/sql.png" title="sql database"  width="900">

- in the server, add access for Azure Services
<img src="/pictures/sql0.png" title="sql database"  width="900">

- grab connection string
<img src="/pictures/sql1.png" title="sql database"  width="900">

- use connection string in the webapp API
<img src="/pictures/sql2.png" title="sql database"  width="900">