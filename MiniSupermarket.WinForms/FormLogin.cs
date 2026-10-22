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

        // Sự kiện khi người dùng bấm nút Đăng nhập
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text.Trim();

            // Kiểm tra ràng buộc cơ bản phía Client
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";

                // Đóng gói dữ liệu gửi lên endpoint POST /api/auth/login
                var loginData = new { Username = username, Password = password };
                var response = await ApiClientService.Client.PostAsJsonAsync("auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc chuỗi JSON trả về từ Server khi đăng nhập thành công
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonString);
                    var root = doc.RootElement;

                    // Trích xuất Token, Role và thông tin User lưu vào SessionManager
                    SessionManager.JwtToken = root.GetProperty("token").GetString() ?? string.Empty;
                    SessionManager.CurrentRole = root.GetProperty("role").GetString() ?? string.Empty;
                    SessionManager.CurrentUsername = root.TryGetProperty("username", out var uElem) ? uElem.GetString() ?? username : username;
                    SessionManager.CurrentFullName = root.TryGetProperty("fullName", out var fnElem) ? fnElem.GetString() ?? username : username;

                    MessageBox.Show($"Đăng nhập thành công!\nNhân viên: {SessionManager.CurrentFullName}\nVai trò: {SessionManager.CurrentRole}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở Form điều khiển trung tâm (FormMainShell) và ẩn Form đăng nhập đi
                    FormMainShell shellForm = new FormMainShell();
                    this.Hide();
                    shellForm.ShowDialog();
                    this.Close(); // Đóng hẳn ứng dụng khi shell chính tắt
                }
                else
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    string errorMsg = "Sai tài khoản hoặc mật khẩu!";
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
                MessageBox.Show("Lỗi kết nối đến Server: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập hệ thống";
            }
        }
    }
}
