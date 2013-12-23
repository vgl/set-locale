Feature: Admin Panel Behaviours
  Scenario: Admin adds a new user as translator

  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role

    When user views "/admin/new/user" 
        And fills "Name", "Email" fields
        And clicks "Create User" button
        And "Email" is not already added

    Then a user should be created 
    	And user should be in "Translator" role
		And a password reset email should be sent to the new created user
    	And redirected to "/admin/users"

