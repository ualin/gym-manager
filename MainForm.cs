using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.OleDb;
using Gym_Manager.DBAccess;
using MetriCam;

namespace Gym_Manager
{
    public partial class MainForm : Form
    {
        DBTables.Kunden selectedKunden = new DBTables.Kunden();
        List<DBTables.VertragDetailed> selectedVertragList = new List<DBTables.VertragDetailed>();
        DBTables.VertragDetailed selectedVertrag = new DBTables.VertragDetailed();
          WebCam camera;
        Image kundenPhoto;
        ContextMenu m;

        public MainForm()
        {
            InitializeComponent();

            
            //autocomplete sources for textboxes
            InitializeNameComboBox();
            InitializeSportComboBox();
            InitializeContextMenu();

            //listView image list
            //BindVertragImageList();
                        
            //initialize comboboxes special
            this.comboBox3.SelectedIndex = -1;
            this.comboBox3.SelectedValueChanged += new System.EventHandler(this.comboBox3_SelectedValueChanged);
            
            camera = new WebCam();
            //webcam = new WebCam();
            //webcam.InitializeWebCam(ref pictureBox4);
        }
        private void InitializeContextMenu()
        {
            m = new ContextMenu();
            m.MenuItems.Add(new MenuItem("Deaktivier Kunden", new EventHandler(DeativateKunden)));
        }
        private void InitializeNameComboBox()
        {
            
            DataTable namesTable = DBAccess.DBTransactions.GetKudinName();

            //var source[new int 5,string 20] = new AutoCompleteStringCollection();

            //foreach (string nr in nrList)
            //{
            //    source.Add(nr);
            //}
            comboBox3.DataSource = namesTable;
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "ID";
        }

        private void InitializeSportComboBox()
        {

            DataTable table = DBAccess.DBTransactions.GetSports();

            //var source[new int 5,string 20] = new AutoCompleteStringCollection();

            //foreach (string nr in nrList)
            //{
            //    source.Add(nr);
            //}
            comboBox2.DataSource = table;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "ID";
            comboBox2.SelectedIndex = -1;
        }

