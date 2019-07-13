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

action "Build Docker Image" {
  uses = "actions/docker/cli@master"
  
  needs = [
    "Run Tests"
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
