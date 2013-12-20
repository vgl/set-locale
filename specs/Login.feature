Feature: User Management
  Scenario:Login


    When user views "/user/login"
	
	    if clicks "Forget your password?"  button
		   redirect to "/user/newpassword"
		if clicks "Sign up"  button
           redirect to "/user/newuser"
		   
        if Not 
		    user must fill "E-Mail" and "Password"
            And clicks "Sign In"
            And user has an account with this "E-mail" and "Password"
       		

    Then user should be directed to the main page