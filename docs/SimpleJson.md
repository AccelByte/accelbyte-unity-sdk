<h1> SimpleJson </h1>
<h3> Overview </h3>
Our SDK use Facebook's json parser and serializer for .NET framework. We already tested SimpleJson to make sure our SDK is compatible with SimpleJson and we decided to use it. Here's the [link](https://github.com/facebook-csharp-sdk/simple-json/) to get the SimpleJson.
<h3> Test Result </h3>
Here's the model of object that we used for testing. 
<!-- language lang-cs -->

    [DataContract]
    public class Category
    { 
        [DataMember] public string @namespace { get; set; } 
        [DataMember] public string parentCategoryPath { get; set; } 
        [DataMember] public string categoryPath { get; set; } 
        [DataMember] public string displayName { get; set; } 
        [DataMember] public Category[] childCategories { get; set; } 
        [DataMember] public string createdAt { get; set; } 
        [DataMember] public string updatedAt { get; set; } 
        [DataMember] public bool root { get; set; } 
    }

* Required time to deserialize 10000 jsons is 1232.91339999999 milisecond.
* Required time to serialize 10000 objects is 2.8552 milisecond.
* Average time to deserialize a single json object is .123291339999999 milisecond.
* Average time to serialize a single object is .00028552 milisecond.

Based on our test, it supports serialization of NULL field, empty string, nested object, and array of an object (directly). It also can deserialize a json object with a missing field, an empty string field, an array of object, and a nested object.
<h3> License </h3>
SimpleJson is a software under a MIT license. Please read the term and condition carefully before take a further action with this third-party's script. Here is the [link](https://github.com/facebook-csharp-sdk/simple-json/blob/master/LICENSE.txt).