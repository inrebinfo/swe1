using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Data.SqlClient;

namespace EmbeddedSensorCloud
{
    public class CTempSensor
    {
        public void Run()
        {
            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(CreateData);
            timer.Interval = 5000;
            timer.Enabled = true;
        }

        private void CreateData(object source, ElapsedEventArgs e)
        {
            try
            {
                Random randomNumb = new Random();
                float result = randomNumb.Next(100, 300);
                float temp = (result / 10);

                using (SqlConnection dbcon = new SqlConnection(@"Data Source=.\SqlExpress;Initial Catalog=EmbeddedSensorCloud;Integrated Security=true;"))
                {
                    dbcon.Open();

                    string temp_str = temp.ToString();
                    temp_str = temp_str.Replace(",", ".");

                    SqlCommand insert = new SqlCommand(@"INSERT INTO [temperatures] ([day], [temp]) VALUES (GETDATE(), " + temp_str + ")", dbcon);
                    insert.ExecuteNonQuery();

                    dbcon.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
