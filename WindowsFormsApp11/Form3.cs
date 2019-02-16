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
    public partial class Form3 : DevExpress.XtraEditors.XtraForm
    {
        public Credentials Svi_kredencijali { get; set; }
        public Form3()
        {
            InitializeComponent();
            Svi_kredencijali = new Credentials();
            XmlSerializer ser = new XmlSerializer(typeof(Credentials));
            FileStream stream = new FileStream(Application.StartupPath + "\\" + "MegaChecker.xml", FileMode.OpenOrCreate);
            Svi_kredencijali = (Credentials)ser.Deserialize(stream);
            credentialBindingSource.DataSource = Svi_kredencijali.kredencijali;
            stream.Close();
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            foreach (Binding X in credentialBindingSource.CurrencyManager.Bindings) X.WriteValue();
            XmlSerializer ser = new XmlSerializer(typeof(Credentials));
            FileStream stream = new FileStream(Application.StartupPath + "\\" + "MegaChecker.xml", FileMode.OpenOrCreate);
            using (FileStream fs = stream)
            {
                ser.Serialize(fs, Svi_kredencijali);
            }
            stream.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            
            Svi_kredencijali.kredencijali.Add(new Credentials.Credential());
            credentialBindingSource.DataSource = Svi_kredencijali.kredencijali;
            gridView1.RefreshData();
            gridControl1.Refresh();
        }

        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            gridView1.SetRowCellValue(e.RowHandle, "colusername", "@cloudmne.com");
            gridView1.SetRowCellValue(e.RowHandle, "colpassword", "Abacus331860");
        }
    }
    //Application.StartupPath+"\\"+
}
