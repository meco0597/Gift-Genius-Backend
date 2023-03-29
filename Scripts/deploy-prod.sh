# Run the image
az aks get-credentials -n givr-cluster -g gift-genius-prod-infra

kubectl apply -f https://raw.githubusercontent.com/Azure/aad-pod-identity/v1.8.8/deploy/infra/mic-exception.yaml
kubectl apply -f ../k8s/giftSuggestion-identity.yaml
kubectl apply -f ../k8s/giftSuggestion-identity-binding.yaml
kubectl apply -f ../k8s/giftSuggestion-depl.yaml
# kubectl apply -f ../k8s/giftSuggestion-np-srv.yaml
# kubectl apply -f ../k8s/giftSuggestion-loadbalancer.yaml
kubectl apply -f ../k8s/ui-depl.yaml
kubectl apply -f ../k8s/ui-np-srv.yaml
kubectl apply -f ../k8s/ui-loadbalancer.yaml

kubectl rollout restart deployment/gift-suggestions-depl
kubectl rollout restart deployment/givr-ui
