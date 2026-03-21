namespace MVMedia.Adm.DTOs
{
    public class ClientWithMediaFileDTO
    {
        public ClientSummaryDTO? Client { get; set; }
        public List<MediaFileLIstItemDTO>? MediaFiles { get; set; }
        public string? CPF { get; set; }
        public string? CNPJ { get; set; }
    }
}
