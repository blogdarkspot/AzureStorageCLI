using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AzureStorageLib;
using Utils;

namespace AzureStorageProgram
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var commandLine = new CommandLine();

            if(commandLine.Parser(args))
            {
               switch(commandLine.command)
               {
                   case "list":
                   {
                       if(commandLine.args.Count != 0)
                       {
                           if(commandLine.args[0] == "containers") 
                           {
                                AzureStorage azure = new AzureStorage();
                                azure.connString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                                azure.Init();
                                var containers = azure.GetContainersName();
                                foreach(string container in containers)
                                {
                                    Console.WriteLine(container);
                                }
                           }
                           else if(commandLine.args[0] == "blobs" && commandLine.container != "")
                           {
                                AzureStorage azure = new AzureStorage();
                                azure.connString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                                azure.Init();
                                azure.OpenContainer("containertest");

                                var itensTaks = azure.GetBlobNamesAsync();
                                var itens = await itensTaks;

                                foreach (string item in itens)
                                {
                                    Console.WriteLine(item);
                                }
                           }
                           else
                           {
                               Console.WriteLine("Missing args...");
                           }
                       }
                       else
                       {
                           Console.WriteLine("missing arg...");
                       }
                       break;
                   }
                   case "upload":
                   {
                        if(commandLine.container != "" && commandLine.args.Count != 0)
                        {
                            AzureStorage azure = new AzureStorage();
                            azure.connString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                            azure.Init();
                            azure.OpenContainer(commandLine.container);
                            var t = new List<Task>();
                            try
                            {
                                foreach (string file in commandLine.args)
                                {
                                    t.Add(azure.UploadBlobAsync(file, commandLine.path));
                                }

                                await Task.WhenAll(t.ToArray());

                            }catch(AggregateException ae)
                            {
                                Console.WriteLine(ae.Data);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Missing args...");
                        }
                        break;
                   }
                   case "download":
                   {
                        if(commandLine.container != "" && commandLine.args.Count != 0)
                        {
                            AzureStorage azure = new AzureStorage();
                            azure.connString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                            azure.Init();
                            azure.OpenContainer(commandLine.container);
                            var t = new List<Task>();
                            try
                            {
                                foreach (string file in commandLine.args)
                                {
                                    t.Add(azure.DownloadBlobAsync(file, commandLine.path));
                                }

                                await Task.WhenAll(t.ToArray());

                            }catch(AggregateException ae)
                            {
                                Console.WriteLine(ae.Data);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Missing Args");
                        }
                        break;
                       break;
                   }
                   default:
                   {
                      break;
                   }
               } 
            }
        }

        static async Task test()
        {
            AzureStorage azure = new AzureStorage();
            azure.connString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            azure.Init();
            azure.OpenContainer("containertest");

            var itensTaks = azure.GetBlobNamesAsync();
            var itens = await itensTaks;

            foreach (string item in itens)
            {
                Console.WriteLine(item);
            }

            var uploadTask = azure.UploadBlobAsync("template.json", "/data/files");
            await uploadTask;
        }
    }
}
