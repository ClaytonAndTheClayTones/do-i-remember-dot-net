param appName string = 'tp-arm-demo'
param environment string = 'dev'

resource databaseServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: '${appName}-sqlserver-${environment}'
  location: location
  properties: {
    administratorLogin: 'db_username'
    administratorLoginPassword: '3I4Plu8lW60a'
  }
}
