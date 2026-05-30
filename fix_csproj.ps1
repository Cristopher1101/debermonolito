$xml = [xml](Get-Content -Path "Monolito4_B\Monolito4_B.csproj")

# Find ItemGroups
$itemGroupContent = $xml.Project.ItemGroup | Where-Object { $_.Content -ne $null } | Select-Object -First 1
$itemGroupCompile = $xml.Project.ItemGroup | Where-Object { $_.Compile -ne $null } | Select-Object -First 1

$filesContent = @("Mantenimiento\listar_tbl_proveedor.aspx", "Mantenimiento\nuevo_tbl_proveedor.aspx", "Mantenimiento\importar_productos.aspx", "Mantenimiento\estadisticas_producto.aspx")

foreach ($file in $filesContent) {
    $exists = $itemGroupContent.Content | Where-Object { $_.Include -eq $file }
    if (-not $exists) {
        $newNode = $xml.CreateElement("Content", "http://schemas.microsoft.com/developer/msbuild/2003")
        $newNode.SetAttribute("Include", $file)
        $itemGroupContent.AppendChild($newNode) | Out-Null
    }
}

$filesCompile = @(
    @{ File="Mantenimiento\listar_tbl_proveedor.aspx.cs"; Dep="listar_tbl_proveedor.aspx"; SubType="ASPXCodeBehind" },
    @{ File="Mantenimiento\listar_tbl_proveedor.aspx.designer.cs"; Dep="listar_tbl_proveedor.aspx"; SubType=$null },
    @{ File="Mantenimiento\nuevo_tbl_proveedor.aspx.cs"; Dep="nuevo_tbl_proveedor.aspx"; SubType="ASPXCodeBehind" },
    @{ File="Mantenimiento\nuevo_tbl_proveedor.aspx.designer.cs"; Dep="nuevo_tbl_proveedor.aspx"; SubType=$null },
    @{ File="Mantenimiento\importar_productos.aspx.cs"; Dep="importar_productos.aspx"; SubType="ASPXCodeBehind" },
    @{ File="Mantenimiento\importar_productos.aspx.designer.cs"; Dep="importar_productos.aspx"; SubType=$null },
    @{ File="Mantenimiento\estadisticas_producto.aspx.cs"; Dep="estadisticas_producto.aspx"; SubType="ASPXCodeBehind" },
    @{ File="Mantenimiento\estadisticas_producto.aspx.designer.cs"; Dep="estadisticas_producto.aspx"; SubType=$null }
)

foreach ($file in $filesCompile) {
    $exists = $itemGroupCompile.Compile | Where-Object { $_.Include -eq $file.File }
    if (-not $exists) {
        $newNode = $xml.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003")
        $newNode.SetAttribute("Include", $file.File)
        
        $depNode = $xml.CreateElement("DependentUpon", "http://schemas.microsoft.com/developer/msbuild/2003")
        $depNode.InnerText = $file.Dep
        $newNode.AppendChild($depNode) | Out-Null
        
        if ($file.SubType) {
            $subNode = $xml.CreateElement("SubType", "http://schemas.microsoft.com/developer/msbuild/2003")
            $subNode.InnerText = $file.SubType
            $newNode.AppendChild($subNode) | Out-Null
        }
        
        $itemGroupCompile.AppendChild($newNode) | Out-Null
    }
}

$xml.Save("Monolito4_B\Monolito4_B.csproj")
