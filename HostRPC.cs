using Microsoft.Playwright;
using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Resources.Tools;
using System.Runtime;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static CreditStatistics.cParseConfigs;
using static CreditStatistics.globals;
using static CreditStatistics.PandoraConfig;
using static System.Runtime.InteropServices.JavaScript.JSType;

// 8/10/2025 the host ID gathering  not used anymore except as reference as   boinccmd.exe is better.

namespace CreditStatistics
{

    internal class PandoraRPC
    {
        private List<cPClimit> PandoraDatabase;
        private cProjectStruct ProjectStats;
        private string UnixIOout = "";
        public cParseConfigs ParseConfigs = new cParseConfigs();
        public void TextToUnix(string input)
        {
            UnixIOout = globals.NewLineToLinux(input);
        }

        private int port = 31416;
        public class CmdRequest
        {
            public bool bIssueCmd = false;
            public bool Fetch_CC = false;
            public bool Write_CC = false;
            public bool Fetch_AC = false;
            public bool Write_AC = false;
            public bool Write_CC_PC = false;
            public int ReqCode = 0;
            public int ReqIndex = -1;   // index into database
            public string AC_ShortName = "";
            public string CmdIssue = "";
            public string CmdPCname = "";
            public string CMDresults = "";
            public string CMDurl = "";
            
            public int CMDerr = ERR_none;
            public byte[] FormReq(string cmd)
            {
                string sRPC = "<boinc_gui_rpc_request>\n" + cmd + "</boinc_gui_rpc_request>\n\u0003";
                return Encoding.UTF8.GetBytes(sRPC);
            }
        }

        public CmdRequest cmdRequest;

        private string cc_write(string cc_in)
        {
            return "<set_cc_config>\n" + cc_in + "</set_cc_config>\n";
        }

        private bool GetErrorInfo(string s, out string sErr)
        {
            sErr = "";
            int i = s.IndexOf("<error>");
            if (i == -1) return false;
            int j = s.IndexOf("</error>", i);
            Debug.Assert(j != -1);
            sErr = s.Substring(i + 7, j - i - 7);
            return true;
        }

        private string StripReply(string r)
        {
            int i = r.IndexOf("<boinc_gui_rpc_reply>");
            if (i < 0) return r;
            int j = r.IndexOf("</boinc_gui_rpc_reply>", i + 21);
            Debug.Assert(j != -1);
            return r.Substring(i + 21, j - i - 21);
        }

        public void Init(ref cProjectStruct rProjectStats)
        { 
            ProjectStats = rProjectStats;
            PandoraDatabase = ProjectStats.PandoraDatabase;
            
            foreach (cPClimit pc in PandoraDatabase)
            {
                if (pc.cc_config == null)
                {
                    pc.cc_config = globals.ReadCCrecord(pc.PCname);
                }
                if (pc.pc_config == null)
                {
                    pc.pc_config = globals.ReadPCrecordS(pc.PCname);
                }
            }
        }

        public async Task FetchSelected_cc_config()
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Fetch_CC = true;
            cmdRequest.ReqCode = 0;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await AllSystemsAsync(cts.Token);
        }

