Feature: Developer Panel Behaviours
  Scenario: User lists apps in other pages
  	Given user with email "dev@test.com" exists 
    	And user should be logged in
    	And user is in "Developer" role

    When user views "/app/list"
		And clicks page numbers under and above the users list table         

    Then redirected to "/app/list/{page}"
		And "50" user from system skiping the previous records and taking the necassary ones should be listed in a table.