        // report alles
        private void label19_Click(object sender, EventArgs e)
        {
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource = ReportsClass.KundenCheckin.ReportDataSource();

            ReportForm reportForm = new ReportForm("KundenCheckin",reportDataSource);
            reportForm.Show();
        }
        //combobox kunden
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox3.SelectedIndex>-1)
            {
                int selectedValue= int.Parse(comboBox3.SelectedValue.ToString());

                selectedKunden = DBTransactions.GetKundenDetails(selectedValue);
                //add kunden infos to ui
                textBox9.Text = selectedKunden.ID.ToString();
                textBox7.Text = selectedKunden.HandyNr;
                textBox8.Text = selectedKunden.Strasse;
                textBox13.Text = selectedKunden.Plz;
                label21.Text = selectedKunden.Active;
                label21.BackColor = selectedKunden.Active == "Aktiv" ? Color.LimeGreen : Color.Tomato;

                selectedVertragList = DBTransactions.GetKundenVertragList(selectedValue);
                

                if (selectedVertragList.Count > 0)
                {
                    if (selectedVertragList[0].Photo != null)
                        pictureBox1.ImageLocation = "pics/" + selectedVertragList[0].Photo;

                    

                    FillVertragListView(selectedVertragList);

                    //reseting stuff
                    ClearVertragTextBoxes();
                    selectedVertrag = new DBTables.VertragDetailed();
                    
                }
                else
                {
                    //DBTables.Kunden kunden = DBTransactions.GetKundenDetails(selectedValue);

                    if (selectedKunden.Photo != null)
                        pictureBox1.ImageLocation = "pics/" + selectedKunden.Photo;

                    //textBox1.Text = kunden.Vorname;

                    //reseting stuff
                    ClearVertragTextBoxes();
                    selectedVertrag = new DBTables.VertragDetailed();
                    listView1.Clear();
                    //textBox1.Focus();
                }
            }
        }

        private void ClearVertragTextBoxes()
        {
            comboBox2.SelectedIndex = -1;
            dateTimePicker3.Text = "";
            dateTimePicker4.Text = "";
            pictureBox2.Image=null;
            label18.Text = "";

            //schlussel
            textBox5.Text = "";
        }
        private void ClearKundenTextBoxes()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox1.Text = "";
            textBox6.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            pictureBox4.Image=null;
        }

        private void BindCliendToTextboxes(DBTables.Kunden kunden)
        {
            //textBox1.Text = kunden.Vorname;
            //comboBox2.SelectedValue = kunden.SportID;
            //dateTimePicker3.Value = kunden.Anfang;
            //dateTimePicker4.Value = kunden.Schluss;

            //if (kunden.Schluss < DateTime.Now)
            //{
            //    label1.ForeColor = System.Drawing.Color.Red;
            //    label1.Text = "MEMBERSHIP EXPIRED";
            //    pictureBox2.Image = Properties.Resources.expired;
            //    pictureBox1.ImageLocation = "pics/" + kunden.Photo;
            //}
            //else
            //{
            //    label1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            //    label1.Text = "MEMBERSHIP ACTIVE";
            //    pictureBox2.Image = Properties.Resources.available;
            //    pictureBox1.ImageLocation = "pics/" + kunden.Photo;
            //}
                

        }

       

        private void LoadDataGridView ()
        {
            //dataGridView1.DataSource = DBTransactions.GetVertragListTable();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DBTables.Kunden kunden= new DBTables.Kunden();
            if (e.RowIndex >= 0)
            {
                string ID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                kunden = DBTransactions.GetKundenDetails(int.Parse(ID));

                comboBox3.SelectedValue = kunden.ID;
                tabControl1.SelectedIndex = 0;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex==1)
            {
                //LoadDataGridView();


            }
        }
      
        private void FillVertragListView(List<DBTables.VertragDetailed> vertragList)
        {
            imageList1.Images.Clear();
            listView1.Clear();
            imageList1.ImageSize = new Size(48, 48);

            imageList1.Images.Add(Properties.Resources.approve_48);
            imageList1.Images.Add(Properties.Resources.deny_48);

            //add items to list view
            foreach (DBTables.VertragDetailed detailedVertrag in vertragList)
            {
                listView1.Items.Add(new ListViewItem { ImageIndex = detailedVertrag.Schluss < DateTime.Now.Date ? 1 : 0, Text = detailedVertrag.Sport,Tag=detailedVertrag.VertragID});
            }

            //ListViewGroup myLVGroup1 = new ListViewGroup("First Five Group", HorizontalAlignment.Left);
            //ListViewGroup myLVGroup2 = new ListViewGroup("Last Five Group", HorizontalAlignment.Left);

            //listView1.Groups.AddRange(new ListViewGroup[] { myLVGroup1, myLVGroup2 });

            listView1.SmallImageList = imageList1;
            listView1.LargeImageList = imageList1;
            //listView1.Items.Add("Item 1", 0);
            //listView1.Items.Add("Item 2", 1);

            //listView1.Items[0].Group = myLVGroup1;
            //listView1.Items[1].Group = myLVGroup2;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DBTables.VertragDetailed vertrag = new DBTables.VertragDetailed();

            if (listView1.SelectedItems.Count != 0)
            {
                //get selected vertrag from vertrag list
                selectedVertrag = selectedVertragList.Find(id => id.VertragID == (int)listView1.SelectedItems[0].Tag);

                //write vertrag details to textboxes
                SetVertragDetailsToBoxes(selectedVertrag);

                
            }
            
            //MessageBox.Show(listView1.SelectedItems[0].Tag.ToString());
        }
        private void SetVertragDetailsToBoxes(DBTables.VertragDetailed vertrag)
        {
            comboBox2.SelectedValue = vertrag.SportID;
            dateTimePicker3.Value = vertrag.Anfang;
            dateTimePicker4.Value = vertrag.Schluss;
            label18.Text = vertrag.Sport;

            //decide what to show depending on vertrag valability
            if (vertrag.Schluss < DateTime.Now.Date)
            {
                //label1.ForeColor = System.Drawing.Color.Red;
                //label1.Text = "MEMBERSHIP EXPIRED";
                pictureBox2.Image = Properties.Resources.expired;
                pictureBox1.ImageLocation = "pics/" + vertrag.Photo;
            }
            else
            {
                //label1.ForeColor = System.Drawing.Color.DarkOliveGreen;
                //label1.Text = "MEMBERSHIP ACTIVE";
                pictureBox2.Image = Properties.Resources.available;
                pictureBox1.ImageLocation = "pics/" + vertrag.Photo;
            }
        }

        //update vertrag
        private void button5_Click(object sender, EventArgs e)
        {
           DBTables.Vertrag vertrag = new DBTables.Vertrag();
           bool response=false;

           if(selectedVertrag!= null)
               if(selectedVertrag.VertragID!=0 && selectedVertrag.Schluss.ToString()!="")
               if (dateTimePicker4.Value != null)
               {
                   if (dateTimePicker4.Value > selectedVertrag.Schluss)
                   {
                       vertrag.ID = selectedVertrag.VertragID;
                       vertrag.Schluss = dateTimePicker4.Value;

                       response = DBTransactions.UpdateVertragDetails(vertrag);

                       if (response == true)
                       {
                           //reset vertrags list view
                           selectedVertragList = DBTransactions.GetKundenVertragList(int.Parse(comboBox3.SelectedValue.ToString()));
                           FillVertragListView(selectedVertragList);
                           SetVertragDetailsToBoxes(selectedVertrag);

                           //MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       }
                       else
                           MessageBox.Show("Transaction failure", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
                   else
                       MessageBox.Show("Achtung fur Schluss Datum.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               }
        }

        //delete vertrag
        private void button4_Click(object sender, EventArgs e)
        {
             DBTables.Vertrag vertrag = new DBTables.Vertrag();
           bool response=false;

           if(selectedVertrag!= null)
           {
               if (selectedVertrag.VertragID != 0)
               {
                   if (MessageBox.Show("Sind Sie sicher ?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                   {
                       vertrag.ID = selectedVertrag.VertragID;
                       response = DBTransactions.DeleteVertrag(vertrag);

                       if (response == true)
                       {

                           //reset vertrags list view
                           selectedVertragList = DBTransactions.GetKundenVertragList(int.Parse(comboBox3.SelectedValue.ToString()));
                           FillVertragListView(selectedVertragList);
                           //SetVertragDetailsToBoxes(selectedVertrag);
                           //MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                           //reset selectedVertrag global variable
                           selectedVertrag = new DBTables.VertragDetailed();
                           //reset textboxes
                           ClearVertragTextBoxes();
                       }
                       else
                           MessageBox.Show("Transaction failure", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
               }
           }
        }
        //insert vertrag
        private void button2_Click(object sender, EventArgs e)
        {
            DBTables.Vertrag vertrag = new DBTables.Vertrag();
            bool response = false;
            if (comboBox3.SelectedIndex >-1 && comboBox3.SelectedValue.ToString() != "")
            {
                bool flag =selectedVertragList.Exists(i => i.Sport == comboBox2.Text);

                if (flag == false)
                {
                    if (comboBox2.SelectedIndex != -1 && comboBox2.SelectedValue.ToString() != "")
                    {
                        vertrag.KundenID = int.Parse(comboBox3.SelectedValue.ToString());
                        vertrag.SportID = int.Parse(comboBox2.SelectedValue.ToString());
                        vertrag.Anfang = dateTimePicker3.Value.Date;
                        vertrag.Schluss = dateTimePicker4.Value.Date;

                        response = DBTransactions.InsertVertrag(vertrag);

                        if (response == true)
                        {

                            //reset vertrags list view
                            selectedVertragList = DBTransactions.GetKundenVertragList(int.Parse(comboBox3.SelectedValue.ToString()));
                            FillVertragListView(selectedVertragList);
                            //MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //reset textboxes and others 
                            ClearVertragTextBoxes();
                            selectedVertrag = new DBTables.VertragDetailed();
                        }
                        else
                            MessageBox.Show("Transaction failure", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show("Es gibt schon ein "+comboBox2.Text+" vertrag.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Wählen Sie ein Kunden.","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        //add new kunden
        private void button1_Click_1(object sender, EventArgs e)
        {
            string name = textBox2.Text.Trim();
            string vorname= textBox3.Text.Trim();
            string photoAdress = name + "_" + vorname;
            string handyNr = textBox1.Text.Trim();
            string strasse = textBox11.Text.Trim();
            string ort = textBox6.Text.Trim();
            string plz = textBox12.Text.Trim();
            DateTime gebDatum = dateTimePicker5.Value;

            if (name != "" && vorname != "" && handyNr != "" && ort != "" && strasse !="")
            {
                if (pictureBox4.Image == null)
                    if (MessageBox.Show("Wollen Sie keine Foto hinzufügen?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                //{
                    
                    SaveImageCapture(pictureBox4.Image, name + "_" + vorname);

                    DBTables.Kunden kunden = new DBTables.Kunden();

                    kunden.Name = name;
                    kunden.Vorname = vorname;
                    kunden.Photo = photoAdress;
                    kunden.HandyNr = handyNr;
                    kunden.Strasse = strasse;
                    kunden.Plz = plz;
                    kunden.Ort = ort;
                    kunden.GebDatum = gebDatum;

                    bool response = DBTransactions.InsertKunden(kunden);

                    if (response == true)
                    {

                        //reset kunden combobox
                        InitializeNameComboBox();

                        //reset textboxes and others 
                        ClearKundenTextBoxes();

                        //MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Transaction failure", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //MessageBox.Show("Machen Sie das Foto.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("Fuhlen Sie alles Daten aus.");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(comboBox3.SelectedIndex>-1 && textBox5.Text.Trim()!="")
            {
                DBTables.Checkin checkin = new DBTables.Checkin();

                checkin.KundenID = int.Parse(comboBox3.SelectedValue.ToString());
                checkin.KundenName = comboBox3.SelectedText;
                checkin.LockerKey = textBox5.Text;
                checkin.CheckinTime = DateTime.Now;

                bool response = DBTransactions.InsertCheckin(checkin);

                if (response == true)
                {
                    //reset kunden combobox

                    //reset textboxes and others 
                    
                    //MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Transaction failure", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource = ReportsClass.Kundinen.KundinenDataSource();

            ReportForm reportForm = new ReportForm("Kundinen", reportDataSource);
            reportForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //webcam.Start();

            if(!camera.IsConnected())
            {
                camera.Connect();
                button3.Text = "Mach Foto";
                backgroundWorker1.RunWorkerAsync();
            }
            else
            { 
                backgroundWorker1.CancelAsync();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //webcam.Stop();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                int selectedValue = int.Parse(comboBox3.SelectedValue.ToString());

                DBTables.Kunden kunden = DBTransactions.GetKundenDetails(selectedValue);

                if(kunden!= null)
                {
                    string name = textBox2.Text =kunden.Name;
                    string vorname = textBox3.Text=kunden.Vorname;
                    pictureBox4.ImageLocation = "pics/" + kunden.Photo; ;
                    //string photoAdress = name + "_" + vorname;
                    string handyNr = textBox1.Text=kunden.HandyNr;
                    string strasse = textBox11.Text=kunden.Strasse;
                    string ort = textBox6.Text=kunden.Ort;
                    string plz = textBox12.Text=kunden.Plz;
                    DateTime gebDatum = dateTimePicker5.Value=DateTime.Parse( kunden.GebDatum.ToString());

                    tabControl1.SelectedIndex = 3;

                }
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!backgroundWorker1.CancellationPending)
            {
                camera.Update();
                pictureBox4.Image = camera.GetBitmap();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            camera.Disconnect();
            button3.Text = "Webcam Einschalten";
        }

        public static void SaveImageCapture(System.Drawing.Image image, string imageName)
        {
            //SaveFileDialog s = new SaveFileDialog();
            //s.FileName = imageName;// Default file name
            //s.DefaultExt = ".Jpg";// Default file extension
            //s.Filter = "Image (.jpg)|*.jpg"; // Filter files by extension
            //s.OverwritePrompt = false;

            // Show save file dialog box
            // Process save file dialog box results
            //if (s.ShowDialog() == DialogResult.OK)
            //{
            // Save Image
            //string filename = s.FileName;
            //FileStream fstream = new FileStream("pics/"+filename, FileMode.Create);
            if(image!= null)
            {
                image.Save("pics/" + imageName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                MessageBox.Show("Machen Sie das Foto.","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            
        }

       

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            int kundenID;

            if (e.KeyCode == Keys.Enter && int.TryParse(textBox9.Text.Trim(), out kundenID))
            {
                selectedKunden = DBTransactions.GetKundenDetails(kundenID);
                
                if(selectedKunden.ID!=0)
                {
                    comboBox3.SelectedValue = selectedKunden.ID;
                }
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            int kundenID;

            if (e.KeyCode == Keys.Enter && int.TryParse(textBox4.Text.Trim(), out kundenID))
            {
                DataTable checkinList = DBTransactions.GetCheckinList(kundenID);

                dataGridView1.DataSource = checkinList;
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            string schlussel = textBox10.Text.Trim();
            if (e.KeyCode == Keys.Enter && schlussel!="")
            {
                DataTable checkinList = DBTransactions.GetCheckinList(schlussel);

                dataGridView1.DataSource = checkinList;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
                DataTable checkinList = DBTransactions.GetCheckinList(dateTimePicker1.Value,dateTimePicker2.Value);

                dataGridView1.DataSource = checkinList;
        }

        private void DeativateKunden(object sender, EventArgs e)
        {
            int kundenID=int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            string state = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string stateToGive = state=="Aktiv"?"Inaktiv":"Aktiv";

            bool response=DBAccess.DBTransactions.ActivateDeactivateKunden(kundenID, stateToGive);
            if(response==true)
            {
                dataGridView1.SelectedRows[0].Cells[1].Value = stateToGive;
            }
        }
        
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
          
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[currentMouseOverRow].Selected = true;

                    string state = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                    string menuItemText = state == "Aktiv" ? "Deaktivier Kunden" : "Aktivier Kunden";

                    m.MenuItems[0].Text = menuItemText;
                    m.Show(dataGridView1, new Point(e.X, e.Y));
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //LoginForm loginForm = new LoginForm();
            //loginForm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string name = textBox2.Text.Trim();
            string vorname = textBox3.Text.Trim();
            string photoAdress = name + "_" + vorname;
            string handyNr = textBox1.Text.Trim();
            string strasse = textBox11.Text.Trim();
            string ort = textBox6.Text.Trim();
            string plz = textBox12.Text.Trim();
            DateTime gebDatum = dateTimePicker5.Value;

            if (name != "" && vorname != "" && handyNr != "" && ort != "" && strasse != "")
            {
                if (pictureBox4.Image == null)
                    if (MessageBox.Show("Wollen Sie keine Foto hinzufügen?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                //{

                SaveImageCapture(pictureBox4.Image, name + "_" + vorname);

                DBTables.Kunden kunden = new DBTables.Kunden();
                kunden.ID = selectedKunden.ID;
                kunden.Name = name;
                kunden.Vorname = vorname;
                kunden.Photo = photoAdress;
                kunden.HandyNr = handyNr;
                kunden.Strasse = strasse;
                kunden.Plz = plz;
                kunden.Ort = ort;
                kunden.GebDatum = gebDatum;

                bool response = DBTransactions.UpdateKunden(kunden);

                if (response == true)
                {

                    //reset kunden combobox
                    InitializeNameComboBox();

                    //reset textboxes and others 
                    ClearKundenTextBoxes();

                    //MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Transaction failure", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //else
                //MessageBox.Show("Machen Sie das Foto.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("Fuhlen Sie alles Daten aus.");
        }
        
    }
}
