Feature: Admin Behaviours
  Scenario: Admin lists apps in other pages

    Given user with email "admin@test.com" exists 
        And user should be logged in
        And user is in "Admin" role

    When user views "/admin/apps"
		And clicks page numbers under the users list table         

    Then redirected to "/admin/apps/{page}"		
		And requested page of apps should be listed in a table