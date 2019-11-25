$GIT_PATH = $Env:GIT_PATH
$GIT_EXE = $GIT_PATH + "\git.exe"      
$GIT_SHA = & "$GIT_EXE" rev-parse HEAD         
$UNITY_PATH = $Env:UNITY_PATH
$WORKSPACE = $Env:WORKSPACE
$BUILD_NUMBER = $Env:BUILD_NUMBER

$logfile = "$WORKSPACE\unitypackage.log"
$unitypackage = "$WORKSPACE\outputpackage\\JusticeSDK-$GIT_SHA-BUILD-$BUILD_NUMBER.unitypackage"         

& "$UNITY_PATH" -quit -batchmode -nographics -projectPath "$WORKSPACE\UnitySampleProject" -logFile $logfile -exportPackage "Assets\JusticeSDK" "Assets\SampleScene" $unitypackage

$lastOutput = ""
while ((get-process "unity" -ea SilentlyContinue) -ne $Null) {
  if(Test-Path $logfile) {
      $content = Get-Content $logfile -Tail 1
      if ( $lastOutput -ne $content) {
          $lastOutput = $content
          echo $content
      }
  }
}