# Release [v2.0.0]
**[2019-08-19]**

## What's new in this release?
* Adding API Gateway Features. You need to set the url on config to Api Gateway url, the url will look something like this: `https://api-preview.accelbyte.io`
* Adding Switchable Session Management Feature. You will need to add new variable on config:
    * `UseSessionManagement`, fill it `true` when you want to use Session Management feature, and `false` when you don't want to.
    * `LoginServerUrl`, a non-API Gateway connection need `/iam` prefix on login server endpoint, so for non-Session Managed connection, the url will look something like this: `https://preview.accelbyte.io/iam` 