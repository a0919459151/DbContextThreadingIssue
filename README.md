# DbContextThreadingIssue

## get start
#### 配置連線字串
``` json
{
  "ConnectionStrings": {
	  "DefaultConnection": "Server=localhost;Database=DbContextThreadingIssue;Trusted_Connection=True;User Id=;Password="
  }
}
```

#### migrate sql server
``` bash
dotnet ef database update -p .\DbContextThreadingIssue\
```

####  用 .http file call api
```
@DbContextThreadingIssue_HostAddress = http://localhost:5007


GET {{DbContextThreadingIssue_HostAddress}}/api/book/getBooks

###


GET {{DbContextThreadingIssue_HostAddress}}/api/book/getBooksWhenAll

###

```
