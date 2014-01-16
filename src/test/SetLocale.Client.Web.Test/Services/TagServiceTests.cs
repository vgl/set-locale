using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Services
{
    [TestFixture]
    public class TagServiceTests
    {
        [Test]
        public async void should_return_words()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new List<Word> { new Word{Id = 1, Key="key"} }.AsQueryable());

            //act
            var sut = new TagServiceBuilder().WithWordRepository(wordRepository.Object)
                                             .Build();

            var result = await sut.GetWords(string.Empty);

            //assert
            Assert.AreEqual(1, result.Count);
            
            wordRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()), Times.AtLeastOnce);
        }

        [Test]
        public async void should_set_pagenumber_to_one_if_it_less()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new List<Word> { new Word { Id = 1, Key = "key" } }.AsQueryable());

            //act
            var sut = new TagServiceBuilder().WithWordRepository(wordRepository.Object)
                                             .Build();

            var result = await sut.GetWords(string.Empty, 0);

            //assert
            Assert.AreEqual(0, result.Number);
        }

        [Test]
        public async void should_return_pagedlist()
        {
            //arrange
            var words = new List<Word>();
            for (var i = 1; i <= ConstHelper.PageSize * 4; i++)
                words.Add(new Word { Id = i });

            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(words.AsQueryable());

            //act
            var sut = new TagServiceBuilder().WithWordRepository(wordRepository.Object)
                                             .Build();

            var result = await sut.GetWords(string.Empty, 2);

            //assert
            Assert.AreEqual(1, result.Number);
            Assert.AreEqual(ConstHelper.PageSize, result.Size);
            Assert.AreEqual(ConstHelper.PageSize * 4, result.TotalCount);
            Assert.AreEqual(true, result.HasPreviousPage);
            Assert.AreEqual(true, result.HasNextPage);
            Assert.AreEqual(ConstHelper.PageSize, result.Items.Count);
            Assert.AreEqual((ConstHelper.PageSize * 4) - (ConstHelper.PageSize), result.Items.First().Id);
            Assert.AreEqual((ConstHelper.PageSize * 4) - (ConstHelper.PageSize * 2) + 1, result.Items.Last().Id);
        }
    }
}
