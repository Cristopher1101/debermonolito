$csprojPath = "Monolito4_B\Monolito4_B.csproj"
$xml = [xml](Get-Content -Path $csprojPath)
$ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
$ns.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003")

$itemGroupRef = $xml.Project.ItemGroup | Where-Object { $_.Reference -ne $null } | Select-Object -First 1

$exists = $itemGroupRef.Reference | Where-Object { $_.Include -eq "EPPlus" }
if (-not $exists) {
    $newNode = $xml.CreateElement("Reference", "http://schemas.microsoft.com/developer/msbuild/2003")
    $newNode.SetAttribute("Include", "EPPlus")
    
    $hintNode = $xml.CreateElement("HintPath", "http://schemas.microsoft.com/developer/msbuild/2003")
    $hintNode.InnerText = "..\packages\EPPlus.4.5.3.3\lib\net40\EPPlus.dll"
    
    $newNode.AppendChild($hintNode) | Out-Null
    $itemGroupRef.AppendChild($newNode) | Out-Null
    $xml.Save($csprojPath)
}
