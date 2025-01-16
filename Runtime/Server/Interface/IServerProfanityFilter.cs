// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server.Interface
{
    public interface IServerProfanityFilter
    {
        /// <summary>
        /// Query all profanity words on the current namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void QueryProfanityWords(ResultCallback<QueryProfanityWordsResponse> callback);

        /// <summary>
        /// Query all profanity words on the current namespace with optional filters and parameters.
        /// </summary>
        /// <param name="optionalParams">Optional parameters to filter query request. Can be null.</param>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void QueryProfanityWords(QueryProfanityWordsOptionalParameters optionalParams
            , ResultCallback<QueryProfanityWordsResponse> callback);

        /// <summary>
        /// Insert a profanity word in the dictionary for the current namespace.
        /// </summary>
        /// <param name="word">Value for profanity word.</param>
        /// <param name="falseNegatives">False negatives to filter for the word.</param>
        /// <param name="falsePositives">False positives to ignore for the word.</param>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void CreateProfanityWord(string word
            , string[] falseNegatives
            , string[] falsePositives
            , ResultCallback<ProfanityDictionaryEntry> callback);

        /// <summary>
        /// Insert many profanity words in the dictionary for the current namespace.
        /// </summary>
        /// <param name="bulkCreateRequest">Data for each profanity word entry to be created.</param>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void BulkCreateProfanityWords(CreateProfanityWordRequest[] bulkCreateRequest, ResultCallback callback);

        /// <summary>
        /// Get all profanity word groups on the current namespace.
        /// </summary>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void GetProfanityWordGroups(ResultCallback<ProfanityWordGroupResponse> callback);

        /// <summary>
        /// Get all profanity word groups on the current namespace with optional parameters.
        /// </summary>
        /// <param name="optionalParameters">Optional parameters to filter get request. Can be null.</param>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void GetProfanityWordGroups(GetProfanityWordGroupsOptionalParameters optionalParameters
            , ResultCallback<ProfanityWordGroupResponse> callback);

        /// <summary>
        /// Update a profanity word in the dictionary for the current namespace.
        /// </summary>
        /// <param name="id">Id of profanity word to update.</param>
        /// <param name="word">New value for profanity word.</param>
        /// <param name="falseNegatives">False negatives to filter for the word.</param>
        /// <param name="falsePositives">False positives to ignore for the word.</param>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void UpdateProfanityWord(string id
            , string word
            , string[] falseNegatives
            , string[] falsePositives
            , ResultCallback<ProfanityDictionaryEntry> callback);

        /// <summary>
        /// Delete a profanity word by id in the dictionary for the current namespace.
        /// </summary>
        /// <param name="id">Id of profanity word to delete.</param>
        /// <param name="callback">Returns a result via callback when operation is finished.</param>
        public void DeleteProfanityWord(string id, ResultCallback callback);
    }
}
