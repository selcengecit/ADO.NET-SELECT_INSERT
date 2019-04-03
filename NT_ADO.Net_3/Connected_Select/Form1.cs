using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Connected_Select
{
    //Yapılacaklar Listesi
    #region
    // SqlConnection ile conn intance'ı oluşturulur.
    //ConfigurationManager ile ConnectionString için path belirlenir.
    //class ile çalışılırsa ConnectionString'i Tools.ConnectionString contructor'ı ile belirleriz.
    // SqlCommand ile cmd intance'ı oluşturulur.
    //select sorgusu intance oluşturulurken parametre olarak da girilebilir, harici olarak da girilebilir cmd.Command.Text ile.
    //Commandtype belirlenir. (cmd.CommandType=CommandType.Text ile)
    //conn.Open(); ile bağlantı açılır.
    //SqlDataReader rdr gibi bir intance alınır.
    //rdr.Read() methodu ile while de databaseden veriler okunur.
    //rdr[sütunAdı].ToString(); ile eklenecek yere yazdırılır.
    //conn.Close(); ile bağlantı kapatılır.
    #endregion
    public partial class Form1 : Form
    {
        SqlConnection conn;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString; // Connection için path belirlenir. 
                //App.config'deki connection adındaki bağlantının connection string'ini alır.
            }
            catch
            {
                MessageBox.Show("Server'a bağlanılamadı.");
            }
        }

        private void btnGetir_Click(object sender, EventArgs e)
        {
            //GetOrdinal verilen stringin select sorgusunda kaçıncı kolon olduğunu gösterir.
            //Tools classı ile bağlanmak istersek.
            #region
            //SqlConnection conn = new SqlConnection(Tools.ConnectionString);
            //conn.ConnectionString = Tools.ConnectionString;
            #endregion
            SqlCommand cmd = new SqlCommand("SELECT EmployeeID, FirstName , LastName FROM Employees", conn);
            #region
            //cmd.Connection = conn;
            //cmd.CommandType = CommandType.Text;
            #endregion
            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            #region
            //conn.Close(); burada close yapılır mı?
            //Yapamayız çünkü read methodu açık bağlantı ister.
            #endregion
            #region
            /*
             * EXECUTE READER TÜRLERi
             * 1-ExecuteReader--> Resultset'in tamamını geri döndürür.
             * 2-ExecuteNonQuery--> Etkilenen satır sayısını geri döndürür.
             * 3-ExecuteScaler-->Resultset'in 1.sütununun 1.satırını geriye döndürür(tek değer)
             */
            #endregion
            if (dr.HasRows)
            {
                #region
                //Kaç satır veri olduğnu biliyorum
                //for için için başlangıç ve bitiş noktası gerekir.
                // foreach için bir koleksiyon gerekir bu nedenle while döngüsü kullanırlır.
                #endregion
                while (dr.Read())
                {
                    string id = dr["EmployeeID"].ToString();
                    string isim = dr["FirstName"].ToString();
                    string soyİsim = dr["LastName"].ToString();
                    lstEmployees.Items.Add(id + "-" + isim + " " + soyİsim);
                }
            }
            conn.Close();
        }
    }
}
