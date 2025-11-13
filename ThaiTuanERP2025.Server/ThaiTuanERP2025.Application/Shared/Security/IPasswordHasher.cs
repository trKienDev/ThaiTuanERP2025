namespace ThaiTuanERP2025.Application.Shared.Security
{
	public interface IPasswordHasher
	{
		string Hash(string password);
		bool Verify(string password, string hashedPassword);
	}
}
