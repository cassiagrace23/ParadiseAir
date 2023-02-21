using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Data;
using ParadiseAir.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.ComponentModel;


namespace ParadiseAir.ViewModels
{
    public class ShellViewModel : Screen, IDataErrorInfo
    {
        private string _manufacturer = "";
        private string _model = "";
        private string _shape = "";
        private string _caliber = "";
        private string _actualSize = "";
        private string _grains = "";

        private string _pelletsPerTin = "";
        private string _tinCount = "";
        private string _openCount = "";

        private string _outputMsg = "";

        private DataTable _pelletData = new DataTable("Pellets");
        private DataTable _tinData = new DataTable("Tins");

        private Dictionary<string, bool> validProperties;
        private bool _allValid = false;

        //private string _paradiseConnect = "Server=localhost; Database=paradise; Uid=ParadiseAir; Password=Andromeda23!";

        public ShellViewModel()
        {
            GetPelletData();
            GetTinData();

            validProperties = new Dictionary<string, bool>();
            validProperties.Add("Manufacturer", false);
            validProperties.Add("Model", false);
            validProperties.Add("Caliber", false);
            validProperties.Add("ActualSize", false);
            validProperties.Add("Grains", false);
            validProperties.Add("PelletsPerTin", false);
            validProperties.Add("TinCount", false);
            validProperties.Add("OpenCount", false);
        }

        public void ClearData()
        {
            _manufacturer = "";
            _model = "";
            _caliber = "";
            _actualSize = "";
            _grains = "";
            _pelletsPerTin = "";
            _tinCount = "";
            _openCount = "";
        }

        public bool AllValid
        {
            get { return _allValid; }
            set
            {
                if (_allValid != value)
                {
                    _allValid = value;
                    NotifyOfPropertyChange(() => AllValid);
                }
            }
        }

