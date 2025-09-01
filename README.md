# CreditStatistics calculates a pandora\_config file that is ideal for bunkering workunits for contests.

## You first acquire 5-20 work units for each project of the contest so the required number of WUs can be calculated.  You then specify the projects, the allowed time, and build the pandora\_config files to send your PCs. When the contest starts and the project is selected, you will have the best point value possible regardless of the random selection.

The CreditStatistics app works with BOINC and BoincTasks to obtain statistics from websites, control local PCs and manage competitions

Download any 7z binaries to get both the release and debug versions of CreditStatistics

To reset the app, run the app by entering 'CreditStatistics.exe reset'

BoincTasks is needed only to obtain the list of remote PCs

Boinc must be running as the app uses boinccmd.exe to obtain some information.

Remote PCs are access over ports 31416 and 22 so you must have Boinc.exe running and have OpenSSH installed.

Website are scraped using Microsoft's Playwright to avoid bot blocks.  Currently, only BOINC websites CPDN and PrimeGrid require credentials.

OpenSSH is needed for control of remote PCs.  Private repository BOINC830Pandora has notes on this.
