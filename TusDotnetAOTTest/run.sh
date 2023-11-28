docker build .. --file ./Dockerfile -t bm_dotnet_aot

docker run \
-d \
--network BenchMark \
--rm \
--name bm_dotnet_aot \
--volume=BenchMark:/tusfiles \
bm_dotnet_aot