        public async Task FetchSelected_app_config(string shortname)
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Fetch_AC = true;
            cmdRequest.AC_ShortName = shortname;
            cmdRequest.ReqCode = 0;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await AllSystemsAsync(cts.Token);
        }

        public async Task FetchOne_app_config(string PCname, string shortname)
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Fetch_AC = true;
            cmdRequest.AC_ShortName = shortname;
            cmdRequest.CmdPCname = PCname;
            cmdRequest.ReqIndex = nameTOindex(PCname);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await SystemAsync(cts.Token);
        }


        public async Task FetchOne_cc_config(string PCname)
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Fetch_CC = true; 
            cmdRequest.CmdPCname = PCname;
            cmdRequest.ReqIndex = nameTOindex(PCname);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await SystemAsync(cts.Token);
        }

        public async Task WriteOne_cc_config(string PCname)
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Write_CC = true;
            cmdRequest.CmdPCname = PCname;
            cmdRequest.ReqIndex = nameTOindex(PCname);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await SystemAsync(cts.Token);
        }


        public async Task WriteOne_app_config(string PCname, string shortname)
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Write_AC = true;
            cmdRequest.AC_ShortName = shortname;
            cmdRequest.CmdPCname = PCname;
            cmdRequest.ReqIndex = nameTOindex(PCname);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await SystemAsync(cts.Token);
        }


        public async Task WriteSelected_app_config(string shortname)
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Write_AC = true;
            cmdRequest.AC_ShortName = shortname;
            cmdRequest.ReqCode = 0;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await AllSystemsAsync(cts.Token);
        }


        public async Task WriteSelected_cc_config()
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Write_CC = true;
            cmdRequest.ReqCode = 0;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await AllSystemsAsync(cts.Token);
        }

        public async Task WriteSelected_cc_pc_config()
        {
            cmdRequest = new CmdRequest();
            cmdRequest.Write_CC_PC = true;
            cmdRequest.ReqCode = PC_NNW_PROJECT_LIMIT | PC_REPORT_STATUS;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await AllSystemsAsync(cts.Token);
        }

        private int nameTOindex(string name)
        {
            int i = 0;
            foreach(cPClimit cpl in PandoraDatabase)
            {
                if (cpl.PCname == name) return i;
                i++;
            }
            Debug.Assert(false, "internal error: cannot find " + name + " in pandora database");
            return -1;
        }

        public async Task SendCmd(string PCname, string arg, string url)
        {
            cmdRequest = new CmdRequest();
            cmdRequest.bIssueCmd = true;
            cmdRequest.CmdIssue = "<project_" + arg + ">\n" + "  <project_url>" + url + "</project_url>\n" + "</project_" + arg + ">\n";
            cmdRequest.CmdPCname = PCname;
            cmdRequest.ReqIndex = nameTOindex(PCname);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await SystemAsync(cts.Token);
        }


        private async Task SystemAsync(CancellationToken token)
        {
            byte[] data = null; ;
            byte[] buffer = new byte[65536];
            int bytesRead = 0;
            string response;
            cPClimit pcx = PandoraDatabase[cmdRequest.ReqIndex];
            string IPaddress = pcx.IPaddress;
            if (pcx.IsLocalhost())
                IPaddress = "127.0.0.1";
            pcx.strResult = "";
            pcx.ErrorStatus = 0;
            string NL = "\n";  // all transfers use linux !!!!!
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(IPaddress, port);
                using var stream = client.GetStream();

                if (cmdRequest.Fetch_CC)
                {
                    data = cmdRequest.FormReq("<get_cc_config/>");
                    await stream.WriteAsync(data, 0, data.Length, token);
                    await stream.FlushAsync(token);
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (GetErrorInfo(response, out string sErr))
                    {
                        pcx.strResult = sErr;
                        pcx.ErrorStatus = ERR_warning;
                        response = "<!-- this is the default cc_config -->\n" + cProjectStruct.DefaultCCconfig;
                    }

                    cCCconfig cc = ParseConfigs.ParseCCconfig(response);
                    if (cc.bValid)
                    {
                        pcx.cc_config = cc.cc_config;
                        pcx.pc_config = cc.pc_config;
                        globals.WriteCCrecord(pcx.PCname, ref pcx.cc_config);
                        if (pcx.pc_config != null)
                        {
                            WritePCrecordS(pcx.PCname, ref pcx.pc_config);
                        }
                        pcx.ErrorStatus = 0;

                    }
                    else pcx.ErrorStatus = ERR_critical;
                }

                if (cmdRequest.Write_CC)
                {
                    if (UnixIOout == "")
                    {
                        pcx.strResult = "Error: output buffer UnixIOout was empty" + Environment.NewLine;
                        return;
                    }
                    data = cmdRequest.FormReq(cc_write(UnixIOout));
                    await stream.WriteAsync(data, 0, data.Length, token);
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (response.Contains("success"))
                        pcx.ErrorStatus = 0;
                    else
                    {
                        pcx.ErrorStatus = ERR_critical;
                        pcx.strResult = "cannot send cc_config to " + pcx.PCname + " at " + pcx.IPaddress;
                    }
                }


                if (cmdRequest.Fetch_AC)
                {
                    string sUrl = "";
                    cCalcLimitProj clp = pcx.GetProjStruct(cmdRequest.AC_ShortName);
                    if (clp == null) // not a sprint project
                    {
                        cPSlist PSl = ProjectStats.GetProjectByShortName(cmdRequest.AC_ShortName);
                        sUrl = PSl.MasterUrl;
                    }
                    else sUrl = clp.ProjUrl;
                    data = cmdRequest.FormReq("<get_app_config>\n" + "<url>" + sUrl + "</url>\n" + "</get_app_config>\n");
                    await stream.WriteAsync(data, 0, data.Length, token);
                    await stream.FlushAsync(token);
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    response = StripReply(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    if (GetErrorInfo(response, out string sErr))
                    {
                        pcx.strResult = sErr;
                        pcx.ErrorStatus = ERR_critical;
                    }
                    else
                    {
                        pcx.strResult = response;
                        if (response.Contains("default cc_config"))
                            pcx.ErrorStatus = ERR_warning;
                        else
                            pcx.ErrorStatus = 0;
                    }
                }

                if (cmdRequest.Write_AC)
                {
                    string sUrl = "";
                    if (UnixIOout == "")
                    {
                        pcx.strResult = "Error: output buffer UnixIOout was empty" + Environment.NewLine;
                        return;
                    }
                    cCalcLimitProj clp = pcx.GetProjStruct(cmdRequest.AC_ShortName);
                    if (clp == null) // not a sprint project
                    {
                        cPSlist PSl = ProjectStats.GetProjectByShortName(cmdRequest.AC_ShortName);
                        sUrl = PSl.MasterUrl;
                    }
                    else sUrl = clp.ProjUrl;
                        data = cmdRequest.FormReq("<set_app_config>\n<url>" + sUrl + "</url>\n" +
                            UnixIOout + "</set_app_config>\n");
                    await stream.WriteAsync(data, 0, data.Length, token);
                    await stream.FlushAsync(token);
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    pcx.strResult = response;
                    cmdRequest.CMDerr = 0;
                    cmdRequest.CMDresults = response;
                }


                if (cmdRequest.bIssueCmd)
                {
                    cCalcLimitProj clp = pcx.GetProjStruct(cmdRequest.CmdPCname);
                    data = cmdRequest.FormReq(cmdRequest.CmdIssue);
                    await stream.WriteAsync(data, 0, data.Length, token);
                    await stream.FlushAsync(token);
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    pcx.strResult = response;
                    if (response.Contains("success"))
                    {
                        pcx.ErrorStatus = 0;
                    }
                    else if (response.Contains("No such project"))
                    {
                        pcx.ErrorStatus = 0;// ERR_warning;
                    }
                    else pcx.ErrorStatus = ERR_critical;
                }
            }
            catch (Exception ex)
            {
                pcx.ErrorStatus = ERR_critical;
                pcx.strResult = ex.Message;
                cmdRequest.CMDerr = pcx.ErrorStatus;
                cmdRequest.CMDresults = ex.Message;
            }
        }

        private async Task AllSystemsAsync(CancellationToken token)
        {
            byte[] data = null; ;
            byte[] buffer = new byte[65536];
            int bytesRead = 0;
            string response;

            foreach (cPClimit pc in PandoraDatabase)
            {
                if (!pc.IsSelected) continue;
                cPClimit pcx = pc;
                string IPaddress = pc.IPaddress;
                if (pc.IsLocalhost())
                    IPaddress = "127.0.0.1";

                /*
                bool bCCmissing = pc.cc_config == null || pc.cc_config.Length == 0;
                bool bPCmissing = pc.pc_config == null || pc.pc_config.Length == 0;
                bool bIsMissing = bCCmissing || bPCmissing;
                */

                bool bCCmissing = false;
                bool bPCmissing = false;
                bool bIsMissing = bCCmissing || bPCmissing;

                pc.strResult = "";
                pc.ErrorStatus = 0;
                //string NL = Environment.NewLine;
                //if (pc.OStype != "w") NL = "\n";
                string NL = "\n";  // all transfers use linux !!!!!
                try
                {
                    using var client = new TcpClient();

                    await client.ConnectAsync(IPaddress, port);
                    using var stream = client.GetStream();

                    if(cmdRequest.Write_AC)
                    {
                        if(UnixIOout == "")
                        {
                            pc.strResult = "Error: output buffer UnixIOout was empty" + Environment.NewLine;
                            return;
                        }
                        cCalcLimitProj clp = pc.GetProjStruct(cmdRequest.AC_ShortName);
                        data = cmdRequest.FormReq("<set_app_config>\n<url>" + clp.ProjUrl + "</url>\n" +
                            UnixIOout + "</set_app_config>\n");
                        await stream.WriteAsync(data, 0, data.Length, token);
                        await stream.FlushAsync(token);
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        pc.strResult = response;
                        cmdRequest.CMDerr = 0;
                        cmdRequest.CMDresults = response;
                    }



                    if(cmdRequest.Fetch_AC)
                    {
                        cCalcLimitProj clp = pc.GetProjStruct(cmdRequest.AC_ShortName);
                        data = cmdRequest.FormReq("<get_app_config>\n" + "<url>" + clp.ProjUrl + "</url>\n" + "</get_app_config>\n");
                        await stream.WriteAsync(data, 0, data.Length, token);
                        await stream.FlushAsync(token);
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        response = StripReply(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        if (GetErrorInfo(response, out string sErr))
                        {
                            pc.strResult = sErr;
                            pc.ErrorStatus = ERR_warning;
                            response = cProjectStruct.DefaultAPPconfig;
                        }

                        cAppConfig ap = ParseConfigs.ParseAppConfig(response);
                        if(ap.bValid)
                        {
                            clp.cpu_usage = ap.dCPU;
                            clp.avg_ncpus = ap.dCPU;
                            clp.gpu_usage = ap.dGPU;
                            clp.ngpus = ap.dGPU;
                            clp.MaxApps = ap.MaxApps;
                            pc.app_config = ap.AppConfig;
                            WriteACrecord(FormAppConfigFilename(pc.PCname, cmdRequest.AC_ShortName), ref ap.AppConfig);
                        }

                        /*
                        if (pc.Parse_AC(ref response, out XDocument doc))
                        {
                            double defaultDouble = -1.0;
                            int defaultInt = -1;

                            // Extract safely with defaults gpu_usage
                            double avg_ncpus = double.TryParse(doc.Descendants("avg_ncpus").FirstOrDefault()?.Value, out double v1) ? v1 : defaultDouble;
                            double ngpus = double.TryParse(doc.Descendants("ngpus").FirstOrDefault()?.Value, out double v2) ? v2 : defaultDouble;
                            int projectMaxConcurrent = int.TryParse(doc.Descendants("project_max_concurrent").FirstOrDefault()?.Value, out int v3) ? v3 : defaultInt;
                            double gpu_usage = double.TryParse(doc.Descendants("gpu_usage").FirstOrDefault()?.Value, out double v4) ? v4 : defaultDouble;
                            double cpu_usage = double.TryParse(doc.Descendants("cpu_usage").FirstOrDefault()?.Value, out double v5) ? v5 : defaultDouble;
                            // following is OK since I do not run the cpu and gpu on the same project.
                            double d = Math.Max(avg_ncpus, cpu_usage);
                            if(d > 0)
                            {
                                clp.cpu_usage = d;
                                clp.avg_ncpus = d;
                            }
                            
                            d = Math.Max(ngpus, gpu_usage);
                            if(d > 0)
                            {
                                clp.gpu_usage = d;
                                clp.ngpus = d;
                            }
                            if(projectMaxConcurrent > 0)
                                clp.MaxApps = projectMaxConcurrent;
                        }
                        */
                    }


                    // do not contact remote PCs unless local files are missing
                    if(cmdRequest.Fetch_CC)
                    {
                        data = cmdRequest.FormReq("<get_cc_config/>");
                        await stream.WriteAsync(data, 0, data.Length, token);
                        await stream.FlushAsync(token);
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        if (GetErrorInfo(response, out string sErr))
                        {
                            pc.strResult = sErr;
                            pc.ErrorStatus = ERR_warning;
                            response = "<!-- this is the default cc_config -->\n" + cProjectStruct.DefaultCCconfig;
                        }

                        //if (pc.Parse_CC_PC(ref response))
                        cCCconfig cc = ParseConfigs.ParseCCconfig(response);
                        if(cc.bValid)
                        {
                            pc.cc_config = cc.cc_config;
                            pc.pc_config = cc.pc_config;
                            globals.WriteCCrecord(pc.PCname, ref pc.cc_config);
                            if (pc.pc_config != null)
                            {
                                bPCmissing = false;
                                WritePCrecordS(pc.PCname, ref pc.pc_config);
                            }
                            else bPCmissing = true;
                            pc.ErrorStatus = bPCmissing ? ERR_warning : 0;
                            
                        }
                        else pc.ErrorStatus = ERR_critical;
                    }


                    if(cmdRequest.Write_CC_PC)
                    {
                        if(bPCmissing)
                            pc.pc_config = ProjectStats.current_default_pandora_config.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (bCCmissing)
                            pc.cc_config = ProjectStats.current_default_cc_config.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        string wNL = Environment.NewLine; // default system is windows, target may  be different
                        string sPCin = string.Join(NL, pc.pc_config);
                        string sTemplet = "<!--pandora" + NL + sPCin + NL + "-->" + NL;
                        string sTemp = cc_write( sTemplet + string.Join(NL, pc.cc_config));
                        data = cmdRequest.FormReq(sTemp);
                        await stream.WriteAsync(data, 0, data.Length, token);
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        if (response.Contains("success"))
                        {
                            pc.ErrorStatus = 0;
                            try
                            {
                                if (bCCmissing)
                                    globals.WriteCCrecord(pc.PCname, ref pc.cc_config);
                                if (bPCmissing)
                                    globals.WritePCrecordS(pc.PCname, ref pc.pc_config);
                            }
                            catch (Exception ex)
                            {
                                pc.strResult = "error: " + ex.Message;
                                pc.ErrorStatus = ERR_warning;
                            }
                        }
                        else
                        {
                            pc.ErrorStatus = ERR_critical;
                            pc.strResult = "cannot send cc and app config to " + pc.PCname + " at " + pc.IPaddress;
                        }
                    }


                    if(cmdRequest.Write_CC)
                    {
                        string sTemp = cc_write(string.Join(NL, pc.cc_config));
                        data = cmdRequest.FormReq(sTemp);
                        await stream.WriteAsync(data, 0, data.Length, token);
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        if (response.Contains("success"))
                            pc.ErrorStatus = 0;
                        else
                        {
                            pc.ErrorStatus = ERR_critical;
                            pc.strResult = "cannot send cc_config to " + pc.PCname + " at " + pc.IPaddress;
                        }
                    }
                   
                }
                catch (SocketException ex)
                {
                    FrmErr(ref pcx, $"Socket error: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    FrmErr(ref pcx, $"Invalid operation: {ex.Message}");
                }
                //catch (ObjectDisposedException ex)
                //{
                //    FrmErr(ref pcx, $"Client was disposed: {ex.Message}");
                //}
                catch (Exception ex)
                {
                    // Catch-all for unexpected errors
                    FrmErr(ref pcx, $"Unexpected error: {ex.Message}");
                }
                catch
                {
                    FrmErr(ref pcx, $"Unidentified error");
                }
                finally
                {
                    //FrmErr(ref pcx, $"finally hit");
                }
            }
        }
        
        private void FrmErr(ref cPClimit pc, string s)
        {
            pc.ErrorStatus = ERR_critical;
            pc.strResult = s;
        }

    }

    // this is no longer used
    internal class HostRPC
    {
        public string[] cc_buff;    // cc_config
        private string cc_string;
        public string[] pc_buff;    // pandora config
        public int ErrStatus = ERR_none;
        public string ErrMsg = "";
        public bool bDone = false;
        string hostname = "";
        bool bIsOnline = false;
        IPAddress ipx = IPAddress.Loopback;
        int port = 31416;
        static int MStimeout = 10000;
        public TcpClient client = new TcpClient();
        NetworkStream PCstream;
        CancellationTokenSource cts;
        string PCActualName = "";



        public bool Init(string PCname, string IPaddress)
        {
            PCActualName = PCname;
            int RtnStatus = ERR_none;
            if (PCname == Dns.GetHostName().ToLower())
                hostname = "localhost";
            else
                hostname = PCname;
            cc_buff = null;
            pc_buff = null;

            Task aTask = Task.Run(async () =>
            {
                bIsOnline = await PortChecker.IsPortOpenAsync(IPaddress, 31416);
            });
            aTask.Wait();

            if (bIsOnline)
            {


                cts = new CancellationTokenSource(MStimeout);
                //aTask = Task.Run(async () =>
                {
                    try
                    {
                        aTask = Task.Run(async () =>
                        {
                        await client.ConnectAsync(IPaddress, port);//, cts.Token);
                        });
                        aTask.Wait();


                        return true;
                    }
                    catch (OperationCanceledException)
                    {
                        ErrMsg = "Connection timed out.";
                        return false;
                    }
                    catch (SocketException ex)
                    {
                        ErrMsg = "Socket error: " + ex.Message;
                        return false;
                    }
                    catch (Exception ex)
                    {
                        ErrMsg = "Unexpected error: {ex}";
                        return false;
                    }

                }//);
                //aTask.Wait();
            }
            return bIsOnline;
        }


        public void set_cc_config(string cc_in)
        {
            string cmd = "<set_cc_config>\n" + cc_in + "</set_cc_config>\n";
            SendCCconfig(cmd);
        }

        public void get_cc_config()
        {
            string cmd ="<get_cc_config/>";
            GetCCconfig(cmd);
        }

        public void AddPandora(cPClimit PCl)
        {
            string NL =  Environment.NewLine;            
            string sTemplet = "<!--pandora" + NL + PCl.CreateTempletPC(PC_NNW_PROJECT_LIMIT | PC_REPORT_STATUS) + NL + "-->" + NL;
            string[] sTemp = sTemplet.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            get_cc_config();
            if (PCl.OStype != "w") NL = "\n";
            if(PCl.OStype == null)
            {
                int i = 0;
            }
            if(cc_buff == null)
            {
                int i1 = 0;
            }
            string sOut = string.Join(NL, sTemp) + NL + string.Join(NL, cc_buff);
            set_cc_config(sOut);
        }
        
        
        public void SendCCconfig(string cmd)
        {
            string sRPC = "<boinc_gui_rpc_request>\n" + cmd + "</boinc_gui_rpc_request>\n\u0003";

            byte[] data = Encoding.UTF8.GetBytes(sRPC);
            var buffer = new byte[65536];

            if (client.Connected)
            {
                PCstream = client.GetStream();
                ErrMsg = "";
                Task aTask = Task.Run(async () =>
                {
                    try
                    {
                        await PCstream.WriteAsync(data, 0, data.Length, cts.Token);
                        int bytesRead = await PCstream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                        if (bytesRead > 0)
                        {
                            string sTemp = Encoding.UTF8.GetString(buffer);
                            int i = sTemp.IndexOf("success");
                            ErrStatus = (i < 0) ? ERR_critical : ERR_none;
                            ErrMsg = "cc_config not updated";
                            return;
                        }
                        else
                        {
                            ErrMsg = "nothing obtained from host";
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        ErrMsg = "The stream was closed or disposed.";
                    }
                    catch (IOException ex) when (ex.InnerException is SocketException se)
                    {
                        ErrMsg = $"Socket error: {se.SocketErrorCode} - {se.Message}";
                    }
                    catch (IOException ex)
                    {
                        ErrMsg = "I/O error: " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        ErrMsg = "Unexpected error: " + ex.Message;
                    }

                });
                aTask.Wait();
                ErrStatus = ErrMsg == "" ? ERR_none : ERR_critical;
            }
        

        }

        public void  GetCCconfig(string cmd)
        {
            string sRPC = "<boinc_gui_rpc_request>\n" + cmd + "</boinc_gui_rpc_request>\n\u0003";

            byte[] data = Encoding.UTF8.GetBytes(sRPC);
            var buffer = new byte[65536];
            
            if (client.Connected)
            {
                PCstream = client.GetStream();
                ErrMsg = "";
                Task aTask = Task.Run(async () =>
                {
                    try
                    {
                        await PCstream.WriteAsync(data, 0, data.Length,cts.Token);
                        int bytesRead = await PCstream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                        if (bytesRead > 0)
                        {
                            string sTemp = Encoding.UTF8.GetString(buffer);
                            int i = sTemp.IndexOf("<cc_config>");
                            Debug.Assert(i >= 0);
                            int j = sTemp.IndexOf("</cc_config>", i + 11);
                            Debug.Assert(j >= 0);
                            cc_string = sTemp.Substring(i, j + 12 - i).Trim();
                            cc_buff = cc_string.Split(new string[] { "\n", "\r" ,"\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            i = sTemp.IndexOf("<!--pandora");
                            if (i < 0) pc_buff = null;
                            else
                            {
                                j = sTemp.IndexOf("-->", 11);
                                Debug.Assert(j > 0);
                                sTemp = sTemp.Substring(i + 11, j - i - 11);
                                pc_buff = sTemp.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            ErrStatus = ERR_none;
                        }
                        else
                        {
                            ErrMsg = "nothing obtained from host";                            
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        ErrMsg = "The stream was closed or disposed.";
                    }
                    catch (IOException ex) when (ex.InnerException is SocketException se)
                    {
                        ErrMsg = $"Socket error: {se.SocketErrorCode} - {se.Message}";
                    }
                    catch (IOException ex)
                    {
                        ErrMsg = "I/O error: " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        ErrMsg = "Unexpected error: " + ex.Message;
                    }

                });
                aTask.Wait();
                ErrStatus = ErrMsg == "" ? ERR_none : ERR_critical;
            }
        }
    }

    internal class HostRPC1
    {

        private static string r = Environment.NewLine;
        private static string[] StartTok = { "<project_name>", "<hostid>" };
        private static string[] StopTok = { "</project_name>", "</hostid>" };
        public static string sOut = "";
        public static bool bDone = true;
        public static int sErr = 0;
        public static bool bInScheduler = false;
        public static string sBuff;
        private static int MStimeout = 1000;
        public void InitScheduler(int iTimeout)
        {
            sOut = "";
            MStimeout = iTimeout * 1000;
            bInScheduler = true;
        }

        public void StopScheduler()
        {
            bInScheduler = false;
        }

        public string GetSchedulerResults()
        {
            bInScheduler = false;
            return sOut;
        }
        public bool InScheduler(){return bInScheduler;}
        public  bool SchedulerDone() { return bDone; }
        public int SchedulerError() { return sErr; }


        public int ReadStream(string hostname)
        {
            var ipx = IPAddress.Loopback;
            var port = 31416;
            string StatisticsRequest = "<boinc_gui_rpc_request>\n" +
                "<get_project_status/>\n" +
                "</boinc_gui_rpc_request>\n\u0003\u0001";//+ "\u0001";
            sBuff = string.Empty;
            int n = 0;
            int Numfound = 0;

            const int blockSize = 256;
            byte[] buffer = new byte[blockSize];

            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(hostname);

                foreach (IPAddress ip in addresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4 only
                    {
                        ipx = ip;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            try
            {

                Socket client = new Socket(ipx.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                client.ReceiveTimeout = MStimeout;
                client.SendTimeout = MStimeout;
                byte[] data = Encoding.UTF8.GetBytes(StatisticsRequest);

                client.Blocking = false;
                try
                {
                    client.Connect(new IPEndPoint(ipx, port));
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.WouldBlock || ex.SocketErrorCode == SocketError.InProgress)
                {
                    if (!client.Poll(MStimeout, SelectMode.SelectWrite))
                    {
                        client.Close();
                        throw new TimeoutException("Connection timed out.");
                    }

                    // Check for connection success
                    if (!client.Connected)
                    {
                        client.Close();
                        throw new SocketException((int)SocketError.NotConnected);
                    }
                }
                finally
                {
                    client.Blocking = true;
                }


                client.Send(data);
                try
                {
                    while (true)
                    {

                        try
                        {
                            n = client.Receive(buffer);
                        }
                        //catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                        //{
                        //    break;
                        //}
                        catch (SocketException ex)
                        {
                            break;
                        }

                        if (n > 0)
                        {
                            string s = Encoding.UTF8.GetString(buffer, 0, n);
                            sBuff += s;
                            //if (n < blockSize) break;
                            continue;
                        }
                        else break;
                    }
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    n = 0;
                    int i, j;
                    sOut += r + hostname + r;
                    i = 0;
                    while (i < sBuff.Length)
                    {
                        int k = sBuff.IndexOf(StartTok[0], i);
                        if (k < 0)
                        {
                            return Numfound;
                        }
                        i = k;
                        j = sBuff.IndexOf(StopTok[0], i);
                        Debug.Assert(j > 0);
                        string sPROJ = sBuff.Substring(i + StartTok[0].Length, j - i - StartTok[0].Length);
                        i += StopTok[0].Length;
                        k = sBuff.IndexOf(StartTok[1], i);
                        if (k < 0)
                        {
                            return Numfound;
                        }
                        i = k;
                        j = sBuff.IndexOf(StopTok[1], i);
                        Debug.Assert(j > 0);
                        string sProjID = sBuff.Substring(i + StartTok[1].Length, j - i - StartTok[1].Length);
                        i += StopTok[1].Length;
                        sOut += sPROJ + "," + sProjID + r;
                        Numfound++;
                    }
                    return Numfound;
                }
                finally
                {
                    sErr = 0;
                    bDone = true;
                    //ProcessOUT(hostname, ref sBuff);
                    client.Dispose();
                }
                return Numfound;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error connecting to " + hostname + ":\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            return Numfound;
        }



        public static int ProcessOUT(string hostname, ref string sBuff)
        {
            int Numfound = 0;
            int i, j;
            sOut += r + hostname + r;
            i = 0;
            
            while (i < sBuff.Length)
            {
                int k = sBuff.IndexOf(StartTok[0], i);
                if (k < 0)
                {
                    return Numfound;
                }
                i = k;
                j = sBuff.IndexOf(StopTok[0], i);
                Debug.Assert(j > 0);
                string sPROJ = sBuff.Substring(i + StartTok[0].Length, j - i - StartTok[0].Length);
                i += StopTok[0].Length;
                k = sBuff.IndexOf(StartTok[1], i);
                if (k < 0)
                {
                    return Numfound;
                }
                i = k;
                j = sBuff.IndexOf(StopTok[1], i);
                Debug.Assert(j > 0);
                string sProjID = sBuff.Substring(i + StartTok[1].Length, j - i - StartTok[1].Length);
                i += StopTok[1].Length;
                sOut += sPROJ + "," + sProjID + r;
                Numfound++;
            }
            return Numfound;
        }


        //8/9/2025 the below worked but I ended up using boinccmd.exe to get the tasking information
        // Replace 'using declarations' with explicit using statements to ensure compatibility with C# 7.3.  
        public static async Task GetHostInfo(string hostname)
        {
            string StatisticsRequest = "<boinc_gui_rpc_request>\n" +
    "<get_project_status/>\n" +
    "</boinc_gui_rpc_request>\n\u0003";//+ "\x01";
            string sBuff = string.Empty;
            int n = 0;
            bool bTrue = true;
            string r = Environment.NewLine;
            string[] StartTok = { "<project_name>", "<hostid>" };
            string[] StopTok = { "</project_name>", "</hostid>" };
            int Numfound = 0;
            var ipx = IPAddress.Loopback;
            string sTemp = "";
            bDone = false;
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(hostname);

                foreach (IPAddress ip in addresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4 only
                    {
                        ipx = ip;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                sErr = 1;
                bDone = true;
                bTrue = false;
                return;
            }

            TcpClient client = new TcpClient();
            try
            {
                await client.ConnectAsync(ipx, 31416);                

                NetworkStream stream = client.GetStream();
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(StatisticsRequest);
                    await stream.WriteAsync(data, 0, data.Length);
                    var buffer = new byte[1024*256];
                    //while (true)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        sBuff += Encoding.UTF8.GetString(buffer);                        
                    }

                }
                finally
                {
                    bTrue = false;
                    stream.Dispose();
                }

            }
            finally
            {
                client.Dispose();
                bTrue = false;
            }
            ProcessOUT(hostname, ref sBuff);
            sErr = 0;
            bDone = true;
        }

    }
}
