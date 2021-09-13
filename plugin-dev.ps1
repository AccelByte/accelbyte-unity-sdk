Param (
	[string]$e = 'dev',     # Backend environment: dev or demo
	[string]$t = '',        # Build and run tests, name of the tests to run, use ; to run multiple tests e.g. Tests.UnitTests.A;Tests.IntegrationTests.Tests.B
	[switch]$h              # Print help
)

$ErrorActionPreference = "Stop"

function PrintHelp()
{
	echo @"
.\plugin-dev.ps1                                                         Launch Unity
.\plugin-dev.ps1 -t Tests.UnitTests.A;Tests.IntegrationTests.Tests.B     Build and run test Tests.UnitTests.A and Tests.IntegrationTests.Tests.B against Justice backend 'dev'
.\plugin-dev.ps1 -e demo -t Tests.UnitTests.A                            Build and run test Tests.UnitTests.A against Justice backend 'demo'
.\plugin-dev.ps1 -h                                                      Print help
"@
}

function ConfigureSdk()
{   
    $SdkConfigPath=".\Assets\Resources\AccelByteSDKConfig.json"
    $SdkConfig=$(Get-Content $SdkConfigPath| ConvertFrom-Json)
    $SdkConfig.ClientId=$env:CLIENT_ID
    $SdkConfig.ClientSecret=$env:CLIENT_SECRET
    $SdkConfig.Namespace=$env:CLIENT_NAMESPACE
    $SdkConfig.PublisherNamespace=$env:PUBLISHER_NAMESPACE
    $SdkConfig.RedirectUri="http://127.0.0.1"
    $SdkConfig.BaseUrl=$env:BASE_URL
    ConvertTo-Json $SdkConfig | Out-File -FilePath $SdkConfigPath
    
    $ServerSdkConfigPath=".\Assets\Resources\AccelByteServerSDKConfig.json"
    $ServerSdkConfig=$(Get-Content $ServerSdkConfigPath| ConvertFrom-Json)
    $ServerSdkConfig.Namespace=$env:CLIENT_NAMESPACE
    $ServerSdkConfig.BaseUrl=$env:ADMIN_BASE_URL
    $ServerSdkConfig.ClientId=$env:SERVER_CLIENT_ID
    $ServerSdkConfig.ClientSecret=$env:SERVER_CLIENT_SECRET
    $ServerSdkConfig.RedirectUri="http://127.0.0.1"
    ConvertTo-Json $ServerSdkConfig | Out-File -FilePath $ServerSdkConfigPath
}

if ($h)
{
	PrintHelp
	
	return
}

if (!(Test-Path '.\plugin-dev.config.ps1'))
{
	Throw "Your '.\plugin-dev.config.ps1' is missing! Please create your own based on LastPass Justice-Unreal-SDK-Plugin-Dev-Config."
}

. .\plugin-dev.config.ps1

if (!($UNITY_EXE))
{
    Throw "UNITY_EXE in '.\plugin-dev.config.ps1' is not set! Please create your own based on LastPass Justice-Unreal-SDK-Plugin-Dev-Config."
}

$env:BASE_URL = $CREDENTIALS[$e]['BASE_URL']
$env:CLIENT_ID = $CREDENTIALS[$e]['CLIENT_ID']
$env:CLIENT_SECRET = $CREDENTIALS[$e]['CLIENT_SECRET']
$env:CLIENT_NAMESPACE = $CREDENTIALS[$e]['CLIENT_NAMESPACE']
$env:SERVER_CLIENT_ID = $CREDENTIALS[$e]['SERVER_CLIENT_ID']
$env:SERVER_CLIENT_SECRET = $CREDENTIALS[$e]['SERVER_CLIENT_SECRET']
$env:PUBLISHER_NAMESPACE = $CREDENTIALS[$e]['PUBLISHER_NAMESPACE']
$env:ADMIN_BASE_URL = $CREDENTIALS[$e]['ADMIN_BASE_URL']
$env:ADMIN_CLIENT_ID = $CREDENTIALS[$e]['ADMIN_CLIENT_ID']
$env:ADMIN_CLIENT_SECRET = $CREDENTIALS[$e]['ADMIN_CLIENT_SECRET']
$env:ADMIN_USER_NAME = $CREDENTIALS[$e]['ADMIN_USER_NAME']
$env:ADMIN_USER_PASS = $CREDENTIALS[$e]['ADMIN_USER_PASS']
$env:STEAM_USER_ID = $STEAM_USER_ID
$env:STEAM_KEY = $STEAM_KEY

ConfigureSdk

if ($t) # Run tests only
{	
    & "$UNITY_EXE" `
        -projectPath "$(pwd)" `
        -batchmode `
        -nographics `
        -logFile "-" `
        -executeMethod JenkinsScript.PerformBuild `
        -quit `
        | tee build.log

    & "$UNITY_EXE" `
        -projectPath "$(pwd)" `
        -batchmode `
        -nographics `
        -logFile "-" `
        -testResults "$(pwd)\test.xml" `
        -runTests `
        -testPlatform playmode `
        -provider=baremetal `
        -testFilter "$t" `
        | tee test.log
}
else # Launch Editor
{
    & "$UNITY_EXE" `
        -projectPath "$(pwd)"
}
