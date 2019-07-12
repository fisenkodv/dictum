workflow "Build and Publish" {
  on       = "push"

  resolves = [
    "Publish"
  ]
}

action "Build" {
  uses = "docker://docker:stable"

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
  needs = ["Publish Filter"]
  uses    = "docker://docker:stable"
  secrets = ["DOCKER_USERNAME", "DOCKER_PASSWORD"]
}

action "Publish" {
  needs = ["Docker Login"]
  uses    = "docker://docker:stable"

  args    = [
    "push",
    "fisenkodv/dictum:latest"
  ]

  needs   = [
    "Build"
  ]

  secrets = [
    "DOCKER_PASSWORD",
    "DOCKER_USERNAME"
  ]
}
