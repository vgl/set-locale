Feature: deactivate-app
  Scenario: application deactivate

    Given user with email "dev@test.com" exists 
            And user should be logged in
            And user is in "developer" role
            And views "app/list"
      
    When user click "deactivate button" on grid
    Then list item is deactivated  
             