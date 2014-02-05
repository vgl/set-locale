using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

using Moq;
using NUnit.Framework;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;
using SetLocale.Client.Web.Test.Builders;

namespace SetLocale.Client.Web.Test.Services
{
    [TestFixture]
    public class WordServiceTests
    {
        [Test]
        public void should_create_word()
        {
            //arrange
            var model = new WordModel
            {
                Key = "key",
                Tag = "tag1"
            };

            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.SaveChanges()).Returns(true);

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var result = sut.Create(model);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model.Key, result.Result);
            
            wordRepository.Verify(x => x.Create(It.IsAny<Word>()), Times.Once);
            wordRepository.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void should_create_check_wordmodel_IsValidForNew()
        {
            //arrange
            var model = new WordModel();

            //act
            var sut = new WordServiceBuilder().WithWordRepository(Mock.Of<IRepository<Word>>()).Build();

            var result = sut.Create(model);

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void create_check_if_word_allready_exists()
        {
            //arrange
            var model = new WordModel
            {
                Key = "key",
                Tag = "tag"
            };

            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.Any(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(true);

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object)
                                              .Build();

            var result = sut.Create(model);

            //assert
            Assert.IsNull(result);
            wordRepository.Verify(x => x.Any(It.IsAny<Expression<Func<Word, bool>>>()), Times.Once);
          
        }

        [Test]
        public void should_create_check_for_savechanges_returns_true()
        {
            //arrange
            var model = new WordModel
            {
                Key = "key",
                Tag = "tag"
            };

            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.SaveChanges()).Returns(false);

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var result = sut.Create(model);

            //assert
            Assert.IsNull(result);
        }
        
        [Test]
        public async void should_return_words_by_user_id()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>(), It.IsAny<Expression<Func<Word, object>>>() ))
                          .Returns(new List<Word>
                                    {new Word { CreatedBy = 1, Tags = new Collection<Tag> { new Tag{ Id = 0 } } },
                                     new Word { CreatedBy = 1, Tags = new Collection<Tag> { new Tag{ Id = 1 } } },
                                    }.AsQueryable()
                                  );
            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var result = await sut.GetByUserId(1,1);

            //assert
            Assert.AreEqual(2, result.TotalCount);

            wordRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>(), It.IsAny<Expression<Func<Word, object>>>()), Times.Once);
        }
        
        [Test]
        public async void should_return_word_by_key()
        {
            const string key = "key";

            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new Word { Key = key });
            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var result = await sut.GetByKey(key);

            //assert
            Assert.AreEqual(key, result.Key);

            wordRepository.Verify(x => x.FindOne(It.IsAny<Expression<Func<Word, bool>>>()), Times.Once);
        }

        [Test]
        public async void should_set_pagenumber_to_one_if_it_less()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new List<Word> { new Word { Id = 1, Key = "key" } }.AsQueryable());

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object)
                                             .Build();

            var result = await sut.GetWords(0);

            //assert
            Assert.AreEqual(1, result.Number);
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
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object)
                                             .Build();

            var result = await sut.GetWords(2);

            //assert
            Assert.AreEqual(2, result.Number);
            Assert.AreEqual(ConstHelper.PageSize, result.Size);
            Assert.AreEqual(ConstHelper.PageSize * 4, result.TotalCount);
            Assert.AreEqual(true, result.HasPreviousPage);
            Assert.AreEqual(true, result.HasNextPage);
            Assert.AreEqual(ConstHelper.PageSize, result.Items.Count);
            Assert.AreEqual((ConstHelper.PageSize * 4) - (ConstHelper.PageSize), result.Items.First().Id);
            Assert.AreEqual((ConstHelper.PageSize * 4) - (ConstHelper.PageSize * 2) + 1, result.Items.Last().Id);
        }

        [Test]
        public async void should_return_not_translated_words()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new List<Word>{new Word { Key = "1" }, new Word{ Key = "2"}}.AsQueryable());
            
            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var result = await sut.GetNotTranslated();

            //assert
            Assert.AreEqual(2, result.TotalCount);

            wordRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()), Times.Once);
        }
        
        [Test]
        public async void should_translate_check_stringIsNullOrEmpty()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            
            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var resultKey = await sut.Translate(string.Empty, "language", "translation");
            var resultLanguage = await sut.Translate("key", string.Empty, "translation");
            var resultTranslation = await sut.Translate("key", "language", string.Empty);

            //assert
            Assert.AreEqual(false, resultKey);
            Assert.AreEqual(false, resultLanguage);
            Assert.AreEqual(false, resultTranslation);
        }
        
        [Test]
        public async void should_translate_check_word_for_null()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<Word, bool>>>())).Returns((Word)null);

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var resultKey = await sut.Translate("key", "language", "translation");
            
            //assert
            Assert.AreEqual(false, resultKey);
        }

        [Test]
        public async void should_translate_check_for_translationProperty_exists()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<Word, bool>>>())).Returns(new Word());
            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var resultKey = await sut.Translate("key", "not existing language", "translation");

            //assert
            Assert.AreEqual(false, resultKey);
        }

        [Test]
        public async void should_translate_call_update_and_savechanges()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new Word());

            wordRepository.Setup(x => x.Update(It.IsAny<Word>()))
                          .Returns(new Word());

            wordRepository.Setup(x => x.SaveChanges())
                          .Returns(true);

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var resultKey = await sut.Translate("key", "EN", "translation");

            //assert
            Assert.AreEqual(true, resultKey);

            wordRepository.Verify(x=>x.Update(It.IsAny<Word>()), Times.Once);
            wordRepository.Verify(x=>x.SaveChanges(), Times.Once);
        }
        
        [Test]
        public async void should_Tag_check_stringIsNullOrEmpty()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            
            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var resultKey = await sut.Tag(string.Empty, "tagName");
            var resulttagName = await sut.Tag("key", string.Empty);

            //assert
            Assert.AreEqual(false, resultKey);
            Assert.AreEqual(false, resulttagName);
        }
        [Test]
        public async void should_Tag_check_word_for_null()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<Word, bool>>>())).Returns((Word)null);

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var resultKey = await sut.Tag("key", "tagName");

            //assert
            Assert.AreEqual(false, resultKey);
        }

        [Test]
        public async void should_Tag_call_update_and_savechanges()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindOne(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new Word());

            wordRepository.Setup(x => x.Update(It.IsAny<Word>()))
                          .Returns(new Word());

            wordRepository.Setup(x => x.SaveChanges())
                          .Returns(true);

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object)
                                              .Build();

            var resultKey = await sut.Tag("key", "tagName");

            //assert
            Assert.AreEqual(true, resultKey);

            wordRepository.Verify(x => x.Update(It.IsAny<Word>()), Times.Once);
            wordRepository.Verify(x => x.SaveChanges(), Times.Once);
        }
        
        [Test]
        public async void should_GetAll_return_words()
        {
            //arrange
            var wordRepository = new Mock<IRepository<Word>>();
            wordRepository.Setup(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()))
                          .Returns(new List<Word> { new Word { Key = "1" }, new Word { Key = "2" } }.AsQueryable());

            //act
            var sut = new WordServiceBuilder().WithWordRepository(wordRepository.Object).Build();

            var result = await sut.GetWords(1);

            //assert
            Assert.AreEqual(2, result.TotalCount);
            wordRepository.Verify(x => x.FindAll(It.IsAny<Expression<Func<Word, bool>>>()), Times.Once);
        }
    }
}
