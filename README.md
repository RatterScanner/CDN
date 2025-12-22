# About The Project
This is the CDN that will be used for Ratter Scanner's file storing.
This project is a rewrite of our existing CDN.

This is also a test project, and allows our team to get more insight in how we would like to do a recode of our other services.

Please do not contribute just yet. As I am still setting up the base for this project. Afterwards contributions are greatly appreciated! :D

When properly configured the CDN should be able to run multiple instances of itself, without causing issues. This allows a high availability setup.
Files info should be merged by mounting 1 dedicated storage mount on a specified folder that is mounted in the docker container.
The mount can then handle other logic like RAID setup's.
The database should be accessible by multiple server's at once.

# How to test / run
---
The project is tested and hosted on a linux arch and a linux ubuntu installation. \
You might have to slightly adjust some commands to get them working on other OS types.

### How to get your own copy running

1. Get the complete source code: \
    `git clone https://github.com/RatterScanner/CDN.git`

1. Copy the content of `.env.example` to `.env`. Change the env to fit your requirements.

1. [Install docker](https://docs.docker.com/engine/install)

The following commands should be ran in the project root directory.
### Run tests:
1. `./run_tests.sh`
### Build and launch:
1. `docker compose up --build`

# CI/CD pipeline
- Merge's or commits should trigger an update of the currently used CDN.
- This will probably be managed by another internal service, also written in c#.
- To be added.

# Next to do
- Add upload logic
- Add more tests
- Add database connection for info related about files and future token checks
- Add token checks for upload logic
- Add CI/CD workflow