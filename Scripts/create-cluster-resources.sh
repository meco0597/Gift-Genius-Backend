resourceGroup=gift-genius-prod-infra
clusterName=givr-cluster
location=westus2
NodeResourceGroupName="MC_gift-genius-prod-infra_givr-cluster_westus2"
SubscriptionId="a2587f85-a10b-4753-8db8-0c6d0b28df6a"

clusterIdentityId=$(az identity create --name ClusterIdentity --resource-group $resourceGroup --query '{id: id}' -o tsv)
podIdentityId=$(az identity create --name PodIdentity --resource-group $resourceGroup --query '{id: id}' -o tsv)
publicIp=$(az network public-ip show -n givrpublicip -g $resourceGroup --query "{ipAddress: ipAddress}" -o tsv)
logAnalyticsId=$(az monitor log-analytics workspace create --resource-group $resourceGroup --workspace-name givr-logs-workspace --query id -o tsv)



az aks create -g $resourceGroup -n $clusterName \
    --enable-managed-identity \
    --enable-addons monitoring \
    --location $location \
    --assign-identity $podIdentityId \
    --assign-kubelet-identity $clusterIdentityId \
    --workspace-resource-id $logAnalyticsId \
    --network-plugin azure \
    --generate-ssh-keys


AksManagedIdentity="$(az aks show -g $resourceGroup -n $clusterName --query identityProfile.kubeletidentity.clientId -o tsv)"
az role assignment create --role "Managed Identity Operator" --assignee $AksManagedIdentity --scope /subscriptions/$SubscriptionId/resourcegroups/$NodeResourceGroupName
az role assignment create --role "Virtual Machine Contributor" --assignee $AksManagedIdentity --scope /subscriptions/$SubscriptionId/resourcegroups/$NodeResourceGroupName


