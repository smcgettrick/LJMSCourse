docker build -t smcgettrick/platformservice -f .\src\LJMSCourse.PlatformService.Api\Dockerfile .
docker build -t smcgettrick/commandservice -f .\src\LJMSCourse.CommandService.Api\Dockerfile .

docker push smcgettrick/platformservice
docker push smcgettrick/commandservice

kubectl rollout restart deployment platformservice-depl
kubectl rollout restart deployment commandservice-depl