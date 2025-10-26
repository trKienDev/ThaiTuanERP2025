namespace ThaiTuanERP2025.Application.Common.Security
{
	public interface IPasswordHasher
	{
		string Hash(string password);
		bool Verify(string password, string hashedPassword);
	}
}
