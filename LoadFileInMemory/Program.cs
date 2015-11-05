using EY.ATF.Tool.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LoadFileInMemory
{
    class Program
    {
        static void Main(string[] args)
        {
            //string jsonstring = @"{""id"":0,""data"":{""copyrequestreceiver"":{""data"":{""email"":""Ranjith Neelam"",""gui"":""3508461""}},""copyname"":""CopyEngagement2""},""collections"":{""copyobjects"":[{""id"":1,""data"":{""copyobjectids"":[13,4,5,12,7],""copyobjectgrouptypeid"":1}}]}}";
            string jsonstring ="{\"id\":0,\"data\":{\"copyrequestreceiver\":{\"data\":{\"email\":\"Ranjith Neelam\",\"gui\":\"3508461\"}},\"copyname\":\"CopyEngagement2\"},\"collections\":{\"copyobjects\":[{\"id\":1,\"data\":{\"copyobjectids\":[13,4,5,12,7],\"label\":null,\"title\":null,\"isRequired\":false,\"isSelected\":false,\"isgroupofcopyobjects\":false,\"copyobjectgrouptypeid\":1},\"collections\":null}]}}";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var serializedObject = serializer.Deserialize<CopyHubModel>(jsonstring);
            CopyHubModel jsonObject = new CopyHubModel();
            using(TextReader reader = new StringReader(jsonstring))
            {
                JsonTextReader jsonreader = new JsonTextReader(reader);
                JsonSerializer jsonSerializer = new JsonSerializer();
                jsonObject = jsonSerializer.Deserialize<CopyHubModel>(jsonreader);
            }

            string convertedString = JsonConvert.SerializeObject(jsonObject);
            string convertedString1 = serializer.Serialize(serializedObject);
            var equalsTrueorNot = convertedString.Equals(jsonstring, StringComparison.InvariantCultureIgnoreCase);
            var equalsTrueorNot1 = convertedString1.Equals(jsonstring, StringComparison.InvariantCultureIgnoreCase);
        }

        private static byte[] ReadFileChunk(string path, long position, long length)
        {
            using (var fileStream = File.OpenRead(path))
            {
                var fileBytes = new byte[length];
                fileStream.Position = position;
                fileStream.Read(fileBytes, 0, (int)length);
                return fileBytes;
            }
        }

        /*
         try
            {
                var filePath = args[0];

                if (string.IsNullOrEmpty(filePath))
                {
                    throw new Exception("File Path is missing");
                }

                /*   string path = filePath;
                  //Chunk length
                  long length= int.Parse(ConfigurationManager.AppSettings.Get("DefaultBufferSize"), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            
                  //Original full file size
                  long fileSize = 0;

                  // Get the file size
                  using (var fileStream = File.OpenRead(path))
                  {
                      fileSize = fileStream.Length;
                  }

                  long noofchunks = fileSize / length;
                  noofchunks = fileSize % length > 0 ? ++noofchunks : noofchunks;

                  var parallelOptions = new ParallelOptions
                  {
                      MaxDegreeOfParallelism = 1
                  };
                  Parallel.For(
                  0,
                  noofchunks,
                  parallelOptions,
                  (i) =>
                  {
                      var contentStart = length * i;
                      var chunksize = length;

                      if ((contentStart + length) > fileSize)
                      {
                          chunksize = fileSize - contentStart;
                      }
                      //Read the chunk from the file as byte[]
                      byte[] chunkdata = ReadFileChunk(path, contentStart, chunksize);

                      //Replace the write method to arc call
                
                  });*/

                /* long position = 0;
                 var writeBufferSize = int.Parse(ConfigurationManager.AppSettings.Get("DefaultBufferSize"), NumberStyles.HexNumber, CultureInfo.InvariantCulture);

                 // create a new FileStream, then send the file using the other overload
                 using (Stream stream = File.OpenRead(filePath))
                 {
                     long iterations = 0;
                     if (stream.Length <= writeBufferSize)
                     {
                         iterations = 1;
                     }
                     else
                     {
                         iterations = stream.Length / writeBufferSize;
                         iterations = stream.Length % writeBufferSize > 0 ?
                              ++iterations
                              : iterations;
                     }

                     if (iterations > 0)
                     {
                         Task<int>[] fileReadTasks = new Task<int>[iterations];

                         byte[][] buffers = new byte[iterations][];
                         for (int index = 0; index < iterations; index++)
                         {
                             buffers[index] = new byte[writeBufferSize];
                             fileReadTasks[index] = stream.ReadAsync(buffers[index], 0, writeBufferSize);
                         }

                         for (int index = 0; index < iterations; index++)
                         {
                             int currentBytesRead = fileReadTasks[index].Result;
                             if (currentBytesRead > 0)
                             {
                                 byte[] tmpBuffer = new byte[0];

                                 if (currentBytesRead < writeBufferSize)
                                 {
                                     //copy only bytes read into new buffer array
                                     tmpBuffer = new byte[currentBytesRead];
                                     Array.Copy(buffers[index], tmpBuffer, currentBytesRead);
                                 }

                                 position += currentBytesRead;
                             }
                         }

                         Task.WaitAll(fileReadTasks);
                     }
                 }
                long startBytes = 0L;
                int count = int.Parse(ConfigurationManager.AppSettings.Get("DefaultBufferSize"), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                using (Stream stream = (Stream)System.IO.File.OpenRead(filePath))
                {
                    long num1 = 0L;
                    long length;
                    if (stream.Length <= (long)count)
                    {
                        length = 1L;
                    }
                    else
                    {
                        long num2 = stream.Length / (long)count;
                        long num3;
                        if (stream.Length % (long)count <= 0L)
                            num3 = num2;
                        else
                            num1 = num3 = num2 + 1L;
                        length = num3;
                    }

                    if (length > 0L)
                    {
                        Task<int>[] taskArray = new Task<int>[length];
                        byte[][] numArray1 = new byte[length][];
                        for (int index = 0; (long)index < length; ++index)
                        {
                            numArray1[index] = new byte[count];
                            taskArray[index] = stream.ReadAsync(numArray1[index], 0, count);
                        }
                        for (int index = 0; (long)index < length; ++index)
                        {
                            int result = taskArray[index].Result;
                            if (result > 0)
                            {
                                byte[] numArray2 = new byte[0];
                                if (result < count)
                                {
                                    numArray2 = new byte[result];
                                    Array.Copy((Array)numArray1[index], (Array)numArray2, result);
                                }

                                startBytes += (long)result;
                            }
                        }
                        Task.WaitAll((Task[])taskArray);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message " + ex.Message);
                Console.WriteLine("Error Stack Trace" + ex.StackTrace);
                
            }

            finally
            {
                Console.ReadKey();
            }
         
         */
    }
}
