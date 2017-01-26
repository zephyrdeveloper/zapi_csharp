# zapi_csharp
ZAPI CSharp example JWT generate and make call to zapi

## JWT Generator Reference
Download and add reference project(jwt generator) from the following repository.

    git clone https://github.com/jwt-dotnet/jwt.git


## Define ZAPI Credentials (ZapiCSharp.cs) and API Information

	//define userName
	var USER = "admin";

	//define AccessKey
	var ACCESS_KEY = "MTZhOGQ5OTEtOTI0OS0zNzdmLWIyZTAtYTFkOTFhZTI2OTczIGFkbWluIFVTRVJfREVGQVVMVF9OQU1F";

	//define SecretKey
	var SECRET_KEY = "4b6v9i0kP7EjVzcuLMc4CUdkyq9AeTWheO2pr5CotGc";

	//define Base Url
	var BASE_URL = "https://e2f89b98.ngrok.io";

	
	//define ContextPath
	var CONTEXT_PATH = "/connect";

	//define Expire Time
	var EXPIRE_TIME = 3600;

	//define api path
	var RELATIVE_PATH = "/public/rest/api/1.0/cycles/search";
	
	//define query string
	var QUERY_STRING = "expand=action&projectId=10000&versionId=10000"
	
	//Create canonical path for JWT. e.g: API Method & RELATIVE_PATH & QUERY_STRING (Asc Ordered)
	var canonical_path = "GET&" + RELATIVE_PATH + "&" + QUERY_STRING;
	
	//Details can be found here:
	//Link: https://developer.atlassian.com/static/connect/docs/latest/concepts/understanding-jwt.html
	var payload = new Dictionary<string, object>()
                {
                    { "sub", USER },                    //assign subject 
                    { "qsh", getQSH(canonical_path) },  //assign query string hash
                    { "iss", ACCESS_KEY },              //assign issuer
                    { "iat", iat },                     //assign issue at(in ms)
                    { "exp", exp }                      //assign expiry time(in ms)
                };

	string token = JWT.JsonWebToken.Encode(payload, SECRET_KEY, JWT.JwtHashAlgorithm.HS256);
	
	Console.WriteLine(token);
	
	