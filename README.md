$${\color{red}Work \space In \space Progress}$$

My copy of popular Pastebin (https://pastebin.com/)
<h1>How to run</h1>
<p>Set google api propertis in appsettings.json</p>
<pre>
"GoogleOAuthOptions": {
    "ClientId": "*",
    "ClientSecret": "*"
  },
</pre>
<p>Using docker-compose run the app in Docker</p>
<pre>$ docker-compose up</pre>
<p>You can optionally specify JWT properties</p>
<pre>
"JwtSettings": {
    "Key": "*",
    "HashKey": "*",
    "LifetimeInMinutes": 5,
    "RefreshTokenExpiryTimeInDays" :  7,
    "ValidAudiences": [],
    "ValidIssuers": [],
    "ValidateLifetime": true,
    "ValidateAudience": false,
    "ValidateIssuer": false
  },
</pre>
<h1>Screenshots</h1>
<img src="img.png">
