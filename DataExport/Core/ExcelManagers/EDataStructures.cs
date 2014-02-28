using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
namespace DataExport.Core.ExcelManagers
{

    public class EDataColumn:DataColumn
    {
        public bool IsFormula { get; set; }
    }
    public class EDataTable:DataTable
    {

        #region FIELDS
        public enum RelationType
        {
            None = -10,
            Denied = -1
        }
        public int SnapShotID { get; set; }
        public int SnapshotRelationID { get; set; }
        public int TimeSliceRelationID { get; set; }
        public int TimeSliceID { get; set; }
        public bool IsSnapShotTable;
        public bool IsTimeSliceTable;
        #endregion

        #region Constructors
        private EDataTable(){}
        public EDataTable(int snapshot,int timeslice, bool isSnapshotable,bool isTimeslicetable)
        {
            SnapshotRelationID = -10;
            IsTimeSliceTable = isTimeslicetable;
            IsSnapShotTable = isSnapshotable;

            if(isSnapshotable)
            { 
                SnapShotID = snapshot;
                TimeSliceID = -10;
            }

         if(isTimeslicetable)
         {
             TimeSliceID = timeslice;
             SnapShotID = -10;
         }

        
        }

        public EDataTable(int relationId,bool isSnapshot,bool isTimeSlice)
        {
            SnapShotID = -10;
            TimeSliceID = -10;
            if(isSnapshot)
            SnapshotRelationID = relationId;

            if(isTimeSlice)
            TimeSliceRelationID = relationId;

            IsSnapShotTable = false;
            IsTimeSliceTable = false;
        }
        #endregion
    }

    public class  EDataTableDictionary:IDictionary<string, EDataTable>
    {

        private readonly Dictionary<string, EDataTable> _internalDictionary;
        
        public EDataTableDictionary()
    {
        _internalDictionary = new Dictionary<string, EDataTable>();
    }


        
        public void Clear()
        {
            _internalDictionary.Clear();
        }

        public void Normalize()
        {
            var snapshotList = from item in _internalDictionary where item.Value.IsSnapShotTable select item.Value;
            var dataList = from item in _internalDictionary where (!item.Value.IsSnapShotTable && !item.Value.IsTimeSliceTable) select item.Value;
            var eDataTables = snapshotList as EDataTable[] ?? snapshotList.ToArray();
            for(var i = 0; i< eDataTables.Length;i++)
            {
              
              var firstSnapshot = eDataTables.ElementAt(i);
              if (!firstSnapshot.IsSnapShotTable || firstSnapshot.SnapShotID==-1 || firstSnapshot.IsTimeSliceTable) continue;

                var items = from it in eDataTables where (it.IsSnapShotTable && (it.SnapShotID != -1) && (it.SnapShotID != firstSnapshot.SnapShotID)) select it;

                foreach(var equit in items)

                {
                    if (!CompareRows(firstSnapshot, equit)) continue;

                    var changeItems = from chitems in dataList
                                      where chitems.SnapshotRelationID == equit.SnapShotID
                                      select chitems;
                    foreach(var itemc in changeItems)
                    {
                        itemc.SnapshotRelationID = firstSnapshot.SnapShotID;
                            
                    }
                    equit.SnapShotID = -1;
                }

            }

            var toremove = from removeitems in _internalDictionary
                        where removeitems.Value.SnapShotID == -1
                      select removeitems;


            var count = toremove.Count();
            for(var i=0;i<count;i++)
            {
                var ffitem = _internalDictionary.ToList().Find(o => o.Value.SnapShotID == -1);
                _internalDictionary.Remove(ffitem.Key);

            }

      

        }
       
       public bool CompareRows(DataTable table1, DataTable table2)
        {
            var equals = false;
            foreach (DataRow row1 in table1.Rows)
            {
                foreach (DataRow row2 in table2.Rows)
                {
                    var array1 = row1.ItemArray;
                    var array2 = row2.ItemArray;

                    @equals = array1.SequenceEqual(array2);
                }
            }
            return equals;
        }
       public  void AddRange(EDataTableDictionary collection)
       {
           if (collection == null)
           {
               throw new ArgumentNullException("Collection is null");
           }

           foreach (var item in collection)
           {
               if (!_internalDictionary.ContainsKey(item.Key))
               {
                   _internalDictionary.Add(item.Key, item.Value);
               }
               else
               {
                   // handle duplicate key issue here
               }
           }
       }
        #region INTERFACE IMPLEMENTATION
        IEnumerator<KeyValuePair<string, EDataTable>> IEnumerable<KeyValuePair<string, EDataTable>>.GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        public void Add(KeyValuePair<string, EDataTable> item)
        {
            _internalDictionary.Add(item.Key,item.Value);
         
        }

        void ICollection<KeyValuePair<string, EDataTable>>.Clear()
        {
            _internalDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, EDataTable> item)
        {
            return _internalDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, EDataTable>[] array, int arrayIndex)
        {

        }

        public bool Remove(KeyValuePair<string, EDataTable> item)
        {
          return   _internalDictionary.Remove(item.Key);
        }
        int ICollection<KeyValuePair<string, EDataTable>>.Count
        {
            get {return _internalDictionary.Count; }
        }

        bool ICollection<KeyValuePair<string, EDataTable>>.IsReadOnly
        {
            get { return true; }
        }

        public bool ContainsKey(string key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        public void Add(string key, EDataTable value)
        {
            _internalDictionary.Add(key,value);
            
        }

        public bool Remove(string key)
        {
           return _internalDictionary.Remove(key);
        }
        public bool TryGetValue(string key, out EDataTable value)
        {
           return _internalDictionary.TryGetValue(key, out value);
        }
        EDataTable IDictionary<string, EDataTable>.this[string key]
        {
            get { return _internalDictionary[key]; }
            set { _internalDictionary[key] = value; }
        }
        ICollection<string> IDictionary<string, EDataTable>.Keys
        {
            get { return _internalDictionary.Keys; }
        }
        ICollection<EDataTable> IDictionary<string, EDataTable>.Values
        {
            get { return  _internalDictionary.Values; }
        }

        #endregion
    }
}
