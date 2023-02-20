using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeStoreTests.TestData
{
    public class Data
    {
        public List<Product> Products { get; set; }
        public Card Card { get; set; }
        public ValidationMessages ValidationMessages { get; set; }
        public Customer Customer { get; set; }
        public User User { get; set; }
    }
}
