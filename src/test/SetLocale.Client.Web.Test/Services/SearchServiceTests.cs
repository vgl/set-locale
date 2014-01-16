using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Services
{
    [TestFixture]
    public class SearchServiceTests
    {
        private Mock<IRepository<Word>> _wordRepository = null;

        private const string _exp = "too_long_exp_that_needed_to_substring";
        private const string _wordDetail = "/word/detail/{0}";
        private const string _name = "{0}, {1} ...";
        private const string _imgUrl = "/public/img/word.png";

        [SetUp]
        public void Initialize()
        {
            _wordRepository = new Mock<IRepository<Word>>();
            _wordRepository.Setup(x => x.Set<Word>())
                           .Returns(new List<Word>
                            {   new Word { Id = 1, Key = "k1", Translation_TR = "tr1", Translation_EN = "eng1" },
                                new Word { Id = 2, Key = "k1_k2", Translation_TR = _exp},
                                new Word { Id = 3, Key = "k1_k3", Translation_TR = "tr3", Translation_EN = "eng3" },
                            }.AsQueryable());
        }

        [Test]
        public async void should_return_emptylist_if_query_string_isempty()
        {
            //Act
            var sut = new SearchServiceBuilder().WithWordRepository(_wordRepository.Object)
                                                .Build();

            var result = await sut.Query(string.Empty);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async void should_return_one_custom_searchresult()
        {
            //act
            var sut = new SearchServiceBuilder().WithWordRepository(_wordRepository.Object)
                                                .Build();

            var result = await sut.Query("k1_k3");

            //assert
            Assert.AreEqual(1, result.Count);

            var searchResult = result.First();
            
            Assert.IsNotNull(searchResult);

            Assert.AreEqual(string.Format(_wordDetail, "k1_k3"), searchResult.Url);
            Assert.AreEqual(string.Format(_name, "k1_k3", "eng3"), searchResult.Name);
            Assert.AreEqual(_imgUrl, searchResult.ImgUrl);
        }

        [Test]
        public async void should_return_searchresult_with_substringed_exp()
        {
            //arrange 
            const int expMaxlength = 15;

            //act
            var sut = new SearchServiceBuilder().WithWordRepository(_wordRepository.Object)
                                                .Build();

            var result = await sut.Query("k1 k2");

            //assert
            Assert.AreEqual(1, result.Count);

            var searchResult = result.First();
            Assert.AreEqual(string.Format(_name, "k1_k2", _exp.Substring(0, expMaxlength)), searchResult.Name);
        }
        
        [Test]
        public async void should_return_results_in_descending_order()
        {
            //act
            var sut = new SearchServiceBuilder().WithWordRepository(_wordRepository.Object)
                                                .Build();

            var result = await sut.Query("k1");

            //assert
            Assert.AreEqual(3, result.Count);

            var firstResult = result.First();
            Assert.AreEqual(string.Format(_wordDetail, "k1_k3"), firstResult.Url);

            var lastResult = result.Last();
            Assert.AreEqual(string.Format(_wordDetail, "k1"), lastResult.Url);
        }
    }
}
