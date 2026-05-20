namespace capa_negocio
{
    /// <summary>Fila unificada para mostrar galería (BD + borradores en sesión) con miniatura.</summary>
    public sealed class GaleriaVistaLinea
    {
        public string CmdArg { get; set; }
        public string DataUri { get; set; }
        public string Pie { get; set; }
    }
}
