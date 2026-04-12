param(
    [string]$testFile = "latency-test.js"
)

docker run --rm -v ${PWD}:/scripts grafana/k6 run /scripts/$testFile