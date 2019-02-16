using CG.Web.MegaApiClient;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MegaChecker
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Credentials Svi_kredencijali { get; set; }
        List<Klijenti> main_lista { get; set; }
        public Form1()
        {
            InitializeComponent();
            Svi_kredencijali = new Credentials();
            List<Klijenti> main_lista = new List<Klijenti>();
            INIT();
        }

        public void INIT()
        {
            this.Cursor = Cursors.WaitCursor;
            XmlSerializer ser = new XmlSerializer(typeof(Credentials));
            FileStream stream = new FileStream(Application.StartupPath + "\\MegaChecker.xml", FileMode.Open);
            Svi_kredencijali = (Credentials)ser.Deserialize(stream);
            stream.Close();

            var x = Svi_kredencijali.kredencijali;
            main_lista = new List<Klijenti>();
            foreach (var item in x)
            {
                //MegaApiClient mega = new MegaApiClient();
                //var auth = mega.GenerateAuthInfos(item.username, item.password);
                //mega.Login(auth);
                //var nodes = mega.GetNodes();

                //List<INode> allFiles = nodes.Where(n => n.Type == NodeType.File).ToList();
                //var latest = allFiles.OrderByDescending(xx => xx.CreationDate).FirstOrDefault();
                Klijenti klijent = new Klijenti(item.name, item.username, item.password);
                main_lista.Add(klijent);


            }
            this.Cursor = Cursors.Default;
            gridControl1.DataSource = main_lista;
            gridView1.RefreshData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Credentials test = new Credentials();
            //List<Credentials.Credential> lista = new List<Credentials.Credential> { new Credentials.Credential { name = "Primato", username = "primatocentrala@cloudmne.com", password = "Abacus331860" } };
            //lista.Add(new Credentials.Credential { name = "PrimatoZL", username = "primatozelenika@cloudmne.com", password = "Abacus331860" });
            //test.kredencijali.AddRange(lista);
            //XmlSerializer ser = new XmlSerializer(typeof(Credentials));
            //using (FileStream fs = new FileStream(@"C:\Users\Jovan2\Desktop\MegaChecker.xml", FileMode.OpenOrCreate))
            //{
            //    ser.Serialize(fs, test);
            //}
        }

        
        public class Klijenti
        {
            public string naziv { get; set; }
            public string username { get; set; }
            public DateTime? zadnji_upload { get; set; }
            public string pass { get; set; }
            public Klijenti(string naziv, string username, DateTime datum, string password)
            {
                this.naziv = naziv;
                this.username = username;
                this.zadnji_upload = datum;
                this.pass = password;
            }
            public Klijenti(string naziv, string username, string password)
            {
                this.naziv = naziv;
                this.username = username;
                this.pass = password;
                zadnji_upload = null;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            XmlSerializer ser = new XmlSerializer(typeof(Credentials));
            try
            {
                FileStream stream = new FileStream(Application.StartupPath + "\\MegaChecker.xml", FileMode.Open);
                Svi_kredencijali = (Credentials)ser.Deserialize(stream);
                stream.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                goto skip;
            }
            File.Copy(Path.Combine(Application.StartupPath, "MegaChecker.xml"), Path.Combine(Application.StartupPath, "MegaChecker-backup.xml"), true);
        skip:;

            var x = Svi_kredencijali.kredencijali;
            List < Klijenti > lista = new List<Klijenti>();
            foreach( var item in x )
            {
                MegaApiClient mega = new MegaApiClient();
                var auth = mega.GenerateAuthInfos(item.username, item.password);
                mega.Login(auth);
                var nodes = mega.GetNodes();

                List<INode> allFiles = nodes.Where(n => n.Type == NodeType.File).ToList();
                var latest = allFiles.OrderByDescending(xx => xx.CreationDate).FirstOrDefault();
                Klijenti klijent = new Klijenti(item.name, item.username, latest.CreationDate, item.password);
                lista.Add(klijent);

                
            }
            this.Cursor = Cursors.Default;
            gridControl1.DataSource = lista;
            gridView1.RefreshData();
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Klijenti klijent = (Klijenti)gridView1.GetRow(e.RowHandle);
            if(klijent != null && klijent.zadnji_upload.HasValue && (DateTime.Now - klijent.zadnji_upload.Value).TotalDays > 2)
            {
                e.Appearance.BackColor = Color.Salmon;
            }
            if (klijent != null && klijent.zadnji_upload.HasValue && (DateTime.Now - klijent.zadnji_upload.Value).TotalDays <= 2)
            {
                e.Appearance.BackColor = Color.LightGreen;
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            
            
            if (gridView1.IsDataRow(gridView1.FocusedRowHandle) && e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                barButtonItem1.PerformClick();
                //Klijenti klijent = (Klijenti)gridView1.GetRow(gridView1.FocusedRowHandle);
                //MegaApiClient mega = new MegaApiClient();
                //var auth = mega.GenerateAuthInfos(klijent.username, klijent.pass);
                //mega.Login(auth);
                //var nodes = mega.GetNodes();

                //List<INode> allFiles = nodes.Where(n => n.Type == NodeType.File).ToList();
                //var baze = allFiles.Where(xx => xx.CreationDate.Date == klijent.zadnji_upload.Value.Date).Select(ww=> ww.Name).ToList();
                //string box = String.Join(Environment.NewLine, baze.ToArray());
                //MessageBox.Show(box,"Backup fajlovi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            if(gridView1.IsDataRow(gridView1.FocusedRowHandle)  && e.Button == MouseButtons.Right)
            {
                Klijenti klijent = (Klijenti)gridView1.GetRow(gridView1.FocusedRowHandle);
                popupMenu1.ShowPopup(MousePosition);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Klijenti red = (Klijenti)gridView1.GetRow(gridView1.FocusedRowHandle);
            var klijent = main_lista.FirstOrDefault(qq => qq.username == red.username);
            MegaApiClient mega = new MegaApiClient();
            var auth = mega.GenerateAuthInfos(klijent.username, klijent.pass);
            mega.Login(auth);
            var nodes = mega.GetNodes();

            
            klijent.zadnji_upload = nodes.Where(n => n.Type == NodeType.File).Max(xx=> xx.CreationDate);
            gridControl1.Refresh();
            gridView1.RefreshData();
            
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Klijenti klijent = (Klijenti)gridView1.GetRow(gridView1.FocusedRowHandle);
            FormDownload frm = new FormDownload(klijent);
            frm.Text = klijent.naziv;
            frm.ShowDialog();
        }
        async void Checkup()
        {
            var wait = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this.simpleButton1);
            progressBarControl1.Properties.Minimum = 0;
            progressBarControl1.Properties.Maximum = main_lista.Count;
            progressBarControl1.Properties.Step = 1;
            progressBarControl1.Properties.PercentView = true;
            try
            {
                foreach (var klijent in main_lista)
                {

                    MegaApiClient mega = new MegaApiClient();
                    var auth = mega.GenerateAuthInfos(klijent.username, klijent.pass);
                    mega.Login(auth);
                    var nodes = await mega.GetNodesAsync();

                    klijent.zadnji_upload = nodes.Where(n => n.Type == NodeType.File).Max(xx => xx.CreationDate);
                    gridControl1.Refresh();
                    gridView1.RefreshData();
                    progressBarControl1.PerformStep();
                    progressBarControl1.Update();

                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(wait);
                XtraMessageBox.Show(ex.Message);
            }
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(wait);
            progressBarControl1.EditValue = 0;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
             Checkup();
        }
    }
    [Serializable]
    public class Credentials
    {
        [XmlElement]
        public List<Credential> kredencijali = new List<Credential>();

        public class Credential
        {
            [XmlAttribute]
            public string username { get; set; }
            [XmlAttribute]
            public string password { get; set; }
            [XmlAttribute]
            public string name { get; set; }

        }
    }
}
