resourceGroup=gift-genius-prod-infra
clusterName=givr-cluster
location=centralus
NodeResourceGroupName="MC_gift-genius-prod-infra_givr-cluster_centralus"
SubscriptionId="a2587f85-a10b-4753-8db8-0c6d0b28df6a"

clusterIdentityId=$(az identity create --name ClusterIdentity --resource-group $resourceGroup --location $location --query '{id: id}' -o tsv)
podIdentityId=$(az identity create --name PodIdentity --resource-group $resourceGroup --location $location --query '{id: id}' -o tsv)
publicIp=$(az network public-ip create -n givrpublicip -g $resourceGroup --location $location --sku Standard --dns-name givr --version IPv4 --ddos-protection-mode Enabled --tier Regional --query "{ipAddress: ipAddress}" -o tsv)
logAnalyticsId=$(az monitor log-analytics workspace create --resource-group $resourceGroup --location $location --workspace-name givr-logs-workspace --query id -o tsv)

az aks create -g $resourceGroup -n $clusterName \
    --enable-managed-identity \
    --enable-addons monitoring \
    --location $location \
    --assign-identity $podIdentityId \
    --assign-kubelet-identity $clusterIdentityId \
    --workspace-resource-id $logAnalyticsId \
    --node-count 1 \
    --enable-cluster-autoscaler \
    --min-count 1 \
    --max-count 5 \
    --cluster-autoscaler-profile scan-interval=30s \
    --network-plugin azure \
    --attach-acr /subscriptions/$SubscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.ContainerRegistry/registries/givrprodacr \
    --generate-ssh-keys

AksManagedIdentity="$(az aks show -g $resourceGroup -n $clusterName --query identityProfile.kubeletidentity.clientId -o tsv)"
az role assignment create --role "Managed Identity Operator" --assignee $AksManagedIdentity --scope /subscriptions/$SubscriptionId/resourcegroups/$NodeResourceGroupName
az role assignment create --role "Virtual Machine Contributor" --assignee $AksManagedIdentity --scope /subscriptions/$SubscriptionId/resourcegroups/$NodeResourceGroupName
az role assignment create --role "AcrPull" --assignee $AksManagedIdentity --scope /subscriptions/$SubscriptionId/resourcegroups/$NodeResourceGroupName

#kubectl create secret tls ssl-cert-secret --cert=./givr_ai.crt --key=./givr_ai.key
