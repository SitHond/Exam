using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public class Session
    {
        public static string dbConnectionString = Settings.Default.ConnectionString;
        public static string CurrentUsername { get; set; }
    }
}
