using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using DADataManager.Models;

namespace DataNetClient.Controls
{
    public partial class SymbolItem : UserControl
    {

        public bool _isClick = false; 

        public SymbolItem()
        {
            InitializeComponent();
            Dock = DockStyle.Top;
            ItemColor = Color.White;
        }

        public bool IsClicked()
        {
            return _isClick;
        }

        public SymbolItem(string symbolName)
        {
            InitializeComponent();
          //  panelEx_main.
            Dock = DockStyle.Top;
            Symbol = symbolName;
            ItemState = " ";
            ItemColor = Color.White;


        }

        public string Symbol
        {
            get { return labelX_name.Text; }
            set { labelX_name.Text = value; }
        }
        public string ItemState
        {
            get { return labelX_symbol.Text; }
            set { labelX_symbol.Text = value; }
        }
        public Color ItemColor
        {
            get { return panelEx_main.Style.BackColor1.Color; }
            set { panelEx_main.Style.BackColor1.Color = value; }
        }

        private void panelEx_main_Click(object sender, EventArgs )
        {
            if (!_isClick)
            {

                foreach (var group in groups)
                {
                    if (group.AllSymbols.Exists(oo => oo == Symbol))
                    {
                        ItemColor = Color.Red;
                        ItemState = "-";
                        _isClick = true;
                        break;
                    }
                    ItemColor = Color.Green;
                    ItemState = "+";    

                }
                //ItemColor = Color.Blue;
              //  _isClick = true;
            }

            else
            {
                ItemColor = Color.White;
                _isClick = false;
            }
        }



    }
}
