def gitGetRepositoryUrl() {
    return scm.getUserRemoteConfigs()[0].getUrl()
}

def gitGetRepositoryName() {
    return sh(returnStdout: true, script: """
        basename \$(git remote get-url origin) | sed -e "s/.git\$//"
        """).trim()
}

def gitGetCommitHash() { 
    return sh(returnStdout: true, script: "git rev-parse HEAD").trim()
}

def gitGetCommitHashShort() { 
    return  sh(returnStdout: true, script: 'git rev-parse --short=10 HEAD').trim()
}

def gitGetCommitMessage() {
    return sh(returnStdout: true, script: 'git log -1 --pretty=%B').trim()
}

def gitCheckout(gitCredentials, gitUrl, branch = 'master', init = false, submoduleInitUpdate = false) {
    sshagent(credentials: [gitCredentials]) {
        if (isUnix()) {
            if (init) {
                 sh """
                    bash -c "[ -f ~/.ssh/known_hosts ] && ssh-keygen -R bitbucket.org" 
                    bash -c "mkdir -p ~/.ssh && ssh-keyscan -H bitbucket.org >> ~/.ssh/known_hosts"
                    if ! git rev-parse --is-inside-work-tree 2>&1 >/dev/null; then

                        git init .
                    fi
                    if ! git remote add origin ${gitUrl} 2>&1 >/dev/null; then
                         git remote set-url origin ${gitUrl}
                    fi
                """
            }
            sh """
                git -c credential.helper= fetch origin
                git -c credential.helper= checkout -f origin/${branch}
                git -c credential.helper= checkout -B ${branch}
            """
            if (submoduleInitUpdate) {
                sh """
                git -c credential.helper= submodule update --init -f
                """
            }
        }
        else {
            if (init) {
                 bat """
                    bash -c "[ -f ~/.ssh/known_hosts ] && ssh-keygen -R bitbucket.org" 
                    bash -c "mkdir -p ~/.ssh && ssh-keyscan -H bitbucket.org >> ~/.ssh/known_hosts"
                    git rev-parse --is-inside-work-tree > NUL 2>&1
                    if %ERRORLEVEL% NEQ 0 (
                        git init .
                    )
                    git remote add origin ${gitUrl} > NUL 2>&1
                    if %ERRORLEVEL% NEQ 0 (
                        git remote set-url origin ${gitUrl}
                    )
                """
            }
            bat """
                git -c credential.helper= fetch origin
                git -c credential.helper= checkout -f origin/${branch}
                git -c credential.helper= checkout -B ${branch}
            """
            if (submoduleInitUpdate) {
                bat """
                    git -c credential.helper= submodule update --init -f
                """
            }
        }
    }
}

def bitbucketSetBuildStatus(credentials, namespace, repositoryName, commitHash, status) { // status: SUCCESSFUL, INPROGRESS, FAILED
    httpRequest(
        url : "https://api.bitbucket.org/2.0/repositories/${namespace}/${repositoryName}/commit/${commitHash}/statuses/build",
        httpMode: "POST",
        requestBody: "{\"state\": \"${status}\", \"key\": \"build\", \"name\": \"${env.BUILD_TAG}\", \"description\":\"Jenkins Build\",\"url\": \"${env.BUILD_URL}console\" }",
        contentType: "APPLICATION_JSON",
        authentication: credentials,
        validResponseCodes: '200:201'
    )
}

def justiceSdkGetTestEnvironmentVariables(credentials, project, environment)
{
     withCredentials([string(credentialsId: credentials, variable: 'deploymentId')]) {
        url = "https://script.google.com/macros/s/${deploymentId}/exec?project=${project}&environment=${environment}"
        response = httpRequest(url: url)
        return readJSON(text: response.content);
    }
}

return this
