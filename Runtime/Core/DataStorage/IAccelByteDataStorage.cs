// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    internal interface IAccelByteDataStorage
    {
        /**
		 * @brief Reset an existing Table in the Storage.
		 *
		 * @param result This will be called when the operation done. The result is bool.
		 * @param tableName optional. The name of the table. Default will drop the default KeyValue table.
		*/
        void Reset(Action<bool> result, string tableName = "DefaultKeyValueTable");

		/**
		 * @brief Delete an Item in the Table.
		 *
		 * @param key The Key of the Item.
		 * @param onDone This will be called when the operation done.
		 * @param tableName optional. The name of the table. Default will drop the default KeyValue table.
		*/
		void DeleteItem(string key, Action<bool> onDone, string tableName = "DefaultKeyValueTable");

		/**
		 * @brief Insert Item to the Key Value Table.
		 *
		 * @param key The Key of the Item.
		 * @param item The Data to be inserted to the Table. The Data would be a FJsonObjectWrapper.
		 * @param onDone This will be called when the operation done. The result is bool.
		 * @param tableName optional. The name of the table. Default will insert an item to the default KeyValue table.
		*/
		void SaveItem(string key, object item, Action<bool> onDone, string tableName = "DefaultKeyValueTable");

        /**
		 * @brief Insert multiple item to the Key Value Table.
		 *
		 * @param keyItemPairs Combination of Key and Item.
		 * @param onDone This will be called when the operation done. The result is bool.
		 * @param tableName optional. The name of the table. Default will insert an item to the default KeyValue table.
		*/
        void SaveItems(System.Collections.Generic.List<Tuple<string, object>> keyItemPairs, Action<bool> onDone, string tableName = "DefaultKeyValueTable");

        /**
		 * @brief Get an Item from the Key Value Table.
		 *
		 * @param key The Key of the Item to Get.
		 * @param onDone This will be called when the operation done. The result is Pair of a bool Success, and a FJsonObjectWrapper Value.
		 * @param tableName optional. The name of the table. Default will get an item from the default KeyValue table.
		*/
        void GetItem<T>(string key, Action<bool, T> onDone, string tableName = "DefaultKeyValueTable");
    }
}