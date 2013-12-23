Feature: delete-token
  Scenario: application deactivate

    Given user with email "dev@test.com" exists 
            And user should be logged in
            And user is in "developer" role
            And views "app/list"
      
    When user click "create new button" 
    Then new token will be created and will be listed  