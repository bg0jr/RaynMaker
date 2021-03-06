﻿using NUnit.Framework;

namespace RaynMaker.Entities.Tests
{
    [TestFixture]
    class CompanyTests
    {
        [Test]
        public void References_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.References, Is.Not.Null );
        }

        [Test]
        public void Stocks_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.Stocks, Is.Not.Null );
        }

        [Test]
        public void Assets_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.CurrentAssets, Is.Not.Null );
        }

        [Test]
        public void Debts_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.TotalLiabilities, Is.Not.Null );
        }

        [Test]
        public void Dividends_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.Dividends, Is.Not.Null );
        }

        [Test]
        public void EBITs_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.EBITs, Is.Not.Null );
        }

        [Test]
        public void Equities_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.Equities, Is.Not.Null );
        }

        [Test]
        public void InterestExpenses_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.InterestExpenses, Is.Not.Null );
        }

        [Test]
        public void Liabilities_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.CurrentLiabilities, Is.Not.Null );
        }

        [Test]
        public void NetIncomes_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.NetIncomes, Is.Not.Null );
        }

        [Test]
        public void Revenues_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.Revenues, Is.Not.Null );
        }

        [Test]
        public void SharesOutstandings_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.SharesOutstandings, Is.Not.Null );
        }

        [Test]
        public void Tags_AfterCtor_NotNull()
        {
            var company = new Company();

            Assert.That( company.Tags, Is.Not.Null );
        }
    }
}
