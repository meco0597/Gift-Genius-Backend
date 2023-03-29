resourceGroup=gift-genius-prod-infra
clusterName=givr-cluster
location=centralus
NodeResourceGroupName="MC_gift-genius-prod-infra_givr-cluster_centralus"
SubscriptionId="a2587f85-a10b-4753-8db8-0c6d0b28df6a"
clusterSize=Standard_D2as_v5

az aks nodepool add --resource-group $resourceGroup --cluster-name $clusterName --name smallsize --node-count 1 --node-vm-size $clusterSize --mode System --no-wait

kubectl cordon aks-nodepool1-19466915-vmss000002

kubectl drain aks-nodepool1-19466915-vmss000002 --ignore-daemonsets --delete-emptydir-data

az aks nodepool delete --resource-group $resourceGroup --cluster-name $clusterName --name nodepool1

# https://learn.microsoft.com/en-us/azure/aks/resize-node-pool?tabs=azure-cli