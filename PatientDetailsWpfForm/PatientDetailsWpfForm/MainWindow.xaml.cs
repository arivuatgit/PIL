using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace PatientDetailsWpfForm
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ComboBox> cbItems { get; set; }
        public ComboBoxItem SelectedcbItem { get; set; }
        public static List<string> countries;
        public List<PatientInfo> records = new List<PatientInfo>();
        string hospital = ConfigurationManager.AppSettings["HospitalKey"];
        public MainWindow()
        {
           
            InitializeComponent();
            dpDOB.DisplayDate = DateTime.Today;
            dpDOB.Text = DateTime.Now.Date.ToString();
            dpDOB.DisplayDateEnd = DateTime.Now;
            dpLastVisit.DisplayDate = DateTime.Now;
            dpLastVisit.Text = DateTime.Now.Date.ToString();
            dpLastVisit.DisplayDateEnd = DateTime.Now;
            ComboBoxItem cboxitemAge;
          
            for (int i = 1; i < 100; i++)
            {
                cboxitemAge = new ComboBoxItem { Content = i };
                SelectedcbItem = cboxitemAge;
                cbAge.Items.Add(cboxitemAge);
            }
            countries = GetAllCountriesName();
            records = FetchRecords();
            BindCountries(countries);
            BindNames(records);
            BindPatientIds();
        }
        public static List<string> GetAllCountriesName()
        {
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var rez = cultures.Select(cult => (new RegionInfo(cult.LCID)).DisplayName).Distinct().OrderBy(q => q).ToList();

            return rez;
        }
        private void BindCountries(List<string> countries)
        {
            ComboBoxItem cboxitemCountry;
            for (int i = 0; i < countries.Count; i++)
            {
                cboxitemCountry = new ComboBoxItem { Content = countries[i].ToString() };
                cbCountry.Items.Add(cboxitemCountry);
            }
        }
        private void BindNames(List<PatientInfo> records)
        {
            ComboBoxItem cboxItemNames;
            for(int i = 0; i < records.Count; i++)
            {
                cboxItemNames = new ComboBoxItem { Content = records[i].PatientName.ToString() };
                cbName.Items.Add(cboxItemNames);
            }
        }
        private void BindPatientIds()
        {
            ComboBoxItem cboxItemNames;
            for (int i = 0; i < records.Count; i++)
            {
                cboxItemNames = new ComboBoxItem { Content = records[i].PatientId.ToString() };
                cbPatientId.Items.Add(cboxItemNames);
            }
        }
        private List<PatientInfo> FetchRecords()
        {
            PatientInfo pInfo = null;
            SqlConnection conn = new SqlConnection(@"Data Source=13.126.136.46\SWS,1433; Initial Catalog=DFN; User ID=sa;Password=poi$098#");
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                // cmd.CommandText = "insert into tblPatientInfo values('" + txtPatientID.Text + "','" + txtPatientName.Text + "','" + txtLastName.Text + "'," + dpDOB.Text + "," + Convert.ToInt32(cbAge.Text) + ",'" + cbGender.Text + "','" + txtSSN.Text + "','" + txtAddress1.Text + "','" + txtAddress2.Text + "','" + txtCity.Text + "','" + txtState.Text + "','" + cbCountry.Text + "','" + txtZip.Text + "','" + txtArchiveLabel.Text + "','" + txtContactNum1.Text + "','" + txtContactNum2.Text + "','" + txtEmail.Text + "','" + txtRefDoctor.Text + "'," + dpLastVisit.Text + ",'" + txtDiagnostics.Text + "','"+ fileName+ "','"+hospital+"','"+dToday+"','"+dTime+"')";
                // cmd.CommandText = "insert into tblPatientInfo values('" + txtPatientID.Text + "','" + txtPatientName.Text + "','" + txtLastName.Text + "'," + dpDOB.Text + "," + Convert.ToInt32(cbAge.Text) + ",'" + cbGender.Text + "','" + txtAddress1.Text + "','" + txtAddress2.Text + "','" + txtCity.Text + "','" + txtState.Text + "','" + cbCountry.Text + "','" + txtZip.Text + "','" + txtContactNum1.Text + "','" + txtContactNum2.Text + "','" + txtEmail.Text + "','" + txtRefDoctor.Text + "'," + dpLastVisit.Text + ",'" + txtDiagnostics.Text + "','" + fileName + "','" + hospital + "','" + dToday + "','" + dTime + "')";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_InitFetchRecords";
                             
                cmd.Parameters.AddWithValue("@HOSPITAL", hospital);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pInfo = new PatientInfo();
                    pInfo.RecordsID = Convert.ToInt64(dr["RECORDSID"]);
                    pInfo.PatientId = Convert.ToString(dr["PATIENTID"]);
                    pInfo.PatientName = Convert.ToString(dr["FIRSTNAME"]);
                    pInfo.LastName = Convert.ToString(dr["LASTNAME"]);
                    pInfo.DOB = Convert.ToDateTime(dr["DOB"]);
                    pInfo.Age = Convert.ToInt32(dr["AGE"]);
                    pInfo.Gender = Convert.ToString(dr["GENDER"]);

                    pInfo.Address1 = Convert.ToString(dr["ADDRESS1"]);
                    pInfo.Address2 = Convert.ToString(dr["ADDRESS2"]);
                    pInfo.City = Convert.ToString(dr["CITY"]);
                    pInfo.PState = Convert.ToString(dr["PSTATE"]);
                    pInfo.PCountry = Convert.ToString(dr["PCOUNTRY"]);
                    pInfo.ZIPCode = Convert.ToString(dr["ZIPCODE"]);

                    pInfo.ContactNumber1 = Convert.ToString(dr["CONTACTNUM1"]);
                    pInfo.ContactNumber2 = Convert.ToString(dr["CONTACTNUM2"]);
                    pInfo.PEmail = Convert.ToString(dr["PEMAIL"]);
                    pInfo.RefDoctor = Convert.ToString(dr["REFERENCEDOCTOR"]);
                    pInfo.LastVisit = Convert.ToDateTime(dr["LASTVISIT"]);
                    pInfo.Diagnosis = Convert.ToString(dr["DIAGNOSIS"]);
                    pInfo.FileName = Convert.ToString(dr["FILENAME"]);
                    pInfo.Hospital = Convert.ToString(dr["HOSPITAL"]);
                    pInfo.ODate = Convert.ToString(dr["ODATE"]);
                    pInfo.OTime = Convert.ToString(dr["OTIME"]);
                    records.Add(pInfo);
                }
              //  cmd.ExecuteNonQuery();

               
                conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return records;
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = IsValidEmailAddress(txtEmail.Text);
            string fileName = "";
           
            DateTime currentDate = DateTime.Today;
            var dToday = currentDate.ToString("dd/MM/yyyy");
            var dTime = DateTime.Now.ToString("hh:mm tt");
            if (this.txtPatientName.Text == "")
            {
                MessageBox.Show("Please enter Patient name");
            }
            else if (this.txtContactNum1.Text == "")
            {
                MessageBox.Show("Please enter contact number");
            }
            else if (this.txtEmail.Text == "")
            {
                MessageBox.Show("Please enter Email address");

            }
            else if (!isValid)
            {
                MessageBox.Show("Please enter valid email address");
            }
            else
            {           
                            
                SqlConnection conn = new SqlConnection(@"Data Source=13.126.136.46\SWS,1433; Initial Catalog=DFN; User ID=sa;Password=poi$098#");
                //var submittedPatientId = txtPatientID.Text;
                //bool IsPatientIdExist = false;
                //for(int i = 0; i < records.Count; i++)
                //{
                //    if (submittedPatientId == records[i].PatientId)
                //    {
                //        IsPatientIdExist = true;
                //        break;
                //    }
                //}
                //if (IsPatientIdExist)
                //{

                //}
                //else
                //{
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        // cmd.CommandText = "insert into tblPatientInfo values('" + txtPatientID.Text + "','" + txtPatientName.Text + "','" + txtLastName.Text + "'," + dpDOB.Text + "," + Convert.ToInt32(cbAge.Text) + ",'" + cbGender.Text + "','" + txtSSN.Text + "','" + txtAddress1.Text + "','" + txtAddress2.Text + "','" + txtCity.Text + "','" + txtState.Text + "','" + cbCountry.Text + "','" + txtZip.Text + "','" + txtArchiveLabel.Text + "','" + txtContactNum1.Text + "','" + txtContactNum2.Text + "','" + txtEmail.Text + "','" + txtRefDoctor.Text + "'," + dpLastVisit.Text + ",'" + txtDiagnostics.Text + "','"+ fileName+ "','"+hospital+"','"+dToday+"','"+dTime+"')";
                        // cmd.CommandText = "insert into tblPatientInfo values('" + txtPatientID.Text + "','" + txtPatientName.Text + "','" + txtLastName.Text + "'," + dpDOB.Text + "," + Convert.ToInt32(cbAge.Text) + ",'" + cbGender.Text + "','" + txtAddress1.Text + "','" + txtAddress2.Text + "','" + txtCity.Text + "','" + txtState.Text + "','" + cbCountry.Text + "','" + txtZip.Text + "','" + txtContactNum1.Text + "','" + txtContactNum2.Text + "','" + txtEmail.Text + "','" + txtRefDoctor.Text + "'," + dpLastVisit.Text + ",'" + txtDiagnostics.Text + "','" + fileName + "','" + hospital + "','" + dToday + "','" + dTime + "')";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_insert_tblPatientInfo";

                        cmd.Parameters.AddWithValue("@PATIENTID", txtPatientID.Text);
                        cmd.Parameters.AddWithValue("@FIRSTNAME", txtPatientName.Text);
                        cmd.Parameters.AddWithValue("@LASTNAME", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@DOB", dpDOB.Text);
                        cmd.Parameters.AddWithValue("@AGE", Convert.ToInt32(cbAge.Text));
                        cmd.Parameters.AddWithValue("@GENDER", cbGender.Text);
                        cmd.Parameters.AddWithValue("@ADDRESS1", txtAddress1.Text);
                        cmd.Parameters.AddWithValue("@ADDRESS2", txtAddress2.Text);
                        cmd.Parameters.AddWithValue("@CITY", txtCity.Text);
                        cmd.Parameters.AddWithValue("@PSTATE", txtState.Text);
                        cmd.Parameters.AddWithValue("@PCOUNTRY", cbCountry.Text);
                        cmd.Parameters.AddWithValue("@ZIPCODE", txtZip.Text);
                        cmd.Parameters.AddWithValue("@CONTACTNUM1", txtContactNum1.Text);
                        cmd.Parameters.AddWithValue("@CONTACTNUM2", txtContactNum2.Text);
                        cmd.Parameters.AddWithValue("@PEMAIL", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@REFERENCEDOCTOR", txtRefDoctor.Text);
                        cmd.Parameters.AddWithValue("@LASTVISIT", dpLastVisit.Text);
                        cmd.Parameters.AddWithValue("@DIAGNOSIS", txtDiagnostics.Text);
                        cmd.Parameters.AddWithValue("@FILENAME", fileName);
                        cmd.Parameters.AddWithValue("@HOSPITAL", hospital);
                        cmd.Parameters.AddWithValue("@ODATE", dToday);
                        cmd.Parameters.AddWithValue("@OTIME", dTime);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record saved");
                        conn.Close();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }
              //  }
                
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtPatientID.Text = "";
            this.txtPatientName.Text = "";
            this.txtLastName.Text = "";
            this.dpDOB.Text = "";
            this.cbAge.Text = "0";
            this.cbGender.Text = "Select";
           
            this.txtAddress1.Text = "";
            this.txtAddress2.Text = "";
            this.txtCity.Text = "";
            this.txtState.Text = "";
            this.cbCountry.Text = "Select";
            this.txtZip.Text = "";
         
            this.txtContactNum1.Text = "";
            this.txtContactNum2.Text = "";
            this.txtEmail.Text = "";
            this.dpLastVisit.Text = "";
            this.txtRefDoctor.Text = "";
            this.txtDiagnostics.Text = "";
            this.cbName.Text = "Search by Name";
            this.cbPatientId.Text = "Search by PatientId";
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public bool IsValidEmailAddress(string s)
        {
            Regex reg = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return reg.IsMatch(s);
        }


        private void txtContactNum1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show("Only numbers please");
            }
        }

        private void txtContactNum2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show("Only numbers please");
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo pInfo = new ProcessStartInfo("obs64");
                pInfo.WorkingDirectory = @"C:\Program Files\obs-studio\bin\64bit";
                Process.Start(pInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void txtZip_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show("Only numbers please");
            }
        }

        private void cbAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (e.Handled)
            {
                MessageBox.Show("Only numbers please");
            }
        }

        private void dpDOB_Error(object sender, ValidationErrorEventArgs e)
        {

        }

        private void dpDOB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void dpDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            var now = DateTime.Parse(DateTime.Now.ToString()).Year;
            var dob = DateTime.Parse(dpDOB.Text).Year;
            var age = now - dob;
            cbAge.Text = age.ToString();
        }

        private void cbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void cmbMobNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void cbName_DropDownClosed(object sender, EventArgs e)
        {
            var selectedName = cbName.Text;
            for(int i = 0; i < records.Count; i++)
            {
                if(selectedName==records[i].PatientName.ToString())
                {
                    this.txtPatientID.Text = records[i].PatientId;
                    this.txtPatientName.Text = records[i].PatientName;
                    this.txtLastName.Text = records[i].LastName;
                    this.dpDOB.Text = records[i].DOB.ToString();
                    this.cbAge.Text = records[i].Age.ToString();
                    this.cbGender.Text = records[i].Gender;

                    this.txtAddress1.Text = records[i].Address1;
                    this.txtAddress2.Text = records[i].Address2;
                    this.txtCity.Text = records[i].City;
                    this.txtState.Text = records[i].PState;
                    this.cbCountry.Text = records[i].PCountry; 
                    this.txtZip.Text = records[i].ZIPCode;

                    this.txtContactNum1.Text = records[i].ContactNumber1; 
                    this.txtContactNum2.Text = records[i].ContactNumber2;
                    this.txtEmail.Text = records[i].PEmail;
                    this.dpLastVisit.Text = records[i].LastVisit.ToString();
                    this.txtRefDoctor.Text = records[i].RefDoctor;
                    this.txtDiagnostics.Text = records[i].Diagnosis;
                }
            }


        }

        private void cbPatientId_DropDownClosed(object sender, EventArgs e)
        {
            var selectedId = cbPatientId.Text;
            for (int i = 0; i < records.Count; i++)
            {
                if (selectedId == records[i].PatientId.ToString())
                {
                    this.txtPatientID.Text = records[i].PatientId;
                    this.txtPatientName.Text = records[i].PatientName;
                    this.txtLastName.Text = records[i].LastName;
                    this.dpDOB.Text = records[i].DOB.ToString();
                    this.cbAge.Text = records[i].Age.ToString();
                    this.cbGender.Text = records[i].Gender;

                    this.txtAddress1.Text = records[i].Address1;
                    this.txtAddress2.Text = records[i].Address2;
                    this.txtCity.Text = records[i].City;
                    this.txtState.Text = records[i].PState;
                    this.cbCountry.Text = records[i].PCountry;
                    this.txtZip.Text = records[i].ZIPCode;

                    this.txtContactNum1.Text = records[i].ContactNumber1;
                    this.txtContactNum2.Text = records[i].ContactNumber2;
                    this.txtEmail.Text = records[i].PEmail;
                    this.dpLastVisit.Text = records[i].LastVisit.ToString();
                    this.txtRefDoctor.Text = records[i].RefDoctor;
                    this.txtDiagnostics.Text = records[i].Diagnosis;
                }
            }
        }
    }
}
