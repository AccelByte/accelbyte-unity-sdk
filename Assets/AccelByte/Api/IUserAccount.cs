// Copyright (c) 2019-2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    internal interface IUserAccount
    {
        IEnumerator Register(RegisterUserRequest registerUserRequest, ResultCallback<RegisterUserResponse> callback);

        IEnumerator Registerv2(RegisterUserRequestv2 registerUserRequest, ResultCallback<RegisterUserResponse> callback);

        IEnumerator GetData(ResultCallback<UserData> callback);

        IEnumerator Update(UpdateUserRequest updateUserRequest, ResultCallback<UserData> callback);

        IEnumerator UpdateEmail(UpdateEmailRequest updateUserRequest, ResultCallback callback);

        IEnumerator Upgrade(string username, string password, ResultCallback<UserData> callback);

        IEnumerator Upgradev2(string emailAddress, string username, string password, ResultCallback<UserData> callback);

        IEnumerator SendVerificationCode(VerificationContext context, string username, ResultCallback callback);

        IEnumerator Verify(string verificationCode, string contactType, ResultCallback callback);

        IEnumerator SendPasswordResetCode(string username, ResultCallback callback);

        IEnumerator ResetPassword(string resetCode, string username, string newPassword, ResultCallback callback);

        IEnumerator LinkOtherPlatform(PlatformType platformType, string ticket, ResultCallback callback);

        IEnumerator ForcedLinkOtherPlatform(PlatformType platformType, string platformUserId, ResultCallback callback);

        IEnumerator UnlinkOtherPlatform(PlatformType platformType, ResultCallback callback, string namespace_ = "");

        IEnumerator GetPlatformLinks(ResultCallback<PagedPlatformLinks> callback);

        IEnumerator SearchUsers(string query, SearchType by, ResultCallback<PagedPublicUsersInfo> callback);

        IEnumerator GetUserByUserId(string userId, ResultCallback<PublicUserData> callback);

        IEnumerator GetUserByOtherPlatformUserId(PlatformType platformType, string otherPlatformUserId,
            ResultCallback<UserData> callback);

        IEnumerator BulkGetUserByOtherPlatformUserIds(PlatformType platformType, BulkPlatformUserIdRequest otherPlatformUserId,
            ResultCallback<BulkPlatformUserIdResponse> callback);

        IEnumerator GetCountryFromIP(ResultCallback<CountryInfo> callback);

        IEnumerator BanUser(string @namespace, string accessToken, string userId, BanCreateRequest banRequest,
            ResultCallback<UserBanResponseV3> callback);
    }
}