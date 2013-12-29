Feature: Admin Panel Behaviours
  Scenario: Admin lists apps first page
  	Given user with email "admin@test.com" exists 
    	And user should be logged in
    	And user is in "Admin" role

    When user views "/admin/apps"         

    Then "50" app from system should be listed in a table.
