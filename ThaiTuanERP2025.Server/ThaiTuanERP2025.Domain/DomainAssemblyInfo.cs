using System.Runtime.CompilerServices;

// Allow Application Layer to access internal domain behaviors
[assembly: InternalsVisibleTo("ThaiTuanERP2025.Application")]

// Allow Infrastructure Layer to use EF Core configurations + repositories
[assembly: InternalsVisibleTo("ThaiTuanERP2025.Infrastructure")]

// Allow Unit Tests to access internal behavior 
// (optional but RECOMMENDED for domain behavior tests)
[assembly: InternalsVisibleTo("ThaiTuanERP2025.Tests")]