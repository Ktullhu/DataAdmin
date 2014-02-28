using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DataExport.Core.ExcelManagers
{

  public  class EStyleManager
    {
        public Color HeaderBackColor = Color.LightGray;
        public Color HeaderForeColor = Color.Black;
        public Color ItemForeColor = Color.Black;
        public Color ItemBackColor = Color.White;
        public Color ItemAlternateBackColor = Color.White;
        public string FontName = "Verdana";
        public bool ItemFontBold = false;
        public bool HeaderFontBold = true;
        public bool ItemItalic = false;
        public bool HeaderItalic = false;
        public ushort FontSize = 9;
        public int ColumnSpace = 1;
        public int RowSpace = 1;
        public int ColumnSpaceBetweenTables = 0;
        public int RowSpaceBetweenTables = 2;
        public bool RepeatColumnHeader = true;
    }


  public enum ExportStyle
  {
      RowWise = 0,
      ColumnWise = 1,
      SheetWise = 2
  }



}
