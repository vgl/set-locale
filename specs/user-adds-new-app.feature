Feature: App Management
  Scenario: insert new application

    Given user with email "dev@test.com" exists 
            And user should be logged in
            And user is in "developer" role

    When user views "/app/new" 
            And must fill "app_name" and "app_url" fields and may fills "Description"

            And clicks "Save"
            And app_name is not already added

    Then data should be saved




    