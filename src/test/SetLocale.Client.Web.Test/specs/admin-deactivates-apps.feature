Feature: Admin Behaviours
  Scenario: Admin deactivates apps

  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role

	When user views "/admin/users"
		And clicks "Deactivate" button of app row   
		And clicks "Ok" button in popup		         

    Then app should be deactivated
		And "Activate" button should be seen in the app row