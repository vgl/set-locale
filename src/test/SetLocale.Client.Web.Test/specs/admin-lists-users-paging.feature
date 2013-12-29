Feature: Admin Panel Behaviours
  Scenario: Admin lists users in other pages
  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role

    When user views "/admin/users"
		And clicks page numbers under and above the users list table         

    Then redirected to "/admin/users/{page}"
		And "50" user from system skiping the previous records and taking the necassary ones should be listed in a table.

