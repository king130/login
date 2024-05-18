using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DatabaseApp
{
    public partial class SignUpForm : Form
    {
        private DatabaseHelper dbHelper;

        public SignUpForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            DateTime dateOfBirth = dtpDateOfBirth.Value;
            bool termsAccepted = chkTerms.Checked;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || !termsAccepted)
            {
                MessageBox.Show("Please fill all fields and accept the terms.");
                return;
            }

            string passwordHash = ComputeHash(password);

            string query = "INSERT INTO Users (Name, Email, PasswordHash, DateOfBirth, TermsAccepted) VALUES (@Name, @Email, @PasswordHash, @DateOfBirth, @TermsAccepted)";
            dbHelper.ExecuteNonQuery(query,
                new SqlParameter("@Name", name),
                new SqlParameter("@Email", email),
                new SqlParameter("@PasswordHash", passwordHash),
                new SqlParameter("@DateOfBirth", dateOfBirth),
                new SqlParameter("@TermsAccepted", termsAccepted));

            MessageBox.Show("Sign-up successful!");
            this.Close();
        }

        private string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

