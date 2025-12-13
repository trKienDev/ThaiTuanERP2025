using System.Net.Http.Json;
using ThaiTuanERP2025.Application.Drive.Contracts;
using ThaiTuanERP2025.Application.Drive.Intergrations;

namespace ThaiTuanERP2025.Infrastructure.Drive.Intergrations
{
	public sealed class DriveQueryClient : IDriveQueryClient
	{
		private readonly HttpClient _http;
		public DriveQueryClient(HttpClient http)
		{
			_http = http;	
		}

		public async Task<IReadOnlyList<DriveObjectDto>> GetObjectsByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken)
		{
			using var response = await _http.PostAsJsonAsync("/internal/objects/batch", ids, cancellationToken);

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadFromJsonAsync<List<DriveObjectDto>>(
				cancellationToken: cancellationToken
			);

			return result ?? [];
		}
	}
}
