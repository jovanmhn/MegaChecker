using CG.Web.MegaApiClient;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegaChecker
{
    public partial class FormDownload : DevExpress.XtraEditors.XtraForm
    {
        public Form1.Klijenti klijent { get; set; }
        public List<BazeGrid> listagrid { get; set; }
        public MegaApiClient mega { get; set; }

        public FormDownload(Form1.Klijenti _klijent)
        {
            InitializeComponent();
            dateEdit1.EditValue = DateTime.Now;
            klijent = _klijent;
            listagrid = new List<BazeGrid>();
            mega = new MegaApiClient();
            var auth = mega.GenerateAuthInfos(klijent.username, klijent.pass);
            mega.Login(auth);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var wait = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this.gridControl1);
            listagrid.Clear();
             
            var nodes = mega.GetNodes();

            List<INode> allFiles = nodes.Where(n => n.Type == NodeType.File && n.CreationDate.Date == dateEdit1.DateTime.Date).ToList();
            foreach(var node in allFiles)
            {
                BazeGrid red = new BazeGrid
                {
                    id = node.Id,
                    name = node.Name,
                    datum = node.CreationDate,
                    velicina = (decimal)node.Size/(1024*1024),
                };
                listagrid.Add(red);
            }
            gridControl1.DataSource = listagrid;
            gridView1.RefreshData();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(wait);
            //var baze = allFiles.Where(xx => xx.CreationDate.Date == dateEdit1.DateTime.Date).Select(ww => ww.Name).ToList();
            //mega.dow
        }
        public class BazeGrid
        {
            public string id { get; set; }
            public string name { get; set; }
            public DateTime datum { get; set; }
            public decimal velicina { get; set; }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                progressPanel1.Visible = true;
                DL();
                simpleButton1.Enabled = false;
            }
            catch (Exception ex)
            {

                progressPanel1.Visible = false;
                simpleButton1.Enabled = true;
                XtraMessageBox.Show(ex.Message);
            }

            //BazeGrid red = (BazeGrid)gridView1.GetRow(gridView1.FocusedRowHandle);
            //using (SaveFileDialog sfd = new SaveFileDialog())
            //{
            //    sfd.FileName = klijent.naziv + " - " + red.datum.Date.ToString("dd.MM.yyyy.")+ " - " +  red.name;
            //    sfd.RestoreDirectory = true;
            //    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //    if (sfd.ShowDialog() == DialogResult.OK)
            //    {
            //        var wait = DevExpress.XtraSplashScreen.SplashScreenManager.ShowOverlayForm(this);
            //        try
            //        {
            //            mega.DownloadFile(mega.GetNodes().Where(qq => qq.Id == red.id).FirstOrDefault(), sfd.FileName);                        
            //        }
            //        catch (Exception)
            //        {
            //            XtraMessageBox.Show("Nesto je failovalo! :(","Greska",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //            DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(wait);
            //            goto kraj;
            //        }
            //        DevExpress.XtraSplashScreen.SplashScreenManager.CloseOverlayForm(wait);
            //        XtraMessageBox.Show("Fajl uspjesno downloadovan.");
            //    kraj:;
            //    }
            //}
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            progressPanel1.Visible = true;
            DL();
            
        }
        async void DL()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                BazeGrid red = (BazeGrid)gridView1.GetRow(gridView1.FocusedRowHandle);
                sfd.FileName = klijent.naziv + " - " + red.datum.Date.ToString("dd.MM.yyyy.") + " - " + red.name;
                sfd.RestoreDirectory = true;
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //var client = new MegaApiClient();
                //client.LoginAnonymous();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(sfd.FileName)) System.IO.File.Delete(sfd.FileName);
                    //Uri fileLink = new Uri("https://mega.nz/#!bkwkHC7D!AWJuto8_fhleAI2WG0RvACtKkL_s9tAtvBXXDUp2bQk");
                    //INodeInfo node = mega.GetNodeFromLink(fileLink);

                    //IProgress<double> progressHandler = new Progress<double>(x => Console.WriteLine("{0}%", x));
                    IProgress<double> progressHandler = new Progress<double>(x => progressPanel1.Description = String.Format("{0}%",Math.Round(x,2).ToString()));
                    await mega.DownloadFileAsync(mega.GetNodes().Where(qq => qq.Id == red.id).FirstOrDefault(), sfd.FileName, progressHandler);

                    progressPanel1.Visible = false;
                    simpleButton1.Enabled = true;
                    //mega.Logout();
                }
            }
        }
    }
}
