Feature: Admin Behaviours
  Scenario: Admin lists apps first page

    Given user with email "admin@test.com" exists 
        And user should be logged in
        And user is in "Admin" role

    When user views "/admin/apps"         

    Then apps fist page should be listed in a table