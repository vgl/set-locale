Feature: User Management
  Scenario: New user as developer

  	Given a visitor wants to signup
    When visitor views "/user/new" 
        And fills "Name", "Email", "Password" fields
        And clicks "Signup" button
        And "Email" is not already added
        And "Password" length is between 6 and 20

    Then a user should be created 
    	And user should be in developer role
    	And redirected to "/user/index/{id}"

