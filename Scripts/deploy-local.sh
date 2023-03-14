# Run the image
docker run --env-file=./env-vars/.env -p 8080:80 -d meco0597/gift-suggestion-service
#kubectl apply -f ../k8s/giftSuggestion-depl.yaml
