public class EnginDto
{
    public Guid Id { get; set; }
    public string Marque { get; set; }
    public string? Model { get; set; }
    public string? Matricule { get; set; }
    public string? Genre { get; set; }
    public string? EnginTypeCode { get; set; }
    public string? EnginTypeName { get; set; }
    public string? EnginTypeDescription { get; set; }
    public string? PuissanceFiscal { get; set; }
    public DateTime? MiseCirculationDate { get; set; }
    public string ModeCarburant { get; set; }
    public string? Acquisition { get; set; }
    public string Etat { get; set; }
}
