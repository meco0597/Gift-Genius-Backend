# Run the image
docker run --env-file=./env-vars/.env -p 5000:5000 -d meco0597/gift-suggestion-service
#kubectl apply -f ../k8s/giftSuggestion-depl.yaml
