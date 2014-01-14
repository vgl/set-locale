Feature: Admin Behaviours
    Scenario: Admin adds a new user as translator

    Given user with email "admin@test.com" exists 
        And user should be logged in
        And user is in "Admin" role

    When user views "/admin/newtranslator" 
        And fills "Name", "Email" fields
        And clicks "Save" button
        And "Email" is not already added

    Then a user should be created 
        And user should be in "Translator" role
        And a password reset email should be sent to the new translator user
        And user should be redirected to "/admin/users"

