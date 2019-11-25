// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api {
    internal interface IUserAccount {
        IEnumerator Register(RegisterUserRequest registerUserRequest, ResultCallback<RegisterUserResponse> callback);
        IEnumerator GetData(ResultCallback<UserData> callback);

        IEnumerator Update(UpdateUserRequest updateUserRequest,
            ResultCallback<UserData> callback);

        IEnumerator Upgrade(string username, string password,
            ResultCallback<UserData> callback);

        IEnumerator SendVerificationCode(VerificationContext context, string username,
            ResultCallback callback);

        IEnumerator Verify(string verificationCode, string contactType,
            ResultCallback callback);

        IEnumerator SendPasswordResetCode(string username, ResultCallback callback);
        IEnumerator ResetPassword(string resetCode, string username, string newPassword, ResultCallback callback);

        IEnumerator LinkOtherPlatform(string platformId, string ticket,
            ResultCallback callback);

        IEnumerator UnlinkOtherPlatform(string platformId, ResultCallback callback);
        IEnumerator GetPlatformLinks(ResultCallback<PagedPlatformLinks> callback);

        IEnumerator GetUserByEmailAddress(string emailAdress, ResultCallback<PagedPublicUsersInfo> callback);

        IEnumerator GetUserByUserId(string userId, ResultCallback<UserData> callback);
    }
}