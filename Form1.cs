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
using System.Xml;
using System.Xml.Linq;
using StnwService;

namespace StnwSampleCS04
{
    public partial class frmInvoice : Form
    {
        DataSet dsInvoice = new DataSet();
        public frmInvoice()
        {
            InitializeComponent();

            this.DataGridView1.AutoGenerateColumns = false;

            dsInvoice.Tables.Add(new DataTable("InvItems"));
            DataTable dt = dsInvoice.Tables["InvItems"];
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Amount", typeof(double));
            dt.Columns.Add("Price", typeof(double));

            dsInvoice.Tables.Add(new DataTable("Dummy"));
            DataTable dtd = dsInvoice.Tables["Dummy"];
            dtd.Columns.Add("Text1", typeof(string));

            DataSet tds = GetData();

            txtCustomer.DataBindings.Add("Text", dtd, "Text1");

            DataGridView1.DataSource = tds.Tables["InvItems"];

            this.DataGridView1.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.DataGridView1.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.DataGridView1.Columns["Amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.DataGridView1.Columns["Price"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            foreach (DataGridViewColumn column in DataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.DataGridView1.CellValidating += DataGridView1_CellValidating;

        }

        private DataSet GetData()
        {
            DataTable dt = dsInvoice.Tables["InvItems"];
           
            DataRow dr;
            dr = dt.NewRow();
            dr["Id"] = Guid.NewGuid().ToString();
            dr["Code"] = "BB8527";
            dr["Name"] = "Bread";
            dr["Amount"] = 1;
            dr["Price"] = 2.15;
            dsInvoice.Tables["InvItems"].Rows.Add(dr);
            dr.EndEdit();
            dr = dt.NewRow();
            dr["Id"] = Guid.NewGuid().ToString();
            dr["Code"] ="SA482";
            dr["Name"] = "Chocolate";
            dr["Amount"] = 2;
            dr["Price"] = 7.65;
            dsInvoice.Tables["InvItems"].Rows.Add(dr);
            dr.EndEdit();
            dr = dt.NewRow();
            dr["Id"] = Guid.NewGuid().ToString();
            dr["Code"] = "QCI24";
            dr["Name"] = "Cheese";
            dr["Amount"] = 1;
            dr["Price"] = 2.08;
            dsInvoice.Tables["InvItems"].Rows.Add(dr);
            dr.EndEdit();
            dr = dt.NewRow();
            dr["Id"] = Guid.NewGuid().ToString();
            dr["Code"] = "MOX58";
            dr["Name"] = "Juice";
            dr["Amount"] = 1;
            dr["Price"] = 3.55;
            dsInvoice.Tables["InvItems"].Rows.Add(dr);
            dr.EndEdit();
            dr = dt.NewRow();
            dr["Id"] = Guid.NewGuid().ToString();
            dr["Code"] = "PB154";
            dr["Name"] = "Milk";
            dr["Amount"] = 3;
            dr["Price"] = 7.87;
            dsInvoice.Tables["InvItems"].Rows.Add(dr);
            dr.EndEdit();

            DataTable dtd = dsInvoice.Tables["Dummy"];
            dr = dtd.NewRow();
            dr["Text1"] = "Phoenixer inc.";
            dsInvoice.Tables["Dummy"].Rows.Add(dr);
            dr.EndEdit();

            return dsInvoice;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSaveSchema_Click(object sender, EventArgs e)
        {
            string schemaXml = "";
            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(sw))
                {
                    dsInvoice.WriteXmlSchema(xmlWriter);
                }
                schemaXml = sw.ToString();

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = "schema.xml";
                saveFileDialog1.Filter = "XML file|*.xml";
                saveFileDialog1.Title = "Save the schema file";
                saveFileDialog1.InitialDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {                   
                    File.WriteAllText(saveFileDialog1.FileName, schemaXml);
                }
            }


        }

        private void Button1_Click(object sender, EventArgs e)
        {
            clsStnwClass tsi = new clsStnwClass();
            string preslAccountCode = "DEMO1";    // your account code
            string preslUserCode = "0000";  // your user code

            tsi.preslAccountCode = preslAccountCode;
            tsi.preslUserCode = preslUserCode;
            tsi.dsRPT = dsInvoice;
            tsi.RPTDEST = 0;

            string rptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InvoiceReport.rpt");
            tsi.ReportFullName = rptPath;

            tsi.ShowForm();
        }

      
        private void DataGridView1_CellValidating(object sender,
                                          DataGridViewCellValidatingEventArgs e)
        {
            if ((e.ColumnIndex == 2) || (e.ColumnIndex == 3))
            {
                decimal dec;
                string s = e.FormattedValue.ToString().Trim('0');

                if (!decimal.TryParse(s, out dec))
                {
                    e.Cancel = true;
                    MessageBox.Show("please enter numeric");
                }
            }
        }

    }
}
