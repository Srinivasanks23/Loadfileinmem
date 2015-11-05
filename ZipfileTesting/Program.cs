using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipfileTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var zip = new ZipFile(Encoding.UTF8))
                {
                    zip.Password = "text";
                    zip.AddDirectory(@"C:\Users\kesavsr\Documents\Velukudi Gita");
                    zip.UseZip64WhenSaving = Zip64Option.Always;
                    zip.ParallelDeflateThreshold = -1;
                    zip.Save(@"C:\Users\kesavsr\Documents\gita.zip");
                }

                Console.WriteLine("Zip file has been created");

                using (var zip1 = ZipFile.Read(@"C:\Users\kesavsr\Documents\gita.zip"))
                using (var stream = new MemoryStream())
                {
                    foreach (var entry in zip1.Entries)
                    {
                        stream.Position = 0;
                        entry.ExtractWithPassword(stream, "text");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Zip file has been extracted");
            Console.ReadKey();
        }
    }
}
