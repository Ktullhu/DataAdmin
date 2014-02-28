using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RozkladCommon.Models;

namespace RozkladCommon.Controls
{
    public partial class StyledLessonsListControl : UserControl
    {
        public StyledLessonsListControl()
        {
            InitializeComponent();
            _lessons = new List<LessonModel>
            {
                new LessonModel {Predmet = "", Prepod = "", Aud = "", NoTitle = "1"},
                new LessonModel {Predmet = "", Prepod = "", Aud = "", NoTitle = "2"},
                new LessonModel {Predmet = "", Prepod = "", Aud = "", NoTitle = "3"},
                new LessonModel {Predmet = "", Prepod = "", Aud = "", NoTitle = "4"}
            };
            UpdateModel();

            oneLessonControl1.TextChanged += oneLessonControl1_TextChanged;
            oneLessonControl2.TextChanged += oneLessonControl1_TextChanged;
            oneLessonControl3.TextChanged += oneLessonControl1_TextChanged;
            oneLessonControl3.TextChanged += oneLessonControl1_TextChanged;
        }
        
        void oneLessonControl1_TextChanged(object sender, System.EventArgs e)
        {
            _lessons[0].Aud = oneLessonControl1.Auditoria;
            _lessons[0].Predmet = oneLessonControl1.Predmet;
            _lessons[0].Prepod = oneLessonControl1.Prepod;
            //_lessons[0].NoTitle = oneLessonControl1.NoTitle;

            _lessons[1].Aud = oneLessonControl2.Auditoria;
            _lessons[1].Predmet = oneLessonControl2.Predmet;
            _lessons[1].Prepod = oneLessonControl2.Prepod;
            //_lessons[1].NoTitle = oneLessonControl2.NoTitle;

            _lessons[2].Aud = oneLessonControl3.Auditoria;
            _lessons[2].Predmet = oneLessonControl3.Predmet;
            _lessons[2].Prepod = oneLessonControl3.Prepod;
           // _lessons[2].NoTitle = oneLessonControl3.NoTitle;

            _lessons[3].Aud = oneLessonControl4.Auditoria;
            _lessons[3].Predmet = oneLessonControl4.Predmet;
            _lessons[3].Prepod = oneLessonControl4.Prepod;
           // _lessons[3].NoTitle = oneLessonControl4.NoTitle;

            OnTextChanged(e);    
        }

        public string Day
        {
            get { return labelX_day.Text; }
            set { labelX_day.Text = value; }
        }

        private List<LessonModel> _lessons;

        public List<LessonModel> GetLessons()
        {
            return _lessons.ToList();
        }

        public void SetLessons(List<LessonModel> lessons)
        {
            _lessons = lessons.ToList();
            UpdateModel();
        }
        private void UpdateModel()
        {
            if (_lessons == null)
            {                
                return;
            }
            
            oneLessonControl1.Auditoria = _lessons[0].Aud;
            oneLessonControl1.Predmet = _lessons[0].Predmet;
            oneLessonControl1.Prepod = _lessons[0].Prepod;
            //oneLessonControl1.NoTitle = _lessons[0].NoTitle;

            oneLessonControl2.Auditoria = _lessons[1].Aud;
            oneLessonControl2.Predmet = _lessons[1].Predmet;
            oneLessonControl2.Prepod = _lessons[1].Prepod;
           // oneLessonControl2.NoTitle = _lessons[1].NoTitle;

            oneLessonControl3.Auditoria = _lessons[2].Aud;
            oneLessonControl3.Predmet = _lessons[2].Predmet;
            oneLessonControl3.Prepod = _lessons[2].Prepod;
            //oneLessonControl3.NoTitle = _lessons[2].NoTitle;

            oneLessonControl4.Auditoria = _lessons[3].Aud;
            oneLessonControl4.Predmet = _lessons[3].Predmet;
            oneLessonControl4.Prepod = _lessons[3].Prepod;
            //oneLessonControl4.NoTitle = _lessons[3].NoTitle;

            panelEx_container.Size = new Size(100, 43);

            
        }      
        

        
    }
}
