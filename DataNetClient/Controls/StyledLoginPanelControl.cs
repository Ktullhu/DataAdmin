using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar;
using HtmlAgilityPack;

namespace RozkladCommon.Controls
{
    [Designer(typeof(UserControlDesigner))]
    public partial class StyledLoginPanelControl : UserControl
    {
        public StyledLoginPanelControl()
        {
            InitializeComponent();
            ToastNotification.DefaultToastPosition = eToastPosition.BottomCenter;
        }
        public string InputText
        {
            get
            {
                return label1.Text;

            }
            set
            {
                label1.Text = value;
            }
        }

        // define a property called "DropZone"
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PanelEx DropZone
        {
            get { return panelEx_back; }
        }

        public delegate void OnLoginClickHandler(Object sender, EventArgs e);
        public event  OnLoginClickHandler LoginClick;
        public string VkHref;
        public string VkName;
        public string VkPhoto;

        protected virtual void OnLoginClick()
        {
            OnLoginClickHandler handler = LoginClick;
            if (handler != null) handler(this, new EventArgs());
        }

        #region Mouse

        private void labelX_loginFacebook_MouseLeave(object sender, EventArgs e)
        {

            labelX_loginFacebook.ForeColor = Color.White;
        }

        private void labelX_loginFacebook_MouseMove(object sender, MouseEventArgs e)
        {
            labelX_loginFacebook.ForeColor = Color.FromArgb(255, 0, 191, 243);
        }

        private void labelX_loginTwitter_MouseLeave(object sender, EventArgs e)
        {
            labelX_loginTwitter.ForeColor = Color.White;
        }

        private void labelX_loginTwitter_MouseMove(object sender, MouseEventArgs e)
        {
            labelX_loginTwitter.ForeColor = Color.FromArgb(255, 0, 191, 243);
        }
        
        #endregion

        #region Login Click

        private void panelEx_loginButton_Click(object sender, EventArgs e)
        {
            styledLoadAnimation1.StartAnimation();
            TryLogin();
        }

        private void styledLoginTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                styledLoadAnimation1.StartAnimation();
                TryLogin();
            }
        }

        private void labelX_loginVk_Click(object sender, EventArgs e)
        {
            styledLoadAnimation1.StartAnimation();
            TryLoginVk();
        }

        private void labelX_loginFacebook_Click(object sender, EventArgs e)
        {
            styledLoadAnimation1.StartAnimation();
            TryLoginFb();
        }
        
        #endregion

        #region Login ITF-CSN, FACEBOOK, VK

        private void TryLogin()
        {
            
            if (styledLoginTextBox1.InputText == "demo" && styledPasswordTextBox1.InputText == "demo")
            {
                VkName = "demo demo";
                OnLoginClick();
            }
            else
            {
                ToastNotification.Show(panelEx_back, "Неправильний логін або пароль. Спробуйте знову.");
            }
        }
        private void TryLoginFb()
        {

            TryLoginVk();
        }
        private void TryLoginVk()
        {
            
            string login = styledLoginTextBox1.InputText;
            string pass = styledPasswordTextBox1.InputText;
            bool avt = http_auth_vk(login, pass);
            if (avt)
            {
                OnLoginClick();

            }
            else
            {
                ToastNotification.Show(panelEx_back, "Неправильний логін або пароль. Спробуйте знову");
            }
        }

        public bool http_auth_vk(string login, string pass)
        {
            try
            {
                //*****************************
                //Получаем action_url
                //*****************************
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var reqGET = WebRequest.Create("http://m.vk.com/");
                var resp = reqGET.GetResponse();
                var stream = resp.GetResponseStream();
                if (stream != null)
                {
                    var sr = new StreamReader(stream);
                    string s = sr.ReadToEnd();


                    //*****************************
                    //Парсим
                    //*****************************
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(s);
                    HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//div[@class='form_item fi_fat']/form");
                    string result1 = bodyNode.Attributes["action"].Value;

                    //*****************************
                    //POST запрос
                    //*****************************
                    var cookies = new CookieContainer();
                    ServicePointManager.Expect100Continue = false;
                    var request = (HttpWebRequest) WebRequest.Create(result1);
                    request.CookieContainer = cookies;
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (var requestStream = request.GetRequestStream())
                    using (var writer = new StreamWriter(requestStream))
                    {
                        writer.Write("email=" + login + "&pass=" + pass);
                    }

                    using (var responseStream = request.GetResponse().GetResponseStream())
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                var result = reader.ReadToEnd();

                                //*****************************
                                //Парсим, поиск ID
                                //*****************************                 
                                var doc2 = new HtmlAgilityPack.HtmlDocument();
                                doc2.LoadHtml(result);
                                try
                                {

                                    HtmlNode bodyNode2 =
                                        doc2.DocumentNode.SelectSingleNode("//div[@class='ip_user_link']/a");
                                    VkHref = bodyNode2.Attributes["href"].Value;
                                    VkName = bodyNode2.Attributes["data-name"].Value;
                                    VkPhoto = bodyNode2.Attributes["data-photo"].Value;
                                    //Если ID найден, то авторизация удалась
                                    return true;
                                }
                                catch
                                {
                                    //Если ID не найден, то авторизация не удалась
                                    // MessageBox.Show("Ошибка авторизации, проверьте правильность введённых данных.\nВозможно там капча.");
                                    return false;
                                }
                            }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        #endregion
        
        
    }
    
    public class UserControlDesigner : ParentControlDesigner
    {
        
        public override void Initialize(IComponent component)
            {
                base.Initialize(component);

                if (Control is StyledLoginPanelControl)
                {
                    EnableDesignMode((
                        (StyledLoginPanelControl)Control).DropZone, "DropZone");
                }
            }
    }
}
