using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CreditStatistics
{
    internal class PCresources
    {
        private static string filePath = globals.WherePcCpuGpu;
        public class cCpuGpu
        {
            public string sPC;
            public string IPaddress;
            public string OSname; //windows linux etc
            public int nVidia;
            public int nAti;
            public int nIntel;
            public int nCPU;
            public int nGPU;
            public void Init()
            {
                nVidia = 0;
                nAti = 0;
                nIntel = 0;
                nCPU = 0;
                nGPU = 0;
            }
        }

        public List<cCpuGpu> CpuGpu = new List<cCpuGpu>();
        
        // find or create it
        private cCpuGpu FindPCresource(string PCname)
        {
            foreach (cCpuGpu cg in CpuGpu)
                if (cg.sPC == PCname) { return cg; }
            cCpuGpu newCG = new cCpuGpu();
            newCG.sPC = PCname;
            CpuGpu.Add(newCG);
            return newCG;
        }
        public cCpuGpu GpuCpuParse(string PCname, string s)
        {
            cCpuGpu CpuGpu = FindPCresource(PCname); // this adds it if it does not exist
            CpuGpu.Init();
            string[] lines = s.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            int i, j;
            foreach (string line in lines)
            {
                i = line.IndexOf("#cpus:");
                if (i >= 0)
                    CpuGpu.nCPU = Convert.ToInt32(line.Substring(i + 6));
                i = line.IndexOf("nvidia gpu:");
                if (i >= 0) CpuGpu.nVidia++;
                i = line.IndexOf("count:");
                if (i >= 0)
                {
                    i += 6;
                    j = Convert.ToInt32(line.Substring(i));
                    CpuGpu.nVidia += (j - 1);  // else would count twice
                }
                i = line.IndexOf("os name:");
                if (i >= 0)
                {
                    string sOS = line.Substring(i + 8).Trim();
                    if (sOS.Contains("win"))
                        CpuGpu.OSname = "w";
                    else if (sOS.Contains("linux"))
                        CpuGpu.OSname = "l";
                    else
                    {
                        Debug.Assert(false, "expected windows or linux OS, found neither");
                        CpuGpu.OSname = "";
                    }
                }
                i = line.IndexOf("ip addr:");
                if (i >= 0)
                {
                    if (PCname == Dns.GetHostName().ToLower())
                        CpuGpu.IPaddress = "127.0.0.1";
                    else CpuGpu.IPaddress = line.Substring(i + 8).Trim();
                }

                //todo to to need an ati amd or intel gpu when i get one
            }
            return CpuGpu;
        }

        public bool SavePCresource()
        {
            string sOut = "";
            foreach (cCpuGpu cg in CpuGpu)
            {
                sOut += cg.sPC + ":" + cg.OSname + ",";
                sOut += cg.IPaddress + ",";
                sOut += cg.nCPU.ToString() + ",";
                sOut += cg.nVidia.ToString() + ",";
                sOut += cg.nAti.ToString() + ",";
                sOut += cg.nIntel.ToString() + Environment.NewLine;
            }
            try
            {
                File.WriteAllText(filePath, sOut);
                return true;
            }
            catch (Exception ex )
            {
                MessageBox.Show("unable to save pc resources: CPUs, GPUs, type " + ex.Message);
            }
            return false;
        }

        public int LoadPCresources()
        {
            int n = 0;
            if (!File.Exists(filePath)) return 0;
            string[] lines = File.ReadAllLines(filePath);
            n = lines.Length;
            foreach(string line in lines)
            {
                string[] OneLine = line.Split(':');
                cCpuGpu cg = new cCpuGpu();
                cg.sPC = OneLine[0].Trim();
                string[] srem = OneLine[1].Split(",");
                cg.OSname = srem[0].Trim();
                cg.IPaddress = srem[1].Trim();
                cg.nCPU = Convert.ToInt32(srem[2]);
                cg.nVidia = Convert.ToInt32(srem[3]);
                cg.nAti = Convert.ToInt32(srem[4]);
                cg.nIntel = Convert.ToInt32(srem[5]);
                cg.nGPU = cg.nVidia + cg.nAti + cg.nIntel;
                CpuGpu.Add(cg);
            }
            return n;
        }

    }
}
