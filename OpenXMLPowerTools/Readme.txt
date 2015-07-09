Open XML Power Tools
============================================

New Installation Procedures
---------------------------
1)  If necessary, run PowerShell as administrator, Set-ExecutionPolicy Unrestricted
2)  Install the Open XML SDK 2.5  http://bit.ly/1qMaf6i
3)  Download and unzip PowerTools for Open XMl 3.1.0 or later:  http://bit.ly/1ss8hfV
    Unzip into C:\Users\<user>\Documents\WindowsPowerShell\Modules\Oxpt
	(Create WindowsPowerShell and Modules directories as needed)
4)  Import-Module OxPt
5)  Visual Studio not needed!!!

Version 3.1.02 : December 1, 2014
- Added Add-DocxText Cmdlet

Version 3.1.01 : November 23, 2014
- Added Convert-DocxToHtml Cmdlet
- Added Chinese and Hebrew sample documents
- Cmdlets in this release
	Clear-DocxTrackedRevision
	Convert-DocxToHtml
	ConvertFrom-Base64
	ConvertFrom-FlatOpc
	ConvertTo-Base64
	ConvertTo-FlatOpc
	Get-OpenXmlValidationErrors
	Merge-Docx
	New-Docx
	Test-OpenXmlValid

Version 3.1.00 : November 13, 2014
- Changed installation process - no longer requires compilation using Visual Studio
- Added ConvertTo-FlatOpc Cmdlet
- Added ConvertFrom-FlatOpc Cmdlet
- Changed parameters for Test-OpenXmlValid, Get-OpenXmlValidationErrors
- Removed the unnecessary 1/2 second sleep when doing Word automation in the New-Docx Cmdlet

Version 3.0.00 : October 29, 2014
- New release of cmdlets that are written as 'Advanced Functions' instead of in C#.

