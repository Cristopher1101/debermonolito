$csprojPath = "Monolito4_B\Monolito4_B.csproj"
$xml = [xml](Get-Content -Path $csprojPath)
$ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
$ns.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003")

$itemGroupContent = $xml.Project.ItemGroup | Where-Object { $_.Content -ne $null } | Select-Object -First 1
$itemGroupCompile = $xml.Project.ItemGroup | Where-Object { $_.Compile -ne $null } | Select-Object -First 1

$aspxFiles = Get-ChildItem -Path "Monolito4_B\Mantenimiento" -Filter "*.aspx"
foreach ($aspx in $aspxFiles) {
    $relPath = "Mantenimiento\" + $aspx.Name
    $existsContent = $itemGroupContent.Content | Where-Object { $_.Include -eq $relPath }
    if (-not $existsContent) {
        $newNode = $xml.CreateElement("Content", "http://schemas.microsoft.com/developer/msbuild/2003")
        $newNode.SetAttribute("Include", $relPath)
        $itemGroupContent.AppendChild($newNode) | Out-Null
    }

    $csPath = $relPath + ".cs"
    $existsCs = $itemGroupCompile.Compile | Where-Object { $_.Include -eq $csPath }
    if (-not $existsCs) {
        $newNode = $xml.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003")
        $newNode.SetAttribute("Include", $csPath)
        $depNode = $xml.CreateElement("DependentUpon", "http://schemas.microsoft.com/developer/msbuild/2003")
        $depNode.InnerText = $aspx.Name
        $newNode.AppendChild($depNode) | Out-Null
        $subNode = $xml.CreateElement("SubType", "http://schemas.microsoft.com/developer/msbuild/2003")
        $subNode.InnerText = "ASPXCodeBehind"
        $newNode.AppendChild($subNode) | Out-Null
        $itemGroupCompile.AppendChild($newNode) | Out-Null
    }

    $designerPath = $relPath + ".designer.cs"
    $existsDes = $itemGroupCompile.Compile | Where-Object { $_.Include -eq $designerPath }
    if (-not $existsDes) {
        $newNode = $xml.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003")
        $newNode.SetAttribute("Include", $designerPath)
        $depNode = $xml.CreateElement("DependentUpon", "http://schemas.microsoft.com/developer/msbuild/2003")
        $depNode.InnerText = $aspx.Name
        $newNode.AppendChild($depNode) | Out-Null
        $itemGroupCompile.AppendChild($newNode) | Out-Null
    }
}

$xml.Save($csprojPath)
