using Shared.Interface;

namespace Web.MigrationFactory
{
    public class MockCurrentRequestUserService : ICurrentRequestUserService
    {
        public string GetCurrentUserEmail() => "migrations@system";
    }
}
