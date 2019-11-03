using Cashflowio.Core.Entities;
using Cashflowio.Core.SharedKernel;
using Xunit;

namespace Cashflowio.Tests.Core
{
    public class IncomeSource : BaseEntity
    {
        public string Name { get; set; }
        public int AvatarId { get; set; }
        public Avatar Avatar { get; set; }
    }
    
    public class IncomeSourceTests
    {
        //TODO: Crear pruebas
        [Fact]
        public void Test()
        {
            Assert.True(true);
        }
    }
}