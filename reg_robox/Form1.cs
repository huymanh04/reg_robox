using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using xNet;
using System.Text.RegularExpressions;
using Keys = OpenQA.Selenium.Keys;
using System.Xml.Linq;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using reg_robox.Properties;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using OpenQA.Selenium.Chrome.ChromeDriverExtensions;
using Microsoft.VisualBasic;
namespace reg_robox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.TextChanged += (s, e) => { Properties.Settings.Default.apikey = textBox1.Text; Properties.Settings.Default.Save(); };
            textBox1.Text = Properties.Settings.Default.apikey;
            textBox3.TextChanged += (s, e) => { Properties.Settings.Default.IPS = textBox3.Text; Properties.Settings.Default.Save(); };
            textBox3.Text = Properties.Settings.Default.IPS;
            numericUpDown3.ValueChanged += (s, e) => { Properties.Settings.Default.time = (int)numericUpDown3.Value; Properties.Settings.Default.Save(); };
            numericUpDown3.Value = Properties.Settings.Default.time;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            istop = false;
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = true;
            List<Thread> threads = new List<Thread>();
            Thread T = new Thread(() =>
            {
                try
                {
                    int dem = 0;
                    int b = 0;
                    int dem1 = 0; Screen primaryScreen = Screen.PrimaryScreen;
                    int screenWidth = primaryScreen.Bounds.Width / 200;
                    // int screenWidth1 = primaryScreen.Bounds.Height / 200;
                    for (int i = 0; i < numericUpDown1.Value; i++)
                    {
                        if (istop == true) { return; }
                        var apikkkk = "";
                        var key = Regex.Split(textBox1.Text, "\n");
                        if (radioButton2.Checked)
                        {
                            if (key.Length > numericUpDown2.Value) { MessageBox.Show("VUi lòng nhập luồng bé hơn list key hoặc bằng"); return; }

                            if (dem1 == key.Length) { dem1 = 0; }
                            apikkkk = key[dem1++];
                        }

                        this.Invoke((MethodInvoker)delegate { dataGridView1.Rows.Add((dataGridView1.RowCount + 1).ToString()); });
                        int x, y; if (b >= screenWidth)
                        {
                            x = 500 * (b % screenWidth);
                        }
                        else
                        {
                            x = 500 * b;
                        }
                        y = (b / screenWidth) * 700;


                        int size = (int)(200 * Convert.ToInt32(b % screenWidth));
                        int size2 = (int)(400 * Convert.ToInt32(b / screenWidth));
                        size = x; size2 = y;
                        Thread t1 = new Thread(() => { star(i, size, size2, apikkkk); }); t1.Start();
                        dem++; threads.Add(t1); b++; Thread.Sleep(800);
                        if (dem == numericUpDown2.Value)
                        {
                            foreach (Thread t2 in threads) { try { t2.Join(); } catch { } }
                            dem = 0;
                            b = 0;
                            try
                            {
                                Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");

                                foreach (var chromeDriverProcess in chromeDriverProcesses)
                                {
                                    chromeDriverProcess.Kill();
                                }
                            }
                            catch { }
                        }


                    }
                }
                catch (Exception ea) { File.AppendAllText("oka.txt", ea.Message + "|=> " + ea.StackTrace); }
                this.Invoke((MethodInvoker)delegate
                {
                    guna2Button1.Enabled = true;
                    guna2Button2.Enabled = false;
                });
            }); T.Start();
        }
        void stt(int i, string text)
        {
            this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].Cells[6].Value = text; });
        }
        void star(int i, int size, int size2, string apikey)
        {
            var code2 = ""; var email = "";
            Thread t = null;
            List<int> driverProcessIds = new List<int>();
            IWebDriver chrome = null;
            string mk = "";
            string tk = ""; string domain = ""; int port = 0;
            try
            {
                stt(i, "Bắt đầu"); string ip = "";
                if (radioButton2.Checked)
                {
                    stt(i, "Get proxy");
                    ip = newproxy(apikey.Replace("\r", ""), i);

                }
                Random r = new Random();
                int demoi = 0;
            okaaaa:
                if (demoi++ == 3) { chrome.FindElement(By.XPath($"//select[@id='YearDropdown']//option[{r.Next(21, 54)}]")).Click(); }

                Point loca = new Point();
                loca.X = size;
                loca.Y = size2;
                tk = GenerateRandomPassword1();
                if (checkBox1.Checked) { mk = GenerateRandomPassword(); } else { mk = textBox2.Text; }
                this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].Cells[2].Value = tk; dataGridView1.Rows[i].Cells[3].Value = mk; });
                IJavaScriptExecutor js = null;
                ChromeOptions chromeOptions = new ChromeOptions();
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                chromeOptions.AddArguments("start-maximized");
                chromeOptions.AddArgument("--disabel-notifications");
                chromeOptions.AddArgument("--disabel-images");
                chromeOptions.AddExcludedArgument("enable-automation");

                if (radioButton2.Checked)
                {
                    domain = ip.Split('|')[0].Split(':')[0];
                    port = int.Parse(ip.Split('|')[0].Split(':')[1]);
                    this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].Cells[1].Value = domain; });
                    chromeOptions.AddExtension("Proxy Auto Auth.crx");
                    chromeOptions.AddHttpProxy(domain, port, ip.Split('|')[1], ip.Split('|')[2]);
                    // chromeOptions.Proxy = new Proxy { HttpProxy = domain, SslProxy = domain, Kind = ProxyKind.Manual };
                    chromeOptions.AddArgument("-proxy-server=" + ip.Split('|')[0]);
                }
                chromeOptions.AddArguments(new string[]
                {
            "--disable-3d-apis",
            "--disable-background-networking",
            "--disable-bundled-ppapi-flash",
            "--disable-client-side-phishing-detection",
            "--disable-default-apps",
            "--disable-hang-monitor",
            "--disable-gpu",
            "--no-sandbox",
            "--disable-prompt-on-repost",
            "--disable-sync",
            "--use-fake-device-for-media-stream",
            "--use-fake-ui-for-media-stream",
            "--disable-webgl",
            "--enable-blink-features=ShadowDOMV0",
            "--enable-logging",
            "--disable-notifications",
            //"--app=https://www.roblox.com/",
            "--disable-dev-shm-usage",
            "--disable-web-security",
            "--disable-rtc-smoothness-algorithm",
            "--disable-webrtc-hw-decoding",
            "--disable-webrtc-hw-encoding",
            "--disable-webrtc-multiple-routes",
            "--disabel-images",
            "--disable-webrtc-hw-vp8-encoding",
            "--enforce-webrtc-ip-permission-check",
            "--force-webrtc-ip-handling-policy",
            "--ignore-certificate-errors",
            "--disable-infobars",
            "--disable-popup-blocking",
            "--enable-precise-memory-info",
            "--disable-3d-apis",
            "--start-maximized",
            "--force-device-scale-factor=0.4",
            "--disable-blink-features=\"BlockCredentialedSubresources\"",
            "--mute-audio",
              "--window-position=" + loca.X + "," + loca.Y,
            "--window-size=200,700",
            "--disable-popup-blocking" });
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.geolocation", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.notifications", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.plugins", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.popups", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.auto_select_certificate", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.mixed_script", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.media_stream", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.media_stream_mic", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.media_stream_camera", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.protocol_handlers", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.midi_sysex", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.push_messaging", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.ssl_cert_decisions", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.metro_switch_to_desktop", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.protected_media_identifier", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.site_engagement", 1);
                chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.durable_storage", 1);
                chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                chromeOptions.AddExtension("manh.crx");
                chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                chromeOptions.AddArgument("--disabel-notifications");
                chromeOptions.AddArgument("--disabel-images");
                for (int y = 0; y < 2; y++)
                {


                    try
                    {
                        chrome = new ChromeDriver(chromeDriverService, chromeOptions);
                        driverProcessIds.Add(chromeDriverService.ProcessId);
                        try
                        {


                            var mos = new System.Management.ManagementObjectSearcher($"Select * From Win32_Process Where ParentProcessID={chromeDriverService.ProcessId}");
                            foreach (var mo in mos.Get())
                            {
                                var pid = Convert.ToInt32(mo["ProcessID"]);
                                driverProcessIds.Add(pid);
                            }
                        }
                        catch { }
                        break;
                        //NetworkAuthenticationHandler handler = new NetworkAuthenticationHandler()
                        //{
                        //    UriMatcher = d => true, //d.Host.Contains("your-host.com")
                        //    Credentials = new PasswordCredentials(ip.Split('|')[1], ip.Split('|')[2])
                        //};

                        //var networkInterceptor = chrome.Manage().Network;
                        //networkInterceptor.AddAuthenticationHandler(handler);
                        //networkInterceptor.StartMonitoring();
                    }
                    catch (Exception ex)
                    {
                    }
                }

                js = (IJavaScriptExecutor)chrome; IList<string> tabs = new List<string>(chrome.WindowHandles);
                if (radioButton2.Checked)
                {
                    for (int ew = 0; ew < tabs.Count; ew++)
                    {
                        chrome.SwitchTo().Window(chrome.WindowHandles[0]).Close(); Thread.Sleep(1000);
                        tabs = new List<string>(chrome.WindowHandles);
                        chrome.SwitchTo().Window(tabs[0]);
                    }
                }
                js.ExecuteScript("window.open();");
                tabs = new List<string>(chrome.WindowHandles);
                chrome.SwitchTo().Window(tabs[1]);
                chrome.SwitchTo().Window(chrome.WindowHandles[0]).Close(); Thread.Sleep(1000);
                tabs = new List<string>(chrome.WindowHandles);
                chrome.SwitchTo().Window(tabs[0]);
                t = new Thread(() =>
                {
                    for (int lk = 0; lk < (int)numericUpDown3.Value; lk++)
                    {
                        Thread.Sleep(60000);
                    }
                    if (chrome != null)
                    {
                        try { chrome.Close(); chrome.Quit(); } catch { }
                    }
                }); t.Start();
                if (radioButton2.Checked)
                {
                    chrome.Navigate().GoToUrl("chrome-extension://ggmdpepbjljkkkdaklfihhngmmgmpggp/options.html");
                    chrome.FindElement(By.Id("login")).SendKeys(ip.Split('|')[1]); Thread.Sleep(1000);
                    chrome.FindElement(By.Id("password")).SendKeys(ip.Split('|')[2]); Thread.Sleep(1000);
                    chrome.FindElement(By.Id("retry")).Clear(); Thread.Sleep(1000);
                    chrome.FindElement(By.Id("retry")).SendKeys("2");
                    Thread.Sleep(1000);
                    chrome.FindElement(By.Id("save")).Click();
                }
                chrome.Navigate().GoToUrl("https://www.roblox.com/");
                Thread.Sleep(r.Next(3, 6) * 1000);
            k: stt(i, "Random bỉthday");
                while (true)
                {

                    if (chrome.PageSource.Contains("This site can’t be reached") || chrome.PageSource.Contains("This page isn’t working")) { try { chrome.Close(); chrome = null; } catch { } goto okaaaa; }
                    try { chrome.FindElement(By.XPath($"//select[@id='MonthDropdown']//option[{r.Next(1, 12)}]")).Click(); break; } catch { }
                    Thread.Sleep(r.Next(1, 3) * 1000);
                }
                Thread.Sleep(r.Next(1, 3) * 1000);
                while (true)
                {
                    try { chrome.FindElement(By.XPath($"//select[@id='DayDropdown']//option[{r.Next(1, 28)}]")).Click(); break; } catch { }
                    Thread.Sleep(r.Next(1, 3) * 1000);
                }
                Thread.Sleep(r.Next(1, 3) * 1000);
                while (true)
                {
                    try { chrome.FindElement(By.XPath($"//select[@id='YearDropdown']//option[{r.Next(21, 54)}]")).Click(); break; } catch { }
                    Thread.Sleep(r.Next(1, 3) * 1000);
                }
                Thread.Sleep(r.Next(1, 3) * 1000);
                stt(i, "Nhập username");
                var trrrrr = js.ExecuteScript("return document.querySelector(\"#signup-username\").value");
                var trrrrrrrr = js.ExecuteScript("return document.querySelector(\"#signup-password\").value");
                if (trrrrr != string.Empty || trrrrrrrr != string.Empty) { chrome.Navigate().Refresh(); goto k; }
                sendkeys(chrome, By.Id("signup-username"), tk); Thread.Sleep(r.Next(1, 3) * 1000);
                try
                {
                    if (chrome.FindElement(By.Id("signup-usernameInputValidation")).Text != string.Empty)
                    {
                        js.ExecuteScript("arguments[0].select();", chrome.FindElement(By.Id("signup-username")));
                        chrome.FindElement(By.Id("signup-username")).SendKeys(Keys.Delete); chrome.FindElement(By.Id("signup-username")).Clear();
                        stt(i, "Nhập lại username");
                        tk = GenerateRandomPassword1();
                        this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].Cells[2].Value = tk; }); goto k;
                    }
                }
                catch { }
                stt(i, "Nhập mật khẩu");
                sendkeys(chrome, By.Id("signup-password"), mk); Thread.Sleep(r.Next(1, 3) * 1000); stt(i, "random giới tính");
                if (r.Next(1, 20) % 2 == 0)
                {
                    chrome.FindElement(By.Id("FemaleButton")).Click();
                }
                else { chrome.FindElement(By.Id("MaleButton")).Click(); }
                Thread.Sleep(r.Next(1, 3) * 1000); stt(i, "click");
                js.ExecuteScript("window.scrollBy(0,250)");
                chrome.FindElement(By.Id("signup-button")).Click(); Thread.Sleep(r.Next(4, 6) * 1000);

                while (true)
                {
                    stt(i, "ktra reg ok chưa");
                    if (chrome.PageSource.Contains("Sorry! An unknown error occurred. Please try again later.")) { chrome.FindElement(By.Id("cccccccccccc")); }
                    try
                    {

                        trrrrr = "";
                        trrrrr = js.ExecuteScript("return document.querySelector(\"#signup-username\").value");
                        if (trrrrr != string.Empty)
                        {
                            js.ExecuteScript("arguments[0].select();", chrome.FindElement(By.Id("signup-username")));
                            chrome.FindElement(By.Id("signup-username")).SendKeys(Keys.Delete); chrome.FindElement(By.Id("signup-username")).Clear();
                            stt(i, "Nhập lại username");
                            tk = GenerateRandomPassword1();
                            this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].Cells[2].Value = tk; }); goto k;
                        }
                    }
                    catch { }
                    try
                    {
                        chrome.FindElement(By.Id("nav-settings")); Thread.Sleep(r.Next(1, 3) * 1000); 
                        break;
                    }
                    catch { }
                    Thread.Sleep(r.Next(1, 3) * 1000);
                }
                string vo = "";

                var trroprrr = chrome.Manage().Cookies.AllCookies;
                foreach (var cookie in trroprrr)
                {
                    if (checkBox4.Checked) { vo += cookie.Name + "=" + cookie.Value + ";"; }
                    else
                    {
                        if (cookie.Name == ".ROBLOSECURITY")
                        {
                            vo += cookie.Value;
                        }
                    }
                }
                if (checkBox2.Checked)
                {
                    stt(i, "Add mail");
                    chrome.Navigate().GoToUrl("https://www.roblox.com/my/account#!/info");
                    while (true)
                    {
                        try { chrome.FindElements(By.XPath("//button[@class='account-field-edit-action']"))[1].Click(); break; } catch { }
                        Thread.Sleep(r.Next(1, 3) * 1000);
                    }
                    HttpRequest h = new HttpRequest();
                    API10MinuteMailNet api = new API10MinuteMailNet();
                    email = api.NewEmail();
                    this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].Cells[4].Value = email; ; }); stt(i, "Nhập mail");
                    while (true)
                    {
                        try { chrome.FindElement(By.Id("emailAddress")).SendKeys(email); break; } catch { }
                        Thread.Sleep(r.Next(1, 3) * 1000);
                    }
                    stt(i, "click");
                    chrome.FindElement(By.XPath("//button[@class=\"modal-full-width-button btn-primary-md btn-min-width\"]")).Click(); stt(i, "very");
                    Thread.Sleep(r.Next(2, 5) * 1000);
                    var link = "";
                    while (link == string.Empty)
                    {
                        var trrrrrrr = api.GetMailId();
                        var iff = "";
                        foreach (var item in trrrrrrr)
                        {
                            if (item.Contains("Roblox"))
                            {
                                iff = Regex.Match(item, @"{""mail_id"":""(.*?)""").Groups[1].Value; break;
                            }
                        }
                        var sdfdfdf = api.ReadMail(iff);
                        link = Regex.Match(sdfdfdf[0], @"0"":""(.*?)""").Groups[1].Value;
                    }
                    try
                    {
                        chrome.Navigate().GoToUrl(link);
                    }
                    catch { }
                    while (true)
                    {
                        try { chrome.FindElement(By.XPath("//div[@class=\"verify-item-image\"]")); break; } catch { }
                        if (chrome.PageSource.Contains("Email của bạn đã được xác thực")) { break; }
                        Thread.Sleep(r.Next(1, 3) * 1000);
                    }
                    //bật 2fa
                    if (checkBox3.Checked)
                    {
                        stt(i, "bật 2fa");
                        chrome.Navigate().GoToUrl("https://www.roblox.com/my/account#!/security");
                        int dem = 0;
                        while (true)
                        {
                            if (dem++ == 50) { chrome.Navigate().Refresh(); }
                            try { chrome.FindElement(By.Id("2sv-authenticator-toggle")).Click(); break; } catch { }
                            Thread.Sleep(r.Next(1, 3) * 1000);
                        }
                        while (true)
                        {
                            try { chrome.FindElement(By.Id("reauthentication-email-otp-input")); break; } catch { }
                            Thread.Sleep(r.Next(1, 3) * 1000);
                        }
                        bool kttt = false;
                        var code = "";
                        while (!kttt)
                        {
                            var trrrrrrr = api.GetMailId();
                            var ifsf = "";
                            foreach (var item in trrrrrrr)
                            {
                                if (item.Contains("Your Roblox One-Time Code"))
                                {
                                    kttt = true;
                                    ifsf = Regex.Match(item, @"{""mail_id"":""(.*?)""").Groups[1].Value; break;
                                }
                            }
                            code = Regex.Match(Regex.Match(api.ReadMail(ifsf)[0], "\"subject\":\"(.*?)\"").Groups[1].Value, @"\d{6}").Value;
                        }
                        chrome.FindElement(By.Id("reauthentication-email-otp-input")).SendKeys(code); Thread.Sleep(r.Next(1, 3) * 1000);
                        while (true)
                        {
                            try { chrome.FindElement(By.Id("2sv-verify-enable-authenticator")); break; } catch { }
                            Thread.Sleep(r.Next(1, 3) * 1000);
                        }
                        Thread.Sleep(r.Next(1, 3) * 1000);
                        chrome.FindElement(By.XPath("//button[@class=\"text-link xsmall transparent-button\"]")).Click(); Thread.Sleep(r.Next(1, 3) * 1000);

                        while (code2 == string.Empty)
                        {
                            code2 = chrome.FindElement(By.XPath("//div[@class=\"body-text section-content-off\"]")).Text; Thread.Sleep(r.Next(1, 3) * 1000);
                        }
                        this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].Cells[5].Value = code2; ; });
                        var eeee = h.Get($"https://2fa.live/tok/{code2.Replace(" ", "")}").ToString();
                        string maxcode2 = Regex.Match(eeee, @"{""token"":""(.*?)""").Groups[1].Value;
                        chrome.FindElement(By.Id("2sv-verify-enable-authenticator")).SendKeys(maxcode2); Thread.Sleep(1000);
                        chrome.FindElements(By.XPath("//button[@type=\"submit\"]"))[1].Click();
                        while (true)
                        {
                            try { chrome.FindElement(By.Id("confirm-recovery-codes-checkbox")); break; } catch { }
                            Thread.Sleep(1000);

                        }
                    }
                }
                if (checkBox2.Checked)
                {
                    if (checkBox3.Checked)
                    {
                        File.AppendAllText("acc.txt", tk + ":" + mk + ":" + email + ":" + code2 + ":" + vo + Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText("acc.txt", tk + ":" + mk + ":" + email + ":" + vo + Environment.NewLine);
                    }
                }
                else { File.AppendAllText("acc.txt", tk + ":" + mk + ":" + ":" + vo + Environment.NewLine); }

                stt(i, "reg thành công");
                this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green; });
            }
            catch (Exception e) { File.AppendAllText("acc die.txt", tk + "|" + mk + Environment.NewLine); File.AppendAllText("ok.txt", e.Message + "|=> " + e.StackTrace); stt(i, "reg thất bại"); this.Invoke((MethodInvoker)delegate { dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red; }); }
            try
            {
                if (t != null)
                {
                    t.Abort();
                }
            }
            catch { }

            if (chrome != null)
            {
                try { chrome.Close(); chrome.Quit(); } catch { }
            }
            try
            {
                foreach (int id in driverProcessIds)
                {
                    System.Diagnostics.Process.GetProcessById(id).Kill();
                }
                driverProcessIds.Clear();
            }
            catch
            {
            }

        }

        void sendkeys(IWebDriver chrome, By xpath, string text, int slepp = 100)
        {
            try
            {
                try
                {
                    chrome.FindElement(xpath).Click();
                }
                catch { }

                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        foreach (var item in text)
                        {
                            chrome.FindElement(xpath).SendKeys(item.ToString());
                            /*   chrome.FindElement(xpath).SendKeys();*/
                            Thread.Sleep(slepp);
                        }
                        break;
                    }
                    catch { }
                    Thread.Sleep(1000);
                }

            }
            catch { }
        }
        string GenerateRandomPassword1()
        {
            Random r = new Random();
            int length = r.Next(5, 9);
            const string chars = "ABCDgs34EFGHIJKL_MNOPQRST477UVWXYZabcdefghijklmnopqrstuvwxyz0123F_G45hgGHJsdf6SDFJNJ789";

            // Tạo đối tượng ngẫu nhiên
            Random random = new Random();

            // Sử dụng StringBuilder để xây dựng mật khẩu
            StringBuilder password = new StringBuilder(length);

            // Tạo mật khẩu ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi chars
            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }
            length = r.Next(1, 3);
            const string chars1 = "dsfrABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            // Tạo đối tượng ngẫu nhiên


            // Sử dụng StringBuilder để xây dựng mật khẩu
            StringBuilder password1 = new StringBuilder(length);

            // Tạo mật khẩu ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi chars
            for (int i = 0; i < length; i++)
            {
                password1.Append(chars1[random.Next(chars1.Length)]); Thread.Sleep(200);
            }
            return password1 + password.ToString();
        }
        string GenerateRandomPassword()
        {
            Random r = new Random();
            int length = r.Next(8, 12);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            // Tạo đối tượng ngẫu nhiên
            Random random = new Random();

            // Sử dụng StringBuilder để xây dựng mật khẩu
            StringBuilder password = new StringBuilder(length);

            // Tạo mật khẩu ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi chars
            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }
            length = r.Next(2, 4);
            const string chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            // Tạo đối tượng ngẫu nhiên


            // Sử dụng StringBuilder để xây dựng mật khẩu
            StringBuilder password1 = new StringBuilder(length);

            // Tạo mật khẩu ngẫu nhiên bằng cách chọn ngẫu nhiên các ký tự từ chuỗi chars
            for (int i = 0; i < length; i++)
            {
                password1.Append(chars[random.Next(chars.Length)]);
                Thread.Sleep(200);
            }
            return "A1" + password.ToString();
        }
        string newproxy(string apikey, int i)
        {
            var f = $"https://api.tinproxy.com/proxy/get-new-proxy?api_key={apikey}&authen_ips={textBox3.Text}&location=random";
            HttpRequest h = new HttpRequest(); ok:
            var t = h.Get($"https://api.tinproxy.com/proxy/get-new-proxy?api_key={apikey}&authen_ips={textBox3.Text}&location=random").ToString();
            if (t.Contains("Refresh Proxy"))
            {
                stt(i, "delay " + Regex.Match(t, @",""next_request"":(.*?),").Groups[1].Value + " giây");
                Thread.Sleep(int.Parse(Regex.Match(t, @",""next_request"":(.*?),").Groups[1].Value) * 1000); stt(i, "get proxy"); goto ok;
            }
            var c = h.Get($"https://api.tinproxy.com/proxy/get-current-proxy?api_key={apikey}&authen_ips={textBox3.Text}").ToString();
            var ip = Regex.Match(t, @"http_ipv4"":""(.*?)""").Groups[1].Value.ToString();
            if (!ip.Contains(".")) { goto ok; }
            return ip + "|" + Regex.Match(c, @"""username"":""(.*?)""").Groups[1].Value + "|" + Regex.Match(c, @"""password"":""(.*?)""").Groups[1].Value;

        }
        bool istop = false;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            istop = true;
            guna2Button1.Enabled = true;
            guna2Button2.Enabled = false;
        }

    }
}
