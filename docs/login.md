## A. Login with Device ID

Login with device ID is used to login without registering first. To login with device id:
    <!-- language: lang-cs -->

    var user = AccelBytePlugin.GetUserAccount();
    user.LoginWithDeviceId(result =>
        {
            if (result.IsError)
            {
                //handle error
            }
            else
            {
                //on login successful
            }
        });

## B. Login with Platform

Login with third party platforms (Steam, Google, Facebook, etc) will implicitly register a user at first login. To login with a platform:
<!-- language: lang-cs -->

    //Login with Steam ticket
    if (SteamManager.Initialized){
        var user = AccelBytePlugin.GetUserAccount();
        var steamAuthTicket = "012345567890ABCDEF";
        user.Login(
            PlatformType.Steam,
            steamAuthTicket,
            result =>
            {
                if (result.IsError)
                {
                    //handle error
                }
                else
                {
                    //on login successful
                }
            });
    }

    //Login with Facebook
    var user = AccelBytePlugin.GetUserAccount();
    var fbCode = "facebook-code-returned-by-facebook-connect";
    user.Login(
        PlatformType.Facebook,
        fbCode,
        result =>
        {
            if (result.IsError)
            {
                //handle error
            }
            else
            {
                //on login successful
            }
        });

## C. Register With Email

Registration steps by email:

1. Registering email account

    <!-- language: lang-cs -->
        var user = AccelBytePlugin.GetUserManagement();
        user.CreateUserAccount(email, password, displayName,
            result =>
            {
                //On email registered
            });

1. Login With Email

    <!-- language: lang-cs -->
        UserAccount user = AccelBytePlugin.GetUserAccount();
        user.LoginWithUsernameAndPassword(email, password,
            result =>
            {
                if (!result.IsError)
                {
                    Log("Login Success");

                }
                else
                {
                    Log("Login Failed, Error Msg:" + result.Error.Message );
                }
            });

1. Verifying email account

    Before send verification code, user should login first. Verification code obtained from email sent when calling Register method. And then user can Verify their account with the code that obtained from email.

    <!-- language: lang-cs -->
        var userManagement = AccelBytePlugin.GetUserManagement();
        userManagement.SendUserRegistrationVerificationCode(AccelBytePlugin.GetUserAccount(), result => 
        {
            userManagement.VerifyUserAccount(AccelBytePlugin.GetUserAccount(), verificationCode, innerResult =>
            {
                if (result.IsError)
                {
                    Log("Verify user failed: " + result.Error.Message);
                }
                else
                {
                    Log("Verify user successful");
                }
            });
        })

## D. Reset Password

Reset password is done in 2 steps:

1. Forgot Password

    <!-- language: lang-cs -->
        var userManagement = AccelBytePlugin.GetUserManagement();
        userManagement.SendPasswordResetCode(email, result =>
            {
                if (!result.IsError)
                {
                    Log("Forgot Password Success");

                }
                else
                {
                    Log("Forgot Password Failed, Error Msg:" + result.Error.Message );
                }
            });

2. Reset Password

    <!-- language: lang-cs -->
        var userManagement = AccelBytePlugin.GetUserManagement();
        user.ResetPassword(resetCodeFromEmail, email, newPassword, result =>
            {
                if (!result.IsError)
                {
                    Log("Forgot Password Success");

                }
                else
                {
                    Log("Forgot Password Failed, Error Msg:" + result.Error.Message );
                }
            });

## D. Login From Launcher

If you are starting the game from our Launcher, you need to call this method:
    <!-- language: lang-cs -->
        var user = AccelBytePlugin.GetUserAccount();
        user.LoginFromLauncher(result =>
            {
                if (!result.IsError)
                {
                    Log("Login Success");

                }
                else
                {
                    Log("Login Failed, Error Msg:" + result.Error.Message );
                }
            });

## E. Upgrading an account

If an account login with device ID or platform, it can be upgraded to proper email account. User should be logged in first. To upgrade into email account:
    <!-- language: lang-cs -->
        var userManagement = AccelBytePlugin.GetUserManagement();
        userManagement.AddUserNameAndPassword(AccelBytePlugin.GetUserAccount(), email, password,
        result =>
        {
            if (!result.IsError)
            {
                Log("Upgrade successful");

            }
            else
            {
                Log("Upgrade failed, Error Msg:" + result.Error.Message );
            }
        });
