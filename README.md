---------------------------------------------------------------------------------------------------
--SETUP
--------------------------------------------------------------------------------------------------
secrets.json must exist on folder (create folder if not exists)
C:\Users\<user>\AppData\Roaming\Microsoft\UserSecrets\a6d43594-b9bc-4b19-8460-cb4a5f202145

in secrets.json the api key must be inserted that will be used for quering the web service

{
  "Kestrel:Certificates:Development:Password": "cb650533-fe43-4834-9273-0fb33f9aef82",
  "CatApiKey": "<insert api key>"
}

Docker desktop must be up and running

--------------------------------------------------------------------------------------------------
---EXECUTION
--------------------------------------------------------------------------------------------------
Select docker-compose as startup project.
Begin debugging the application in visual studio.

NOTE:
1.
Downloaded images are stored in azurite.
Can be accessed with Azure Storage Explorer.
In addition, they can be navigated to with a web browser, if the container is up and running.
However, in that case, localhost must be used instead of docker base url.
e.g.
in database there is the value: http://host.docker.internal:10000/devstoreaccount1/cat-images/KWVenr3Pq.jpg
use in a web browser: http://localhost:10000/devstoreaccount1/cat-images/KWVenr3Pq.jpg

2. 
Database can be accessed by management studio
server: localhost,1433
user: sa
pwd: YourPassword123!

--------------------------------------------------------------------------------------------------
---UNIT TESTS
--------------------------------------------------------------------------------------------------
Unit tests can be executed from visual studio by right click on test project and click "Run Tests"

--------------------------------------------------------------------------------------------------
---Future Work
--------------------------------------------------------------------------------------------------

Store images in parallel async on azurite.
Improve test coverage
Implement integration tests with WebApplicationFactory
