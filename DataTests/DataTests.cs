using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

using SampleMVC.Data.Repositories;
using SampleMVC.Data.Entities;
using SampleMVC.Data;
using System.Diagnostics;
using System.Data.Entity;

namespace SampleMVC.Data.Tests
{
    [TestClass]
    public class DataTests : UnitTestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            UnitTestBase.ClassInitialize(testContext);
        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
            UnitTestBase.ClassCleanup();
        }

        [TestMethod]
        public void DeleteAppPeopleTest()
        {
            using (MyDbContext context = new MyDbContext())
            {
                var people = context.People;
                context.People.RemoveRange(people);
                context.SaveChanges();
            }
            //totally new connection
            using (MyDbContext context = new MyDbContext())
            {
                var people = context.People.ToList();
                Assert.AreEqual(people.Count, 0, "There shouldn't be any people.");

            }
        }

        [TestMethod]
        public void TestGetPeopleWithDefaults()
        {
            using (MyRepository repository = new MyRepository())
            {
                PagedSearchDto dto = new PagedSearchDto();
                dto.PageSize = 25;
                dto.PageNumber = 2;
                dto.OrderByColumn = "PersonId";
                dto.OrderAscending = true;
                dto.TotalRows = 0;
                PagedSearchResponseDto<List<PersonSearchResultDto>> response = repository.SearchPeople(dto);
                Assert.IsTrue(response.Result.Count == 25);
                Assert.IsTrue(response.Result.First().PersonId == 26);
            }
        }
        [TestMethod]
        public void TestGetPeople()
        {
            using (MyDbContext context = new MyDbContext())
            {
                var people = context.People.Take(25);
                Assert.IsTrue(people.Count() == 25);
            }
        }
    }
}
