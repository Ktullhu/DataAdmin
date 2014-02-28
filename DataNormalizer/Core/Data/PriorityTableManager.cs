using System;
using System.Collections.Generic;
using System.Linq;
using DataNormalizer.Core.Service;

namespace DataNormalizer.Core.Data
{
   public static class PriorityTableManager
   {
       public static CollectPriorityTable _CollectPriorityTable;
       private static void InitSymbols()
       {
           var symbols = DataManager.GetSymbols();
           
           foreach(var symbol in symbols)
           {
              
           }
       }
       private static void AddPriorityList(Dictionary<string, List<CollectorClient>> priorList)
       {
           var id = 0;
           foreach (var item in priorList)
           {
               var symbRow = _CollectPriorityTable.Symbols.NewRow();
               symbRow["Name"] = item.Key;
               _CollectPriorityTable.Symbols.Rows.Add(symbRow);
               
               foreach (var user in item.Value.OrderByDescending(oo=> oo.DepthValue))
               {
                   var symbolRow = _CollectPriorityTable.CollectPriority.NewRow();
                   symbolRow["symbol_id"] = id;
                   symbolRow["Client"] = user.UserName;
                   symbolRow["Depth"] = user.DepthValue;
                   _CollectPriorityTable.CollectPriority.Rows.Add(symbolRow);
                   _CollectPriorityTable.AcceptChanges();
               }
               id++;
           }
       }
       private static void CreateRelations()
       {
           _CollectPriorityTable.Relations.Add("1", _CollectPriorityTable.Tables["Symbols"].Columns["ID"],
                _CollectPriorityTable.Tables["CollectPriority"].Columns["symbol_id"], false);
          
       }
       public static void Initialize()
       {
           _CollectPriorityTable = new CollectPriorityTable();
            CreateRelations();
       }
       public static void SetUpTheTables(Dictionary<string, List<CollectorClient>> priorList)
       {
           Clear();
         
           AddPriorityList(priorList);
       
       }      
       private static void Clear()
       {
           _CollectPriorityTable = new CollectPriorityTable();
           CreateRelations();
       }
   }
}
