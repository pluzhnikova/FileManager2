using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;

namespace File_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach(DirectoryInfo crrDirectory in dirs)
            {
                listBox1.Items.Add(crrDirectory);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo crrFile in files)
            {
                listBox1.Items.Add(crrFile);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (Path.GetExtension(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString())) == "")
                {
                    textBox1.Text = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                    listBox1.Items.Clear();
                    DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    foreach (DirectoryInfo crrDirectory in dirs)
                    {
                        listBox1.Items.Add(crrDirectory);
                    }
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo crrFile in files)
                    {
                        listBox1.Items.Add(crrFile);
                    }
                    fileSystemWatcher1.Path = textBox1.Text;
                }
                else
                {
                    Process.Start(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
                }
                
            }
            catch { }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text[textBox1.Text.Length - 2] != ':')
            {
                if (textBox1.Text[textBox1.Text.Length - 1] == '\\')
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    }
                }
                else if (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                {
                    while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                    {
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                    }
                }

                listBox1.Items.Clear();
                DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo crrDirectory in dirs)
                {
                    listBox1.Items.Add(crrDirectory);
                }
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo crrFile in files)
                {
                    listBox1.Items.Add(crrFile);
                }
            }
            else
            {
                textBox1.Text = "";
                listBox1.Items.Clear();
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo crrDrive in drives)
                {
                    listBox1.Items.Add(crrDrive.Name);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listBox1.Items.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo crrDrive in drives)
            {
                listBox1.Items.Add(crrDrive.Name);
            }
        }
        bool b4clicked = false;
        string b4path = "";
        private void button4_Click(object sender, EventArgs e)
        {
            if(b4clicked == false)
            {
                b4path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                
                b4clicked = true;
            }
            else
            {
                try
                {
                    FileInfo fileInf = new FileInfo(b4path);
                    if (fileInf.Exists)
                    {
                        fileInf.MoveTo(Path.Combine(textBox1.Text, fileInf.Name));
                    }
                }
                catch { }
                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(b4path);
                    if (dirInfo.Exists && Directory.Exists(Path.Combine(textBox1.Text, dirInfo.Name)) == false)
                    {
                        dirInfo.MoveTo(Path.Combine(textBox1.Text, dirInfo.Name));
                    }
                }
                catch { }
                b4clicked = false;
            }
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo crrDirectory in dirs)
            {
                listBox1.Items.Add(crrDirectory);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo crrFile in files)
            {
                listBox1.Items.Add(crrFile);
            }
        }

        bool b5clicked;
        string b5path = "";
        private void button5_Click(object sender, EventArgs e)
        {
             b5path = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
             b5clicked = true;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (b5clicked == true)
            {
                try
                {
                    FileInfo fileInf = new FileInfo(b5path);

                    if (fileInf.Exists)
                    {
                        fileInf.CopyTo(Path.Combine(textBox1.Text, fileInf.Name), true);
                    }
                }
                catch { }
                try
                {
                   DirectoryInfo from = new DirectoryInfo(b5path);
                    DirectoryInfo to = new DirectoryInfo(Path.Combine(textBox1.Text, from.Name));
                    CopyAll(from, to);
                   
                    void CopyAll(DirectoryInfo source, DirectoryInfo target)
                    {
                        if (source.FullName.ToLower() == target.FullName.ToLower())
                        {
                            return;
                        }

                        // Check if the target directory exists, if not, create it.
                        if (Directory.Exists(target.FullName) == false)
                        {
                            Directory.CreateDirectory(target.FullName);
                        }

                        // Copy each file into it's new directory.
                        foreach (FileInfo fi in source.GetFiles())
                        {
                            Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                            fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                        }

                        // Copy each subdirectory using recursion.
                        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                        {
                            DirectoryInfo nextTargetSubDir =
                                target.CreateSubdirectory(diSourceSubDir.Name);
                            CopyAll(diSourceSubDir, nextTargetSubDir);
                        }
                    }
                }
                catch { }
                b5clicked = false;
                listBox1.Items.Clear();
                DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo crrDirectory in dirs)
                {
                    listBox1.Items.Add(crrDirectory);
                }
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo crrFile in files)
                {
                    listBox1.Items.Add(crrFile);
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //Rename
           FileInfo fileInf = new FileInfo(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
            Form2 form2 = new Form2();
            form2.ShowDialog();
            string path = Path.Combine(textBox1.Text,form2.newName); 
            
            if (fileInf.Exists)
            {
                fileInf.MoveTo(path);
            }
            if (dirInfo.Exists && Directory.Exists(path) == false)
            {
                dirInfo.MoveTo(path);
            }
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo crrDirectory in dirs)
            {
                listBox1.Items.Add(crrDirectory);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo crrFile in files)
            {
                listBox1.Items.Add(crrFile);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //Delete
            
                FileInfo fileInf = new FileInfo(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
                if (fileInf.Exists)
                {
                    fileInf.Delete();
                }
            
            try
            {
                Directory.Delete(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()), true);
            }
            catch { }
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo crrDirectory in dirs)
            {
                listBox1.Items.Add(crrDirectory);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo crrFile in files)
            {
                listBox1.Items.Add(crrFile);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            //Archive
            if (Path.GetExtension(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString())) == "")
            {
               
                    string startPath = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                    string zipPath = startPath + ".zip";
                    ZipFile.CreateFromDirectory(startPath, zipPath);
               
            }
            else
            {
                try
                {
                    string sourceFile = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                    string compressedFile = sourceFile.Remove(sourceFile.LastIndexOf('.'), sourceFile.Length - sourceFile.LastIndexOf('.')) + ".gz";
                    Compress(sourceFile, compressedFile);
                }
                catch { }
            }
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo crrDirectory in dirs)
            {
                listBox1.Items.Add(crrDirectory);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo crrFile in files)
            {
                listBox1.Items.Add(crrFile);
            }
        }
        public static void Compress(string sourceFile, string compressedFile)
            {
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                {
                    using (FileStream targetStream = File.Create(compressedFile))
                    {
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); 
                        }
                    }
                }
            }

        private void button10_Click(object sender, EventArgs e)
        {
            //Rearchivate
            if (textBox1.Text[textBox1.Text.Length - 1] == '\\')
            {
                try
                {
                    string zipPath = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                    string endPath = zipPath.Remove(zipPath.Length - 4, 4) + '\\';
                    ZipFile.ExtractToDirectory(zipPath, endPath);
                }
                catch { }
            }
            else
            {
                try
                {
                    string sourceFile = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                    Form2 form2 = new Form2();
                    form2.ShowDialog();
                    string path = Path.Combine(textBox1.Text, form2.newName);
                    Decompress(sourceFile, path);
                }
                catch { }
            }
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo crrDirectory in dirs)
            {
                listBox1.Items.Add(crrDirectory);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo crrFile in files)
            {
                listBox1.Items.Add(crrFile);
            }

        }
        public static void Decompress(string compressedFile, string targetFile)
        {
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            const string FORMAT = "The {0} has changed in {1}";
            string text = string.Format(FORMAT, e.ChangeType, e.Name);
            MessageBox.Show(text);
        }
      
        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            string text = e.OldName+ " has been renamed "+ e.Name;
            MessageBox.Show(text);
        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            string text = e.Name + " has been deleted ";
            MessageBox.Show(text);
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            string text = e.Name + " has been created ";
            MessageBox.Show(text);
        }
    }
}