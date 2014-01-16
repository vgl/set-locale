Feature: Admin Behaviours
  Scenario: Admin lists users first page

  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role
		    
	When user views "/admin/users"         

    Then users fist page should be listed in a table
