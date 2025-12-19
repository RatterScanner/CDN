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
- Add upload logic
- Add more tests
- Add database connection for info related about files and future token checks
- Add token checks for upload logic
- Add CI/CD workflow

# How to test / run
- To be added.

# CI/CD pipeline
- Merge's or commits should trigger an update of the currently used CDN.
- This will probably be managed by another internal service, also written in c#.
- To be added.