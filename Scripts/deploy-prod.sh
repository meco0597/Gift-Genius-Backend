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
kubectl apply -f ../k8s/ui-tls-ingress.yaml -n default

kubectl rollout restart deployment/gift-suggestions-depl
kubectl rollout restart deployment/givr-ui

# helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx

# helm install nginx-ingress ingress-nginx/ingress-nginx \
#     --set rbac.create=false \
#     --namespace ingress \
#     --set controller.replicaCount=1 \
#     --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux \
#     --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux \
#     --set controller.service.loadBalancerIP="20.9.21.31" \
#     --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-dns-label-name"="givr.centralus.cloudapp.azure.com"
