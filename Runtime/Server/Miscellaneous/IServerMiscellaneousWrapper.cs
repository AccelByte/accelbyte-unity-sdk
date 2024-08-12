// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    public interface IServerMiscellaneousWrapper : ICommonMiscellaneousWrapper
    {
        /// <summary>
        /// List country groups. Will return all available country groups.
        /// </summary>
        void ListCountryGroups(ResultCallback<CountryGroup[]> callback);
        
        /// <summary>
        /// List country groups. Will return all available country groups if the groupCode is not specified.
        /// </summary>
        /// <param name="groupCode">Only accept alphabet and whitespace</param>
        void ListCountryGroups(string groupCode, ResultCallback<CountryGroup[]> callback);

        /// <summary>
        /// Add a country groups
        /// </summary>
        public void AddCountryGroup(CountryGroup newCountryGroupData, ResultCallback<CountryGroup> callback = null);

        /// <summary>
        /// Update a country groups. The countryGroupCode must be exist beforehand.
        /// Valid update behaviour :
        /// - To update countryGroupName only, do not include Countries key or just specify it with empty array.
        /// - To update countries only, do not include CountryGroupName key or just specify it with blank value.
        /// </summary>
        /// <param name="groupCode">groupCode, only accept alphabet and whitespace</param>
        public void UpdateCountryGroup(string groupCode, CountryGroup newCountryGroupData,
            ResultCallback<CountryGroup> callback = null);

        /// <summary>
        /// Delete a country groups by its country group code.
        /// </summary>
        /// <param name="groupCode">groupCode, only accept alphabet and whitespace</param>
        public void DeleteCountryGroup(string groupCode = null, ResultCallback callback = null);
    }
}