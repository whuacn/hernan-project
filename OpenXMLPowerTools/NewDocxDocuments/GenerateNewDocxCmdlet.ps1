[environment]::CurrentDirectory = $(Get-Location)
if (Test-Path .\GenerateNewDocxCmdlet.ps1)
{

$dx = "..\OxPtCmdlets\DocxLib.ps1"
if (Test-Path $dx) { del $dx}
@"
<#***************************************************************************

Copyright (c) Microsoft Corporation 2014.

This code is licensed using the Microsoft Public License (Ms-PL).  The text of the license can be found here:

http://www.microsoft.com/resources/sharedsource/licensingbasics/publiclicense.mspx

Published at http://OpenXmlDeveloper.org
Resource Center and Documentation: http://openxmldeveloper.org/wiki/w/wiki/powertools-for-open-xml.aspx

***************************************************************************#>

"@ >> $dx
dir *.docx | % {
    $fi = New-Object System.IO.FileInfo $_
    "`$SampleDocx$($_.BaseName) =" >> $dx
    ConvertTo-Base64 $_ -PowerShellLiteral >>$dx
    "" >> $dx
}

$template = [System.IO.File]::ReadAllLines("..\OxPtCmdlets\New-Docx-Template.ps1")
$paramDocs = -1;
$paramDecl = -1;
$paramUse = -1;
for ($i = 0; $i -lt $template.Length; $i++)
{
    $t = $template[$i]
    if ($t.Contains("ParameterDocumentation")) { $paramDocs = $i }
    if ($t.Contains("ParameterDeclaration")) { $paramDecl = $i }
    if ($t.Contains("ParameterUse")) { $paramUse = $i }
}

$ndx = "..\OxPtCmdlets\New-Docx.ps1"
if (Test-Path $ndx)
{
    Remove-Item $ndx
}

$template[0..($paramDocs - 1)] | % { $_ } >> $ndx
dir *.docx | % {
    $fi = New-Object System.IO.FileInfo $_
    "    .PARAMETER $($fi.BaseName)" >> $ndx
    $fiDesc = New-Object System.IO.FileInfo $($_.BaseName + ".txt")
    if ($fiDesc.Exists)
    {
        Get-Content $($fiDesc.FullName) | % { '    ' + $_ } >> $ndx
    }
    else
    {
        $errMessage = "Error: $($fi.BaseName).docx does not have a corresponding $($fi.BaseName).txt"
        Write-Host -ForegroundColor Red $errMessage
        '    ' + $errMessage >> $ndx
    }
}
$start = $paramDocs + 1
$end = $paramDecl - 1
$template[$start..$end] | % { $_ } >> $ndx
$last = (($(dir *.docx) | measure).Count) - 1
$count = 0
dir *.docx | % {
    $fi = New-Object System.IO.FileInfo $_
    '        [Parameter(Mandatory=$False)]' >> $ndx
    '        [Switch]' >> $ndx
    if ($count -ne $last)
    {
        "        [bool]`$$($_.BaseName)," >> $ndx
    }
    else
    {
        "        [bool]`$$($_.BaseName)" >> $ndx
    }
    "" >> $ndx
    $count++
}
$start = $paramDecl + 1
$end = $paramUse - 1
$template[$start..$end] | % { $_ } >> $ndx
dir *.docx | % {
    $fi = New-Object System.IO.FileInfo $_
    "    if (`$All -or `$$($fi.BaseName)) { AppendDoc `$srcList `$SampleDocx$($fi.BaseName) `"$($fi.BaseName)`" }" >> $ndx
}
$start = $paramUse + 1
$template[$start..99999] | % { $_ } >> $ndx

}
else
{
    Throw "You must run this script from within the NewDocxDocuments directory"
}
