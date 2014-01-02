Feature: Key Management
  Scenario: Developer role adds new key to set-locale

    Given user with email "dev@test.com" exists 
    	And user should be logged in
    	And user is in "developer" role

    When user views "/word/new" 
    	And must fill "Key" field and may fills "Tag", "Description"

    	And clicks "Save"
    	And key is not already added

    Then data should be saved