        public string OutputMsg
        {
            get { return _outputMsg; }
            set { _outputMsg = value; NotifyOfPropertyChange(() => OutputMsg); }
        }

        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; NotifyOfPropertyChange(() => Manufacturer); }
        }

        public string Model
        {
            get { return _model; }
            set { _model = value; NotifyOfPropertyChange(() => Model); }
        }
        public string Shape
        {
            get { return _shape; }
            set { _shape = value; NotifyOfPropertyChange(() => Shape); }
        }
        public string Caliber
        {
            get { return _caliber; }
            set { _caliber = value; NotifyOfPropertyChange(() => Caliber); }
        }
        public string ActualSize
        {
            get { return _actualSize; }
            set { _actualSize = value; NotifyOfPropertyChange(() => ActualSize); }
        }
        public string Grains
        {
            get { return _grains; }
            set { _grains = value; NotifyOfPropertyChange(() => Grains); }
        }
        public string PelletsPerTin
        {
            get { return _pelletsPerTin; }
            set { _pelletsPerTin = value; NotifyOfPropertyChange(() => PelletsPerTin); }
        }
        public string TinCount
        {
            get { return _tinCount; }
            set { _tinCount = value; NotifyOfPropertyChange(() => TinCount); }
        }
        public string OpenCount
        {
            get { return _openCount; }
            set { _openCount = value; NotifyOfPropertyChange(() => OpenCount); }
        }

        public bool CanAddData(string manufacturer, string model, string shape, string caliber, string actualSize, string grains, string pelletsPerTin, string tinCount, string openCount, string outputMsg)
        {
            _allValid = true;
            foreach (bool isValid in validProperties.Values)
            {
                if (isValid == false)
                {
                    _allValid = false;
                }
            }
            return _allValid;
        }

        public void AddData(string manufacturer, string model, string shape, string caliber, string actualSize, string grains, string pelletsPerTin, string tinCount, string openCount, string outputMsg)
        {
            // add pellet type to DB

            string ConString = ConfigurationManager.ConnectionStrings["paradiseConnect"].ConnectionString;
            string CmdString = string.Empty;

            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "paradise.add_data";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@make", manufacturer);
                cmd.Parameters["@make"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters["@model"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@shape", shape);
                cmd.Parameters["@shape"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@caliber", caliber);
                cmd.Parameters["@caliber"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@grains", grains);
                cmd.Parameters["@grains"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@actualsize", actualSize);
                cmd.Parameters["@actualsize"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@ppt", pelletsPerTin);
                cmd.Parameters["@ppt"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@tincount", tinCount);
                cmd.Parameters["@tincount"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("@opencount", openCount);
                cmd.Parameters["@opencount"].Direction = ParameterDirection.Input;
                cmd.ExecuteNonQuery();
            }
            ClearData();
            GetPelletData();
            GetTinData();
            NotifyOfPropertyChange(() => PelletData);
            NotifyOfPropertyChange(() => TinData);
            _outputMsg += "Data added.\n";
            NotifyOfPropertyChange(() => OutputMsg);
        }

        public DataTable PelletData
        {
            get { return _pelletData; }
            set
            {
                if (_pelletData == value)
                    return;
            }
        }

        public DataTable TinData
        {
            get { return _tinData; }
            set
            {
                if (_tinData == value)
                    return;
            }
        }
        
        public void GetPelletData()
        {
            string ConString = ConfigurationManager.ConnectionStrings["paradiseConnect"].ConnectionString;
            string CmdString = string.Empty;
            _pelletData.Clear();

            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                CmdString = "SELECT * FROM paradise.pellet_data";
                MySqlCommand cmd = new MySqlCommand(CmdString, con);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(_pelletData);
                return;
            }
        }

        public void GetTinData()
        {
            string ConString = ConfigurationManager.ConnectionStrings["paradiseConnect"].ConnectionString;
            string CmdString = string.Empty;
            _tinData.Clear();

            using (MySqlConnection con = new MySqlConnection(ConString))
            {
                CmdString = "SELECT * FROM paradise.tin_data";
                MySqlCommand cmd = new MySqlCommand(CmdString, con);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(_tinData);
                return;
            }
        }

         // Data Validation
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        
        // Data Validation
        public string this[string propertyName]
        {
            get
            {
                string result = null;
                bool valid = true;
                int intValue = 0;
                decimal decimalValue = 0.0m;
                decimal decimalValue2 = 0.0m;

                switch (propertyName)
                {
                    case "OutputMsg":
                        {
                            valid = true;
                            break;
                        }
                    case "Manufacturer":
                        {
                            if (String.IsNullOrWhiteSpace(_manufacturer))
                                valid = false;
                            if (!valid)   // add validation logic
                            {
                                result = "Invalid Manufacturer";
                            }
                            break;
                        }
                    case "Model":
                        {
                            if (String.IsNullOrWhiteSpace(_model))
                                valid = false;
                            if (!valid)   // add validation logic
                            {
                                result = "Invalid Model";
                            }
                            break;
                        }
                    case "Shape":
                        {
                            if (String.IsNullOrWhiteSpace(_shape))
                                valid = false;
                            if (!valid)   // add validation logic
                            {
                                result = "Invalid Shape";
                            }
                            break;
                        }
                    case "Caliber":
                        {
                            try { decimalValue = System.Convert.ToDecimal(_caliber); }
                            catch (System.OverflowException)
                            {
                                valid = false;
                            }
                            catch (System.FormatException)
                            {
                                valid = false;
                            }
                            catch (System.ArgumentNullException)
                            {
                                valid = false;
                            }

                            if (valid)
                            {
                                switch (decimalValue)
                                {
                                    case 0.177m:
                                    case 0.20m:
                                    case 0.22m:
                                    case 0.25m:
                                    case 0.30m:
                                    case 0.35m:
                                        break;
                                    default:
                                        valid = false;
                                        break;
                                }
                            }

                            if (!valid)   // add validation logic
                            {
                                result = "Invalid Caliber";
                            }
                            break;
                        }
                    case "ActualSize":
                        {
                            try { decimalValue = System.Convert.ToDecimal(_actualSize); decimalValue2 = System.Convert.ToDecimal(_caliber); }
                            catch (System.OverflowException)
                            {
                                valid = false;
                            }
                            catch (System.FormatException)
                            {
                                valid = false;
                            }
                            catch (System.ArgumentNullException)
                            {
                                valid = false;
                            }

                            if (valid)
                            {
                                if ((decimalValue - decimalValue2) < -0.01m || (decimalValue - decimalValue2) > 0.01m)
                                    valid = false;
                            }

                            if (!valid)   // add validation logic
                                result = "Invalid Size";
                            break;
                        }
                    case "Grains":
                        {
                            try { decimalValue = System.Convert.ToDecimal(_grains); }
                            catch (System.OverflowException)
                            {
                                valid = false;
                            }
                            catch (System.FormatException)
                            {
                                valid = false;
                            }
                            catch (System.ArgumentNullException)
                            {
                                valid = false;
                            }

                            if (valid)
                            {
                                if (decimalValue < 4.0m)
                                    valid = false;
                            }

                            if (!valid)   // add validation logic
                                result = "Invalid Grains";
                            break;
                        }
                    case "PelletsPerTin":
                        {
                            try { intValue = System.Convert.ToInt32(_pelletsPerTin); }
                            catch (System.OverflowException)
                            {
                                valid = false;
                            }
                            catch (System.FormatException)
                            {
                                valid = false;
                            }
                            catch (System.ArgumentNullException)
                            {
                                valid = false;
                            }

                            if (valid)
                            {
                                if (intValue < 25)
                                    valid = false;
                            }

                            if (!valid)   // add validation logic
                                result = "Invalid Pellets-Per-Tin value";
                            break;
                        }
                    case "TinCount":
                        {
                            try { intValue = System.Convert.ToInt32(_tinCount); }
                            catch (System.OverflowException)
                            {
                                valid = false;
                            }
                            catch (System.FormatException)
                            {
                                valid = false;
                            }
                            catch (System.ArgumentNullException)
                            {
                                valid = false;
                            }

                            if (valid)
                            {
                                if (intValue < 1)
                                    valid = false;
                            }

                            if (!valid)   // add validation logic
                                result = "Invalid Tin Count";
                            break;
                        }
                    case "OpenCount":
                        {
                            try { intValue = System.Convert.ToInt32(_openCount); }
                            catch (System.OverflowException)
                            {
                                valid = false;
                            }
                            catch (System.FormatException)
                            {
                                valid = false;
                            }
                            catch (System.ArgumentNullException)
                            {
                                valid = false;
                            }
                            if (!valid)   // add validation logic
                                result = "Invalid Open Count";
                            break;
                        }
                }
                validProperties[propertyName] = valid;
                return result;
            }
        }
    }
}
