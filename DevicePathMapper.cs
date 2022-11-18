﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

//string devicePath = @"\Device\HarddiskVolume4\Windows\System32\RuntimeBroker.exe";
//string driveLetterPath = DevicePathMapper.FromDevicePath(devicePath);

//// driveLetterPath == C:\Windows\System32\RuntimeBroker.exe


namespace USBeject
{
    public static class DevicePathMapper
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern uint QueryDosDevice([In] string lpDeviceName, [Out] StringBuilder lpTargetPath, [In] int ucchMax);

        public static string FromDevicePath(string devicePath)
        {
            var drive = Array.Find(DriveInfo.GetDrives(), d => devicePath.StartsWith(d.GetDevicePath(), StringComparison.InvariantCultureIgnoreCase));
            return drive != null ?
                devicePath.ReplaceFirst(drive.GetDevicePath(), drive.GetDriveLetter()) :
                null;
        }

        private static string GetDevicePath(this DriveInfo driveInfo)
        {
            var devicePathBuilder = new StringBuilder(128);
            return QueryDosDevice(driveInfo.GetDriveLetter(), devicePathBuilder, devicePathBuilder.Capacity + 1) != 0 ?
                devicePathBuilder.ToString() :
                null;
        }

        private static string GetDriveLetter(this DriveInfo driveInfo)
        {
            return driveInfo.Name.Substring(0, 2);
        }

        private static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
