workflow "Publish Container" {
  on = "push"
  resolves = ["Build"]
}

action "Build" {
  uses = "actions/docker/cli@8cdf801b322af5f369e00d85e9cf3a7122f49108"
  args = "build --rm --tag fisenkodv/dictum:latest --file Dockerfile ."
}

action "Publish" {
  uses = "actions/docker/cli@8cdf801b322af5f369e00d85e9cf3a7122f49108"
  args = "push fisenkodv/dictum:latest"
  needs = ["Build"]
}
