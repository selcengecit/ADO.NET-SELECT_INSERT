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

namespace Connected_Insert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(Tools.ConnectionString);
        SqlCommand cmd;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            #region SQL INJECTION MANTIĞI
            //'or' a '=' a sorgu bu şekilde sql injection'a açıktır. Bu aşamada bu vurgulanmaktadır.

            //SQL INJECTION MANTIĞI=>
            //SqlCommand cmd= new SqlCommand("INSERT INTO Categories (CategoryName , Description) values ('" + txtCategory.Text + "','" + txtDescription.Text"')" , conn);
            //Burada Drop database , drop table gibi gibi komutlar gösterilebilir.
            //Injectıon da tırnak hatası yüzünden hatalı bir girdi oluşturmamak için güvenlik açısından da parametre giriyoruz.
            #endregion
            try
            {
                if (txtCategory.Text != "")
                {
                    conn.Open();
                    cmd = new SqlCommand("INSERT INTO Categories (CategoryName,Description) VALUES (@catName,@desc) SELECT CAST (scope_identity() AS int)", conn);

                    //INSERT INTO Categories (CategoryName) VALUES ('TEST') SELECT @@IDENTITY
                    cmd.Parameters.AddWithValue("@catName", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@desc", txtDescription.Text);

                    int categoryID = (int)cmd.ExecuteScalar();
                    label4.Text = categoryID.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            if (conn.State == ConnectionState.Open)
                conn.Close();
            #region Etkilenen Satır Sayısıyla Kayıt Kontrolü
            //if(etkilenenSatirSayisi>0)
            //{
            //    MessageBox.Show("Kayıt Başarı ile Eklenmiştir.");
            //}
            //else
            //{
            //    MessageBox.Show("Kayıt Eklenirken Bir Hata Oluştu.");
            //}

            //Ya da alttaki şekilde yazabiliriz.
            #endregion

        }
        #region
        private void label4_Click(object sender, EventArgs e)
        {

        }
        #endregion
        private void btnInsertSP_Click(object sender, EventArgs e)
        {
            int donenDeger = 0;
            #region SQL için Stored Procedure
            /*CREATE PROCEDURE INSERTCAT
                    @catName nvarchar(15) ,
					@catDesc nText
            AS
            BEGIN

            SET NOCOUNT ON
            INSERT INTO Categories(CategoryName, Description) VALUES(@catName, @catDesc)
            END
            GO*/
            #endregion
            try
            {
                if (txtCategory.Text != "")
                {
                    SqlCommand cmd = new SqlCommand("INSERTCAT", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@catName", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@catDesc", txtDescription.Text);

                    conn.Open();

                    donenDeger = cmd.ExecuteNonQuery();

                    MessageBox.Show(donenDeger != 0 ? "İŞLEM BAŞARILI" : "İŞLEM BAŞARISIZ");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("BİR HATA OLUŞTU" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
