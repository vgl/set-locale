Feature: Admin Panel Behaviours
  Scenario: Admin deactivates users in "Translator" and "Developer" roles
  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role

    When user views "/admin/users"
		And clicks "Deactivate" button of user row   
		And user fills "Password" in popup
		And clicks "Confirm"      

    Then user should be deactivated
		And "Activate" button should be seen in the user row

