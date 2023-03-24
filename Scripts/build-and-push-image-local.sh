# build docker image with tag :latest
docker build -t meco0597/gift-suggestion-service ../GiftSuggestionService
docker build -t meco0597/givr-ui ../ui

# Push the docker image to the docker hub
docker push meco0597/gift-suggestion-service
docker push meco0597/givr-ui
