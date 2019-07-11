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

action "Publish" {
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
