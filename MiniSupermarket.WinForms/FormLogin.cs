using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;

namespace MiniSupermarket.WinForms
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        // su kien khi nguoi dung bam nut Dang nhap
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text.Trim();

            // kiem tra rang buoc co ban phia Client
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // khoa nut de tranh bam nhieu lan khi dang cho Server
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";

                // dong goi du lieu gui len endpoint POST /api/auth/login qua HttpClient dung chung
                var loginData = new { Username = username, Password = password };
                var response = await ApiClientService.Client.PostAsJsonAsync("auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    // Doc chuoi JSON tra ve tu Server khi dang nhap thanh cong
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonString);
                    var root = doc.RootElement;

                    // trich xuat Token, Role va thong tin User luu vao SessionManager (bien tinh)
                    SessionManager.JwtToken = root.GetProperty("token").GetString() ?? string.Empty;
                    SessionManager.CurrentRole = root.GetProperty("role").GetString() ?? string.Empty;
                    SessionManager.CurrentUsername = root.TryGetProperty("username", out var uElem) ? uElem.GetString() ?? username : username;
                    SessionManager.CurrentFullName = root.TryGetProperty("fullName", out var fnElem) ? fnElem.GetString() ?? username : username;

                    MessageBox.Show($"Đăng nhập thành công!\nNhân viên: {SessionManager.CurrentFullName}\nVai trò: {SessionManager.CurrentRole}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FormMainShell shellForm = new FormMainShell();
                    this.Hide();
                    shellForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    string errorMsg = response.StatusCode == System.Net.HttpStatusCode.Unauthorized
                        ? "Sai tài khoản hoặc mật khẩu!"
                        : $"Máy chủ báo lỗi ({(int)response.StatusCode}). Kiểm tra lại API và cơ sở dữ liệu!";
                    try
                    {
                        using var errDoc = JsonDocument.Parse(errorJson);
                        if (errDoc.RootElement.TryGetProperty("message", out var msgProp))
                        {
                            errorMsg = msgProp.GetString() ?? errorMsg;
                        }
                    }
                    catch { }

                    MessageBox.Show(errorMsg, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // mat mang / API chua chay
                MessageBox.Show("Lỗi kết nối đến Server: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // luon mo lai nut du thanh cong hay loi
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập hệ thống";
            }
        }
    }
}
