using Microsoft.Playwright;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static CreditStatistics.PandoraRPC;

namespace CreditStatistics
{
    internal class cParseConfigs
    {
        public class cAppConfig
        {
            public double avg_ncpus;
            public double ngpus;
            public int projectMaxConcurrent;
            public double gpu_usage;
            public double cpu_usage;
            public bool bValid = false;
            public string[] AppConfig = null;
            public double dGPU = 1.0;
            public double dCPU = 1.0;
            public int MaxApps = 0;
        }

        public class cCCconfig
        {
            public string[] pc_config = null;
            public string[] cc_config = null;
            public bool bValid = false;
        }

        public cCCconfig ParseCCconfig(string sIn)
        {
            cCCconfig cc = new cCCconfig();
            int i = sIn.IndexOf("<cc_config>");
            if (i == -1)
            {
                return cc;
            }
            Debug.Assert(i >= 0);
            int j = sIn.IndexOf("</cc_config>", i + 11);
            Debug.Assert(j >= 0);
            cc.cc_config = sIn.Substring(i, j + 12 - i).Trim().Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            i = sIn.IndexOf("<!--pandora");
            if(i >= 0)
            {
                j = sIn.IndexOf("-->", 11);
                Debug.Assert(j > 0);
                sIn = sIn.Substring(i + 11, j - i - 11);
                cc.pc_config = sIn.Trim().Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
            cc.bValid = true;
            return cc;
        }

        public cAppConfig ParseAppConfig(string sIn)
        {
            cAppConfig ap = new cAppConfig();
            XDocument doc = null;
            int i = sIn.IndexOf("<app_config>");
            if (i == -1)
            {
                return ap;
            }
            Debug.Assert(i >= 0);
            int j = sIn.IndexOf("</app_config>", i + 12);
            Debug.Assert(j >= 0);
            sIn = sIn.Substring(i, j + 13 - i).Trim();
            ap.AppConfig = sIn.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            doc = XDocument.Parse(sIn);
            //globals.WriteACrecord(globals.GetAppConfigFilename(pc.PCname, cmdRequest.AC_ShortName), ref pc.app_config);
            double defaultDouble = -1.0;
            int defaultInt = -1;

            // Extract safely with defaults gpu_usage
            ap.avg_ncpus = double.TryParse(doc.Descendants("avg_ncpus").FirstOrDefault()?.Value, out double v1) ? v1 : defaultDouble;
            ap.ngpus = double.TryParse(doc.Descendants("ngpus").FirstOrDefault()?.Value, out double v2) ? v2 : defaultDouble;
            ap.projectMaxConcurrent = int.TryParse(doc.Descendants("project_max_concurrent").FirstOrDefault()?.Value, out int v3) ? v3 : defaultInt;
            ap.gpu_usage = double.TryParse(doc.Descendants("gpu_usage").FirstOrDefault()?.Value, out double v4) ? v4 : defaultDouble;
            ap.cpu_usage = double.TryParse(doc.Descendants("cpu_usage").FirstOrDefault()?.Value, out double v5) ? v5 : defaultDouble;
            // following is OK since I do not run the cpu and gpu on the same project.
            double d = Math.Max(ap.avg_ncpus, ap.cpu_usage);
            if (d > 0)
            {
                ap.dCPU = d;
//                clp.cpu_usage = d;
//                clp.avg_ncpus = d;
            }

            d = Math.Max(ap.ngpus, ap.gpu_usage);
            if (d > 0)
            {
                ap.dGPU = d;
//                clp.gpu_usage = d;
//                clp.ngpus = d;
            }
            if (ap.projectMaxConcurrent > 0)
                ap.MaxApps = ap.projectMaxConcurrent;
            ap.bValid = true;
            return ap;
        }

    }
}
