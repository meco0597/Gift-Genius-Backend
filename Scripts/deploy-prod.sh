# Run the image
az aks get-credentials -n givr-cluster -g gift-genius-prod-infra

helm install aad-pod-identity aad-pod-identity/aad-pod-identity
kubectl apply -f ../k8s/giftSuggestion-depl.yaml
kubectl apply -f ../k8s/loadbalancer.yaml
kubectl apply -f ../k8s/giftSuggestion-np-srv.yaml
kubectl apply -f ../k8s/giftSuggestion-identity.yaml
kubectl apply -f ../k8s/giftSuggestion-identity-binding.yaml
