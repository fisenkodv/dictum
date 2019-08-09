workflow "Build and Publish" {
  on       = "push"

  resolves = [
    "Publish To Docker Hub"
  ]
}

action "Run Tests" {
  uses = "actions/docker/cli@master"

  args = [
    "build",
    "--file",
    "docker/test/Dockerfile",
    "."
  ]
}

action "Filter Master" {
  needs = "Run Tests"
  uses = "actions/bin/filter@master"
  args = "branch master"
}

action "Build Docker Image" {
  uses = "actions/docker/cli@master"
  
  needs = [
    "Filter Master"
  ]

  args = [
    "build",
    "--rm",
    "--tag",
    "fisenkodv/dictum:latest",
    "--file",
    "docker/api/Dockerfile",
    "."
  ]
}

action "Docker Login" {
  uses    = "actions/docker/login@master"
  
  needs = [
    "Filter Master"
  ]

  secrets = [
    "DOCKER_USERNAME",
    "DOCKER_PASSWORD"
  ]
}

action "Publish To Docker Hub" {
  uses  = "actions/docker/cli@master"

  needs = [
    "Build Docker Image",
    "Docker Login"
  ]

  args  = [
    "push",
    "fisenkodv/dictum:latest"
  ]
}
