

DEVELOPMENT RULES:

1. COMMENTS ARE REQUIRED! 
In the project option the autogenerate XML help option is set. Based on this XML file a help page for client is generation by the application.
Help page is available by url ~/help

2. WEB API ACTION/CONTROLLER DEVELOPMENT
In order not to confuse the API clients we use next rules:
	0) Every Corbis API controller must be inherited from Corbis.WebAPI.Controllers.API.ApiControllerBase
	1) --- Default mapping for web api as: 'api/{controller}/{action}/{id}'
It means that in the url client must exactly point controller and action method!!!. For example
http://corbis.webapi.com/api/user/authenticate?username=John@gmail.com&password=qwerty
This url invokes Authenticate(string username, string password) in the UserController. The 'Authenticate' method must be marked with [HttpGet] attribute
	2) --- For action methods use attributes [HttpGet], [HttpPost], [HttpDelete], [HttpPut] to specify http verb
See the example above.
	3) --- Give preference to POST. 
TBD: To my mind ALL action methods can be implemented using POST
	4) --- If you use POST (or POST) then in order to map json/xml of HTTP request body (in this case Body of HTTP request must be json/xml only) use [FromBody] attribute
	If url of this POST request has parameters to then restore them from url use [FromUri] attribute

Request example
	Verb: POST
	Uri:  http://corbis.webapi.com/api/user/auth?datetime=20120920  (dateime in format yyyymmdd)
	Body: {username:"john@gmail.com", password:"qwerty", clientID:12345}
	C# Classes:
public class UserInfo
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public int ClientID { get; set; }
}

public class UserController : ApiControllerBase
{
    /// <summary>
    /// Authintecates user via GET
    /// </summary>
    [HttpGet]
    public ApiResponse<...> Authenticate(string username, string password)
    {
        ...
    }

    /// <summary>
    /// Authintecates user via POST
    /// </summary>
    [HttpPost]
    [ActionName("Auth")]
    public ApiResponse<...> Authenticate([FromBody]UserInfo o, [FromUri]int datetime)
    {
        ...
    }
}

	5) --- Any response must be inherited from Corbis.WebAPI.Code.ApiResponse<> or Corbis.WebAPI.Code.ApiResponse<,> class



List of articles:
!!!ADD here articles which are usefull for all us!!!
http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api

Validation
1. DataAnnotation validation is used
http://www.asp.net/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
