namespace eUIT.API.DTOs.Create
{
    public class CertificateConfirmationCreateDto
    {
        public string MaChungChi { get; set; } = string.Empty;
        public string LoaiChungChi { get; set; } = string.Empty;
        public DateTime NgayThi { get; set; }
    }
}