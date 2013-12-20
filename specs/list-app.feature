Feature: list-app
  Scenario: list application

    Given user with email "dev@test.com" exists 
            And user should be logged in
            And user is in "developer" role

    When user views "/app/list" 
    
    Then application's "app_name", "app_url" and "description" fields are listed from desc by creation date
             