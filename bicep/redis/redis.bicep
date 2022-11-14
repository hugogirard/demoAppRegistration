param location string
param suffix string


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



output id string = cache.id
output cacheName string = cache.name
