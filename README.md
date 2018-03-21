# ANCMInProcessSite

Steps to run on Azure websites:
1. Clone repository locally.
2. Run dotnet restore.
3. Publish site to azure (via web deploy or ftp).
4. In the Kudu/ azure portal create a folder at D:\home\ANCM and add the aspnetcore.dll there
5. Add the applicationHost.xdt file to D:\home\site\wwwroot
6. Start the app.
