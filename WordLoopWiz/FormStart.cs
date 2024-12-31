using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WordLoopWiz
{
    public partial class FormStart : Form
    {
        string connectionString = "Data Source=MELIH-LAPTOP\\SQLEXPRESS;Initial Catalog = WordLoopWiz; Integrated Security = True; Encrypt=True; TrustServerCertificate=True;";
        public FormStart()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            InitializeComponent();
            //Başta pageleri sakla
            HideSecondTabPage(pageNewLesson);
            HideThirdTabPage(pageWork);
            HideFourTabPage(pageQuiz);

            pnlConnect.Visible = false;

            LoadTableNamesIntoComboBox();

            //Work Lesson page 
            listView1.Columns.Add("Kelime", 150);
            listView1.Columns.Add("Çevirisi", 150);
            listView1.Columns.Add("Açıklaması", 390);
            //New lesson page
            listView2.Columns.Add("Kelime", 130);
            listView2.Columns.Add("Çevirisi", 130);
            listView2.Columns.Add("Açıklaması", 370);

            btnBackHomePage.Visible = false;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            webBrowser1.Url = new Uri("https://translate.google.com/?hl=tr&tab=TT&sl=en&tl=tr&op=translate"); 
        }


        #region // Home Page Blog
        // Ana sayga butonu sakalama
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ana sayfa butonunu sayfalar arası saklayıp açma
            if (tabControl1.SelectedIndex == 0)
            {
                btnBackHomePage.Visible = false;
            }
            else
            {
                btnBackHomePage.Visible = true;
            }
        }
        private void btnNewLessonPage_Click(object sender, EventArgs e)
        {
            //New Lesson Pagesine geçiş
            ShowSecondTabPage();
            btnBackHomePage.Visible = true;
            tabControl1.SelectedTab = tabControl1.TabPages["pageNewLesson"];
            LoadTableNamesIntoComboBox();

        }
        private void btnWorkLesson_Click(object sender, EventArgs e)
        {
            //Work Lesson Pagesine geçiş
            ShowThirdTabPage();
            btnBackHomePage.Visible = true;
            tabControl1.SelectedTab = tabControl1.TabPages["pageWork"];
            LoadTableNamesIntoComboBox();
        }
        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            //Quiz Pagesine geçiş
            ShowFourTabPage();
            btnBackHomePage.Visible = true;
            tabControl1.SelectedTab = tabControl1.TabPages["pageQuiz"];
            LoadTableNamesIntoComboBox();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            pnlConnect.Visible = true;

        }
        private void btnDoConnect_Click(object sender, EventArgs e)
        {
            connectionString = "Data Source="+txtConnetionString.Text+";Initial Catalog = WordLoopWiz; Integrated Security = True; Encrypt = True; TrustServerCertificate = True; "; 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Bağlantı başarılı!");

                    // Burada veri tabanı işlemlerinizi yapabilirsiniz
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bağlantı başarısız: " + ex.Message);
                }
            }
            pnlConnect.Visible=false;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Close();
        }
        #endregion


        #region // Hide Show Page
        private TabPage hiddenTabPage2;
       
        private void HideSecondTabPage(TabPage page)
        {
            // İkinci sekmeyi gizlemek için onu geçici bir değişkene alıyoruz
            hiddenTabPage2 = tabControl1.TabPages["pageNewLesson"];
            tabControl1.TabPages.Remove(hiddenTabPage2);
        }

        private void ShowSecondTabPage()
        {
            // Eğer ikinci sekme (TabPage) gizlenmişse onu geri ekleriz
            if (hiddenTabPage2 != null && !tabControl1.TabPages.Contains(hiddenTabPage2))
            {
                tabControl1.TabPages.Insert(1, hiddenTabPage2);
                hiddenTabPage2 = null;
            }
        }

        private TabPage hiddenTabPage3;
        private void HideThirdTabPage(TabPage page)
        {
            // İkinci sekmeyi gizlemek için onu geçici bir değişkene alıyoruz
            hiddenTabPage3 = tabControl1.TabPages["pageWork"];
            tabControl1.TabPages.Remove(hiddenTabPage3);
        }

        private void ShowThirdTabPage()
        {
            // Eğer ikinci sekme (TabPage) gizlenmişse onu geri ekleriz
            if (hiddenTabPage3 != null && !tabControl1.TabPages.Contains(hiddenTabPage3))
            {
                tabControl1.TabPages.Insert(1, hiddenTabPage3);
                hiddenTabPage3 = null;
            }
        }
        private TabPage hiddenTabPage4;
        private void HideFourTabPage(TabPage page)
        {
            // İkinci sekmeyi gizlemek için onu geçici bir değişkene alıyoruz
            hiddenTabPage4 = tabControl1.TabPages["pageQuiz"];
            tabControl1.TabPages.Remove(hiddenTabPage4);
        }
        private void ShowFourTabPage()
        {
            // Eğer ikinci sekme (TabPage) gizlenmişse onu geri ekleriz
            if (hiddenTabPage4 != null && !tabControl1.TabPages.Contains(hiddenTabPage4))
            {
                tabControl1.TabPages.Insert(1, hiddenTabPage4);
                hiddenTabPage4 = null;
            }
        }
        private void btnBackHomePage_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages["pageStart"];
        }
        #endregion


        #region // Work Lesson Page Blog
        private void LoadTableNamesIntoComboBox()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            {
                try
                {
                    connection.Open();

                    // Veritabanındaki tablo isimlerini almak için sorgu
                    string query = "SELECT name FROM sys.tables";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        cmbLessonList.Items.Clear(); // ComboBox'ı temizle
                        cmbQuizLesson.Items.Clear();

                        while (reader.Read())
                        {
                            // Tablo ismini ComboBox'a ekle
                            cmbLessonList.Items.Add(reader["name"].ToString());
                            cmbQuizLesson.Items.Add(reader["name"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnSelectLesson_Click(object sender, EventArgs e)
        {
            // ComboBox'tan seçili öğeyi al ve label'a ata
            if (cmbLessonList.SelectedItem != null)
            {
                lblActiveLesson.Text = cmbLessonList.SelectedItem.ToString();
                lblActiveLesson.Visible = false;
                lblActiveWorkLesoon.Text = "'"+lblActiveLesson.Text.ToString()+"'" + " Dersini Çalışıyorsun.";
                LoadDataPageNewLesson(1);
            }
            else
            {
                lblActiveLesson.Text = "Lütfen bir seçenek seçin.";
            }
        }
        private void btnLessonDelete_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string tableName = cmbLessonList.SelectedItem.ToString();
            if (cmbLessonList.SelectedItem != null)
            {
                cmbLessonList.SelectedText = "";
                cmbQuizLesson.SelectedText = "";
            }
            string query = $"DROP TABLE {tableName}";
            {
                SqlCommand command1 = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    command1.ExecuteNonQuery();
                    cmbLessonList.Items.Remove(tableName);
                    cmbQuizLesson.Items.Remove(tableName);
                    connection.Close();
                }
                catch (Exception ex)
                {
                    lblActiveWorkLesoon.Text = "Hata: " + ex.Message;
                    connection.Close();
                }
            }

        }

        private void LoadDataPageNewLesson(int index)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            // Label1'deki tablo ismini al
            string tableName = lblActiveLesson.Text;
            {
                try
                {
                    connection.Open();
                    // Sorguyu oluştur, tablo ismi dinamik olarak alınacak
                    string query = $"SELECT Word, Translate, Meaning FROM {tableName} WHERE ID= {index}";//ORDER BY NEWID()

                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Eğer veri varsa
                        {
                            // Veriyi TextBox'lara ata
                            lblCWord.Text = reader["Word"].ToString();
                            //lblCWord.Visible = false;
                            lblCTrans.Text = reader["Translate"].ToString();
                            lblCMean.Text = reader["Meaning"].ToString();
                        }
                        
                    } 

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private int CountID(string ActiveLesson, string ResultLabel)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            int satirSayisi=0;
            string tableName = ActiveLesson;
            string query = $"SELECT COUNT(*) FROM {tableName}";
            {
                SqlCommand command1 = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    satirSayisi = (int)command1.ExecuteScalar();
                    
                    connection.Close();
                }
                catch (Exception ex)
                {
                    ResultLabel = "Hata: " + ex.Message;
                }
            }
            return satirSayisi;
         }
        int ids=1;
        private void cmbLessonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ids = 2;
        }
        private void btnDoCorrect_Click(object sender, EventArgs e)
        {
            List<string> stringList = new List<string>();
            if (txtEnterWord == null)
            {
                lblCorrectResult.Text = "Lütfen bir kelime gir.";

            }
            else if (txtEnterWord.Text == "Kelimeyi Girin")
            {
                lblCorrectResult.Text = "Lütfen bir kelime gir.";
            }
            else if (txtEnterWord.Text == lblCWord.Text)
            {
                lblCorrectResult.Text = "Tebrikler Kelimeyi Doğru Bildin...";
                string[] kelimeler = { lblCWord.Text, lblCTrans.Text, lblCMean.Text };
                ListViewItem words = new ListViewItem(kelimeler);
                listView1.Items.Add(words);
                if (ids == CountID(lblActiveLesson.Text,lblCorrectResult.Text )+1)
                {
                    MessageBox.Show("Tebrikler Bütün Kelimeleri Çalıştın...");
                }
                LoadDataPageNewLesson(ids);
                ids++;
            }
            else
            {
                lblCorrectResult.Text = "Maalesef Kelimeyi Doğru Bilemedin.";
                lblCorrectResult.Text = CheckSimilarity(lblCWord.Text, txtEnterWord.Text, 70.0);
            }
        }
        #region // Kelime Benzerlik Uygulaması..
        public static int LevenshteinDistance(string s1, string s2)
        {
            int[,] dp = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++)
                dp[i, 0] = i;

            for (int j = 0; j <= s2.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;

                    dp[i, j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1), dp[i - 1, j - 1] + cost);
                }
            }

            return dp[s1.Length, s2.Length];
        }

        // Benzerlik oranını hesaplayan metod
        public static double SimilarityPercentage(string s1, string s2)
        {
            int distance = LevenshteinDistance(s1, s2);
            int maxLength = Math.Max(s1.Length, s2.Length);

            return (1.0 - (double)distance / maxLength) * 100;
        }

        // Benzerlik kontrolü yapan metod
        public static string CheckSimilarity(string word1, string word2, double threshold = 80.0)
        {
            double similarity = SimilarityPercentage(word1, word2);
            string s;
            if (similarity >= threshold)
            {
                s = "Doğru Kelimeye Çok Yaklaştın!!!";             
            }
            else
            {
               s = "Doğru Kelimeden Biraz Uzaksın...";
            }
            return s;
        }
        #endregion 

        private void txtEnterWord_MouseEnter(object sender, EventArgs e)
        {
            if (txtEnterWord.Text == "Kelimeyi Girin")
            {
                txtEnterWord.Text = "";          // Metni temizleyin
                txtEnterWord.ForeColor = Color.White; // kullanıcı girdisini belirgin hale getirin
            }
        }
        private void txtEnterWord_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEnterWord.Text))
            {
                txtEnterWord.Text = "Kelimeyi Girin"; // Boşsa varsayılan metni geri getirin
                txtEnterWord.ForeColor = Color.White;
            }
        }
        #endregion

        #region // Quiz Page
        //Quizi Başlatmak için 
        List<int> tabs = new List<int>();
        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            // ComboBox'tan seçili öğeyi al ata
            if (cmbQuizLesson.SelectedItem != null)
            {
                lblQuizLesson.Text = cmbQuizLesson.SelectedItem.ToString();
                lblQuizLesson.Visible = false;
                Random randomID = new Random();
                int x = randomID.Next(1, tabs.Count() + 1);
                LoadDataQuizLesson(x);
                InitializeTabsList();
            }
            else
            {
                lblQuizLessonResult.Text = "Lütfen bir seçenek seçin.";
            }
            
            btnStartQuiz.Visible = false;
            
        }

        //Quiz Başladığında ders seçin butonu
        private void cmbQuizLesson_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnStartQuiz.Visible = true;
        }
       //Quz sorularını ve cevapalarını şıkları döndüren methot
        private void LoadDataQuizLesson(int index)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            //tablo ismini al
            string tableName = lblQuizLesson.Text;
            
            // Şıklarda random oluşturmak için
            List<int> list = new List<int>();
            for (int i = 1;i<=CountID(lblQuizLesson.Text, lblQuizLessonResult.Text); i++)
            {
                Random random = new Random();
                random.Next(1, CountID(lblQuizLesson.Text, lblQuizLessonResult.Text));
                list.Add(i);
            }
            if (list.Contains(index))
            {
                list.Remove(index);
            }
            {
                try
                {
                    connection.Open();
                    // Sorguyu oluşturtablo ismi dinamik olarak alınacak
                    string query = $"SELECT Word, Translate FROM {tableName} WHERE ID= {index}";

                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            btnChoose1.ButtonText = "";
                            btnChoose2.ButtonText = "";
                            btnChoose3.ButtonText = "";
                            btnChoose4.ButtonText = "";

                            btnWords.ButtonText = reader["Word"].ToString();
                            string correctAnswer = reader["Translate"].ToString();


                            //Doğru Şıkkı Random atamak ve kalan şıkları yanlış doldurmak için
                            lblCorrectAnsver.Text = correctAnswer;
                            Random randomChoose = new Random();
                            int choose = randomChoose.Next(1, 5);
                            
                            Random rInd = new Random();
                            switch (choose)
                            {
                                case 1:
                                    btnChoose1.ButtonText = correctAnswer;
                                    connection.Close();
 
                                    int i2 = rInd.Next(0,list.Count);
                                    btnChoose2.ButtonText = WrongAnsver(list[i2]);
                                    

                                    int i3 = rInd.Next(0, list.Count);
                                    while(i3 == i2)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose3.ButtonText = WrongAnsver(list[i3]);
                                    

                                    int i4 = rInd.Next(0, list.Count);
                                    while (i4 == i3 && i4 == i2)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose4.ButtonText = WrongAnsver(list[i4]);
                                    
                                    break;
                                case 2:
                                    btnChoose2.ButtonText = correctAnswer;
                                    connection.Close();

                                    int i5 = rInd.Next(0, list.Count);
                                    btnChoose1.ButtonText = WrongAnsver(list[i5]);


                                    int i6 = rInd.Next(0, list.Count);
                                    while (i6 == i5)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose3.ButtonText = WrongAnsver(list[i6]);


                                    int i7 = rInd.Next(0, list.Count);
                                    while (i7 == i5 && i7 == i6)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose4.ButtonText = WrongAnsver(list[i7]);

                                    break;
                                case 3:
                                    btnChoose3.ButtonText = correctAnswer;
                                    connection.Close();

                                    int i8 = rInd.Next(0, list.Count);
                                    btnChoose1.ButtonText = WrongAnsver(list[i8]);


                                    int i9 = rInd.Next(0, list.Count);
                                    while (i9 == i8)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose2.ButtonText = WrongAnsver(list[i9]);


                                    int i10 = rInd.Next(0, list.Count);
                                    while (i10 == i8 && i10 == i9)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose4.ButtonText = WrongAnsver(list[i10]);

                                    break;
                                case 4:
                                    btnChoose4.ButtonText = correctAnswer;
                                    connection.Close();

                                    int i11 = rInd.Next(0, list.Count);
                                    btnChoose1.ButtonText = WrongAnsver(list[i11]);


                                    int i12 = rInd.Next(0, list.Count);
                                    while (i12 == i11)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose2.ButtonText = WrongAnsver(list[i12]);


                                    int i13 = rInd.Next(0, list.Count);
                                    while (i13 == i11 && i13 == i12)
                                        i3 = rInd.Next(0, list.Count);
                                    btnChoose3.ButtonText = WrongAnsver(list[i13]);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        //Yanlış Şıklara Atama yapan Methot
        private string WrongAnsver(int index)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string tableName = lblQuizLesson.Text;
            string wrongAnswer = "z";
            try
            {
                connection.Open();
                // Sorguyu oluştur, tablo ismi dinamik olarak alınacak
                string q1 = $"SELECT Word, Translate FROM {tableName} WHERE ID= {index}";

                SqlCommand command1 = new SqlCommand(q1, connection);

                using (SqlDataReader reader1 = command1.ExecuteReader())
                {
                    if (reader1.Read())
                    {
                        wrongAnswer = reader1["Translate"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return wrongAnswer;
        }
        //Quiz deki combobax 
        private void InitializeTabsList()
        {
            int count = CountID(lblQuizLesson.Text, lblQuizLessonResult.Text);
            for (int i = 1; i <= count; i++)
            {
                tabs.Add(i);
            }
        }

        
        // Quiz Butonları arayüzü ve timer methodu

        private void btnChoose1_Click(object sender, EventArgs e)
        {
            timerNextQuess.Start();
            timerNextQuess.Interval = 3000;
            if (lblCorrectAnsver.Text == btnChoose1.ButtonText)
            {
                btnChoose1.IdleFillColor = Color.Green;
                btnChoose1.IdleLineColor = Color.Green;
                btnChoose1.ForeColor = Color.White;
                btnChoose1.ActiveFillColor = Color.Green;
            }
            else
            {
                btnChoose1.IdleFillColor = Color.Red;
                btnChoose1.IdleLineColor = Color.Red;
                btnChoose1.ForeColor = Color.White;
                btnChoose1.ActiveFillColor = Color.Red;

                if (btnChoose2.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose2.IdleFillColor = Color.Green;
                    btnChoose2.IdleLineColor = Color.Green;
                    btnChoose2.ForeColor = Color.White;
                    btnChoose2.ActiveFillColor = Color.Green;
                }
                else if (btnChoose3.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose3.IdleFillColor = Color.Green;
                    btnChoose3.IdleLineColor = Color.Green;
                    btnChoose3.ForeColor = Color.White;
                    btnChoose3.ActiveFillColor = Color.Green;
                }
                else if (btnChoose4.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose4.IdleFillColor = Color.Green;
                    btnChoose4.IdleLineColor = Color.Green;
                    btnChoose4.ForeColor = Color.White;
                    btnChoose4.ActiveFillColor = Color.Green;
                }
            }
        }
        private void btnChoose2_Click(object sender, EventArgs e)
        {
            timerNextQuess.Start();
            timerNextQuess.Interval = 3000;
            if (lblCorrectAnsver.Text == btnChoose2.ButtonText)
            {
                btnChoose2.IdleFillColor = Color.Green;
                btnChoose2.IdleLineColor = Color.Green;
                btnChoose2.ForeColor = Color.White;
                btnChoose2.ActiveFillColor = Color.Green;
            }
            else
            {
                btnChoose2.IdleFillColor = Color.Red;
                btnChoose2.IdleLineColor = Color.Red;
                btnChoose2.ForeColor = Color.White;
                btnChoose2.ActiveFillColor = Color.Red;

                if (btnChoose1.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose1.IdleFillColor = Color.Green;
                    btnChoose1.IdleLineColor = Color.Green;
                    btnChoose1.ForeColor = Color.White;
                    btnChoose1.ActiveFillColor = Color.Green;
                }
                else if (btnChoose3.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose3.IdleFillColor = Color.Green;
                    btnChoose3.IdleLineColor = Color.Green;
                    btnChoose3.ForeColor = Color.White;
                    btnChoose3.ActiveFillColor = Color.Green;
                }
                else if (btnChoose4.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose4.IdleFillColor = Color.Green;
                    btnChoose4.IdleLineColor = Color.Green;
                    btnChoose4.ForeColor = Color.White;
                    btnChoose4.ActiveFillColor = Color.Green;
                }
            }
        }
        private void btnChoose3_Click(object sender, EventArgs e)
        {
            timerNextQuess.Start();
            timerNextQuess.Interval = 3000;
            if (lblCorrectAnsver.Text == btnChoose3.ButtonText)
            {
                btnChoose3.IdleFillColor = Color.Green;
                btnChoose3.IdleLineColor = Color.Green;
                btnChoose3.ForeColor = Color.White;
                btnChoose3.ActiveFillColor = Color.Green;
            }
            else
            {
                btnChoose3.IdleFillColor = Color.Red;
                btnChoose3.IdleLineColor = Color.Red;
                btnChoose3.ForeColor = Color.White;
                btnChoose3.ActiveFillColor = Color.Red;

                if (btnChoose2.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose2.IdleFillColor = Color.Green;
                    btnChoose2.IdleLineColor = Color.Green;
                    btnChoose2.ForeColor = Color.White;
                    btnChoose2.ActiveFillColor = Color.Green;
                }
                else if (btnChoose1.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose1.IdleFillColor = Color.Green;
                    btnChoose1.IdleLineColor = Color.Green;
                    btnChoose1.ForeColor = Color.White;
                    btnChoose1.ActiveFillColor = Color.Green;
                }
                else if (btnChoose4.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose4.IdleFillColor = Color.Green;
                    btnChoose4.IdleLineColor = Color.Green;
                    btnChoose4.ForeColor = Color.White;
                    btnChoose4.ActiveFillColor = Color.Green;
                }
            }
        }
        private void btnChoose4_Click(object sender, EventArgs e)
        {
            timerNextQuess.Start();
            timerNextQuess.Interval = 3000;
            if (lblCorrectAnsver.Text == btnChoose4.ButtonText)
            {
                btnChoose4.IdleFillColor = Color.Green;
                btnChoose4.IdleLineColor = Color.Green;
                btnChoose4.ForeColor = Color.White;
                btnChoose4.ActiveFillColor = Color.Green;
            }
            else
            {
                btnChoose4.IdleFillColor = Color.Red;
                btnChoose4.IdleLineColor = Color.Red;
                btnChoose4.ForeColor = Color.White;
                btnChoose4.ActiveFillColor = Color.Red;

                if (btnChoose2.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose2.IdleFillColor = Color.Green;
                    btnChoose2.IdleLineColor = Color.Green;
                    btnChoose2.ForeColor = Color.White;
                    btnChoose2.ActiveFillColor = Color.Green;
                }
                else if (btnChoose3.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose3.IdleFillColor = Color.Green;
                    btnChoose3.IdleLineColor = Color.Green;
                    btnChoose3.ForeColor = Color.White;
                    btnChoose3.ActiveFillColor = Color.Green;
                }
                else if (btnChoose1.ButtonText == lblCorrectAnsver.Text)
                {
                    btnChoose1.IdleFillColor = Color.Green;
                    btnChoose1.IdleLineColor = Color.Green;
                    btnChoose1.ForeColor = Color.White;
                    btnChoose1.ActiveFillColor = Color.Green;
                }
            }
        }

        Random randomID = new Random();
        private void timerNextQuess_Tick(object sender, EventArgs e)
        {
            timerNextQuess.Interval = 3000;

            btnChoose1.IdleFillColor = Color.White;
            btnChoose1.IdleLineColor = Color.Gold;
            btnChoose1.ForeColor = Color.Black;
            btnChoose1.ActiveFillColor = Color.Gold;

            btnChoose2.IdleFillColor = Color.White;
            btnChoose2.IdleLineColor = Color.Gold;
            btnChoose2.ForeColor = Color.Black;
            btnChoose2.ActiveFillColor = Color.Gold;


            btnChoose3.IdleFillColor = Color.White;
            btnChoose3.IdleLineColor = Color.Gold;
            btnChoose3.ForeColor = Color.Black;
            btnChoose3.ActiveFillColor = Color.Gold;


            btnChoose4.IdleFillColor = Color.White;
            btnChoose4.IdleLineColor = Color.Gold;
            btnChoose4.ForeColor = Color.Black;
            btnChoose4.ActiveFillColor = Color.Gold;


            timerNextQuess.Stop();
            if (tabs.Count > 0)
            {
                int x = randomID.Next(0, tabs.Count);
                int randomIDValue = tabs[x];
                LoadDataQuizLesson(randomIDValue);
                tabs.RemoveAt(x);

                if (tabs.Count == 0)
                {
                    btnChoose1.ButtonText = "";
                    btnChoose2.ButtonText = "";
                    btnChoose3.ButtonText = "";
                    btnChoose4.ButtonText = "";
                    MessageBox.Show("Tebrikler Bu Derse Ait Tüm Kelimeleri Çalıştın.");
                }
            }
            else
            {
                MessageBox.Show("Çalışılacak Kelime Bulunamadı");
            }
        }
        #endregion

        #region //New Lesson Page Blog

        // Ders Ekleme butonu
        private void btnAddLesson_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string tableName = txtLessonsName.Text.ToString();

           
            if (string.IsNullOrEmpty(tableName))
            {
                lblLessonResult.Text = "Lütfen geçerli bir tablo adı girin!";
                return;
            }
            else if (tableName == "Ders Adı Girin")
            {
                lblLessonResult.Text = "Lütfen geçerli bir tablo adı girin!";
                return;
            }
         
            string createTableQuery = $@"
        CREATE TABLE [{tableName}] (
            ID INT NOT NULL,
            Word NVARCHAR(100) NOT NULL,
            Translate NVARCHAR(100) NOT NULL,
            Meaning NVARCHAR(600)
        )";

            {
                SqlCommand command = new SqlCommand(createTableQuery, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    lblLessonResult.Text = $"Tablo '{tableName}' başarıyla oluşturuldu.";
                    btnAddLesson.Visible = false ;
                    LoadTableNamesIntoComboBox();
                }
                catch (Exception ex)
                {
                    lblLessonResult.Text = "Hata: " + ex.Message;
                }
            }

        }
        
        int id = 1;
        // Kelime açıklama ve çeviri ekleme Butonu
        private void btnWordAdd_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string tableName = txtLessonsName.Text.ToString();

           
            if (string.IsNullOrEmpty(tableName))
            {
                lblLessonResult.Text = "Lütfen geçerli bir ders adı girin.";
                return;
            }else if (tableName == "Ders Adı Girin")
            {
                lblLessonResult.Text = "Lütfen geçerli bir ders adı girin.";
                return;
            }

           
            string insertQuery = $@"INSERT INTO [{tableName}] (ID, Word, Translate, Meaning) VALUES (@ID, @Word, @Translate, @Meaning)";

            {
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                try
                {
                    
                    insertCommand.Parameters.AddWithValue("@Id", id);
                    insertCommand.Parameters.AddWithValue("@Word", txtWords.Text);
                    insertCommand.Parameters.AddWithValue("@Translate", txtTranss.Text);
                    insertCommand.Parameters.AddWithValue("@Meaning", txtMeans.Text);
                    
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                    lblLessonResult.Text = "Kelime başarıyla eklendi!";
                    id++;
                    string list = $@"Select * From [{tableName}]";
                    {
                        try
                        {
                            SqlDataAdapter adapter = new SqlDataAdapter(list, connection);
                            string[] kelimeler1 = { txtWords.Text, txtTranss.Text, txtMeans.Text };
                            ListViewItem words1 = new ListViewItem(kelimeler1);
                            listView2.Items.Add(words1);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    lblLessonResult.Text = "Hata: " + ex.Message;
                    connection.Close();
                }
            }
           
        }

        // New Lesson Page için Arayüz methotları
        private void txtLessonsName_MouseEnter(object sender, EventArgs e)
        {
            if (txtLessonsName.Text == "Ders Adı Girin")
            {
                txtLessonsName.Text = "";         
                txtLessonsName.ForeColor = Color.White; 
            }
        }

        private void txtLessonsName_MouseLeave(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtLessonsName.Text))
            {
                txtLessonsName.Text = "Ders Adı Girin"; 
                txtLessonsName.ForeColor = Color.Gray;
            }
        }

        private void txtWords_MouseEnter(object sender, EventArgs e)
        {
            if (txtWords.Text == "Kelimeyi Girin")
            {
                txtWords.Text = "";          
                txtWords.ForeColor = Color.White; 
            }
        }

        private void txtWords_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWords.Text))
            {
                txtWords.Text = "Kelimeyi Girin";
                txtWords.ForeColor = Color.White;
            }
        }

        private void txtTranss_MouseEnter(object sender, EventArgs e)
        {
            if (txtTranss.Text == "Çevirisini Girin")
            {
                txtTranss.Text = "";         
                txtTranss.ForeColor = Color.White; 
            }
        }

        private void txtTranss_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTranss.Text))
            {
                txtTranss.Text = "Çevirisini Girin";
                txtTranss.ForeColor = Color.White;
            }
        }

        private void txtMeans_MouseEnter(object sender, EventArgs e)
        {
            if (txtMeans.Text == "Açıklamasını Girin")
            {
                txtMeans.Text = "";          // Metni temizleyin
                txtMeans.ForeColor = Color.White; // kullanıcı girdisini belirgin hale getirin
            }
        }

        private void txtMeans_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMeans.Text))
            {
                txtMeans.Text = "Açıklamasını Girin"; // Boşsa varsayılan metni geri getirin
                txtMeans.ForeColor = Color.White;
            }
        }

        
    }
    #endregion




    

}