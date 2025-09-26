using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static CreditStatistics.globals;

namespace CreditStatistics
{
    internal class ReadSitePage
    {
        public string sRawPage { get; set; } = "";
        public CancellationTokenSource cts { get; set; }
        public string sMsgOut { get; set; } = "";

        private string boinc_passwd = Properties.Settings.Default.BoincWebPassword;
        private string boinc_email = Properties.Settings.Default.BoincWebUsername;

        string ProjectName = "";
        private System.Threading.Timer timeoutTimer;
        private bool DoHeadless = true;
        private string NL = Environment.NewLine;
        private RunList sig;

        public void Init(ref RunList Rsig)
        {
            sig = Rsig;
        }
        
        public async Task ReadProjectThisSite(string shortname, string sUrl)
        {
            sig.url = sUrl;
            ProjectName = shortname;
            sMsgOut = "";
            sig.bDone = false;
            await RunAsync(sig.TimeoutSecs);
        }

        public void Cancel()
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel();
                sMsgOut += "Manual cancel requested." + NL;
            }
        }

        /*
        public async Task RunAsync(int t)
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(t));  // cancels automatically after t seconds

            try
            {
                await DoPlaywrightWorkAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                sMsgOut += "Operation was cancelled due to timeout." + NL;
                sig.bDone = true;
            }
        }
        */

        public async Task RunAsync(int t)
        {
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(t));  // Auto cancel after t seconds

            try
            {
                await DoPlaywrightWorkAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                sMsgOut += "Operation was cancelled." + NL;
                sig.bDone = true;
            }
            finally
            {
                if(cts != null)
                    cts.Dispose();
                cts = null;
            }
        }


        private async Task DoPlaywrightWorkAsync(CancellationToken token)
        {            
            if (ProjectName == "primegrid" || ProjectName == "cpdn")
                TaskStartPrime(token);
            else if (ProjectName == "mine")
                await TaskStartMine(token);
            else                
                await TaskStart(token); // added await 9/9 and had to add Task to TaskStart
        }

        private void TaskStartPrime(CancellationToken token)
        {
            try
            {
                Task longRunningTask = Task.Run(async () =>
                {
                    await LoginWithPlaywrightAsync(token);
                }, token);
            }
            catch (Exception ex)
            {
                sig.sMsgErr += ex.Message + NL;
                sig.bDone = true;
            }
        }

        // Change the signature of TaskStart from 'private async void TaskStart(CancellationToken token)' to 'private async Task TaskStart(CancellationToken token)'
        // This allows it to be awaited.

        private async Task TaskStart(CancellationToken token) // changed from async void to async Task
        {
            try
            {
                Task longRunningTask = Task.Run(async () =>
                {
                    await ReadOnePageNoPassword(token);
                }, token);
                await longRunningTask;
            }
            catch (Exception ex)
            {
                sig.sMsgErr += ex.Message + NL;
                sig.bDone = true;
            }
        }

        private async Task TaskStartMine(CancellationToken token)
        {
            try
            {
                Task longRunningTask = Task.Run(async () =>
                {
                    await ReadOnePageNoPasswordMine(token);
                }, token);
                await longRunningTask;
            }
            catch (Exception ex)
            {
                sig.sMsgErr += ex.Message + NL;
                sig.bDone = true;
            }

        }

        private async Task ReadOnePageNoPasswordMine(CancellationToken token)
        {
            using var playwright = await Playwright.CreateAsync();

            token.ThrowIfCancellationRequested();

            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = DoHeadless
            });

            token.ThrowIfCancellationRequested();


            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            });

            var page = await context.NewPageAsync();

            token.ThrowIfCancellationRequested();

            try
            {
                await page.GotoAsync(sig.url, new() { Timeout = 0});
            }
            catch (PlaywrightException ex)
            {
                sig.sMsgErr += ex.Message + NL;
            }


            //await Task.Delay(1000, token); // Give time for server to initialize session
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            token.ThrowIfCancellationRequested();

            try
            {
                await page.GotoAsync(sig.url, new() { Timeout = 0});
            }
            catch (PlaywrightException ex)
            {
                sig.sMsgErr += ex.Message + NL;
            }



            //await Task.Delay(500, token); // Optional: wait again if needed
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            token.ThrowIfCancellationRequested();

            sRawPage = await page.ContentAsync();
            await browser.CloseAsync();
            sig.bDone = true;
        }




        private async Task ReadOnePageNoPassword(CancellationToken token)
        {

            using var playwright = await Playwright.CreateAsync();

            token.ThrowIfCancellationRequested();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = DoHeadless // set true to run without UI
            });

            token.ThrowIfCancellationRequested();

            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            });

            var page = await context.NewPageAsync();

            token.ThrowIfCancellationRequested();
            try
            {
                await page.GotoAsync(sig.url, new() { Timeout = 0});
            }
            catch (PlaywrightException ex)
            {
                string sErr = ex.Message;
            }

            

            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            token.ThrowIfCancellationRequested();
            sRawPage = await page.ContentAsync();

            await browser.CloseAsync();
            sig.bDone = true;
        }


        // this is how passwords are handled in prime
        public async Task LoginWithPlaywrightAsync(CancellationToken token)
        {
            using var playwright = await Playwright.CreateAsync();

            //token.ThrowIfCancellationRequested();

            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless =  DoHeadless // Set to false to watch the login happen
            });

            var context = await browser.NewContextAsync(new()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) ...",
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
                IgnoreHTTPSErrors = true
            });
            //token.ThrowIfCancellationRequested();

            /*
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            });
            */

            var page = await context.NewPageAsync();
            string url = "https://main.cpdn.org/login_form.php";

            if (ProjectName == "cpdn")
            {
                await Task.Delay(500);
                try
                {
                    //await page.GotoAsync(url, new() { Timeout = 0});
                    await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.DOMContentLoaded });
                }
                catch (PlaywrightException ex)
                {
                    sig.sMsgErr += ex.Message + NL;
                }

                try
                {

                               
                    var locator = page.Locator("input[name='email_addr']");
                    await locator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });
                    var isVisible = await locator.IsVisibleAsync();
                    string sErr = $"Is input visible? {isVisible}";
                    //sig.sMsgErr += sErr + NL;
                    await locator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });                
                    await locator.FillAsync(boinc_email);
                    await page.FillAsync("input[name='passwd']", boinc_passwd, new() { Timeout = 10000 });
                    await page.ClickAsync("button[type='submit']");
                    
                }
                catch (TimeoutException tex)
                {
                    sig.sMsgErr += "Timeout: input[name='email_addr'] did not become visible: " + tex.Message + NL;
                }
                catch (Exception ex)
                {
                    sig.sMsgErr += "Error filling input: " + ex.ToString() + NL;
                }
                //await page.GotoAsync(sig.url, new() { Timeout = 0});
                await page.GotoAsync(sig.url, new() { WaitUntil = WaitUntilState.DOMContentLoaded });
                await Task.Delay(500);

                sRawPage = await page.ContentAsync();
                sig.bDone = true;
            }




            // Go to the login page
            if (ProjectName == "primegrid")
            {
                try
                {
                    await page.GotoAsync("https://www.primegrid.com/login_form.php?next_url=/", new() { Timeout = 0});
                }
                catch (PlaywrightException ex)
                {
                    string sErr = ex.Message;
                }

                token.ThrowIfCancellationRequested();

                await page.FillAsync("input[name='email_addr']", boinc_email);
                await page.FillAsync("input[name='passwd']", boinc_passwd);

                token.ThrowIfCancellationRequested();

                // Submit the form
                await page.ClickAsync("input[type='submit']"); // they used type instead of name
                token.ThrowIfCancellationRequested();
                await Task.Delay(500);

                await page.GotoAsync(sig.url, new() { Timeout = 0});
                token.ThrowIfCancellationRequested();
                await Task.Delay(500);

                sRawPage = await page.ContentAsync();

                await browser.CloseAsync();
            }

            sig.bDone = true;
        }
    }
}
