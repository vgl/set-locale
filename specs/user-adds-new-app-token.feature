Feature: Applicatin Behaviours
    Scenario: application deactivate

    Given user with email "dev@test.com" exists 
        And user should be logged in
        And user is in "developer" role
        And views "app/index/{id}"
      
    When user clicks "create new token" button at right corner of the screen
    
    Then new token will be created 
        And new line will be added to the table for the new token
