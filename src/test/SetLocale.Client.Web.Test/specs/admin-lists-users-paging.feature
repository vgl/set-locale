Feature: Admin Behaviours
  Scenario: Admin lists users in other pages

    Given user with email "admin@test.com" exists 
        And user should be logged in
        And user is in "Admin" role

    When user views "/admin/users"
		And clicks page numbers under the users list table         

    Then redirected to "/admin/users/{roleId}?page={page}"
		And requested page of users should be listed in a table