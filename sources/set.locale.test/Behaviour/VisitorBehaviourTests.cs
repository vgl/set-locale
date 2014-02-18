using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using set.locale.Data.Services;
using set.locale.Helpers;
using set.locale.Models;
using set.locale.test.Shared;
using set.locale.test.Shared.Builders;

namespace set.locale.test.Behaviour
{
    [TestFixture]
    public class VisitorBehaviourTests : BaseBehaviourTest
    {
        [Test]
        public async void any_visitor_can_create_user_account()
        {
            //arrange 
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.Create(ValidUserModel, ConstHelper.User))
                       .Returns(Task.FromResult(true));

            var authService = new Mock<IAuthService>();
            authService.Setup(x => x.SignIn(ValidUserModel.Id, ValidUserModel.Name, ValidUserModel.Email, ConstHelper.User, true));

            //act
            var sut = new UserControllerBuilder().WithUserService(userService.Object)
                                                 .WithAuthService(authService.Object)
                                                 .Build();

            var result = await sut.New(ValidUserModel);

            ////assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<RedirectResult>(result);

            userService.Verify(x => x.Create(ValidUserModel, ConstHelper.User), Times.Once);
            authService.Verify(x => x.SignIn(ValidUserModel.Id, ValidUserModel.Name, ValidUserModel.Email, ConstHelper.User, true), Times.Once);

            sut.AssertPostAttribute(ACTION_NEW, new[] { typeof(UserModel) });
            sut.AssertAllowAnonymousAttribute(ACTION_NEW, new[] { typeof(UserModel) });
        }

        [Test]
        public async void any_visitor_can_send_feedback()
        {
            //arrange 
            var feedbackService = new Mock<IFeedbackService>();
            feedbackService.Setup(x => x.CreateFeedback(ValidContactMessageModel.Message, ValidContactMessageModel.Email))
                           .Returns(Task.FromResult(true));

            //act
            var sut = new FeedbackControllerBuilder().WithFeedbackService(feedbackService.Object)
                                                     .BuildWithMockControllerContext(ValidUserModel.Id, ValidUserModel.Name, ValidUserModel.Email, ValidUserModel.RoleName);

            var result = await sut.New(ValidContactMessageModel.Message);

            //assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<JsonResult>(result);

            feedbackService.Verify(x => x.CreateFeedback(ValidContactMessageModel.Message, ValidContactMessageModel.Email), Times.Once);

            sut.AssertPostAttributeWithOutAntiForgeryToken(ACTION_NEW, new[] { typeof(string) });
            sut.AssertAllowAnonymousAttribute(ACTION_NEW, new[] { typeof(string) });
        }

        [Test]
        public async void any_visitor_can_send_contact_message()
        {
            //arrange 
            var feedbackService = new Mock<IFeedbackService>();
            feedbackService.Setup(x => x.CreateContactMessage(ValidContactMessageModel.Subject, ValidContactMessageModel.Email, ValidContactMessageModel.Message))
                           .Returns(Task.FromResult(true));
            var reportService = new Mock<IReportService>();

            //act
            var sut = new HomeControllerBuilder().WithFeedbackService(feedbackService.Object)
                                                 .Build();
            var result = await sut.Contact(ValidContactMessageModel);

            //assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<ViewResult>(result);

            feedbackService.Verify(x => x.CreateContactMessage(ValidContactMessageModel.Subject, ValidContactMessageModel.Email, ValidContactMessageModel.Message), Times.Once);

            sut.AssertPostAttribute(ACTION_CONTACT, new[] { typeof(ContactMessageModel) });
            sut.AssertAllowAnonymousAttribute(ACTION_CONTACT, new[] { typeof(ContactMessageModel) });
        }

        [Test]
        public void any_visitor_can_change_language()
        {
            //arrange
            var controllerContext = new Mock<ControllerContext>();

            var httpContext = new Mock<HttpContextBase>();

            var httpRequest = new Mock<HttpRequestBase>();
            var httpResponse = new Mock<HttpResponseBase>();

            controllerContext.Setup(x => x.HttpContext).Returns(httpContext.Object);

            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);

            httpResponse.Setup(x => x.SetCookie(It.IsAny<HttpCookie>()));

            //act
            var sut = new LangControllerBuilder().Build();
            sut.ControllerContext = controllerContext.Object;

            var view = sut.Change(ConstHelper.tr);

            //assert
            Assert.NotNull(view);

            sut.AssertGetAttribute(ACTION_CHANGE, new[] { typeof(string) });
            sut.AssertAllowAnonymousAttribute(ACTION_CHANGE, new[] { typeof(string) });

            httpResponse.Verify(x => x.SetCookie(It.IsAny<HttpCookie>()), Times.AtLeastOnce);
        }

        [Test]
        public async void any_visitor_can_search()
        {
            //arrange 
            const string text = "search_text";

            var searchService = new Mock<ISearchService>();
            searchService.Setup(x => x.Query(text)).Returns(Task.FromResult(new List<SearchResult> { new SearchResult { Name = text } }));

            //act
            var sut = new SearchControllerBuilder().WithSearchService(searchService.Object)
                                                   .Build();

            var result = await sut.Query(text);

            //assert
            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<JsonResult>(result);

            sut.AssertGetAttribute(ACTION_QUERY, new[] { typeof(string) });
            sut.AssertAllowAnonymousAttribute(ACTION_QUERY, new[] { typeof(string) });
        }
    }
}