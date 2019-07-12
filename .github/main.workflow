workflow "Build and Publish" {
  on       = "push"

  resolves = [
    "Publish"
  ]
}

action "Build" {
  uses = "actions/docker/cli@master"

  args = [
    "build",
    "--rm",
    "--tag",
    "fisenkodv/dictum:latest",
    "--file",
    "Dockerfile",
    "."
  ]
}

action "Docker Login" {
  uses    = actions/docker/login@master"

  needs   = [
    "Build"
  ]

  secrets = [
    "DOCKER_USERNAME",
    "DOCKER_PASSWORD"
  ]
}

action "Publish" {
  uses    = "actions/docker/cli@master"

  needs   = [
    "Docker Login"
  ]

  args    = [
    "push",
    "fisenkodv/dictum:latest"
  ]
}
