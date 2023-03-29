az acr login --name givrprodacr

# build docker image with tag :latest
docker build --no-cache -t givrprodacr.azurecr.io/gift-suggestion-service --platform linux/amd64 ../GiftSuggestionService
docker build --no-cache -t givrprodacr.azurecr.io/givr-ui --platform linux/amd64 ../ui

# Push the docker image to the docker hub
docker push givrprodacr.azurecr.io/givr-ui
docker push givrprodacr.azurecr.io/gift-suggestion-service