// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Text;


namespace AccelByte.Core
{
    public class TagQueryBuilder
    {
        private static readonly string orOperator = "|";
        private static readonly string andOperator = "&";
        private static readonly string openParentheses = "(";
        private static readonly string closeParentheses = ")";
        
        private readonly Queue<object> tagObjects = new Queue<object>();
        private readonly Queue<string> operationOperators = new Queue<string>();
        
        public static TagQueryBuilder Start(string tag)
        {
            CheckForEmptyString(tag);
            return new TagQueryBuilder(tag);
        }

        public static TagQueryBuilder Start(TagQueryBuilder tagQueryBuilder)
        {
            CheckNullTagBuilder(tagQueryBuilder);
            return new TagQueryBuilder(tagQueryBuilder);
        }

        private TagQueryBuilder(string tag)
        {
            tagObjects.Enqueue(tag);
        }
        
        private TagQueryBuilder(TagQueryBuilder tagQueryBuilder)
        {
            tagObjects.Enqueue(tagQueryBuilder);
        }

        public TagQueryBuilder Or(string tag)
        {
            CheckForEmptyString(tag);
            Or_(tag);
            return this;
        }

        public TagQueryBuilder Or(TagQueryBuilder tagQueryBuilder)
        {
            CheckNullTagBuilder(tagQueryBuilder);
            Or_(tagQueryBuilder);
            return this;
        }

        private void Or_(object tagObject)
        {
            tagObjects.Enqueue(tagObject);
            operationOperators.Enqueue(TagQueryBuilder.orOperator);
        }

        public TagQueryBuilder And(string tag)
        {
            CheckForEmptyString(tag);
            And_(tag);
            return this;
        }

        public TagQueryBuilder And(TagQueryBuilder tagQueryBuilder)
        {
            CheckNullTagBuilder(tagQueryBuilder);
            And_(tagQueryBuilder);
            return this;
        }

        private void And_(object tagObject)
        {
            tagObjects.Enqueue(tagObject);
            operationOperators.Enqueue(TagQueryBuilder.andOperator);
        }

        internal string Build()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(StringifyTagObject(tagObjects.Dequeue()));
            
            while (tagObjects.Count > 0)
            {
                var tag = StringifyTagObject(tagObjects.Dequeue());
                var opr = operationOperators.Dequeue();

                stringBuilder.Append(opr);
                stringBuilder.Append(tag);
            }
            
            tagObjects.Clear();
            operationOperators.Clear();
            
            return stringBuilder.ToString();
        }

        private static string StringifyTagObject(object tagObject)
        {
            switch (tagObject)
            {
                case string s:
                    return s;
                case TagQueryBuilder tagBuilder:
                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append(TagQueryBuilder.openParentheses);
                    stringBuilder.Append(tagBuilder.Build());
                    stringBuilder.Append(TagQueryBuilder.closeParentheses);
                    
                    return stringBuilder.ToString();
                default:
                    throw new NotSupportedException("Tag object type not supported");
            }
        }

        private static void CheckForEmptyString(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("Invalid empty string parameter");
            }
        }

        private static void CheckNullTagBuilder(TagQueryBuilder tagQueryBuilder)
        {
            if (tagQueryBuilder is null)
            {
                throw new NullReferenceException("Tag builder argument is null");
            }
        }
    }
}