<<<<<<< HEAD
﻿using Moq;
using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
﻿using SetLocale.Client.Web.Controllers;
using SetLocale.Client.Web.Services;
>>>>>>> 82a9ed50349993e7185d704e2ac7ca3c6e24ff04

namespace SetLocale.Client.Web.Test.Builders
{
    public class AdminControllerBuilder
    {
<<<<<<< HEAD

=======
>>>>>>> 82a9ed50349993e7185d704e2ac7ca3c6e24ff04
        private IFormsAuthenticationService _formAuthenticationService;
        private IUserService _userService;
        private IAppService _appService;
    
        public AdminControllerBuilder()
        {
            _formAuthenticationService = null;
            _userService = null;
            _appService = null;
        }

        internal AdminControllerBuilder WithFormsAuthenticationService(IFormsAuthenticationService formAuthenticationService)
        {
            _formAuthenticationService = formAuthenticationService;
            return this;
        }

        internal AdminControllerBuilder WithUserService(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        internal AdminControllerBuilder WithAppService(IAppService appService)
        {
            _appService = appService;
            return this;
<<<<<<< HEAD

=======
>>>>>>> 82a9ed50349993e7185d704e2ac7ca3c6e24ff04
        }

        internal AdminController Build()
        {
            return new AdminController(_userService, _formAuthenticationService, _appService);
        }
<<<<<<< HEAD



=======
>>>>>>> 82a9ed50349993e7185d704e2ac7ca3c6e24ff04
    }
}
