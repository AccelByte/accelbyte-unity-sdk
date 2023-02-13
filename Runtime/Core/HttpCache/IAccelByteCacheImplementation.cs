// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal interface IAccelByteCacheImplementation<FileTypeT>
    {
        /**
	    * @brief Check if the class stores an item with specified key
	    *
	    * @param key Identifier of the data
	    * @return True if the key is found
	    */
        bool Contains(string key);

        /**
        * @brief Cleanup the storage
        */
        void Empty();

        /**
        * @brief Remove a data from the class
        *
        * @param key Identifier of the data
        * @return True if the deleted data exist
        * @return True if the removal success
        */
        bool Remove(string key);

        /**
        * @brief Write/replace an item with specified key
        *
        * @param key Identifier of the data
        * @return True if the key is found
        */
        bool Emplace(string key, FileTypeT item);

        /**
        * @brief Update an item with specified key
        *
        * @param key Identifier of the data
        * @return True if the key is found
        */
        bool Update(string key, FileTypeT item);

        /**
        * @brief Obtain and access the data
        *
        * @param key Identifier of the data
        * @return Reference to the data.
        */
        FileTypeT Retrieve(string key);

        /**
        * @brief Obtain the data, only for update the data. Used for some cases like LRU algorithm.
        *
        * @param key Identifier of the data
        * @return Reference to the data.
        */
        FileTypeT Peek(string key);
    }
}
