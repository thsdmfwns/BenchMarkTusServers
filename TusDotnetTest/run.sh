docker build .. --file ./Dockerfile -t bm_dotnet

docker run \
-d \
--network BenchMark \
--rm \
--name bm_dotnet \
--volume=BenchMark:/tusfiles \
bm_dotnet

docker run \
-d \
--rm \
--network BenchMark \
--name bm_golang \
--user="root" \
--volume=BenchMark:/files \
tusproject/tusd:latest \
-upload-dir=/files