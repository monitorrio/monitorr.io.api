# INTRODUCTION #

Monitorr is a web service/admin that allow for storing elmah errors in the cloud.

It provides an easy to use admin for manageing your application generated errors.

### What is this repository for? ###

* Anyone who wants to quickly setup error logging for their dev or production application.

### How do I get set up? ###

Due to security reasons all sensitive information ("auth ClientId", "auth ClientSecret", MongoUri, AWS credentials, etc.) is moved to separated configuration files
(SecureSettings.Debug.config, SecureSettings.Release.config, SecureSettings.Staging.config, SecureSettings.Testing.config) which are attached to the project as 
linked files and stored out of project folder by path "C:\Projects\Monitorr.io\". 
So you need to create appropriated security settings files by that path using 'SecureSettings.Debug.config.example' as a template or just include them directly to your project if you don't plan to share them.

* Dowload this source code via git.
* Open in Visual Studio 2017, setup security configuration and hit run.

