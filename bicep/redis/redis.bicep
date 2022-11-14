param location string
param suffix string

@description('The web app API principal ID')
param principalId string

@description('This is the built-in Azure Redis Cache Contributor role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles#contributor')
resource redisCacheContributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {  
  name: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
}

resource cache 'Microsoft.Cache/redis@2021-06-01' = {
  name: 'cache-${suffix}'
  location: location
  properties: {
    redisVersion: '6.0'
    sku: {
      capacity: 0
      family: 'C'
      name: 'Basic'
    }
  }
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  scope: cache
  name: guid(cache.id, principalId, redisCacheContributorRoleDefinition.id)
  properties: {
    roleDefinitionId: redisCacheContributorRoleDefinition.id
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}



output id string = cache.id
output cacheName string = cache.name
