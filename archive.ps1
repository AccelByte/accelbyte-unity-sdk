$GIT_PATH = $Env:GIT_PATH
$GIT_EXE = $GIT_PATH + "\git.exe"      
$GIT_SHA = & "$GIT_EXE" rev-parse HEAD 

$WORKSPACE = $Env:WORKSPACE
$BUILD_NUMBER = $Env:BUILD_NUMBER        
$ZIP_PATH = $Env:ZIP_PATH

& "$ZIP_PATH\7z.exe" a -t7z "$WORKSPACE\outputpackage\JusticeSampleGameDemo-$GIT_SHA-BUILD-$BUILD_NUMBER.7z" "$WORKSPACE\output"    