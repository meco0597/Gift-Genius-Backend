az acr login --name givrprodacr

# build docker image with tag :latest
docker build -t givrprodacr.azurecr.io/gift-suggestion-service --platform linux/amd64 ../GiftSuggestionService #--no-cache
#docker build -t givrprodacr.azurecr.io/givr-ui --platform linux/amd64 ../ui #--no-cache

# Push the docker image to the docker hub
#docker push givrprodacr.azurecr.io/givr-ui
docker push givrprodacr.azurecr.io/gift-suggestion-service