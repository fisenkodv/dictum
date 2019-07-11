workflow "Publish Container" {
  on = "push"
  resolves = ["Publish"]
}

action "Build" {
  uses = "actions/docker/cli@8cdf801b322af5f369e00d85e9cf3a7122f49108"
  args = "build --rm --tag fisenkodv/dictum:latest --file Dockerfile ."
}

action "Login" {
  uses = "actions/docker/login@8cdf801b322af5f369e00d85e9cf3a7122f49108"
  needs = ["Build"]
  secrets = ["DOCKER_USERNAME", "DOCKER_PASSWORD"]
}

action "Publish" {
  uses = "actions/docker/cli@8cdf801b322af5f369e00d85e9cf3a7122f49108"
  needs = ["Login"]
  runs = "push fisenkodv/dictum:latest"
  secrets = ["DOCKER_PASSWORD", "DOCKER_USERNAME"]
}
