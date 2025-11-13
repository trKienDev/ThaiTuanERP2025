using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Shared.Services
{
	public interface IDocumentSubIdGeneratorService
	{
		/// <summary> Sinh ShortId dạng ddMMyyyy + '1' + STT-trong-ngày </summary>
		Task<string> NextSubIdAsync(DocumentType documentType, DateTime nowUtc, CancellationToken cancellationToken);
	}
}
