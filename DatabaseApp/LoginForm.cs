using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DatabaseApp
{
    public partial class LoginForm : Form
    {
        private DatabaseHelper dbHelper;

        public LoginForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            string passwordHash = ComputeHash(password);

            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";
            SqlParameter[] parameters = {
                new SqlParameter("@Email", email),
                new SqlParameter("@PasswordHash", passwordHash)
            };

            int count = (int)dbHelper.ExecuteScalar(query, parameters);
            if (count == 1)
            {
                MessageBox.Show("Login successful!");
                // You can proceed to the next form or functionality here
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.Show();
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
