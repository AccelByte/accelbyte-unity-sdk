$GIT_PATH = $Env:GIT_PATH
$GIT_EXE = $GIT_PATH + "\git.exe"
$GIT_SHA = & "$GIT_EXE" rev-parse HEAD 

$WORKSPACE = $Env:WORKSPACE
$BUILD_NUMBER = $Env:BUILD_NUMBER   

& aws s3 cp "$WORKSPACE\outputpackage\JusticeSampleGameDemo-$GIT_SHA-BUILD-$BUILD_NUMBER.7z" s3://justice-sdk/Unity/JusticeSampleGameDemo-$GIT_SHA-BUILD-$BUILD_NUMBER.7z --storage-class STANDARD_IA --metadata GitSha=$GIT_SHA,BuildNo=$BUILD_NUMBER
& aws s3 cp "$WORKSPACE\outputpackage\JusticeSDK-$GIT_SHA-BUILD-$BUILD_NUMBER.unitypackage" s3://justice-sdk/Unity/JusticeSDK-$GIT_SHA-BUILD-$BUILD_NUMBER.unitypackage --storage-class STANDARD_IA --metadata GitSha=$GIT_SHA,BuildNo=$BUILD_NUMBER        