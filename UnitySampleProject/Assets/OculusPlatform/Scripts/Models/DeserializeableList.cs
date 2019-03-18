namespace Oculus.Platform.Models
{
  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;
  using Models;

  public class DeserializableList<T> : IList<T>
  {

    //IList
    public int Count { get {return this._Data.Count;} }
    bool ICollection<T>.IsReadOnly { get {return ((IList<T>) this._Data).IsReadOnly;} } //if you insist in getting it...
    public int IndexOf(T obj) {return this._Data.IndexOf(obj);}
    public T this[int index] { get{return this._Data[index];} set{ this._Data[index] = value;} }

    public void Add(T item) { this._Data.Add(item);}
    public void Clear() { this._Data.Clear();}
    public bool Contains(T item) {return this._Data.Contains(item);}
    public void CopyTo(T[] array, int arrayIndex) { this._Data.CopyTo(array, arrayIndex);}
    public IEnumerator<T> GetEnumerator() {return this._Data.GetEnumerator();}
    public void Insert(int index, T item) { this._Data.Insert(index, item);}
    public bool Remove(T item) {return this._Data.Remove(item);}
    public void RemoveAt(int index) { this._Data.RemoveAt(index);}

    // taken from examples here: https://msdn.microsoft.com/en-us/library/s793z9y2(v=vs.110).aspx
    private IEnumerator GetEnumerator1()
    {
      return GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator1();
    }

    // Internals and getters

    // Seems like Obsolete properties are broken in this version of Mono.
    // Anyway, don't use this.
    [System.Obsolete("Use IList interface on the DeserializableList object instead.", false)]
    public List<T> Data {
      get {return this._Data;}
    }

    protected List<T> _Data;
    protected string  _NextUrl;
    protected string  _PreviousUrl;

    public bool   HasNextPage     { get { return !System.String.IsNullOrEmpty(this.NextUrl);     } }
    public bool   HasPreviousPage { get { return !System.String.IsNullOrEmpty(this.PreviousUrl); } }
    public string NextUrl         { get { return this._NextUrl;                                  } }
    public string PreviousUrl     { get { return this._PreviousUrl;                              } }
  }
}
