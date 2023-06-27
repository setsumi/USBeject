using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;


namespace USBeject
{
    public partial class FormMain : Form
    {
        //==============================================================
        private const uint WM_MOUSEMOVE = 0x0200;
        private const uint WM_CLOSE = 0x0010;
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        //[DllImport("user32.dll")]
        //static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //-----------------
        private enum GWL : int
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }
        private enum WindowStyles : uint
        {
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_OVERLAPPED = 0x0
        }
        [DllImport("user32.dll")]
        static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        //-----------------
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
        static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id,
                        (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }
        //==============================================================

        string _processName = "";
        bool _initForm = true;
        FormWindowState _lastWindowState = FormWindowState.Normal;

        public FormMain()
        {
            InitializeComponent();
        }

        void AppendText(XmlNode n, string s, bool newline = true)
        {
            bool isnode = n != null;
            if (!isnode) newline = false;
            tbInfo.AppendText((isnode ? n.LocalName + ": " : "") + (newline ? Environment.NewLine : "") + s + Environment.NewLine);
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.F5:
                    btnRefresh_Click(null, null);
                    break;
            }
        }

        class USBDeviceInfo
        {
            public USBDeviceInfo(string deviceId, string name, string description)
            {
                DeviceId = deviceId;
                Name = name;
                Description = description;
            }

            public string DeviceId { get; }
            public string Name { get; }
            public string Description { get; }

            public override string ToString()
            {
                return Name;
            }
        }

        static USBDeviceInfo GetUSBDevice(string deviceID)
        {
            USBDeviceInfo rv = null;
            using (var mos = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
            {
                using (ManagementObjectCollection collection = mos.Get())
                {
                    foreach (var device in collection)
                    {
                        var id = device.GetPropertyValue("DeviceId").ToString();

                        if (!id.StartsWith("USB", StringComparison.OrdinalIgnoreCase))
                            continue;

                        var name = device.GetPropertyValue("Name").ToString();
                        var description = device.GetPropertyValue("Description").ToString();
                        if (id == deviceID)
                        {
                            rv = new USBDeviceInfo(id, name, description);
                            break;
                        }
                    }
                }
            }
            return rv;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
            timer1.Start();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            lblProcessName.Text = "";

            if (Properties.Settings.Default.winWidth > 0)
                this.Width = Properties.Settings.Default.winWidth;
            if (Properties.Settings.Default.winHeight > 0)
                this.Height = Properties.Settings.Default.winHeight;
            if (Properties.Settings.Default.winMaximized)
                this.WindowState = FormWindowState.Maximized;
            _lastWindowState = this.WindowState;

            _initForm = false;
        }

        void ScrollToBottom()
        {
            if (tbInfo.Visible)
            {
                tbInfo.SelectionStart = tbInfo.TextLength;
                tbInfo.ScrollToCaret();
            }
        }

        void GetEvents()
        {
            string eventID = "225";
            string LogSource = "System";
            string sQuery = "*[System/EventID=" + eventID + "]";

            string PID = "";

            var elQuery = new EventLogQuery(LogSource, PathType.LogName, sQuery);
            using (var elReader = new EventLogReader(elQuery))
            {
                elReader.Seek(System.IO.SeekOrigin.End, -5);
                EventRecord eventInstance = elReader.ReadEvent();
                try
                {
                    for (; null != eventInstance; eventInstance = elReader.ReadEvent())
                    {
                        //Access event properties here:
                        //eventInstance.LogName;
                        //eventInstance.ProviderName;

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(eventInstance.ToXml());
                        XmlNode node = doc.DocumentElement.SelectSingleNode("/*[local-name()='Event']/*[local-name()='System']");

                        AppendText(null, "------------- ");
                        int offset = 1;
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            switch (n.LocalName)
                            {
                                case "Provider":
                                    offset++;
                                    AppendText(n, n.Attributes["Name"].Value, false);
                                    break;
                                case "EventID":
                                    offset++;
                                    AppendText(n, n.InnerText, false);
                                    break;
                                case "TimeCreated":
                                    offset++;
                                    //AppendText(n, DateTime.Parse(n.Attributes["SystemTime"].Value).ToLocalTime().ToString(), false);
                                    string[] tempArray = tbInfo.Lines;
                                    tempArray[tempArray.Count() - offset] += DateTime.Parse(n.Attributes["SystemTime"].Value).ToLocalTime().ToString() + " -------------";
                                    tbInfo.Lines = tempArray;
                                    break;
                            }
                        }

                        node = node.NextSibling;
                        foreach (XmlNode n in node.ChildNodes)
                        {
                            // skip useless fields
                            if (n.Attributes["Name"].Value.Contains("Length")) continue;

                            switch (n.Attributes["Name"].Value)
                            {
                                case "ProcessName":
                                    {
                                        string driveLetterPath = DevicePathMapper.FromDevicePath(n.InnerText);
                                        _processName = driveLetterPath != null ? driveLetterPath : n.InnerText;
                                    }
                                    AppendText(null, n.Attributes["Name"].Value + ": " + _processName);
                                    break;
                                case "DeviceInstance":
                                    AppendText(null, n.Attributes["Name"].Value + ": " + n.InnerText);
                                    // actual device state
                                    {
                                        USBDeviceInfo di = GetUSBDevice(n.InnerText);
                                        if (di != null)
                                        {
                                            AppendText(null, "DeviceName: " + di.Name
                                                + " (" + di.Description + ")");
                                        }
                                        else
                                        {
                                            AppendText(null, "DEVICE NOT FOUND");
                                        }
                                    }
                                    break;
                                case "ProcessId":
                                    PID = n.InnerText;
                                    goto default;
                                default:
                                    AppendText(null, n.Attributes["Name"].Value + ": " + n.InnerText);
                                    break;
                            }
                        }
                    }
                }
                finally
                {
                    if (eventInstance != null)
                        eventInstance.Dispose();
                }
            }

            tbPID.Text = PID;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            tbInfo.Visible = false;
            tbInfo.Clear();
            GetEvents();
            tbInfo.Visible = true;
            ScrollToBottom();
            tbInfo.Focus();
            DetectProcess();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbPID.Text))
            {
                int pid = 0;
                try { pid = int.Parse(tbPID.Text); } catch { }
                if (pid > 0)
                    KillProcess(pid);
            }
            //System.Media.SystemSounds.Hand.Play();//debug
        }

        void KillProcess(int pid)
        {
            Process p = null;
            try { p = Process.GetProcessById(pid); } catch { }
            if (p != null)
            {
                IntPtr hmain = IntPtr.Zero;
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    hmain = p.MainWindowHandle;
                    try { p.CloseMainWindow(); }
                    catch { }
                }
                if (hmain == IntPtr.Zero || !p.WaitForExit(1000))
                {
                    foreach (var handle in EnumerateProcessWindowHandles(p.Id))
                    {
                        IntPtr style = GetWindowLong(handle, (int)GWL.GWL_STYLE);
                        if (((UInt64)style & (UInt64)WindowStyles.WS_POPUP) != 0)
                        {
                        }
                        else if (((UInt64)style & (UInt64)WindowStyles.WS_CHILD) != 0)
                        {
                        }
                        else // WS_OVERLAPPED
                        {
                            // close all overlapped windows
                            if (handle != hmain)
                                PostMessage(handle, WM_CLOSE, 0, 0);
                        }
                    }
                    if (!p.WaitForExit(1000))
                    {
                        p.Kill();
                    }
                }
                p.Close();
                p.Dispose();
                //Thread.Sleep(50);
            }
        }

        private void tbPID_TextChanged(object sender, EventArgs e)
        {
            DetectProcess();
        }

        void DetectProcess()
        {
            int pid = 0;
            try { pid = int.Parse(tbPID.Text); } catch { }
            if (pid == 0)
            {
                lblProcessName.Text = "";
            }
            else
            {
                string name = GetProcessName(pid);
                lblProcessName.Text = name == null ? "Process not found" : name;
                if (Path.GetFileName(name) == Path.GetFileName(_processName))
                {
                    toolTip1.SetToolTip(lblProcessName, lblProcessName.Text);
                    lblProcessName.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    toolTip1.SetToolTip(lblProcessName, "Warning: Process is different!" + Environment.NewLine + lblProcessName.Text);
                    lblProcessName.ForeColor = Color.Red;
                }
            }
        }

        string GetProcessName(int pid)
        {
            string rv = null;
            Process proc = null;
            try { proc = Process.GetProcessById(pid); } catch { }
            if (proc != null)
            {
                try { rv = proc.MainModule.FileName; } catch { }
                proc.Close();
                proc.Dispose();
            }
            return rv;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DetectProcess();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (_initForm) return;

            if (WindowState != _lastWindowState)
            {
                _lastWindowState = WindowState;
                if (WindowState == FormWindowState.Maximized) // Maximized!
                {
                    Properties.Settings.Default.winMaximized = true;
                }
                else if (WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.winMaximized = false;
                }
            }
            else if (WindowState == FormWindowState.Normal) // resize
            {
                Properties.Settings.Default.winWidth = this.Width;
                Properties.Settings.Default.winHeight = this.Height;
            }
        }

        private void tbPID_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btnKill_Click(null, null);
                    e.Handled = e.SuppressKeyPress = true;
                    break;
            }
        }

        private void btnEjectDialog_Click(object sender, EventArgs e)
        {
            Process.Start("RunDll32.exe", "shell32.dll,Control_RunDLL hotplug.dll");
        }
    }
}
