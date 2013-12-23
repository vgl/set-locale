Feature: Admin Panel Behaviours
  Scenario: Admin deactivates apps
  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role

    When user views "/admin/apps"
		And clicks "Deactivate" button of app row       
		And user fills "Password" in popup
		And clicks "Confirm"

    Then app should be deactivated
		And "Activate" button should be seen in the app row

