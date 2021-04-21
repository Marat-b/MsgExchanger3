using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace MessagerConsole
{
    public class Hardware
    {
        
        private Metrics metrics;
        public Hardware(Metrics metrics)
        {
            this.metrics = metrics;
        }
        private void HDD()
        {
            ManagementObject disk = new
                ManagementObject("win32_logicaldisk.deviceid='c:'");
            disk.Get();
            metrics.FullSpaceHdd = disk["Size"].ToString();
            metrics.FreeSpaceHdd = disk["FreeSpace"].ToString();
            Console.WriteLine("Logical Disk Size = " + disk["Size"] + " bytes");
            Console.WriteLine("Logical Disk FreeSpace = " + disk["FreeSpace"] + "bytes");
            
        }

        
        public void Memory()
        {
            string Query = "SELECT * FROM Win32_OperatingSystem";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);

            UInt64 FreePhysicalMemory = 0;
            UInt64 TotalVisibleMemorySize = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                FreePhysicalMemory = Convert.ToUInt64(obj.Properties["FreePhysicalMemory"].Value);
                TotalVisibleMemorySize = Convert.ToUInt64(obj.Properties["TotalVisibleMemorySize"].Value);
                
            }
            
            //Console.WriteLine("Memory Size = " + memory["TotalPhysicalMemory"] + " bytes");
            Console.WriteLine("Memory Size = " + FreePhysicalMemory.ToString());
            Console.WriteLine("Memory Size = " + TotalVisibleMemorySize.ToString());
            metrics.FullMemory = TotalVisibleMemorySize.ToString();
            metrics.FreeMemory = FreePhysicalMemory.ToString();

        }

        private void Processor()
        {
            
            string processorLoad = "0";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                if (obj["Name"].ToString().Trim() == "_Total")
                {
                    processorLoad = obj["PercentProcessorTime"].ToString().Trim();
                    Console.WriteLine( "Total : " + processorLoad);
                }
            }

            metrics.ProcessorLoad = processorLoad;
        }

       

        public Metrics GetMetrics()
        {
            Processor();
            HDD();
            Memory();
            
            return metrics;
        }



        private string SizeFormat(string size_a, string format)
        {
            try
            {
                string ret;
                Int64 size = Convert.ToInt64(size_a);
                Console.WriteLine($"size={size}");
                switch (format)
                {
                    case "Kb":
                        ret = Convert.ToString(Convert.ToInt32(size / 1024)) + " Kb";
                        break;
                    case "Mb":
                        ret = Convert.ToString(Convert.ToInt32(size / 1024 / 1024)) + " Mb";
                        break;
                    case "Gb":
                        ret = Convert.ToString(Convert.ToInt32(size / 1024 / 1024 / 1024)) + " Gb";
                        Console.WriteLine($"ret={ret}");
                        break;
                    default:
                        ret = Convert.ToString(size);
                        break;


                }
                
                return ret;
            }
            catch (Exception e)
            {
                return "0 bit";
            }
        }

    }
}
