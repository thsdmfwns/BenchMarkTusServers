docker build .. --file ./Dockerfile -t bm

docker run \
--rm \
--network BenchMark \
--name bm \
-v "$(pwd)"/TestFiles:/TestFiles \
--volume=BenchMark:/files \
bm