using System;
using Cashflowio.Core.Entities;
using FluentDateTime;
using Xunit;

namespace Cashflowio.UnitTests.Transactions
{
    public class RawTransactionTests
    {
        [Fact]
        public void Validate()
        {
            var sut = new RawTransaction
            {
                Amount = 120,
                Date = DateTime.Now.PreviousDay()
            };
            
            Assert.True(sut.IsValid());
        }
    }
}