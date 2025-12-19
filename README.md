# Ratter Scanner CDN
This is the CDN that will be used for Ratter Scanner's file storing.
This project is a rewrite of our existing CDN.

This is also a test project, and allows our team to get more insight in how we would like to do a recode of our other services.

Please do not contribute just yet. As I am still setting up the base for this project. Afterwards contributions are greatly appreciated! :D

When properly configured the CDN should be able to run multiple instances of itself, without causing issues. This allows a high availability setup.
Files info should be merged by mounting 1 dedicated storage mount on a specified folder that is mounted in the docker container.
The mount can then handle other logic like RAID setup's.
The database should be accessible by multiple server's at once.

# Next to do
- Add tests
- Add retrieve / upload logic
- Add token checks for retrieve logic
- Add database connection for info related about files

# How to test / run
- To be added.
- Endpoints should be split from their Handlers, so the logic of the handlers can be tested individually.

# CI/CD pipeline
- To be added.
- Merge's or commits should trigger an update of the currently used CDN.
- This will probably be managed by another internal service, also written in c#.