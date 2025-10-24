using ThaiTuanERP2025.Domain.Common.Enums;

namespace ThaiTuanERP2025.Application.Common.Services
{
	public interface IDocumentSubIdGeneratorService
	{
		/// <summary> Sinh ShortId dạng ddMMyyyy + '1' + STT-trong-ngày </summary>
		Task<string> NextSubIdAsync(DocumentType documentType, DateTime nowUtc, CancellationToken cancellationToken);
	}
}
