Feature: delete-token
  Scenario: application deactivate

    Given user with email "dev@test.com" exists 
            And user should be logged in
            And user is in "developer" role
            and views "token/list"
      
    When user click "delete button" on grid
    Then list item is deleted  