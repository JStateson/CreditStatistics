using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CreditStatistics.globals;

namespace CreditStatistics
{
    internal class ReadSitePage
    {
        public string sRawPage { get; set; } = "";
        public CancellationTokenSource cts { get; set; }
        public string sMsgOut { get; set; } = "";


        private string cpdn_passwd = "";
        private string prime_passwd = "";

        string ProjectName = "";
        string Url = "";
        //private CancellationTokenSource cts;
        private System.Threading.Timer timeoutTimer;
        private bool DoHeadless = true;
        private string NL = Environment.NewLine;
        private RunList sig;
        
        public void ReadProjectThisSite(string shortname, string sUrl, ref RunList Rsig)
        {
            Url = sUrl;
            ProjectName = shortname;
            sMsgOut = "";
            sig = Rsig;
            sig.bDone = false;
            RunAsync();
        }

        public void SignalTaskDone()
        {
            if (!cts.IsCancellationRequested)
            {
                timeoutTimer?.Dispose(); // Dismiss the timer
            }
        }

        public void Cancel()
        {

            try
            {
                // Try accessing something to check if it's disposed
                var token = cts.Token;  // This will throw if disposed
                if (cts.Token.CanBeCanceled)
                    cts.Cancel();
            }
            catch (ObjectDisposedException)
            {
               
            }
            if (!cts.IsCancellationRequested)
            {
                timeoutTimer?.Dispose(); // Dismiss the timer
            }
        }

        public async Task RunAsync()
        {
            cts = new CancellationTokenSource();

            // Set up a timer to cancel after 20 seconds
            timeoutTimer = new System.Threading.Timer(state =>
            {
                sMsgOut += "Timeout reached. Cancelling operation." + NL;
                cts.Cancel();
                sig.bDone = true;
            }, null, TimeSpan.FromSeconds(20), Timeout.InfiniteTimeSpan);

            try
            {
                await DoPlaywrightWorkAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                sMsgOut += "Operation was cancelled due to timeout." + NL;
                sig.bDone = true;
            }
            finally
            {
                //timeoutTimer?.Dispose();
                //cts.Dispose();
            }

            if(cts.Token.IsCancellationRequested)
            {

            }
        }


        private async Task DoPlaywrightWorkAsync(CancellationToken token)
        {
            if (ProjectName == "prime" || ProjectName == "cpdn")
                TaskStartPrime(token);
            else if (ProjectName == "mine")
                TaskStartMine(token);
            else            
                TaskStart(token);            
        }

        private void TaskStartPrime(CancellationToken token)
        {

            Task longRunningTask = Task.Run(async () =>
            {
                await LoginWithPlaywrightAsync(token);
            }, token);
        }

        private void TaskStart(CancellationToken token)
        {
            Task longRunningTask = Task.Run(async () =>
            {
                await ReadOnePageNoPassword(token);
            }, token);
        }

        private void TaskStartMine(CancellationToken token)
        {
            Task longRunningTask = Task.Run(async () =>
            {
                await ReadOnePageNoPasswordMine(token);
            }, token);
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
                await page.GotoAsync(Url, new() { Timeout = 0 });
            }
            catch (PlaywrightException ex)
            {
                string sErr = ex.Message;
            }


            //await Task.Delay(1000, token); // Give time for server to initialize session
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            token.ThrowIfCancellationRequested();

            try
            {
                await page.GotoAsync(Url, new() { Timeout = 0 });
            }
            catch (PlaywrightException ex)
            {
                string sErr = ex.Message;
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
                await page.GotoAsync(Url, new() { Timeout = 0 });
            }
            catch (PlaywrightException ex)
            {
                string sErr = ex.Message;
            }

            
            //await Task.Delay(500);
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

            //token.ThrowIfCancellationRequested();


            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            });

            var page = await context.NewPageAsync();


            if (ProjectName == "cpdn")
            {

                try
                {
                    await page.GotoAsync("https://main.cpdn.org/login_form.php", new() { Timeout = 0 });
                }
                catch (PlaywrightException ex)
                {
                    string sErr = ex.Message;
                }

                try
                {
                    
                    var locator = page.Locator("input[name='email_addr']");
                    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                    var isVisible = await locator.IsVisibleAsync();
                    string sErr = $"Is input visible? {isVisible}";
                    Console.WriteLine(sErr);
                    await locator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 5000 });                
                    await locator.FillAsync("josephy@stateson.net");

                    


                    //await page.FillAsync("input[name='email_addr']", "josephy@stateson.net", new() { Timeout = 5000 });
                    await page.FillAsync("input[name='passwd']", cpdn_passwd, new() { Timeout = 5000 });
                    await page.ClickAsync("button[type='submit']");

                }
                catch (TimeoutException tex)
                {
                    Console.WriteLine("Timeout: input[name='email_addr'] did not become visible.");
                    Console.WriteLine(tex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error filling input:");
                    Console.WriteLine(ex.ToString());
                }
                await page.GotoAsync(Url, new() { Timeout = 0 });
                await Task.Delay(500);

                sRawPage = await page.ContentAsync();


            }




            // Go to the login page
            if (ProjectName == "prime")
            {
                try
                {
                    await page.GotoAsync("https://www.primegrid.com/login_form.php?next_url=/", new() { Timeout = 0 });
                }
                catch (PlaywrightException ex)
                {
                    string sErr = ex.Message;
                }

                token.ThrowIfCancellationRequested();

                await page.FillAsync("input[name='email_addr']", "josephy@stateson.net");
                await page.FillAsync("input[name='passwd']", prime_passwd);

                token.ThrowIfCancellationRequested();

                // Submit the form
                await page.ClickAsync("input[type='submit']"); // they used type instead of name
                token.ThrowIfCancellationRequested();
                await Task.Delay(500);

                await page.GotoAsync(Url, new() { Timeout = 0 });
                token.ThrowIfCancellationRequested();
                await Task.Delay(500);

                sRawPage = await page.ContentAsync();

                await browser.CloseAsync();
            }

            sig.bDone = true;
        }




    }
}
