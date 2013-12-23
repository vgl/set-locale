Feature: Admin Panel Bahviours
  Scenario: Admin lists users first page
  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role

    When user views "/admin/users"         

    Then "50" user from system should be listed in a table.
