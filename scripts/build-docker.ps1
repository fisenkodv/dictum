trap { Write-Host $Error[0].ScriptStackTrace; throw $Error[0] }

function Invoke-DockerBuildAndPublish {
  Set-Location ..
  Write-Host "Building Docker image"
  docker build --rm --tag fisenkodv/dictum:latest --file Dockerfile .

  Write-Host "Publishing Docker image"
  docker push fisenkodv/dictum:latest
}

function Invoke-Main {
  Invoke-DockerBuildAndPublish
}

Invoke-Main
exit 0