# build docker image with tag :latest
docker build -t meco0597/gift-suggestion-service ../GiftSuggestionService

# Push the docker image to the docker hub
docker push meco0597/gift-suggestion-service
