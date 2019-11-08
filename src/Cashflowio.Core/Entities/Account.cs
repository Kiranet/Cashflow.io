using Cashflowio.Core.SharedKernel;

namespace Cashflowio.Core.Entities
{
    public abstract class Account : BaseEntity
    {
        public string Name { get; set; }
        public Currency Currency { get; set; } = Currency.Mxn;

        public int AvatarId { get; set; }
        public Avatar Avatar { get; set; }
    }
}