using System;
using System.IO;

namespace HotelReservationSystem.Utils
{
    public static class BackupService
    {
        public static void BackupAllData()
        {
            string[] filesToBackup = {
                "data/users.json",
                "data/rooms.json",
                "data/bookings.json",
                "data/room_types.json"
            };

            string backupDir = $"backup/{DateTime.Now:yyyyMMdd_HHmmss}";
            Directory.CreateDirectory(backupDir);

            foreach (var file in filesToBackup)
            {
                if (File.Exists(file))
                {
                    string fileName = Path.GetFileName(file);
                    string destPath = Path.Combine(backupDir, fileName);
                    File.Copy(file, destPath);
                }
            }

            Console.WriteLine($"✅ Backup created in folder: {backupDir}");
        }
    }
}
