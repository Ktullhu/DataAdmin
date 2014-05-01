using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DataExport.Controls
{

    public partial class ElementContainerControl : UserControl
    {
        private List<ElementControl> _elements = new List<ElementControl>();

        public ElementContainerControl()
        {
            InitializeComponent();
            ElementsColor = Color.SteelBlue;
            panelExElementsContainer.BackColor = Color.White;
        }

        private int _elementHeight = 35;

        public int ElementHeight
        {
            get { return _elementHeight; }
            set { _elementHeight = value; }
        }

        public string Title
        {
            get { return labelXTitle.Text; }
            set { labelXTitle.Text = value; }
        }
        public int Count
        {
            get { return _elements.Count; }
        }
        private int _selectedIndex = -1;
        private Color _elementsColor;
        public Color ElementsColor
        {
            get { return _elementsColor; }
            set
            {
                _elementsColor = value;
                ChangeColors(value);
            }
        }
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            private set { _selectedIndex = value; OnSelectedIndexChanged(this, new ElementEventArgs { Index = value }); }
        }

        private void ChangeColors(Color newColor)
        {
            labelXTitle.BackgroundStyle.BorderTopColor = newColor;
            foreach (var item in _elements)
            {
                item.ElementColor = newColor;
            }
        }

        public delegate void SelectedIndexChangedHandler(object sender, ElementEventArgs e);
        public event SelectedIndexChangedHandler SelectedIndexChanged;
        public void OnSelectedIndexChanged(object sender, ElementEventArgs e)
        {
            foreach (var item in _elements)
            {
                item.SetSelected(item.Index == e.Index);
            }
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(sender, e);
            }
        }

        void el_ButtonClick(object sender, ElementEventArgs e)
        {
            SelectedIndex = e.Index;
        }

        public void AddElement(string text)
        {
            var index = _elements.Count;
            var el = new ElementControl
            {
                Index = index,
                LabeledText = text,
                Dock = DockStyle.Top,
                ElementColor = ElementsColor,
                Height = ElementHeight
            };
            el.ButtonClick += el_ButtonClick;
            panelExElementsContainer.Controls.Add(el);
            _elements.Add(el);

            SelectedIndex = index;
        }

        public void RemoveElement(int index)
        {
            if (index >= _elements.Count)
                throw new IndexOutOfRangeException();
            var el = _elements[index];

            _elements.Remove(el);
            panelExElementsContainer.Controls.Remove(el);
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Index = i;
            }
            SelectedIndex = Count - 1;
        }

        public void ClearElements()
        {
            while (_elements.Count > 0)
            {
                var item = _elements[0];
                _elements.Remove(item);
                panelExElementsContainer.Controls.Remove(item);
                if (item != null) item.Dispose();
            }
        }

        public string GetText(int index)
        {
            if (index >= Count)
                throw new IndexOutOfRangeException();
            return _elements[index].LabeledText;
        }

        public void SetSelected(int index)
        {
            if (index >= Count)
                throw new IndexOutOfRangeException();

            foreach (var item in _elements)
            {
                item.SetSelected(item.Index == index);
            }

            SelectedIndex = index;
        }

        public void SetText(int index, string text)
        {
            if (index >= Count)
                throw new IndexOutOfRangeException();
            _elements[index].LabeledText = text;
        }

        public void Repaint()
        {
            foreach (var item in panelExElementsContainer.Controls)
            {
                var btn = (item as ElementControl);
                if (btn != null)
                {
                    btn.Repaint();
                }
            }
        }

        public void SetIncorrect(int index, bool isIncorrect)
        {
            var element = (panelExElementsContainer.Controls[index] as ElementControl);
            if (element != null) element.SetIncorrect(isIncorrect);
        }
    }
}